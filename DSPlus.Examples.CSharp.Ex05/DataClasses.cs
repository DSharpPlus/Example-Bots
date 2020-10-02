// THIS FILE IS A PART OF EMZI0767'S BOT EXAMPLES
//
// --------
// 
// Copyright 2019 Emzi0767
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

using DSharpPlus.Entities;

namespace DSPlus.Examples
{
    public struct BotGuild
    {
        public DiscordGuild Guild { get; }
        public ulong Id => this.Guild.Id;

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
