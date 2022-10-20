using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
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
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        private void Registration_Click(object sender, RoutedEventArgs e)
        {
            Registration RegistrationWindow = new Registration();
            RegistrationWindow.Show();
            this.Hide();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidateInputs() == false)
                {
                    return;
                }

                    Globals.CurrentUser = Globals.DbContextAutoGen.Users.Where(Users => Users.Username == TbxUsername.Text).FirstOrDefault();

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
                        MainWindow mainWindow = new MainWindow();
                        //this will open your child window
                        mainWindow.Show();
                        //this will close parent window. Registration window in this case
                        this.Close();
                }
                    else
                    {
                        MessageBox.Show(this, "Wrong username or password!", "Input error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
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
                TbxUsername.BorderBrush = Brushes.Red;
                throw new ArgumentException("Username must be 2-20 characters long, no semicolons");
            }
            else
            {
                TbxUsername.BorderBrush = Brushes.Black;
            }

            if (PbxPass.Password == "")
            {
                PbxPass.BorderBrush = Brushes.Red;
                throw new ArgumentException("Password field is empty");
            }
            else
            {
                PbxPass.BorderBrush = Brushes.Black;
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

        private static System.Drawing.Image ConvertImageSourceToImage(ImageSource image)
        {
            if (image == null) return null;

            MemoryStream memoryStream = new MemoryStream();
            BmpBitmapEncoder bmpBitmapEncoder = new BmpBitmapEncoder();
            bmpBitmapEncoder.Frames.Add(BitmapFrame.Create((BitmapSource)image));
            bmpBitmapEncoder.Save(memoryStream);

            return System.Drawing.Image.FromStream(memoryStream);
        }
    }
}
