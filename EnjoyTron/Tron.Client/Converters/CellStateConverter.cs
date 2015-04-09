﻿using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Tron.AdminClient.Converters
{
    public class CellStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string code = null;
            if (value is int)
                code = value.ToString();

            if (value is string)
                code = (string) value;

            switch (code)
            {
                case "#":
                    return new SolidColorBrush(Colors.Black);
                case ".":
                    return new SolidColorBrush(Colors.White);
                case "0":
                case "a":
                case "A":
                    return new SolidColorBrush(Colors.Red);
                case "1":
                case "b":
                case "B":
                    return new SolidColorBrush(Colors.Teal);
                case "2":
                case "c":
                case "C":
                    return new SolidColorBrush(Colors.Yellow);
                case "3":
                case "d":
                case "D":
                    return new SolidColorBrush(Colors.Green);
                case "4":
                case "e":
                case "E":
                    return new SolidColorBrush(Colors.Orange);
                case "5":
                case "f":
                case "F":
                    return new SolidColorBrush(Colors.BlueViolet);
                case "6":
                case "g":
                case "G":
                    return new SolidColorBrush(Colors.DodgerBlue);
                case "7":
                case "h":
                case "H":
                    return new SolidColorBrush(Colors.Violet);
                default:
                    throw new NotSupportedException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}