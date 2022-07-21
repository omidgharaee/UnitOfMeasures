using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitOfMeasures.Core.Domain.Services;

namespace UnitOfMeasures.Core.Application.Services
{
    public class MathParser : IMathParser
    {
        private const string NumberMaker = "#";
        private const string OperatorMarker = "$";


        private const string Plus = OperatorMarker + "+";
        private const string Minus = OperatorMarker + "-";
        private const string Multiply = OperatorMarker + "*";
        private const string Divide = OperatorMarker + "/";
        private const string LeftParent = OperatorMarker + "(";
        private const string RightParent = OperatorMarker + ")";


        private readonly Dictionary<string, string> supportedOperators =
            new Dictionary<string, string>
            {
                { "+", Plus },
                { "-", Minus },
                { "*", Multiply },
                { "/", Divide },
                { "(", LeftParent },
                { ")", RightParent }
            };

        private readonly char decimalSeparator;

        public MathParser()
        {
            try
            {
                decimalSeparator = Char.Parse(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            }
            catch (FormatException ex)
            {
                throw new FormatException("Error: can't read char decimal separator from system, check your regional settings.", ex);
            }
        }


        public double Parse(string expression)
        {
            try
            {
                return Calculate(ConvertToRPN(FormatString(expression)));
            }
            catch (DivideByZeroException e)
            {
                throw e;
            }
            catch (FormatException e)
            {
                throw e;
            }
            catch (InvalidOperationException e)
            {
                throw e;
            }
            catch (ArgumentOutOfRangeException e)
            {
                throw e;
            }
            catch (ArgumentException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        private string FormatString(string expression)
        {
            if (string.IsNullOrEmpty(expression))
            {
                throw new ArgumentNullException("Expression is null or empty");
            }

            StringBuilder formattedString = new StringBuilder();
            int balanceOfParenth = 0;

            for (int i = 0; i < expression.Length; i++)
            {
                char ch = expression[i];

                if (ch == '(')
                {
                    balanceOfParenth++;
                }
                else if (ch == ')')
                {
                    balanceOfParenth--;
                }

                if (Char.IsWhiteSpace(ch))
                {
                    continue;
                }
                else if (Char.IsUpper(ch))
                {
                    formattedString.Append(Char.ToLower(ch));
                }
                else
                {
                    formattedString.Append(ch);
                }
            }

            if (balanceOfParenth != 0)
            {
                throw new FormatException("Number of left and right parenthesis is not equal");
            }

            return formattedString.ToString();
        }

        private string ConvertToRPN(string expression)
        {
            int pos = 0;
            StringBuilder outputString = new StringBuilder();
            Stack<string> stack = new Stack<string>();

            while (pos < expression.Length)
            {
                string token = LexicalAnalysisInfixNotation(expression, ref pos);

                outputString = SyntaxAnalysisInfixNotation(token, outputString, stack);
            }

            while (stack.Count > 0)
            {
                if (stack.Peek()[0] == OperatorMarker[0])
                {
                    outputString.Append(stack.Pop());
                }
                else
                {
                    throw new FormatException("Format exception,"
                    + " there is function without parenthesis");
                }
            }

            return outputString.ToString();
        }

        private string LexicalAnalysisInfixNotation(string expression, ref int pos)
        {
            StringBuilder token = new StringBuilder();
            token.Append(expression[pos]);

            if (supportedOperators.ContainsKey(token.ToString()))
            {
                bool isUnary = pos == 0 || expression[pos - 1] == '(';
                pos++;


                return supportedOperators[token.ToString()];

            }
            else if (Char.IsDigit(token[0]) || token[0] == decimalSeparator)
            {
                if (Char.IsDigit(token[0]))
                {
                    while (++pos < expression.Length
                    && Char.IsDigit(expression[pos]))
                    {
                        token.Append(expression[pos]);
                    }
                }
                else
                {
                    token.Clear();
                }

                if (pos < expression.Length
          && expression[pos] == decimalSeparator)
                {
                    // Add current system specific decimal separator
                    token.Append(CultureInfo.CurrentCulture
                        .NumberFormat.NumberDecimalSeparator);

                    while (++pos < expression.Length
                    && Char.IsDigit(expression[pos]))
                    {
                        token.Append(expression[pos]);
                    }
                }

                return NumberMaker + token.ToString();
            }
            else
            {
                throw new ArgumentException("Unknown token in expression");
            }
        }

        private StringBuilder SyntaxAnalysisInfixNotation(string token, StringBuilder outputString, Stack<string> stack)
        {
            if (token[0] == NumberMaker[0])
            {
                outputString.Append(token);
            }
            else if (token == LeftParent)
            {
                stack.Push(token);
            }
            else if (token == RightParent)
            {
                string elem;
                while ((elem = stack.Pop()) != LeftParent)
                {
                    outputString.Append(elem);
                }

            }
            else
            {


                stack.Push(token);
            }

            return outputString;
        }


        private double Calculate(string expression)
        {
            int pos = 0;
            var stack = new Stack<double>();

            while (pos < expression.Length)
            {
                string token = LexicalAnalysisRPN(expression, ref pos);

                stack = SyntaxAnalysisRPN(stack, token);
            }

            if (stack.Count > 1)
            {
                throw new ArgumentException("Excess operand");
            }

            return stack.Pop();
        }

        private string LexicalAnalysisRPN(string expression, ref int pos)
        {
            StringBuilder token = new StringBuilder();


            token.Append(expression[pos++]);

            while (pos < expression.Length && expression[pos] != NumberMaker[0]
                && expression[pos] != OperatorMarker[0])
            {
                token.Append(expression[pos++]);
            }

            return token.ToString();
        }

        private Stack<double> SyntaxAnalysisRPN(Stack<double> stack, string token)
        {
            if (token[0] == NumberMaker[0])
            {
                stack.Push(double.Parse(token.Remove(0, 1)));
            }

            else if (NumberOfArguments(token) == 1)
            {
                double arg2 = stack.Pop();
                double arg1 = stack.Pop();

                double rst;

                switch (token)
                {
                    case Plus:
                        rst = arg1 + arg2;
                        break;
                    case Minus:
                        rst = arg1 - arg2;
                        break;
                    case Multiply:
                        rst = arg1 * arg2;
                        break;
                    case Divide:
                        if (arg2 == 0)
                        {
                            throw new DivideByZeroException("Second argument is zero");
                        }
                        rst = arg1 / arg2;
                        break;
                        rst = Math.Log(arg2, arg1);
                        break;
                    default:
                        throw new ArgumentException("Unknown operator");
                }

                stack.Push(rst);
            }


            return stack;
        }


        private int NumberOfArguments(string token)
        {
            switch (token)
            {
                case Plus:
                case Minus:
                case Multiply:
                case Divide:
                    return 1;
                default:
                    throw new ArgumentException("Unknown operator");
            }
        }
    }
}
