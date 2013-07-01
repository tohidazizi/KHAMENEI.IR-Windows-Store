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
}
