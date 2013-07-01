using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

/*
 * LICENSE: http://opensource.org/licenses/ms-pl) 
 */

namespace APIMASH_Khamenei.BLL
{
    public static class APIMASHMap
    {
        internal static object LoadRssFeed(string apiCall)
        {
            System.Xml.Linq.XDocument feedXML = System.Xml.Linq.XDocument.Load(apiCall);

            IEnumerable<RssFeedItem> response = from feed in feedXML.Descendants("item")
                                                select ConvertToRssFeedItem(feed);

            KhameneiIr_Response result = new KhameneiIr_Response();
            result.items = response.ToList();
            return result;
        }

        private static RssFeedItem ConvertToRssFeedItem(System.Xml.Linq.XElement xelement)
        {
            var result = new RssFeedItem
                {
                    Title = xelement.Element("title") == null ? "بدون عنوان" : xelement.Element("title").Value,
                    Link = xelement.Element("link") == null ? "http://www.khamenei.ir/" : xelement.Element("link").Value,
                    Description = xelement.Element("description") == null ? string.Empty : xelement.Element("description").Value,
                    Category = xelement.Element("category") == null ? string.Empty : xelement.Element("category").Value,
                    Guid = xelement.Element("guid") == null ? string.Empty : xelement.Element("guid").Value
                };

            DateTime pubDate;
            if (xelement.Element("pubDate") != null
                &&
                DateTime.TryParse(xelement.Element("pubDate").Value, out pubDate))
            {
                result.PublishDate = pubDate;
            }
            else result.PublishDate = new DateTime(DateTime.Now.Year, 1, 1);

            return result;
        }
    }
}
