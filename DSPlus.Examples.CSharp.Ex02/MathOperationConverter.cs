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
// This is an interactivity example. It shows how to properly utilize 
// Interactivity module.

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;

namespace DSPlus.Examples
{
    public class MathOperationConverter : IArgumentConverter<MathOperation>
    {
        public bool TryConvert(string value, CommandContext ctx, out MathOperation result)
        {
            switch (value)
            {
                case "+":
                    result = MathOperation.Add;
                    return true;

                case "-":
                    result = MathOperation.Subtract;
                    return true;

                case "*":
                    result = MathOperation.Multiply;
                    return true;

                case "/":
                    result = MathOperation.Divide;
                    return true;

                case "%":
                    result = MathOperation.Modulo;
                    return true;
            }

            result = MathOperation.Add;
            return false;
        }
    }
}
