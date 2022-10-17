using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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


namespace QuizApp.UserManagement
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }


        private QuizAppProjectEntities quizAppProjectEntities;

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidateInputs() == false)
                {
                    return;
                }
                int imgId = -1; // default if image was not added
                if ((BitmapImage)userAvatar.Source != null)
                {
                    var bmp = userAvatar.Source as BitmapImage;

                    int height = bmp.PixelHeight;
                    int width = bmp.PixelWidth;
                    int stride = width * ((bmp.Format.BitsPerPixel + 7) / 8);

                    byte[] bits = new byte[height * stride];
                    bmp.CopyPixels(bits, stride, 0);
                    Image avatar = new Image { Image1 = bits }; // ArgumentException
                    using (var context = new QuizAppProjectEntities())
                    {
                        context.Images.Add(avatar); // adds the image to the DbSet in memory
                        context.SaveChanges(); // commits the changes to the database
                        imgId = context.Images.Where(Images => Images.Image1 == bits).Single().Id;
                    }
                }
                string myPassword = Pass.Password;
                string mySalt = BCrypt.Net.BCrypt.GenerateSalt();
                string myHash = BCrypt.Net.BCrypt.HashPassword(myPassword, mySalt);
                //bool doesPasswordMatch = BCrypt.Net.BCrypt.CheckPassword(myPassword, myHash);

                if (imgId != -1)
                {
                    User newUser = new User { Email = Email.Text, Username = Username.Text, Password = myHash, Score = 0, MaxScore = 0, ImgId = imgId }; // ArgumentException
                    using (var context = new QuizAppProjectEntities())
                    {
                        context.Users.Add(newUser); // adds the image to the DbSet in memory
                        context.SaveChanges(); // commits the changes to the database
                    }
                }
                else
                {
                    User newUser = new User { Email = Email.Text, Username = Username.Text, Password = myHash, Score = 0, MaxScore = 0 }; // ArgumentException
                    using (var context = new QuizAppProjectEntities())
                    {
                        context.Users.Add(newUser); // adds the image to the DbSet in memory
                        context.SaveChanges(); // commits the changes to the database
                    }
                }


                //clear inputs
                ResetRegistrationFields();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(this, ex.Message, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Error reading from database\n" + ex.Message, "Database error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                userAvatar.Source = new BitmapImage(new Uri(op.FileName));
            }
        }

        private void ResetRegistrationFields()
        {
            Email.Text = "";
            Username.Text = "";
            Pass.Password = "";
            ConfirmPass.Password = "";
            userAvatar.Source = null;
        }


        private bool ValidateInputs()
        {
            if (!Regex.IsMatch(Email.Text, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")) //, RegexOptions.IgnoreCase))
            {
                Email.Text = "";
                throw new ArgumentException("Email must be a valid email up to 100 characters");
            }

            if (!Regex.IsMatch(Username.Text, @"^[^;]{2,20}$")) //, RegexOptions.IgnoreCase))
            {
                Username.Text = "";
                throw new ArgumentException("Username must be 2-20 characters long, no semicolons");
            }

            if ((ConfirmPass.Password == "") || (Pass.Password == ""))
            {
                //MessageBox.Show(this, "Passwords do not match!", "Input error",
                //MessageBoxButton.OK, MessageBoxImage.Error);
                throw new ArgumentException("One or both of the password fields empty");
            }

            if (ConfirmPass.Password != Pass.Password)
            {
                //MessageBox.Show(this, "Passwords do not match!", "Input error",
                //MessageBoxButton.OK, MessageBoxImage.Error);
                Pass.Password = "";
                ConfirmPass.Password = "";
                throw new ArgumentException("Passwords do not match");
            }

            return true;
        }
    }
}
