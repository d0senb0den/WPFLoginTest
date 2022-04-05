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
        DeleteUser,
        AddUser,
        EditUserName
    }
    public partial class MainWindow : Window
    {
        private Random rnd = new();
        ProfileMode mode = ProfileMode.SelectUser;
        Border plusBorder = new();
        List<UserCircle> userCircles = new List<UserCircle>();

        void LoadUserCircles()
        {
            using (var fs = File.Open($"{AppContext.BaseDirectory}Userdata\\UserColors.txt", FileMode.OpenOrCreate))
            {
                StreamReader sr = new(fs);
                var text = sr.ReadToEnd();
                foreach (var t in text.Split("\n"))
                {
                    var arr = t.Split(new char[] { ' ' }, StringSplitOptions.None);
                    userCircles.Add(new UserCircle(arr[0], (Color)ColorConverter.ConvertFromString(arr[1])));
                }
            };
        }

        public MainWindow()
        {
            InitializeComponent();
            this.Background = new ImageBrush(new BitmapImage(new Uri($"{AppContext.BaseDirectory}Images\\MysticalKeyboard.png")));
            LoadUserCircles();
            DrawUserCircles();
            InitializePlusBorder();
            EditBorder.MouseLeftButtonDown += (s, e) => OnEditButtonClicked();
            EditBorder.MouseEnter += (s, e) => Border_MouseEnter(EditBorder, Brushes.White, new Thickness(2));
            EditBorder.MouseLeave += (s, e) => Border_MouseLeave(EditBorder, Brushes.Gray, new Thickness(2));
        }
        public void InitializePlusBorder()
        {
            plusBorder.Width = 30;
            plusBorder.Height = 30;
            plusBorder.Background = Brushes.Transparent;
            plusBorder.BorderBrush = Brushes.White;
            plusBorder.BorderThickness = new Thickness(2);
            plusBorder.CornerRadius = new CornerRadius(5);
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
                    Point = new Point(0, -100)
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

            plusBorder.MouseEnter += (sender, e) => Border_MouseEnter(plusBorder, Brushes.White, new Thickness(3));
            plusBorder.MouseLeave += (sender, e) => Border_MouseLeave(plusBorder, Brushes.White, new Thickness(2));

            int i = Profiles.Children.Count;
            AddBorderToProfilesGrid(plusBorder, i);
        }
        public void DrawUserCircles()
        {

            for (int i = 0; i < userCircles.Count; i++)
            {
                UserCircle? user = userCircles[i];
                var border = user.Border;
                AddBorderToProfilesGrid(border, i);

                border.MouseEnter += (sender, e) => Border_MouseEnter(border, Brushes.White, new Thickness(3));
                border.MouseLeave += (sender, e) => Border_MouseLeave(border);
                border.MouseLeftButtonDown += Border_MouseLeftButtonDown;
            }
        }

        private void AddBorderToProfilesGrid(Border border, int i)
        {
            Grid.SetColumn(border, i % 4);
            Grid.SetRow(border, (i < 4 ? 0 : 1));
            Profiles.Children.Add(border);
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

        private void Border_MouseEnter(Border border, Brush borderBrush, Thickness borderThickness)
        {
            Mouse.OverrideCursor = Cursors.Hand;
            border.BorderBrush = borderBrush;
            border.BorderThickness = borderThickness;
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

        public void OnEditButtonClicked()
        {
            ChangeModeTo(ProfileMode.DeleteUser); //DEN DÄRA VARA EDIT
        }
        public void BackButtonClicked(object sender, RoutedEventArgs e)
        {
            ChangeModeTo(ProfileMode.SelectUser);
        }

        public bool ChangeModeTo(ProfileMode newMode)
        {
            if (mode == ProfileMode.SelectUser && newMode == ProfileMode.SelectUser)
                return false;

            return (mode, newMode) switch
            {
                (ProfileMode.SelectUser, ProfileMode.DeleteUser) => ChooseDeleteMode(),
                (ProfileMode.DeleteUser, ProfileMode.AddUser) => ChooseAddMode(),
                (ProfileMode.AddUser, ProfileMode.DeleteUser) => ChooseDeleteMode(),
                (ProfileMode.DeleteUser, ProfileMode.EditUserName) => ChooseEditUserName(),
                (ProfileMode.EditUserName, ProfileMode.DeleteUser) => ChooseDeleteMode(),
                (_, ProfileMode.SelectUser) => ChooseSelectMode(),
                _ => false
            };

            bool ChooseSelectMode()
            {
                HideBackButton();
                HideAddUserBorder();
                HideDeleteInterface();
                HideEditUserNameInterface();
                HideAddInterface();
                mode = ProfileMode.SelectUser;
                return true;
            }

            bool ChooseAddMode()
            {
                ShowBackButton();
                HideDeleteInterface();
                DeactivateAddUserBorder();
                ShowAddInterface();
                HideEditUserNameInterface();
                mode = ProfileMode.AddUser;
                return true;
            }

            bool ChooseDeleteMode()
            {
                ShowBackButton();
                ShowDeleteInterface();
                ShowAddUserBorder();
                HideAddInterface();
                HideEditUserNameInterface();
                mode = ProfileMode.DeleteUser;
                return true;
            }

            bool ChooseEditUserName()
            {
                ShowBackButton();
                HideDeleteInterface();
                DeactivateAddUserBorder();
                ShowEditUserNameInterface();
                mode = ProfileMode.EditUserName;
                return true;
            }

            void HideBackButton()
            {
                BackButton.Visibility = Visibility.Hidden;
            }
            void HideAddUserBorder()
            {
                plusBorder.Visibility = Visibility.Hidden;
            }
            void HideDeleteInterface()
            {
                foreach (var userCircle in userCircles)
                {
                    userCircle.DeleteBorder.Visibility = Visibility.Hidden;
                }
            }
            void HideEditUserNameInterface()
            {

            }
            void HideAddInterface()
            {

            }
            void DeactivateAddUserBorder()
            {

            }
            void ShowAddInterface()
            {

            }
            void ShowDeleteInterface()
            {
                foreach (var userCircle in userCircles)
                {
                    userCircle.DeleteBorder.Visibility = Visibility.Visible;
                }
            }
            void ShowAddUserBorder()
            {
                plusBorder.Visibility = Visibility.Visible;
            }
            void ShowEditUserNameInterface()
            {

            }
            void ShowBackButton()
            {
                BackButton.Visibility = Visibility.Visible;
            }
        }
    }
}
