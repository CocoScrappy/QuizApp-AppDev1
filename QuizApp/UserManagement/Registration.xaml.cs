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

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidateInputs() == false)
                {
                    return;
                }

                if (UsernameExists() != null) {
                    MessageBox.Show(this, "Username already exists", "Input error", MessageBoxButton.OK, MessageBoxImage.Information);
                    TbxUsername.Text = "";
                    return;
                }

                if (EmailExists() != null)
                {
                    MessageBox.Show(this, "Email already exists", "Input error", MessageBoxButton.OK, MessageBoxImage.Information);
                    TbxEmail.Text = "";
                    return;
                }


                int imgId = -1; // default if image was not added
                if ((BitmapImage)userAvatar.Source != null)
                {
                    System.Drawing.Image AvatarImg = ConvertImageSourceToImage(userAvatar.Source);
                    byte[] bits = ImageToByteArray(AvatarImg);
                    Image avatar = new Image { Image1 = bits };


                    Globals.DbContextAutoGen.Images.Add(avatar); // adds the image to the DbSet in memory
                    Globals.DbContextAutoGen.SaveChanges(); // commits the changes to the database
                    Image MostRecentImg = Globals.DbContextAutoGen.Images.Where(Images => Images.Image1 == bits).First();
                    //stud.studentID = u.ID; can just get the id of most recent image this way
                    imgId = MostRecentImg.Id;

                }
                string myPassword = PbxPass.Password;
                string mySalt = BCrypt.Net.BCrypt.GenerateSalt();
                string myHash = BCrypt.Net.BCrypt.HashPassword(myPassword, mySalt);

                if (imgId != -1)
                {
                    User newUser = new User { Email = TbxEmail.Text, Username = TbxUsername.Text, Password = myHash, Score = 0, MaxScore = 0, ImgId = imgId }; // ArgumentException
                    Globals.DbContextAutoGen.Users.Add(newUser); // adds the image to the DbSet in memory
                    Globals.DbContextAutoGen.SaveChanges(); // commits the changes to the database

                }
                else
                {
                    User newUser = new User { Email = TbxEmail.Text, Username = TbxUsername.Text, Password = myHash, Score = 0, MaxScore = 0 }; // ArgumentException
                    Globals.DbContextAutoGen.Users.Add(newUser); // adds the image to the DbSet in memory
                    Globals.DbContextAutoGen.SaveChanges(); // commits the changes to the database
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
            TbxEmail.Text = "";
            TbxUsername.Text = "";
            PbxPass.Password = "";
            PbxConfirmPass.Password = "";
            userAvatar.Source = null;
        }

        private bool ValidateInputs()
        {
            if (!Regex.IsMatch(TbxEmail.Text, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")) //, RegexOptions.IgnoreCase))
            {
                TbxEmail.Text = "";
                TbxEmail.BorderBrush = Brushes.Red;
                throw new ArgumentException("Email must be a valid email up to 100 characters");
            }
            else
            {
                TbxEmail.BorderBrush = Brushes.Black;
            }

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
                PbxPass.Password = "";
                PbxPass.BorderBrush = Brushes.Red;
                throw new ArgumentException("Password field is empty");
            }
            else
            {
                PbxPass.BorderBrush = Brushes.Black;
            }

            if (PbxConfirmPass.Password == "")
            {
                PbxConfirmPass.Password = "";
                PbxConfirmPass.BorderBrush = Brushes.Red;
                throw new ArgumentException("Confirm Password field is empty");
            }
            else
            {
                PbxConfirmPass.BorderBrush = Brushes.Black;
            }

            if (PbxConfirmPass.Password != PbxPass.Password)
            {
                PbxPass.Password = "";
                PbxConfirmPass.Password = "";
                PbxConfirmPass.BorderBrush = Brushes.Red;
                PbxPass.BorderBrush = Brushes.Red;

                throw new ArgumentException("Passwords do not match");
            }
            else
            {
                PbxConfirmPass.BorderBrush = Brushes.Black;
                PbxPass.BorderBrush = Brushes.Black;
            }

            return true;
        }

        private byte[] ImageToByteArray(System.Drawing.Image images)
        {
            using (var _memorystream = new MemoryStream())
            {
                images.Save(_memorystream, images.RawFormat);
                return _memorystream.ToArray();
            }
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

        private User UsernameExists()
        {
            User Duplicate = Globals.DbContextAutoGen.Users.Where(Users => Users.Username == TbxUsername.Text ).FirstOrDefault();
            return Duplicate;
        }

        private User EmailExists()
        {
            User Duplicate = Globals.DbContextAutoGen.Users.Where(Users => Users.Email == TbxEmail.Text).FirstOrDefault();
            return Duplicate;
        }
    }
}
