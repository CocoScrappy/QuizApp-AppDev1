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

        //private static string _currentUsername;
        //private static byte[] _currentUserImage;
        //private static int _currentUserScore;
        //private static int _currentUserMaxScore;
        //private static BitmapImage _userAvatar;
        //private static System.Drawing.Image _userAvatar;
        //private static BitmapImage _userAvatar;

        //public static string CurrentUsername
        //{
        //    get { return _currentUsername; }
        //    set { _currentUsername = value; }
        //}

        //public static byte[] CurrentUserImage
        //{
        //    get { return _currentUserImage; }
        //    set { _currentUserImage = value; }
        //}

        //public static int CurrentUserScore
        //{
        //    get { return _currentUserScore; }
        //    set { _currentUserScore = value; }
        //}

        //public static int CurrentUserMaxScore
        //{
        //    get { return _currentUserMaxScore; }
        //    set { _currentUserMaxScore = value; }
        //}

        //public static BitmapImage CurrentUserAvatar
        //{
        //    get { return _userAvatar; }
        //    set { _userAvatar = value; }
        //}

        //-----------------2----------------
        //public static System.Drawing.Image CurrentUserAvatar
        //{
        //    get { return _userAvatar; }
        //    set { _userAvatar = value; }
        //}

        //-----------------3----------------
        //public static BitmapImage CurrentUserAvatar
        //{
        //    get { return _userAvatar; }
        //    set { _userAvatar = value; }
        //}

    }
}
