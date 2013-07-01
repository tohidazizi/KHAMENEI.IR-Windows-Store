/*
 * LICENSE: http://opensource.org/licenses/ms-pl 
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using APIMASH_Khamenei.BLL;

namespace APIMASH_Khamenei.BLL
{
    [Windows.Foundation.Metadata.WebHostHidden]
    public class APIMASH_OM_Bindable : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if (object.Equals(storage, value)) return false;

            storage = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }


    [Windows.Foundation.Metadata.WebHostHidden]
    public class BusinessGroup
    {
        private ObservableCollection<BusinessItem> _items;

        public BusinessGroup()
        {
            _items = new ObservableCollection<BusinessItem>();
        }

        public ObservableCollection<BusinessItem> Items
        {
            get { return this._items; }
        }

        public void Copy(IEnumerable<RssFeedItem> articles)
        {
            foreach (var bisinessItem in articles.Select(a => new BusinessItem(a)))
            {
                this._items.Add(bisinessItem);
            }
        }
    }

    public class BusinessItem : APIMASH_OM_Bindable
    {
        public BusinessItem(RssFeedItem rssItem)
        {
            this.id = rssItem.Guid;
            this.title = rssItem.Title;
            this.link = rssItem.Link;
            this.description = rssItem.Description;
            this.publishDate = rssItem.PersianPublishDate; //rssItem.PublishDate;
            this.category = rssItem.Category;
        }

        private string id = string.Empty;
        public string Id
        {
            get { return this.id; }
            set { this.SetProperty(ref this.id, value); }
        }

        private string title = string.Empty;
        public string Title
        {
            get { return this.title; }
            set { this.SetProperty(ref this.title, value); }
        }

        private string description = string.Empty;
        public string Description
        {
            get { return this.description; }
            set { this.SetProperty(ref this.description, value); }
        }

        private string publishDate = string.Empty;
        public string PublishDate
        {
            get { return this.publishDate; }
            set { this.SetProperty(ref this.publishDate, value); }
        }

        private string link = string.Empty;
        public string Link
        {
            get { return this.link; }
            set { this.SetProperty(ref this.link, value); }
        }

        private string category = string.Empty;
        public string Category
        {
            get { return this.category; }
            set { this.SetProperty(ref this.category, value); }
        }

        private string photoUrl = string.Empty;
        public string PhotoUrl
        {
            get { return this.photoUrl; }
            set { this.SetProperty(ref this.photoUrl, value); }
        }
    }
}