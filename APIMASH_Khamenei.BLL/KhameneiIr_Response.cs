using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace APIMASH_Khamenei.BLL
{
    [DataContract]
    public class KhameneiIr_Response
    {
        [DataMember(Name = "businesses")]
        public List<RssFeedItem> items
        {
            get { return new List<RssFeedItem>(_items); }
            set { _items = value.ToArray(); }
        }
        private RssFeedItem[] _items;
    }

    /// <summary>
    /// RSS feed item entity
    /// </summary>
    public class RssFeedItem
    {
        /// <summary>
        /// Gets or sets the title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the link
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// Gets or sets the item id
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// Gets or sets the publish date
        /// </summary>
        public DateTime PublishDate { get; set; }

        /// <summary>
        /// Gets or sets the channel id
        /// </summary>
        public int ChannelId { get; set; }


        public string Category { get; set; }

        public string Guid { get; set; }

        public string PersianPublishDate
        {
            get
            {
                return Shamsi(PublishDate);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        /// <remarks>
        /// Code is from: http://www.treatise.blogfa.com/post-107.aspx
        /// </remarks>
        private string Shamsi(DateTime date)
        {
            int[] arrMonths = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            int[] arrStart = { 21, 20, 21, 21, 22, 22, 23, 23, 23, 23, 22, 22 };
            char[] sep = { '/' };
            //string[] arrDate = date.Split(sep);
            int year = date.Year; //Convert.ToInt32(arrDate[0]);
            int month = date.Month; //Convert.ToInt32(arrDate[1]);
            int day = date.Day; //Convert.ToInt32(arrDate[2]);

            if (year % 4 == 0)
            {
                for (int i = 2; i < 12; i++)
                    arrStart[i]--;
                arrMonths[1]++;
                if (month == 1) arrStart[11]++;
            }
            else if (year % 4 == 1)
            {
                arrStart[0]--;
                arrStart[1]--;
                if (month == 1) arrStart[11]--;
            }
            year = month <= 3 ? year - 622 : year - 621;
            if (month == 3 && day >= arrStart[2]) year++;
            if (day < arrStart[month - 1])
            {
                int i = month == 1 ? 11 : month - 2;
                day = day - arrStart[i] + arrMonths[i] + 1;
                month -= 3;
            }
            else
            {
                day = day - arrStart[month - 1] + 1;
                month -= 2;
            }
            if (month <= 0) month += 12;
            return year + "/" + Convert.ToString(month).PadLeft(2, '0') + "/" +
                    Convert.ToString(day).PadLeft(2, '0');
        }
    }
}

