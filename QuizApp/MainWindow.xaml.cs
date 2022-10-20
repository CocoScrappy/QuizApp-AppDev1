// using Newtonsoft.Json;
// using Newtonsoft.Json.Linq;
using QuizApp.UserManagement;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media.Imaging;


namespace QuizApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            if (Globals.CurrentUser == null) 
            {
                // user is Logged out - close up controls, display Logoin/Register buttons and Avatar
                BtnLogin.Visibility = Visibility.Visible;
                BtnLogout.Visibility = Visibility.Collapsed;
                StackPnlMenu.Visibility = Visibility.Hidden;
            }
            else
            {
                // user is Logged in - open up controls, display Logout button and hide Avatar
                CommunityQuizzesRect.Visibility = Visibility.Hidden;
                StackPnlMenu.Visibility = Visibility.Visible;


                BtnLogin.Visibility = Visibility.Collapsed;
                BtnLogout.Visibility = Visibility.Visible;

                if (Globals.CurrentUser.ImgId == null)
                {
                    userAvatar.Source = new BitmapImage(new Uri("pack://application:,,,/images/ProfileDefaultPic.JPG"));
                }
                else
                {
                    byte[] ImgBytes = Globals.CurrentUser.Image.Image1; //Globals.DbContextAutoGen.Images.Where(Images => Images.Id == Globals.CurrentUser.ImgId).Single().Image1; //
                    userAvatar.Source = ToImage(ImgBytes);
                }

            }

        }

        // Allows user to move the window around the screen
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }

        }

        private void Registration_Click(object sender, RoutedEventArgs e)
        {
            Registration RegistrationWindow = new Registration();
            RegistrationWindow.Show();
            this.Hide();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Login LoginWindow = new Login();
            LoginWindow.Show();
            this.Hide();
        }

        private void MinimizeButton_MouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.WindowState = WindowState.Minimized;
            }
            catch (XamlParseException ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void CloseButton_MouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            Globals.CurrentUser = null;
            this.Close();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            Globals.CurrentUser = null;
            this.Close();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }

        public BitmapImage ToImage(byte[] array)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = new MemoryStream(array);
            image.EndInit();
            return image;
        }
    }
}