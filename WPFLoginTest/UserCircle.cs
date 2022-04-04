using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFLoginTest
{
    public class UserCircle
    {
        public string Name { get; set; }
        public Color Background { get; set; }
        public Border Border { get; }
        public Border DeleteBorder { get; }
        private readonly Canvas _canvas;
        
        public UserCircle(string name, Color background)
        {
            Name = name;
            Background = background;
            _canvas = new Canvas();
            Border = GetBorder();
            DeleteBorder = GetDeleteBorder();
        }
        private Border GetBorder()
        {
            Label label = new();
            label.Content = Name;
            Border border = new();
            border.Height = 100;
            border.Width = 100;
            border.Margin = new Thickness(10);
            border.CornerRadius = new CornerRadius(50);
            border.Background = new SolidColorBrush(Background);
            VisualBrush brush = new();
            brush.Visual = border;
            _canvas.Height = border.Height;
            _canvas.Width = border.Width;
            _canvas.Background = Brushes.Green;
            border.Child = _canvas;
            _canvas.Children.Add(label);
            label.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            label.VerticalAlignment = VerticalAlignment.Center;
            label.HorizontalAlignment = HorizontalAlignment.Center;
            Canvas.SetTop(label, (_canvas.Height - label.Height) / 2);
            Canvas.SetLeft(label, (_canvas.Width - label.Width) / 2);

            return border;
        }
        private Border GetDeleteBorder()
        {
            Label label = new();
            label.Content = "x";
            Border border = new();
            border.Height = 20;
            border.Width = 20;
            border.CornerRadius = new CornerRadius(50);
            border.Background = new SolidColorBrush(Colors.Red);
            border.BorderBrush = Brushes.White;
            border.BorderThickness = new Thickness(1);
            VisualBrush brush = new();
            brush.Visual = border;
            border.Child = label;
            _canvas.Children.Add(border);
            label.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            label.VerticalAlignment = VerticalAlignment.Center;
            label.HorizontalAlignment = HorizontalAlignment.Center;
            Canvas.SetTop(border, 0);
            Canvas.SetRight(border, 0);

            return border;
        }
    }
}
