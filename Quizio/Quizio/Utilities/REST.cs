using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.IO;
using System.Collections.Specialized;

namespace Quizio
{
    public static class REST
    {
        public const string APIURL = "http://localhost:10300";
        public static CookieContainer cookieContainer = new CookieContainer();
        public static string get(string url)
        {
            HttpWebRequest wrGETURL = (HttpWebRequest)HttpWebRequest.Create(url);
            wrGETURL.CookieContainer = cookieContainer;

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

        public static string put(string url, string json)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json";
            request.Method = "PUT";
            request.CookieContainer = cookieContainer;

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(json);
            }

            var response = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                string result = streamReader.ReadToEnd();
                return result;
            }
        }

        /*
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
        }*/

        public static string postLogin(string url, string username, string password) {

            StringBuilder postData = new StringBuilder();
            postData.Append("username=" + HttpUtility.UrlEncode(username) + "&");
            postData.Append("password=" + HttpUtility.UrlEncode(password) + "&");


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //request.ContentType = "text/json";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            request.CookieContainer = cookieContainer;

            string data = postData.ToString();

            var streamWriter = new StreamWriter(request.GetRequestStream());
            streamWriter.Write(data, 0, data.Length);
            streamWriter.Flush();

            try
            {

                Stream objStream = request.GetResponse().GetResponseStream();

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
            catch(InvalidOperationException ioEx)
            {
                if (ioEx is WebException)
                {
                    if (ioEx.Message.Contains("404"))
                    {
                        return "404";
                    }
                }
                return "FATAL ERROR IN REST.logIn";
            }
        }

        /*
        public static string post(string url, string json)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "text/json";
            request.Method = "POST";
            request.CookieContainer = cookieContainer;

            byte[] postBytes = Encoding.UTF8.GetBytes(json);
 

            Stream reqStream = request.GetRequestStream();
            reqStream.Write(postBytes, 0, postBytes.Length);
            reqStream.Flush();

            try
            {
                Stream objStream = request.GetResponse().GetResponseStream();

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
            catch (InvalidOperationException ioEx)
            {
                if (ioEx is WebException)
                {
                    if (ioEx.Message.Contains("404"))
                    {
                        return "404";
                    }
                }
                return ioEx.ToString();
            }

         }*/

        public static string post(string url, string json)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json";
            request.Method = "POST";
            request.CookieContainer = cookieContainer;

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(json);
            }

            var response = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                string result = streamReader.ReadToEnd();
                return result;
            }
        }

        public static string delete(string url, string json)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json";
            request.Method = "DELETE";
            request.CookieContainer = cookieContainer;

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(json);
            }

            var response = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                string result = streamReader.ReadToEnd();
                return result;
            }

        }
    }


}
