using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WeatherWarningW8
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>

    public class RegionItem
    {
        public RegionItem(string _name, bool _isChecked)
        {
            RegionName = _name;
            IsChecked = _isChecked;
        }
        public string RegionName {get; set;}
        public bool IsChecked { get; set; }
    }
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Regions.ItemsSource = new ObservableCollection<RegionItem>
            {
                new RegionItem("Western_Cape", true), 
                new RegionItem("Eastern_Cape", false),
                new RegionItem("Northern_Cape", false),
                new RegionItem("Free_State", true),
                new RegionItem("KwaZulu-Natal", true), 
                new RegionItem("Mpumalanga", false),
                new RegionItem("Gauteng", true),
                new RegionItem("Limpopo_Province", true),
                new RegionItem("North_West", true)
            };
            this.English.IsChecked = true;
            this.Afrikaans.IsChecked = false;
        }

        private async void SubscribeButton_Click(object sender, RoutedEventArgs e)
        {
            var categories = new HashSet<string>();
            if (SnowToggle.IsOn) categories.Add("Snow");
            if (HeavyRainToggle.IsOn) categories.Add("Heavy_Rain");
            if (GailsToggle.IsOn) categories.Add("Gails");
            if (BigWavesToggle.IsOn) categories.Add("Big_Waves");

            var regions = ((ObservableCollection<RegionItem>)Regions.ItemsSource).Where(r => r.IsChecked == true).Select(r => r.RegionName);
            var language = (English.IsChecked == true) ? "EN" : "AF";
            await ((App)Application.Current).notifications.StoreCategoriesAndSubscribe(regions, categories, language);

            var dialog = new MessageDialog("Subscribed to: " + string.Join(",", categories) + " in regions: " +string.Join(",", regions) + " in " + language);
            dialog.Commands.Add(new UICommand("OK"));
            await dialog.ShowAsync();
        }


    }
}
