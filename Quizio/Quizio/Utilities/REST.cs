using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace Quizio
{
    class REST
    {
        public const string APIURL = "http://localhost:10300/api";
        public static string get(string url)
        {
            WebRequest wrGETURL = WebRequest.Create(url);
            Stream objStream = wrGETURL.GetResponse().GetResponseStream();

            StreamReader objReader = new StreamReader(objStream);

            string sLine = "";
            string JSON = "";
            while (sLine != null)
            {
                sLine = objReader.ReadLine();
                if (sLine != null)
                    JSON += sLine;
            }
            return JSON;
        }

        public static void put(string url)
        {
            WebRequest wrPutURL = WebRequest.Create(url);
            wrPutURL.Method = "PUT";
            Stream objStream = wrPutURL.GetResponse().GetResponseStream();

            StreamReader objReader = new StreamReader(objStream);

            string sLine = "";
            string JSON = "";
            while (sLine != null)
            {
                sLine = objReader.ReadLine();
                if (sLine != null)
                    JSON += sLine;
            }
            //return JSON; only if check for affected Rows
        }
    }


}
