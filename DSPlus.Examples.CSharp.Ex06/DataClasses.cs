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

using DSharpPlus;
using DSharpPlus.Entities;

namespace DSPlus.Examples
{
    public struct BotGuild
    {
        public DiscordGuild Guild { get; }
        public ulong Id => this.Guild.Id;
        public string Name => this.Guild.Name;
        public string IconUrl => $"{this.Guild.IconUrl}?size=32";

        public BotGuild(DiscordGuild gld)
        {
            this.Guild = gld;
        }

        public override string ToString()
        {
            return this.Guild.Name;
        }
    }

    public struct BotChannel
    {
        public DiscordChannel Channel { get; }
        public ulong Id => this.Channel.Id;
        public string Name => this.Channel.Name;

        public BotChannel(DiscordChannel chn)
        {
            this.Channel = chn;
        }

        public override string ToString()
        {
            return $"#{this.Channel.Name}";
        }
    }

    public struct BotMessage
    {
        public DiscordMessage Message { get; }
        public ulong Id => this.Message.Id;
        public string AuthorName => this.Message.Author.Username;
        public string AuthorAvatarUrl => this.Message.Author.GetAvatarUrl(ImageFormat.Png, 32);
        public string Content => this.Message.Content;

        public BotMessage(DiscordMessage msg)
        {
            this.Message = msg;
        }

        public override string ToString()
        {
            return this.Message.Content;
        }
    }
}
