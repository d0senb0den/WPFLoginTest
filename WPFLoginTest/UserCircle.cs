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
            Label userNameLabel = new();
            userNameLabel.Content = Name;
            Border border = new();
            border.Height = 100;
            border.Width = 100;
            border.Margin = new Thickness(10);
            border.CornerRadius = new CornerRadius(50);
            border.Background = new SolidColorBrush(Background);
            //VisualBrush brush = new();
            //brush.Visual = border;
            _canvas.Height = border.Height;
            _canvas.Width = border.Width;
            //_canvas.Background = Brushes.Green;
            border.Child = _canvas;
            _canvas.Children.Add(userNameLabel);
            userNameLabel.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            userNameLabel.VerticalAlignment = VerticalAlignment.Center;
            userNameLabel.HorizontalAlignment = HorizontalAlignment.Center;
            userNameLabel.SizeChanged += (s, e) =>
            {
                Canvas.SetTop(userNameLabel, (_canvas.Height - userNameLabel.RenderSize.Height) / 2);
                Canvas.SetLeft(userNameLabel, (_canvas.Width - userNameLabel.RenderSize.Width) / 2);
            };

            border.VerticalAlignment = VerticalAlignment.Center;
            border.HorizontalAlignment = HorizontalAlignment.Center;
            return border;
        }
        private Border GetDeleteBorder()
        {
            Label xLabel = new();
            xLabel.Content = "X";
            xLabel.FontWeight = FontWeights.Bold;
            xLabel.FontSize = 12;
            xLabel.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            xLabel.VerticalAlignment = VerticalAlignment.Center;
            xLabel.HorizontalAlignment = HorizontalAlignment.Center;

            Border deleteBorder = new();
            deleteBorder.Height = 30;
            deleteBorder.Width = 30;
            deleteBorder.CornerRadius = new CornerRadius(50);
            deleteBorder.Background = new SolidColorBrush(Colors.Red);
            deleteBorder.BorderBrush = Brushes.Black;
            deleteBorder.BorderThickness = new Thickness(1);
            deleteBorder.Child = xLabel;
            _canvas.Children.Add(deleteBorder);
            Canvas.SetTop(deleteBorder, 0);
            Canvas.SetRight(deleteBorder, 0);
            deleteBorder.Focusable = true;

            

            deleteBorder.Visibility = Visibility.Hidden;
            return deleteBorder;
        }
    }
}
