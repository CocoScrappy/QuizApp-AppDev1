using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Drawing;

namespace QuizApp
{
    internal class Globals
    {
        private static QuizAppProjectEntities1 _DbContextSingleton;

        private static HttpClient _clientSingleton;

        private static User _currentUser;


        public static User CurrentUser
        {
            get { return _currentUser; }
            set { _currentUser = value; }
        }

        public static QuizAppProjectEntities1 DbContextAutoGen
        {
            get
            {
                if( _DbContextSingleton == null)
                {
                    _DbContextSingleton = new QuizAppProjectEntities1();
                }
                return _DbContextSingleton;
            }
        }

        public static HttpClient Client
        {
            get
            {
                if (_clientSingleton == null)
                {
                    _clientSingleton = new HttpClient();
                }
                return _clientSingleton;
            }
        }
    }
}
