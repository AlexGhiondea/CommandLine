using CommandLine.ColorScheme;
using OutputColorizer;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommandLine.Tests
{
    internal class TestWriter : IOutputWriter
    {
        public List<TextAndColor> Segments = new List<TextAndColor>();

        public ConsoleColor ForegroundColor { get; set; }

        public void Write(string text)
        {
            // try to guess the color it came from...
            string propertyName = GetColorPropertyNameForColor();

            if (text != string.Empty)
                Segments.Add(new TextAndColor(ForegroundColor, text, propertyName));
        }

        public void WriteLine(string text)
        {
            // try to guess the color it came from...
            string propertyName = GetColorPropertyNameForColor();

            if (text != string.Empty)
                Segments.Add(new TextAndColor(ForegroundColor, text, propertyName));
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

        private string GetColorPropertyNameForColor()
        {
            IColors currentColorScheme = Parser.Configuration.ColorScheme.Get();
            string propertyName = "";
            // check which color matches the current ForegroundColor.
            if (currentColorScheme.ArgumentGroupColor == ForegroundColor) propertyName = nameof(currentColorScheme.ArgumentGroupColor);
            if (currentColorScheme.ArgumentValueColor == ForegroundColor) propertyName = nameof(currentColorScheme.ArgumentValueColor);
            if (currentColorScheme.AssemblyNameColor == ForegroundColor) propertyName = nameof(currentColorScheme.AssemblyNameColor);
            if (currentColorScheme.ErrorColor == ForegroundColor) propertyName = nameof(currentColorScheme.ErrorColor);
            if (currentColorScheme.OptionalArgumentColor == ForegroundColor) propertyName = nameof(currentColorScheme.OptionalArgumentColor);
            if (currentColorScheme.RequiredArgumentColor == ForegroundColor) propertyName = nameof(currentColorScheme.RequiredArgumentColor);
            return propertyName;
        }
    }

    class TextAndColor
    {
        public ConsoleColor Color { get; set; }
        public string Text { get; set; }

        public string ColorPropertyName { get; set; }

        public TextAndColor(ConsoleColor color, string text)
            :this(color, text, string.Empty)
        {
        }

        public TextAndColor(ConsoleColor color, string text, string colorProperty)
        {
            Color = color;
            Text = text;
            ColorPropertyName = colorProperty;
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
            return $"Text={Text}, Color={ColorPropertyName}:{Color}";
        }

        public string ToTestCode()
        {
            //generate the test code for this particular element

            // if we have a color that is not the foreground color.
            if (!string.IsNullOrEmpty(ColorPropertyName))
            {
                return $"new TextAndColor(color.{ColorPropertyName}, \"{Text}\")";
            }
            
            // expect the foreground color.
            return $"new TextAndColor(_printer.ForegroundColor, \"{Text}\")";
        }
    }
}