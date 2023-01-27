using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP3_Final
{
    public class Song
    {
        public bool favor = false;
        public string path;
        public Song(string x, bool foo)
        {
            path = x;
            favor = foo;
        }
    }
}
