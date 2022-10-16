using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

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
    }
}



