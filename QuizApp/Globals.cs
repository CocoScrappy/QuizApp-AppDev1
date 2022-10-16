using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp
{
    internal class Globals
    {
        private static QuizAppProjectEntities _DbContextSingleton;

        private static HttpClient _clientSingleton;

        public static QuizAppProjectEntities DbContextAutoGen
        {
            get
            {
                if( _DbContextSingleton == null)
                {
                    _DbContextSingleton = new QuizAppProjectEntities();
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
