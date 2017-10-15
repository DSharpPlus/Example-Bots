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
// This is a WinForms example. It shows how to use WinForms without deadlocks.

using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace DSPlus.Examples
{
    // the class holding the form logic
    public partial class FormBot : Form
    {
        // this will hold the thread on which the bot will run
        private Task BotThread { get; set; }

        // this will hold the bot itself
        private Bot Bot { get; set; }

        // this will hold a token required to make the bot quit cleanly
        private CancellationTokenSource TokenSource { get; set; }

        // these are for UI state
        private BotGuild SelectedGuild { get; set; }
        private BotChannel SelectedChannel { get; set; }

        public FormBot()
        {
            InitializeComponent();
        }

        // this occurs when user selects a guild from the guild box
        private void lbGuilds_SelectedValueChanged(object sender, EventArgs e)
        {
            // check if the item is a guild, if not, don't do anything
            if (!(lbGuilds.SelectedItem is BotGuild bg))
                return;

            // set the UI state
            this.SelectedGuild = bg;
            this.SelectedChannel = default(BotChannel);

            // clear the channel and message lists
            this.lbChannels.ClearSelected();
            this.lbChannels.Items.Clear();
            this.lbBanter.ClearSelected();
            this.lbBanter.Items.Clear();

            // populate the channel list
            var chns = bg.Guild.Channels
                .Where(xc => xc.Type == ChannelType.Text)
                .OrderBy(xc => xc.Position)
                .Select(xc => new BotChannel(xc));
            this.lbChannels.Items.AddRange(chns.Cast<object>().ToArray());
        }

        // this occurs when user selects a channel from the channel box
        private void lbChannels_SelectedValueChanged(object sender, EventArgs e)
        {
            // check if the item is a channel, if not, don't do anything
            if (!(lbChannels.SelectedItem is BotChannel bc))
                return;

            // set the UI state
            this.SelectedChannel = bc;
            
            // clear the message list
            this.lbBanter.ClearSelected();
            this.lbBanter.Items.Clear();
        }

        // this occurs when user presses the send message button
        private void btMsgSend_Click(object sender, EventArgs e)
            => this.SendMessage();

        // this occurs when user presses a button inside the message
        // text box, we use that to handle enter key press
        private void tbMsg_KeyPress(object sender, KeyPressEventArgs e)
        {
            // check if the key pressed was enter
            if (e.KeyChar == (char)Keys.Return)
            {
                // if yes, mark the event as handled, and send
                // the message
                e.Handled = true;
                this.SendMessage();
            }
        }

        // this occurs when user presses the start/stop button
        private void btBotctl_Click(object sender, EventArgs e)
        {
            // lock the controls until they can be used again
            this.btBotctl.Enabled = false;
            this.tbMsg.Enabled = false;
            this.btMsgSend.Enabled = false;

            // check if a bot thread is running
            if (this.BotThread == null)
            {
                // start the bot

                // change the button's text to indicate it now 
                // stops the bot instead
                this.btBotctl.Text = "Stop the bot";
                
                // create the bot container
                this.Bot = new Bot(this.tbToken.Text);

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
                this.btBotctl.Text = "Start the bot";

                // request cancelling the task preventing the 
                // bot from stopping
                // this will effectively stop the bot
                this.TokenSource.Cancel();
            }

            // clear the token text box, we don't need it anymore
            this.tbToken.Text = "";
        }

        // this is called by the send button and message textbox 
        // key press handler
        private void SendMessage()
        {
            // check if we have a channel selected, if not, do 
            // nothing
            if (this.SelectedChannel.Channel == null)
                return;
            
            // check if a message was typed in at all, if not,
            // do nothing
            if (string.IsNullOrWhiteSpace(this.tbMsg.Text))
                return;

            // start an asynchronous task which will send the 
            // message, and once it's done, set the message 
            // textbox's text to empty using the UI thread
            _ = Task.Run(() => this.BotSendMessageCallback(this.tbMsg.Text, this.SelectedChannel))
                .ContinueWith(t => this.tbMsg.SetProperty(x => x.Text, ""));
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
            this.btBotctl.SetProperty(x => x.Enabled, true);
            this.btMsgSend.SetProperty(x => x.Enabled, true);
            this.tbMsg.SetProperty(x => x.Enabled, true);

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
            this.btBotctl.SetProperty(x => x.Enabled, true);
            this.btMsgSend.SetProperty(x => x.Enabled, false);
            this.tbMsg.SetProperty(x => x.Enabled, false);
            this.SetProperty(x => x.Text, "Example WinForms Bot");

            // furthermore, we need to clear the listboxes
            this.lbGuilds.InvokeAction(new Action(this.lbGuilds.Items.Clear));
            this.lbChannels.InvokeAction(new Action(this.lbChannels.Items.Clear));
            this.lbBanter.InvokeAction(new Action(this.lbBanter.Items.Clear));

            // and reset the UI state
            this.SelectedGuild = default(BotGuild);
            this.SelectedChannel = default(BotChannel);

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
            this.SetProperty(xf => xf.Text, "Example WinForms Bot (connected)");
            return Task.CompletedTask;
        }

        // called when any of the bot's guilds becomes available
        private Task Bot_GuildAvailable(GuildCreateEventArgs e)
        {
            // add the guild to the bot's guild collection
            this.lbGuilds.InvokeAction(new Action<BotGuild>(this.AddGuild), new BotGuild(e.Guild));
            return Task.CompletedTask;
        }

        // called when any of the bot joins a guild
        private Task Bot_GuildCreated(GuildCreateEventArgs e)
        {
            // add the guild to the bot's guild collection
            this.lbGuilds.InvokeAction(new Action<BotGuild>(this.AddGuild), new BotGuild(e.Guild));
            return Task.CompletedTask;
        }

        // called when any of the bot's guilds becomes unavailable
        private Task Bot_GuildUnavailable(GuildDeleteEventArgs e)
        {
            // remove the guild from the bot's guild collection
            this.lbGuilds.InvokeAction(new Action<ulong>(this.RemoveGuild), e.Guild.Id);
            return Task.CompletedTask;
        }

        // called when any of the bot leaves a guild
        private Task Bot_GuildDeleted(GuildDeleteEventArgs e)
        {
            // remove the guild from the bot's guild collection
            this.lbGuilds.InvokeAction(new Action<ulong>(this.RemoveGuild), e.Guild.Id);
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
            this.lbBanter.InvokeAction(new Action<BotMessage>(this.AddMessage), new BotMessage(e.Message));
            return Task.CompletedTask;
        }

        // called when an unhandled exception occurs in any of the 
        // event handlers
        private Task Bot_ClientErrored(ClientErrorEventArgs e)
        {
            // show a message box by dispatching it to the UI thread
            this.InvokeAction(new Action(() => MessageBox.Show(this, $"Exception in {e.EventName}: {e.Exception.ToString()}", "Unhandled exception in the bot", MessageBoxButtons.OK, MessageBoxIcon.Warning)));
            return Task.CompletedTask;
        }

        // this is called when a new guild becomes available
        private void AddGuild(BotGuild gld)
            => this.lbGuilds.Items.Add(gld);

        // this is called when a guild is no longer available
        private void RemoveGuild(ulong id)
        {
            var gld = this.lbGuilds.Items.OfType<BotGuild>().FirstOrDefault(xbg => xbg.Id == id);
            this.lbGuilds.Items.Remove(gld);
        }

        // this is called to add a message to the banter box
        private void AddMessage(BotMessage msg)
        {
            this.lbBanter.Items.Add(msg);
            this.lbBanter.SelectedItem = msg;
        }
    }
}
