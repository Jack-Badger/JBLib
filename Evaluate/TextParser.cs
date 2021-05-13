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
    using System.Linq;

    public class TextParser
    {
        private string text;

        private int pos;

        public string Text
            => text;

        public int Position
            => pos;

        public int Remaining
            => text.Length - pos;

        private static readonly char NullChar = (char)0;

        public TextParser()
            => Reset(null);

        public TextParser(string text)
            => Reset(text);

        public void Reset() => pos = 0;

        public void Reset(string text)
        {
            this.text = text ?? string.Empty;
            pos = 0;
        }

        public bool EndOfText
            => (pos >= text.Length);

        public char Peek(int ahead = 0)
        {
            int newPos = pos + ahead;

            return newPos < text.Length ? text[newPos] : NullChar;
        }

        public string Extract(int start)
            => Extract(start, text.Length);

        public string Extract(int start, int end)
            => text.Substring(start, end - start);

        public void MoveAhead()
            => MoveAhead(1);

        public void MoveAhead(int ahead)
            => pos = Math.Min(pos + ahead, text.Length);

        public void MoveTo(string s, bool ignoreCase = false)
        {
            pos = text.IndexOf(s, pos, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);

            if (pos < 0)
            {
                pos = text.Length;
            }
        }

        public void MoveTo(char c)
        {
            pos = text.IndexOf(c, pos);
            if (pos < 0)
            {
                pos = text.Length;
            }
        }

        public void MoveTo(char[] chars)
        {
            pos = text.IndexOfAny(chars, pos);
            if (pos < 0)
            {
                pos = text.Length;
            }
        }

        // is this a bug? infinite loop ever?
        public void MovePast(char[] chars)
        {
            while (chars.Contains(Peek()))
            {
                MoveAhead();
            }
        }

        public void MoveToEndOfLine()
        {
            char c = Peek();
            while (c != '\r' && c != '\n' && !EndOfText)
            {
                MoveAhead();
                c = Peek();
            }
        }

        public void MovePastWhitespace()
        {
            while (char.IsWhiteSpace(Peek()))
            {
                MoveAhead();
            }
        }
    }
}
