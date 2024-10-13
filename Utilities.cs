using dotNS.Classes;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace dotNS
{
    public static class Utilities
    {
        public static string GetRequest(NameValueCollection nvc)
        {
            string query = "";

            foreach (string key in nvc.Keys)
            {
                query += $"{key}={nvc[key]}&";
            }

            query = query.TrimEnd('&');

            return query;
        }

        private static HttpClient Http = new HttpClient();
        public static HttpResponseMessage API(NameValueCollection nvc, string pass = null, string pin = "0", string userAgent = "DotNS Default UserAgent nk.ax")
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"https://www.nationstates.net/cgi-bin/api.cgi?{GetRequest(nvc)}");
            if (pin != "0")
            {
                httpRequest.Headers.Add("X-Pin", pin);
            }
            if (!string.IsNullOrEmpty(pass))
            {
                httpRequest.Headers.Add("X-Password", pass);
            }
            httpRequest.Headers.Add("User-Agent", userAgent);
            var response = Http.SendAsync(httpRequest).GetAwaiter().GetResult();
            if (response.StatusCode == HttpStatusCode.Forbidden) { throw new Exception("Not authentificated."); }
            return response;
        }

        public static XmlNodeList Parse(string xml, string path = "/NATION/*")
        {
            xml = WebUtility.HtmlDecode(xml);
            xml = Regex.Replace(xml, "&(?!(amp|apos|quot|lt|gt);)", "&amp;");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            var nodes = doc.SelectNodes(path);
            return nodes;
        }

        public static XmlNodeList TakeNodes(this XmlNodeList nodes, string path, int depth = 0)
        {
            path = path.ToLower();
            if (depth == 5) return null;
            foreach (XmlNode node in nodes)
            {
                if (node.Name.ToLower() == path) return node.ChildNodes;
                if (node.ChildNodes.Count > 0)
                {
                    XmlNodeList anything = TakeNodes(node.ChildNodes, path, depth + 1);
                    if (anything != null && anything.Count > 0) return anything;
                }
            }
            return null;
        }

        public static SKBitmap GetPicture(string url)
        {
            WebClient web = new WebClient();
            byte[] data = web.DownloadData(url);
            if (url.EndsWith(".svg"))
            {
                // SVG files not supported
                return new SKBitmap(1, 1, true);
            }
            return SKBitmap.Decode(data);
        }

        public static string FindProperty(XmlNodeList nodes, string name, int depth = 0)
        {
            if (nodes == null) return "";
            if (depth == 5) return "";
            name = name.ToLower();
            foreach (XmlNode node in nodes)
            {
                if (node.Name.ToLower() == name) return node.InnerText;
                if (node.Attributes != null && node.Attributes[name] != null) return node.Attributes[name].InnerText;
                if (node.ChildNodes.Count > 0)
                {
                    string anything = FindProperty(node.ChildNodes, name, depth + 1);
                    if (anything != "") return anything;
                }
            }
            return "";
        }

        public static string FindProperty(this XmlNodeList nodes, string name)
        {
            return FindProperty(nodes, name, 0);
        }

        public static string StrResp(HttpResponseMessage resp)
        {
            var str = resp.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            str = str.Replace("“","\"").Replace("”","\"");
            return str;
        }

        internal static Authority[] ParseAuthority(string text)
        {
            List<Authority> auth = new List<Authority>();
            foreach (char z in text)
            {
                switch (z)
                {
                    case 'X':
                        auth.Add(Authority.Executive); break;
                    case 'W':
                        auth.Add(Authority.WorldAssembly); break;
                    case 'A':
                        auth.Add(Authority.Appearance); break;
                    case 'B':
                        auth.Add(Authority.BorderControl); break;
                    case 'C':
                        auth.Add(Authority.Communications); break;
                    case 'E':
                        auth.Add(Authority.Embassies); break;
                    case 'P':
                        auth.Add(Authority.Polls); break;
                }
            }
            return auth.ToArray();
        }
    }
}
