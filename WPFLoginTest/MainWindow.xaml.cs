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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public enum ProfileMode
    {
        SelectUser,
        RemoveUser,
        AddUser
    }
    public partial class MainWindow : Window
    {
        private Random rnd = new();
        ProfileMode mode = ProfileMode.SelectUser;

        void UpdateUserColors(out UserCircle[] userCircles)
        {
            using (var fs = File.Open($"{AppContext.BaseDirectory}Userdata\\UserColors.txt", FileMode.OpenOrCreate))
            {
                StreamReader sr = new(fs);
                var text = sr.ReadToEnd();
                List<UserCircle> list = new();
                foreach (var t in text.Split("\n"))
                {
                    var arr = t.Split(new char[] { ' ' }, StringSplitOptions.None);
                    list.Add(new UserCircle(arr[0], (Color)ColorConverter.ConvertFromString(arr[1])));
                }
                userCircles = list.ToArray();
            };
        }

        public MainWindow()
        {
            InitializeComponent();
            //var usernames = GetUsernames();
            UpdateUserColors(out UserCircle[] userCircles);
            LoadUsers(userCircles);
        }

        public void LoadUsers(UserCircle[] users)
        {
            foreach (var user in users)
            {
                Label label = new();
                label.Content = user.Name;
                Border border = new();
                border.Height = 100;
                border.Width = 100;
                border.Margin = new Thickness(10);
                border.CornerRadius = new CornerRadius(50);
                border.Background = new SolidColorBrush(user.Background);
                VisualBrush brush = new();
                brush.Visual = border;
                border.Child = label;
                label.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                label.VerticalAlignment = VerticalAlignment.Center;
                label.HorizontalAlignment = HorizontalAlignment.Center;

                Profiles.Children.Add(border);

                border.MouseEnter += (sender, e) => Border_MouseEnter(border);
                border.MouseLeave += (sender, e) => Border_MouseLeave(border);
                border.MouseLeftButtonDown += Border_MouseLeftButtonDown;
            }
            /*Image img = new();
            img.Source = new BitmapImage("Images/PlusSign.png");
            AddProfile.Children.Add(button);*/

            Border plusBorder = new();
            plusBorder.Width = 30;
            plusBorder.Height = 30;
            plusBorder.Background = Brushes.Transparent;
            plusBorder.BorderBrush = Brushes.White;
            plusBorder.BorderThickness = new Thickness(2);
            plusBorder.CornerRadius = new CornerRadius(5);
            Profiles.Children.Add(plusBorder);
            plusBorder.Visibility = Visibility.Hidden;

            System.Windows.Shapes.Path plusPath = new();
            plusPath.Height = 13;
            plusPath.Width = 13;
            plusPath.Stroke = Brushes.White;
            plusPath.HorizontalAlignment = HorizontalAlignment.Center;
            plusPath.VerticalAlignment = VerticalAlignment.Center;
            plusPath.StrokeStartLineCap = PenLineCap.Square;
            plusPath.StrokeEndLineCap = PenLineCap.Square;
            plusPath.Stretch = Stretch.Uniform;
            plusPath.Margin = new Thickness(1);
            /*myPath.Data = new PathGeometry() { Figures = new PathFigure() { StartPoint = new Point(0, 100),  = new LineSegment() { Point = new Point(0, -100)}}};
            myPath.Data = new PathGeometry() { Figures = new PathFigure() { StartPoint = new LineSegment() { Point = -100, 0 } } };*/

            PathGeometry myPathGeometry = new();
            PathFigure myPathFigure = new();
            myPathFigure.StartPoint = new Point(0, 100);
            myPathFigure.Segments.Add(
                new LineSegment()
                {
                    Point = new Point(0,-100)
                });
            PathGeometry myPathGeometry2 = new();
            PathFigure myPathFigure2 = new();
            myPathFigure2.StartPoint = new Point(100, 0);
            myPathFigure2.Segments.Add(
                new LineSegment()
                {
                    Point = new Point(-100, 0)
                });
            myPathGeometry.Figures.Add(myPathFigure);
            myPathGeometry.Figures.Add(myPathFigure2);

            plusPath.Data = myPathGeometry;

            plusBorder.Child = plusPath;

            plusBorder.MouseEnter += (sender, e) => Border_MouseEnter(plusBorder);
            plusBorder.MouseLeave += (sender, e) => Border_MouseLeave(plusBorder, Brushes.White, new Thickness(2));
            //plusBorder.MouseLeftButtonDown += (sender, e) => Border2_MouseLeftButtonDown(plusBorder);
        }

        public void PathGeometry()
        {

        }

        public RadialGradientBrush RadialGradient()
        {
            RadialGradientBrush radialGradient = new();
            radialGradient.GradientOrigin = new Point(0, 0);
            radialGradient.Center = new Point(0.5, 0.5);
            radialGradient.RadiusX = 0.5;
            radialGradient.RadiusY = 0.5;
            radialGradient.GradientStops.Add(new GradientStop(GetColor(), 1));
            radialGradient.GradientStops.Add(new GradientStop(Color.FromArgb(225, 255, 255, 255), 0));
            radialGradient.Freeze();
            return radialGradient;
        }

        public string[] GetUsernames()
        {
            string[] fileArray = Directory.GetFiles($"{AppContext.BaseDirectory}Userdata\\");
            return fileArray.Select(f => System.IO.Path.GetFileName(f).Replace("-userdata.json", "")).ToArray();
        }

        private void Border_MouseEnter(Border border)
        {
            Mouse.OverrideCursor = Cursors.Hand;
            border.BorderBrush = Brushes.White;
            border.BorderThickness = new Thickness(3);
        }
        private void Border_MouseLeave(Border border, Brush borderBrush = null, Thickness borderThickness = default)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            border.BorderBrush = borderBrush;
            border.BorderThickness = borderThickness;
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SecondWindow secondWindow = new();
            this.Visibility = Visibility.Hidden;
            secondWindow.Show();
        }
        private void ManageUsersButton_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            //plusSign.Visibility = Visibility.Visible;
        }

        public Color GetColor()
        {
            List<Color> colors = new();
            colors.Add((Color)ColorConverter.ConvertFromString("#8F5190"));
            colors.Add((Color)ColorConverter.ConvertFromString("#6861AC"));
            colors.Add((Color)ColorConverter.ConvertFromString("#4170C8"));
            colors.Add((Color)ColorConverter.ConvertFromString("#219EA0"));
            colors.Add((Color)ColorConverter.ConvertFromString("#00CC77"));
            colors.Add((Color)ColorConverter.ConvertFromString("#75A742"));
            colors.Add((Color)ColorConverter.ConvertFromString("#E9820C"));
            colors.Add((Color)ColorConverter.ConvertFromString("#E45419"));
            colors.Add((Color)ColorConverter.ConvertFromString("#DE2626"));
            return colors.ElementAt(rnd.Next(0, 8));
        }
    }
}
