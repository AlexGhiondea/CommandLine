using OutputColorizer;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommandLine.Tests
{
    internal class TestWriter : IOutputWriter
    {
        private ConsoleColor _currentColor;

        public List<TextAndColor> Segments = new List<TextAndColor>();

        public ConsoleColor ForegroundColor
        {
            get
            {
                return _currentColor;
            }

            set
            {
                _currentColor = value;
            }
        }

        public void Write(string text)
        {
            if (text != string.Empty)
                Segments.Add(new TextAndColor(_currentColor, text));
        }

        public void WriteLine(string text)
        {
            if (text != string.Empty)
                Segments.Add(new TextAndColor(_currentColor, text));
        }

        public void Reset()
        {
            Segments.Clear();
        }

        public string ToTestCode()
        {
            StringBuilder code = new StringBuilder();

            code.AppendLine("Validate(");

            foreach (var item in Segments)
            {
                code.AppendLine($"{item.ToTestCode()}, ");
            }

            code = code.Remove(code.Length - 4, 2); //remove the last ", "

            code.AppendLine(");");

            return code.ToString();
        }
    }

    class TextAndColor
    {
        public ConsoleColor Color { get; set; }
        public string Text { get; set; }

        public TextAndColor(ConsoleColor color, string text)
        {
            Color = color;
            Text = text;
        }

        public override int GetHashCode()
        {
            return Color.GetHashCode() ^ Text.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TextAndColor))
                return false;

            TextAndColor other = obj as TextAndColor;
            return other.Color == Color && other.Text == Text;
        }

        public override string ToString()
        {
            return $"Text={Text}, Color={Color}";
        }

        public string ToTestCode()
        {
            //generate the test code for this particular element

            return $"new TextAndColor(ConsoleColor.{Color}, \"{Text}\")";
        }
    }
}