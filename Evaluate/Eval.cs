//////////////////////////////////////////////////////////////////////////////
// This source code and all associated files and resources are copyrighted by
// the author(s). This source code and all associated files and resources may
// be used as long as they are used according to the terms and conditions set
// forth in The Code Project Open License (CPOL), which may be viewed at
// http://www.blackbeltcoder.com/Legal/Licenses/CPOL.
//
// Copyright (c) 2010 Jonathan Wood
//

namespace JBLib.Evaluate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Expression evaluator class.
    /// </summary>
    public class Eval
    {
        // Event handers
        public delegate void ProcessSymbolHandler(object sender, SymbolEventArgs e);

        public delegate void ProcessFunctionHandler(object sender, FunctionEventArgs e);

        public event ProcessSymbolHandler ProcessSymbol;

        public event ProcessFunctionHandler ProcessFunction;

        // Token state enums
        protected enum State
        {
            None = 0,
            Operand = 1,
            Operator = 2,
            UnaryOperator = 3,
        }

        // Error messages
        private string errInvalidOperand = "Invalid operand";
        private string errOperandExpected = "Operand expected";
        private string errOperatorExpected = "Operator expected";
        private string errUnmatchedClosingParen = "Closing parenthesis without matching open parenthesis";
        private string errMultipleDecimalPoints = "Operand contains multiple decimal points";
        private string errUnexpectedCharacter = "Unexpected character encountered";
        private string errUndefinedSymbol = "Undefined symbol";
        private string errUndefinedFunction = "Undefined function";
        private string errClosingParenExpected = "Closing parenthesis expected";
        private string errWrongParamCount = "Wrong number of function parameters";

        // To distinguish it from a minus operator,
        // we'll use a character unlikely to appear
        // in expressions to signify a unary negative
        protected const string UnaryMinus = "\x80";

        public Eval()
        {
        }

        /// <summary>
        /// Evaluates the given expression and returns the result.
        /// </summary>
        /// <param name="expression">The expression to evaluate.</param>
        /// <returns></returns>
        public double Execute(string expression)
        {
            return ExecuteTokens(TokenizeExpression(expression));
        }

        /// <summary>
        /// Converts a standard infix expression to list of tokens in
        /// postfix order.
        /// </summary>
        /// <param name="expression">Expression to evaluate.</param>
        /// <returns></returns>
        protected List<string> TokenizeExpression(string expression)
        {
            List<string> tokens = new List<string>();
            Stack<string> stack = new Stack<string>();
            State state = State.None;
            int parenCount = 0;
            string temp;

            TextParser parser = new TextParser(expression);

            while (!parser.EndOfText)
            {
                if (char.IsWhiteSpace(parser.Peek()))
                {
                    // Ignore spaces, tabs, etc.
                }
                else if (parser.Peek() == '(')
                {
                    // Cannot follow operand
                    if (state == State.Operand)
                    {
                        throw new EvalException(errOperatorExpected, parser.Position);
                    }

                    // Allow additional unary operators after "("
                    if (state == State.UnaryOperator)
                    {
                        state = State.Operator;
                    }

                    // Push opening parenthesis onto stack
                    stack.Push(parser.Peek().ToString());

                    // Track number of parentheses
                    parenCount++;
                }
                else if (parser.Peek() == ')')
                {
                    // Must follow operand
                    if (state != State.Operand)
                    {
                        throw new EvalException(errOperandExpected, parser.Position);
                    }

                    // Must have matching open parenthesis
                    if (parenCount == 0)
                    {
                        throw new EvalException(errUnmatchedClosingParen, parser.Position);
                    }

                    // Pop all operators until matching "(" found
                    temp = stack.Pop();
                    while (temp != "(")
                    {
                        tokens.Add(temp);
                        temp = stack.Pop();
                    }

                    // Track number of parentheses
                    parenCount--;
                }
                else if ("+-*/".Contains(parser.Peek()))
                {
                    // Need a bit of extra code to support unary operators
                    if (state == State.Operand)
                    {
                        // Pop operators with precedence >= current operator
                        int currPrecedence = GetPrecedence(parser.Peek().ToString());
                        while (stack.Count > 0 && GetPrecedence(stack.Peek()) >= currPrecedence)
                        {
                            tokens.Add(stack.Pop());
                        }

                        stack.Push(parser.Peek().ToString());
                        state = State.Operator;
                    }
                    else if (state == State.UnaryOperator)
                    {
                        // Don't allow two unary operators together
                        throw new EvalException(errOperandExpected, parser.Position);
                    }
                    else
                    {
                        // Test for unary operator
                        if (parser.Peek() == '-')
                        {
                            // Push unary minus
                            stack.Push(UnaryMinus);
                            state = State.UnaryOperator;
                        }
                        else if (parser.Peek() == '+')
                        {
                            // Just ignore unary plus
                            state = State.UnaryOperator;
                        }
                        else
                        {
                            throw new EvalException(errOperandExpected, parser.Position);
                        }
                    }
                }
                else if (char.IsDigit(parser.Peek()) || parser.Peek() == '.')
                {
                    if (state == State.Operand)
                    {
                        // Cannot follow other operand
                        throw new EvalException(errOperatorExpected, parser.Position);
                    }

                    // Parse number
                    temp = ParseNumberToken(parser);
                    tokens.Add(temp);
                    state = State.Operand;
                    continue;
                }
                else
                {
                    double result;

                    // Parse symbols and functions
                    if (state == State.Operand)
                    {
                        // Symbol or function cannot follow other operand
                        throw new EvalException(errOperatorExpected, parser.Position);
                    }

                    if (!(char.IsLetter(parser.Peek()) || parser.Peek() == '_'))
                    {
                        // Invalid character
                        throw new EvalException($"{errUnexpectedCharacter}, {parser.Peek()}", parser.Position);
                    }

                    // Save start of symbol for error reporting
                    int symbolPos = parser.Position;

                    // Parse this symbol
                    temp = ParseSymbolToken(parser);

                    // Skip whitespace
                    parser.MovePastWhitespace();

                    // Check for parameter list
                    if (parser.Peek() == '(')
                    {
                        // Found parameter list, evaluate function
                        result = EvaluateFunction(parser, temp, symbolPos);
                    }
                    else
                    {
                        // No parameter list, evaluate symbol (variable)
                        result = EvaluateSymbol(temp, symbolPos);
                    }

                    // Handle negative result
                    if (result < 0)
                    {
                        stack.Push(UnaryMinus);
                        result = Math.Abs(result);
                    }

                    tokens.Add(result.ToString());
                    state = State.Operand;
                    continue;
                }

                parser.MoveAhead();
            }

            // Expression cannot end with operator
            if (state == State.Operator || state == State.UnaryOperator)
            {
                throw new EvalException(errOperandExpected, parser.Position);
            }

            // Check for balanced parentheses
            if (parenCount > 0)
            {
                throw new EvalException(errClosingParenExpected, parser.Position);
            }

            // Retrieve remaining operators from stack
            while (stack.Count > 0)
            {
                tokens.Add(stack.Pop());
            }

            return tokens;
        }

        /// <summary>
        /// Parses and extracts a numeric value at the current position.
        /// </summary>
        /// <param name="parser">TextParser object.</param>
        /// <returns></returns>
        protected string ParseNumberToken(TextParser parser)
        {
            bool hasDecimal = false;
            int start = parser.Position;
            while (char.IsDigit(parser.Peek()) || parser.Peek() == '.')
            {
                if (parser.Peek() == '.')
                {
                    if (hasDecimal)
                    {
                        throw new EvalException(errMultipleDecimalPoints, parser.Position);
                    }

                    hasDecimal = true;
                }

                parser.MoveAhead();
            }

            // Extract token
            string token = parser.Extract(start, parser.Position);
            if (token == ".")
            {
                throw new EvalException(errInvalidOperand, parser.Position - 1);
            }

            return token;
        }

        /// <summary>
        /// Parses and extracts a symbol at the current position.
        /// </summary>
        /// <param name="parser">TextParser object.</param>
        /// <returns></returns>
        protected string ParseSymbolToken(TextParser parser)
        {
            int start = parser.Position;
            while (char.IsLetterOrDigit(parser.Peek()) || parser.Peek() == '_' || parser.Peek() == '@')
            {
                parser.MoveAhead();
            }

            return parser.Extract(start, parser.Position);
        }

        /// <summary>
        /// Evaluates a function and returns its value. It is assumed the current
        /// position is at the opening parenthesis of the argument list.
        /// </summary>
        /// <param name="parser">TextParser object.</param>
        /// <param name="name">Name of function.</param>
        /// <param name="pos">Position at start of function.</param>
        /// <returns></returns>
        protected double EvaluateFunction(TextParser parser, string name, int pos)
        {
            double result = default;

            // Parse function parameters
            List<double> parameters = ParseParameters(parser);

            // We found a function reference
            FunctionStatus status = FunctionStatus.UndefinedFunction;
            if (ProcessFunction != null)
            {
                FunctionEventArgs args = new FunctionEventArgs
                {
                    Name = name,
                    Parameters = parameters,
                    Result = result,
                    Status = FunctionStatus.OK,
                };
                ProcessFunction(this, args);
                result = args.Result;
                status = args.Status;
            }

            if (status == FunctionStatus.UndefinedFunction)
            {
                throw new EvalException($"Undefined Function {name}", pos);
            }

            if (status == FunctionStatus.WrongParameterCount)
            {
                throw new EvalException(errWrongParamCount, pos);
            }

            return result;
        }

        /// <summary>
        /// Evaluates each parameter of a function's parameter list and returns
        /// a list of those values. An empty list is returned if no parameters
        /// were found. It is assumed the current position is at the opening
        /// parenthesis of the argument list.
        /// </summary>
        /// <param name="parser">TextParser object.</param>
        /// <returns></returns>
        protected List<double> ParseParameters(TextParser parser)
        {
            // Move past open parenthesis
            parser.MoveAhead();

            // Look for function parameters
            List<double> parameters = new List<double>();
            parser.MovePastWhitespace();
            if (parser.Peek() != ')')
            {
                // Parse function parameter list
                int paramStart = parser.Position;
                int parenCount = 1;

                while (!parser.EndOfText)
                {
                    if (parser.Peek() == ',')
                    {
                        // Note: Ignore commas inside parentheses. They could be
                        // from a parameter list for a function inside the parameters
                        if (parenCount == 1)
                        {
                            parameters.Add(EvaluateParameter(parser, paramStart));
                            paramStart = parser.Position + 1;
                        }
                    }

                    if (parser.Peek() == ')')
                    {
                        parenCount--;
                        if (parenCount == 0)
                        {
                            parameters.Add(EvaluateParameter(parser, paramStart));
                            break;
                        }
                    }
                    else if (parser.Peek() == '(')
                    {
                        parenCount++;
                    }

                    parser.MoveAhead();
                }
            }

            // Make sure we found a closing parenthesis
            if (parser.Peek() != ')')
            {
                throw new EvalException(errClosingParenExpected, parser.Position);
            }

            // Move past closing parenthesis
            parser.MoveAhead();

            // Return parameter list
            return parameters;
        }

        /// <summary>
        /// Extracts and evaluates a function parameter and returns its value. If an
        /// exception occurs, it is caught and the column is adjusted to reflect the
        /// position in original string, and the exception is rethrown.
        /// </summary>
        /// <param name="parser">TextParser object.</param>
        /// <param name="paramStart">Column where this parameter started.</param>
        /// <returns></returns>
        protected double EvaluateParameter(TextParser parser, int paramStart)
        {
            try
            {
                // Extract expression and evaluate it
                string expression = parser.Extract(paramStart, parser.Position);
                return Execute(expression);
            }
            catch (EvalException ex)
            {
                // Adjust column and rethrow exception
                ex.Column += paramStart;
                throw ex;
            }
        }

        protected double EvaluateSymbol(string name, int pos)
        {
            double result = default;

            // We found a symbol reference
            SymbolStatus status = SymbolStatus.UndefinedSymbol;
            if (ProcessSymbol != null)
            {
                SymbolEventArgs args = new SymbolEventArgs
                {
                    Name = name,
                    Result = result,
                    Status = SymbolStatus.OK,
                };
                ProcessSymbol(this, args);
                result = args.Result;
                status = args.Status;
            }

            if (status == SymbolStatus.UndefinedSymbol)
            {
                throw new EvalException($"ErrUndefinedSymbol {name}", pos);
            }

            return result;
        }

        /// <summary>
        /// Evaluates the given list of tokens and returns the result.
        /// Tokens must appear in postfix order.
        /// </summary>
        /// <param name="tokens">List of tokens to evaluate.</param>
        /// <returns></returns>
        protected double ExecuteTokens(List<string> tokens)
        {
            Stack<double> stack = new Stack<double>();
            double tmp, tmp2;

            foreach (string token in tokens)
            {
                // Is this a value token?
                int count = token.Count(c => char.IsDigit(c) || c == '.');
                if (count == token.Length)
                {
                    stack.Push(double.Parse(token));
                }
                else if (token == "+")
                {
                    stack.Push(stack.Pop() + stack.Pop());
                }
                else if (token == "-")
                {
                    tmp = stack.Pop();
                    tmp2 = stack.Pop();
                    stack.Push(tmp2 - tmp);
                }
                else if (token == "*")
                {
                    stack.Push(stack.Pop() * stack.Pop());
                }
                else if (token == "/")
                {
                    tmp = stack.Pop();
                    tmp2 = stack.Pop();
                    stack.Push(tmp2 / tmp);
                }
                else if (token == UnaryMinus)
                {
                    stack.Push(-stack.Pop());
                }
            }

            // Remaining item on stack contains result
            return stack.Count > 0 ? stack.Pop() : 0.0;
        }

        /// <summary>
        /// Returns a value that indicates the relative precedence of
        /// the specified operator.
        /// </summary>
        /// <param name="s">Operator to be tested.</param>
        /// <returns></returns>
        protected int GetPrecedence(string s)
        {
            switch (s)
            {
                case "+":
                case "-":
                    return 1;
                case "*":
                case "/":
                    return 2;
                case "^":
                    return 3;
                case UnaryMinus:
                    return 10;
            }

            return 0;
        }
    }
}
