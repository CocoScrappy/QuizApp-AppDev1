using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace QuizApp
{
    //Class used to seed Categories into DB
    public class CategorySeed
    {
        HttpClient client = new HttpClient();
        public int id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }
        //Create Button to send Json Request
        private async void BtnSeed(object sender, RoutedEventArgs e)
        {
            CategorySeed window = new CategorySeed();
            await window.GetJson();
        }
        //send HTTP Get Request to category endpoint
        private async Task GetJson()
        {
            string response = await client.GetStringAsync(
                "https://opentdb.com/api_category.php");
            //parse string to Json Object
            JObject results = JObject.Parse(response);
            //Extract category json tokens from Json response object
            IList<JToken> catTokens = results["trivia_categories"].Children().ToList();

            foreach (JToken cat in catTokens)
            {
                Category category = new Category();
                //deserialize Json to Category Entity Objects
                category = cat.ToObject<Category>();
                category.Id = 0;
                //Add and Save to database
                Globals.DbContextAutoGen.Categories.Add(new Category() { Name = category.Name });
                Globals.DbContextAutoGen.SaveChanges();
            }
        }

        // Adding images to database Category Table
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<Category> CategoryList = Globals.DbContextAutoGen.Categories.ToList();
            foreach (Category category in CategoryList)
            {
                int id = category.Id;
                string strId = "";
                if (id < 10)
                {
                    strId = $"0{id}";
                }
                else
                {
                    strId = $"{id}";
                }
                BitmapImage bitmapImage = new BitmapImage(new Uri($@"pack://application:,,,/Images/Categories/{strId}.jpg"));
                Img.Source = bitmapImage;
                System.Drawing.Image CategoryImage = ConvertImageSourceToImage(bitmapImage);
                byte[] bits = ImageToByteArray(CategoryImage);
                Image finalImage = new Image { Image1 = bits };
                Globals.DbContextAutoGen.Images.Add(finalImage);
                Globals.DbContextAutoGen.SaveChanges();
                category.Image = finalImage;
                Globals.DbContextAutoGen.SaveChanges();
            }


        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Category cat = Globals.DbContextAutoGen.Categories.Where(c => c.Id == 1).FirstOrDefault();
            Image img = Globals.DbContextAutoGen.Images.Where(i => i.Id == cat.ImgId).FirstOrDefault();
            byte[] bits = img.Image1;
            Img.Source = ToImage(bits);
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

        private byte[] ImageToByteArray(System.Drawing.Image images)
        {
            using (var _memorystream = new MemoryStream())
            {
                images.Save(_memorystream, images.RawFormat);
                return _memorystream.ToArray();
            }
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



