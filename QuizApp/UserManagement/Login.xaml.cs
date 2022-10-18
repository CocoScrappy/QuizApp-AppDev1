using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media.Imaging;

namespace QuizApp.UserManagement
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidateInputs() == false)
                {
                    return;
                }

                using (QuizAppProjectEntities1 context = new QuizAppProjectEntities1())
                {
                    string userHash = Globals.DbContextAutoGen.Users.Where(Users => Users.Username == Username.Text).Single().Password;
                    if (userHash == null)
                    {
                        MessageBox.Show(this, "Wrong username or password!", "Input error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    string userPassword = Pass.Password;
                    bool doesPasswordMatch = BCrypt.Net.BCrypt.Verify(userPassword, userHash);

                    if (doesPasswordMatch)
                    {
                        Globals.CurrentUser = Globals.DbContextAutoGen.Users.Where(Users => Users.Username == Username.Text).Single();

                        if (Globals.CurrentUser.ImgId != null)
                        {

                            byte[] ImgBytes = Globals.DbContextAutoGen.Images.Where(Images => Images.Id == Globals.CurrentUser.ImgId).Single().Image1;
                            userAvatar.Source = ToImage(ImgBytes);
                            UsernameLbl.Content = Globals.CurrentUser.Username;
                        }
                        else
                        {
                            userAvatar.Source = new BitmapImage(new Uri("pack://application:,,,/images/ProfileDefaultPic.JPG"));
                            UsernameLbl.Content = Globals.CurrentUser.Username;
                        }
                    }
                    else
                    {
                        Pass.Password = "";
                        MessageBox.Show(this, "Wrong username or password!", "Input error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(this, ex.Message, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            catch (NullReferenceException ex)
            {
                MessageBox.Show(this, "No user with such username exists!\n" + ex.Message, "Input error",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }

            catch (SystemException ex)
            {
                MessageBox.Show(this, "Error reading from database\n" + ex.Message, "Database error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private bool ValidateInputs()
        {
            if (!Regex.IsMatch(Username.Text, @"^[^;]{2,20}$")) //, RegexOptions.IgnoreCase))
            {
                Username.Text = "";
                throw new ArgumentException("Username must be 2-20 characters long, no semicolons");
            }

            if (Pass.Password == "")
            {
                throw new ArgumentException("Password field is empty");
            }

            return true;
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
