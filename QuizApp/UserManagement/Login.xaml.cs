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

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidateInputs() == false)
                {
                    return;
                }

                using (QuizAppProjectEntities1 context = new QuizAppProjectEntities1())
                {
                    Globals.CurrentUser = context.Users.Where(Users => Users.Username == TbxUsername.Text).FirstOrDefault();

                    if (Globals.CurrentUser == null)
                    {
                        MessageBox.Show(this, "Wrong username or password!", "Input error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    string userHash = Globals.CurrentUser.Password;
                    string userPassword = PbxPass.Password;
                    bool doesPasswordMatch = BCrypt.Net.BCrypt.Verify(userPassword, userHash);

                    if (doesPasswordMatch)
                    {

                        if (Globals.CurrentUser.ImgId != null)
                        {

                            byte[] ImgBytes = context.Images.Where(Images => Images.Id == Globals.CurrentUser.ImgId).Single().Image1;
                            userAvatar.Source = ToImage(ImgBytes);
                            LblUsername.Content = Globals.CurrentUser.Username;
                        }
                        else
                        {
                            userAvatar.Source = new BitmapImage(new Uri("pack://application:,,,/images/ProfileDefaultPic.JPG"));
                            LblUsername.Content = Globals.CurrentUser.Username;
                        }
                    }
                    else
                    {
                        PbxPass.Password = "";
                        TbxUsername.BorderBrush = System.Windows.Media.Brushes.Red;
                        PbxPass.BorderBrush = System.Windows.Media.Brushes.Red;
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
            if (!Regex.IsMatch(TbxUsername.Text, @"^[^;]{2,20}$")) //, RegexOptions.IgnoreCase))
            {
                TbxUsername.Text = "";
                TbxUsername.BorderBrush = System.Windows.Media.Brushes.Red;
                throw new ArgumentException("Username must be 2-20 characters long, no semicolons");
            }

            if (PbxPass.Password == "")
            {
                PbxPass.BorderBrush = System.Windows.Media.Brushes.Red;
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
