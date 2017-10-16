// THIS FILE IS A PART OF EMZI0767'S BOT EXAMPLES
//
// --------
// 
// Copyright 2017 Emzi0767
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//  http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// --------
//
// This is a WPF example. It shows how to use WPF without deadlocks.

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace DSPlus.Examples
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged // INotifyPropertyChanged is for easy UI updates
    {
        // this property controls the title of this window
        public string WindowTitle
        {
            get => this._title;
            set { this._title = value; this.OnPropertyChanged(nameof(this.WindowTitle)); }
        }
        private string _title;

        // this property will hold the text on the bot start/stop button
        public string ControlButtonText
        {
            get => this._ctl_btn_text;
            set { this._ctl_btn_text = value; this.OnPropertyChanged(nameof(this.ControlButtonText)); }
        }
        private string _ctl_btn_text;

        // this property will hold the next message the user intends to send
        public string NextMessage
        {
            get => this._next_message;
            set { this._next_message = value; this.OnPropertyChanged(nameof(this.NextMessage)); }
        }
        private string _next_message;

        // this property will enable or disable certain UI elements
        public bool EnableUI
        {
            get => this._enable_ui;
            set { this._enable_ui = value; this.OnPropertyChanged(nameof(this.EnableUI)); }
        }
        private bool _enable_ui;

        // this will hold the thread on which the bot will run
        private Task BotThread { get; set; }

        // this will hold the bot itself
        private Bot Bot { get; set; }

        // this will hold a token required to make the bot quit cleanly
        private CancellationTokenSource TokenSource { get; set; }

        // these are for UI state
        public BotGuild SelectedGuild
        {
            get => this._selected_guild;
            set
            {
                this._selected_guild = value;
                this._selected_channel = default(BotChannel);
                this._selected_message = default(BotMessage);
                this.Channels.Clear();
                this.Banter.Clear();
                
                if (this.SelectedGuild.Guild != null)
                {
                    var chns = this.SelectedGuild.Guild.Channels
                        .Where(xc => xc.Type == ChannelType.Text)
                        .OrderBy(xc => xc.Position)
                        .Select(xc => new BotChannel(xc));
                    foreach (var xbc in chns)
                        this.Channels.Add(xbc);
                }

                this.OnPropertyChanged(nameof(this.SelectedGuild), nameof(this.SelectedChannel), nameof(this.SelectedMessage));
            }
        }
        private BotGuild _selected_guild;

        public BotChannel SelectedChannel
        {
            get => this._selected_channel;
            set
            {
                this._selected_channel = value;
                this._selected_message = default(BotMessage);
                this.Banter.Clear();
                this.OnPropertyChanged(nameof(this.SelectedChannel), nameof(this.SelectedMessage));
            }
        }
        private BotChannel _selected_channel;

        public BotMessage SelectedMessage
        {
            get => this._selected_message;
            set { this._selected_message = value; this.OnPropertyChanged(nameof(this.SelectedMessage)); }
        }
        private BotMessage _selected_message;

        // these will hold the respective collections
        // they're observable, so any changes made to them will be automatically
        // reflected in the UI
        public ObservableCollection<BotGuild> Guilds { get; }
        public ObservableCollection<BotChannel> Channels { get; }
        public ObservableCollection<BotMessage> Banter { get; }

        public MainWindow()
        {
            this._title = "Example WPF Bot";      // set the initial title
            this._ctl_btn_text = "Start the bot"; // set the initial button text
            this._next_message = "";              // set the initial message
            this._enable_ui = true;               // enable the UI

            this.Guilds = new ObservableCollection<BotGuild>();     // initialize the guild collection
            this.Channels = new ObservableCollection<BotChannel>(); // initialize the channel collection
            this.Banter = new ObservableCollection<BotMessage>(); // initialize the message collection

            InitializeComponent();
        }

        // this occurs when user presses the send message button
        private void Button_Click(object sender, RoutedEventArgs e)
            => this.SendMessage();

        // this occurs when user presses a button inside the message
        // text box, we use that to handle enter key press
        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            // check if the key pressed was enter
            if (e.Key == Key.Enter)
            {
                // if yes, mark the event as handled, and send
                // the message
                e.Handled = true;
                this.SendMessage();
            }
        }

        // this occurs when user presses the start/stop button
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // lock the controls until they can be used again
            this.EnableUI = false;

            // check if a bot thread is running
            if (this.BotThread == null)
            {
                // start the bot

                // change the button's text to indicate it now 
                // stops the bot instead
                this.ControlButtonText = "Stop the bot";

                // create the bot container
                this.Bot = new Bot(this.pbToken.Password);

                // hook all the bot events
                this.Bot.Client.Ready += this.Bot_Ready;
                this.Bot.Client.GuildAvailable += this.Bot_GuildAvailable;
                this.Bot.Client.GuildCreated += this.Bot_GuildCreated;
                this.Bot.Client.GuildUnavailable += this.Bot_GuildUnavailable;
                this.Bot.Client.GuildDeleted += this.Bot_GuildDeleted;
                this.Bot.Client.MessageCreated += this.Bot_MessageCreated;
                this.Bot.Client.ClientErrored += this.Bot_ClientErrored;

                // create a cancellation token, this will be used 
                // to cancel the infinite delay task
                this.TokenSource = new CancellationTokenSource();

                // finally, start the thread with the bot
                this.BotThread = Task.Run(this.BotThreadCallback);
            }
            else
            {
                // stop the bot

                // change the button's text to indicate it now 
                // starts the bot instead
                this.ControlButtonText = "Start the bot";

                // request cancelling the task preventing the 
                // bot from stopping
                // this will effectively stop the bot
                this.TokenSource.Cancel();
            }

            // clear the token text box, we don't need it anymore
            this.pbToken.Password = "";
        }

        // this is called by the send button and message textbox 
        // key press handler
        private void SendMessage()
        {
            // check if we have a channel selected, if not, do 
            // nothing
            if (this.SelectedChannel.Channel == null)
                return;

            // capture the next message and reset the text box
            var txt = this.NextMessage;
            this.NextMessage = "";

            // check if a message was typed in at all, if not,
            // do nothing
            if (string.IsNullOrWhiteSpace(txt))
                return;

            // start an asynchronous task which will send the 
            // message, and once it's done, set the message 
            // textbox's text to empty using the UI thread
            _ = Task.Run(() => this.BotSendMessageCallback(txt, this.SelectedChannel));
        }

        // this method will be ran on the bot's thread
        // it will take care of the initialization logic, as 
        // well as actually handling the bot
        private async Task BotThreadCallback()
        {
            // this will start the bot
            await this.Bot.StartAsync().ConfigureAwait(false);

            // once the bot is started, we can enable the UI
            // elements again
            this.SetProperty(x => x.EnableUI, true);

            // here we wait indefinitely, or until the wait is
            // cancelled
            try
            {
                // the token will cancel the way once it's 
                // requested
                await Task.Delay(-1, this.TokenSource.Token).ConfigureAwait(false);
            }
            catch { /* ignore the exception; it's expected */ }

            // this will stop the bot
            await this.Bot.StopAsync().ConfigureAwait(false);

            // once the bot is stopped, we can enable the UI 
            // elements again
            this.SetProperty(x => x.EnableUI, true);
            this.SetProperty(x => x.WindowTitle, "Example WPF Bot");

            // and reset the UI state
            this.SetProperty(x => x.SelectedGuild, default(BotGuild));
            this.SetProperty(x => x.SelectedChannel, default(BotChannel));
            this.InvokeAction(new Action(this.Guilds.Clear));

            // and finally, dispose of our bot stuff
            this.Bot = null;
            this.TokenSource = null;
            this.BotThread = null;
        }

        // this is used by the send message method, to 
        // asynchronously send the message
        private Task BotSendMessageCallback(string text, BotChannel chn)
            => chn.Channel.SendMessageAsync(text);

        // this handles the bot's ready event
        private Task Bot_Ready(ReadyEventArgs e)
        {
            // set the window title to indicate we are connected
            this.SetProperty(xf => xf.WindowTitle, "Example WPF Bot (connected)");
            return Task.CompletedTask;
        }

        // called when any of the bot's guilds becomes available
        private Task Bot_GuildAvailable(GuildCreateEventArgs e)
        {
            // add the guild to the bot's guild collection
            this.InvokeAction(new Action<BotGuild>(this.AddGuild), new BotGuild(e.Guild));
            return Task.CompletedTask;
        }

        // called when any of the bot joins a guild
        private Task Bot_GuildCreated(GuildCreateEventArgs e)
        {
            // add the guild to the bot's guild collection
            this.InvokeAction(new Action<BotGuild>(this.AddGuild), new BotGuild(e.Guild));
            return Task.CompletedTask;
        }

        // called when any of the bot's guilds becomes unavailable
        private Task Bot_GuildUnavailable(GuildDeleteEventArgs e)
        {
            // remove the guild from the bot's guild collection
            this.InvokeAction(new Action<ulong>(this.RemoveGuild), e.Guild.Id);
            return Task.CompletedTask;
        }

        // called when any of the bot leaves a guild
        private Task Bot_GuildDeleted(GuildDeleteEventArgs e)
        {
            // remove the guild from the bot's guild collection
            this.InvokeAction(new Action<ulong>(this.RemoveGuild), e.Guild.Id);
            return Task.CompletedTask;
        }

        // called when the bot receives a message
        private Task Bot_MessageCreated(MessageCreateEventArgs e)
        {
            // if this message is not meant for the currently 
            // selected channel, ignore it
            if (this.SelectedChannel.Channel?.Id != e.Channel.Id)
                return Task.CompletedTask;

            // if it is, add it to the banter box
            this.InvokeAction(new Action<BotMessage>(this.AddMessage), new BotMessage(e.Message));
            return Task.CompletedTask;
        }

        // called when an unhandled exception occurs in any of the 
        // event handlers
        private Task Bot_ClientErrored(ClientErrorEventArgs e)
        {
            // show a message box by dispatching it to the UI thread
            this.InvokeAction(new Action(() => MessageBox.Show(this, $"Exception in {e.EventName}: {e.Exception.ToString()}", "Unhandled exception in the bot", MessageBoxButton.OK, MessageBoxImage.Warning)));
            return Task.CompletedTask;
        }

        // this is called when a new guild becomes available
        private void AddGuild(BotGuild gld)
            => this.Guilds.Add(gld);

        // this is called when a guild is no longer available
        private void RemoveGuild(ulong id)
        {
            var gld = this.Guilds.FirstOrDefault(xbg => xbg.Id == id);
            this.Guilds.Remove(gld);
        }

        // this is called to add a message to the banter box
        private void AddMessage(BotMessage msg)
        {
            this.Banter.Add(msg);
            this.SelectedMessage = msg;
            this.lbBanter.ScrollIntoView(msg);
        }

        private void OnPropertyChanged(params string[] props)
        {
            if (this.PropertyChanged != null)
                foreach (var prop in props)
                    this.PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
