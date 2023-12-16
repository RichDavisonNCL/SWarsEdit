using System.Collections.Generic;
using System.IO;
using SWars;

namespace SWars
{
    public static class FilePath
    {
        static private string FILE_ROOT = "";
    
        static public string Get()
        {
            return FILE_ROOT;
        }

        static public void Set(string path)
        {
            FILE_ROOT = path;
        }  
    }
}