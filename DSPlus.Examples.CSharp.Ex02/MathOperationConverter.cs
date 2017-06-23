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
