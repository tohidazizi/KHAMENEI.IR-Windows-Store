/*
 * LICENSE: http://opensource.org/licenses/ms-pl 
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using APIMASH_Khamenei.BLL;
using Windows.System;
using Windows.UI.ApplicationSettings;
using Windows.UI.Popups;
using Windows.UI.Xaml.Navigation;

namespace APIMASH_Khamenei
{
    public sealed partial class MainPage : LayoutAwarePage
    {
        APIMASHInvoke apiInvoke;
        ObservableCollection<KeyValue> _newsTypes = GetNewsTypes();

        public MainPage()
        {
            this.InitializeComponent();
            apiInvoke = new APIMASHInvoke();

            NewsTypeComboBox.DataContext = _newsTypes;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SettingsPane settingsPane = Windows.UI.ApplicationSettings.SettingsPane.GetForCurrentView();
            settingsPane.CommandsRequested += settingsPane_CommandsRequested;

            if (NewsTypeComboBox.SelectedIndex == -1)
            {
                NewsTypeComboBox.SelectedIndex = 1;
            }
            else
            {
                Invoke(newsTypeID: NewsTypeComboBox.SelectedIndex);
            }
        }

        void settingsPane_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            // update to supply links to your about, support and privavy policy web pages
            var aboutCmd = new SettingsCommand("About", "About", (x) => Launcher.LaunchUriAsync(new Uri("http://farsi.khamenei.ir/about")));
            var supportCmd = new SettingsCommand("Support", "Support", (x) => Launcher.LaunchUriAsync(new Uri("http://farsi.khamenei.ir/contact")));
            var policyCmd = new SettingsCommand("PrivacyPolicy", "Privacy Policy", (x) => Launcher.LaunchUriAsync(new Uri("http://YOUR_SITE_HERE/PrivacyPolicy.html")));

            args.Request.ApplicationCommands.Add(aboutCmd);
            args.Request.ApplicationCommands.Add(supportCmd);
            args.Request.ApplicationCommands.Add(policyCmd);
        }

        private void Invoke(int newsTypeID)
        {
            Progress.IsActive = true;
            apiInvoke.OnResponse += apiInvoke_OnResponse;

            string apiCall = @"http://farsi.khamenei.ir/developer/api/news?return_lead=true";
            if (newsTypeID > 0) //improve to check it with _newsTypes
            {
                apiCall += "&news_type=" + newsTypeID;
            }

            apiInvoke.Invoke(apiCall);
        }

        async private void apiInvoke_OnResponse(object sender, APIMASHEvent e)
        {
            Progress.IsActive = false;
            KhameneiIr_Response response = e.Object as KhameneiIr_Response;

            if (e.Status == APIMASHStatus.SUCCESS)
            {
                var bg = new BusinessGroup();
                bg.Copy(response.items.ToArray());
                ArticlesGridView.ItemsSource = bg.Items;
            }
            else
            {
                MessageDialog md = new MessageDialog(e.Message, "Error");
                bool? result = null;
                md.Commands.Add(new UICommand("Ok", new UICommandInvokedHandler((cmd) => result = true)));
                await md.ShowAsync();
            }
        }

        private void ComboBox_SelectionChanged(object sender, Windows.UI.Xaml.Controls.SelectionChangedEventArgs e)
        {
            int selectedValueInt;

            if (int.TryParse(Convert.ToString(NewsTypeComboBox.SelectedValue), out selectedValueInt))
                Invoke(newsTypeID: selectedValueInt);
        }

        static private ObservableCollection<KeyValue> GetNewsTypes()
        {
            var result = new ObservableCollection<KeyValue>();

            //result.Add(new KeyValue(0, "همه"));
            result.Add(new KeyValue(1, "خبر"));
            result.Add(new KeyValue(2, "بيانات"));
            result.Add(new KeyValue(3, "نقشه راه"));
            result.Add(new KeyValue(4, "پیام‌ها و نامه‌ها"));
            result.Add(new KeyValue(5, "حاشیه دیدار"));
            result.Add(new KeyValue(6, "عکس"));
            result.Add(new KeyValue(7, "فيلم"));
            result.Add(new KeyValue(8, "صوت"));
            result.Add(new KeyValue(9, "گزيده بيانات"));
            result.Add(new KeyValue(11, "پرونده"));
            result.Add(new KeyValue(12, "ديگران - يادداشت"));
            result.Add(new KeyValue(13, "ديگران - گفتگو"));
            result.Add(new KeyValue(14, "ديگران - خاطره"));
            result.Add(new KeyValue(16, "خاطرات"));
            result.Add(new KeyValue(17, "تلکس متنی"));
            result.Add(new KeyValue(18, "تلکس عکس"));
            result.Add(new KeyValue(19, "تلکس صوت"));
            result.Add(new KeyValue(20, "تلکس فيلم"));
            result.Add(new KeyValue(22, "ديگران - گزارش"));
            result.Add(new KeyValue(26, "مقاله"));
            result.Add(new KeyValue(28, "عکس ويژه"));
            result.Add(new KeyValue(29, "عکس پوستری"));
            result.Add(new KeyValue(30, "تلکس وبلاگی"));
            result.Add(new KeyValue(32, "ابلاغيه"));
            result.Add(new KeyValue(33, "بسته"));
            result.Add(new KeyValue(34, "شرح حدیث"));
            result.Add(new KeyValue(38, "کتاب"));
            result.Add(new KeyValue(39, "پیامک"));
            result.Add(new KeyValue(40, "صحیفه امام خمینی (ره)"));
            result.Add(new KeyValue(43, "شعر"));
            result.Add(new KeyValue(44, "عکس دیگران"));
            result.Add(new KeyValue(45, "برگزیده"));
            result.Add(new KeyValue(46, "تلمیحات و اشارات"));
            result.Add(new KeyValue(47, "دست نوشته"));
            result.Add(new KeyValue(48, "استفتائات"));
            result.Add(new KeyValue(49, "مرور سریع"));
            result.Add(new KeyValue(50, "روزنگار"));

            return result;
        }
    }

    // Suprisingly it doesn't work with KeyValuePair!!
    public class KeyValue
    {
        public int Key { get; set; }
        public string Value { get; set; }
        public KeyValue(int key, string value)
        {
            this.Key = key;
            this.Value = value;
        }
    }
}