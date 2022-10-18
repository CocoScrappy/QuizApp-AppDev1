using Microsoft.Win32;
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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }

        //private QuizAppProjectEntities1 quizAppProjectEntities;

        private void RegisterBtn_Click(object sender, RoutedEventArgs e)
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
                    System.Drawing.Image AvatarImg = ConvertImageSourceToImage(userAvatar.Source);
                    byte[] bits = ImageToByteArray(AvatarImg);
                    Image avatar = new Image { Image1 = bits };

                    using (QuizAppProjectEntities1 context = new QuizAppProjectEntities1())
                    {
                        context.Images.Add(avatar); // adds the image to the DbSet in memory
                        context.SaveChanges(); // commits the changes to the database
                        Image imgMostRecent = context.Images.Where(Images => Images.Image1 == bits).First();
                        //stud.studentID = u.ID; can just get the id of most recent image this way 
                        imgId = imgMostRecent.Id;
                    }

                }
                string myPassword = Pass.Password;
                string mySalt = BCrypt.Net.BCrypt.GenerateSalt();
                string myHash = BCrypt.Net.BCrypt.HashPassword(myPassword, mySalt);

                if (imgId != -1)
                {
                    User newUser = new User { Email = Email.Text, Username = Username.Text, Password = myHash, Score = 0, MaxScore = 0, ImgId = imgId }; // ArgumentException
                    //using (Globals.DbContextAutoGen)
                    using (QuizAppProjectEntities1 context = new QuizAppProjectEntities1())
                    {
                        context.Users.Add(newUser); // adds the image to the DbSet in memory
                        context.SaveChanges(); // commits the changes to the database
                    }
                }
                else
                {
                    User newUser = new User { Email = Email.Text, Username = Username.Text, Password = myHash, Score = 0, MaxScore = 0 }; // ArgumentException
                    //using (Globals.DbContextAutoGen)
                    using (QuizAppProjectEntities1 context = new QuizAppProjectEntities1())
                    {
                        context.Users.Add(newUser); // adds the image to the DbSet in memory
                        context.SaveChanges(); // commits the changes to the database
                    }
                }

                //clear inputs
                ResetRegistrationFields();

                Login login = new Login();
                //this will open your child window
                login.Show();
                //this will close parent window. Registration window in this case
                this.Close();

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
                throw new ArgumentException("One or both of the password fields empty");
            }

            if (ConfirmPass.Password != Pass.Password)
            {
                Pass.Password = "";
                ConfirmPass.Password = "";
                throw new ArgumentException("Passwords do not match");
            }

            return true;
        }

        public byte[] ImageToByteArray(System.Drawing.Image images)
        {
            using (var _memorystream = new MemoryStream())
            {
                images.Save(_memorystream, images.RawFormat);
                return _memorystream.ToArray();
            }
        }

        public static System.Drawing.Image ConvertImageSourceToImage(ImageSource image)
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
