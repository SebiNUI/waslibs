﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using AppStudio.DataProviders.YouTube;


namespace AppStudio.Uwp.Samples
{
    public sealed partial class YouTubeSample : Page
    {
        private YouTubeDataProvider _youTubeDataProvider;

        public YouTubeSample()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        public ObservableCollection<object> Items
        {
            get { return (ObservableCollection<object>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty = DependencyProperty
            .Register(nameof(Items), typeof(ObservableCollection<object>), typeof(YouTubeSample), new PropertyMetadata(null));

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            GetItems();
        }      
        
        public async void GetItems()
        {            
            string queryParam = "PLZCHH_4VqpRjpQP36-XM1jb1E_JIxJZFJ";
            YouTubeQueryType queryType = YouTubeQueryType.Playlist;
            int maxRecordsParam = 20;
            YouTubeSearchOrderBy orderBy = YouTubeSearchOrderBy.None;

            InitializeDataProvider();
            this.Items = new ObservableCollection<object>();

            var config = new YouTubeDataConfig
            {
                Query = queryParam,
                QueryType = queryType,
                SearchVideosOrderBy = orderBy
            };
            
            var items = await _youTubeDataProvider.LoadDataAsync(config, maxRecordsParam);
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }

        private async void GetMoreItems()
        {
            var items = await _youTubeDataProvider.LoadMoreDataAsync();         

            foreach (var item in items)
            {
                Items.Add(item);
            }          
        }

        private void InitializeDataProvider()
        {
            string apiKey = "YourApiKey";
            _youTubeDataProvider = new YouTubeDataProvider(new YouTubeOAuthTokens { ApiKey = apiKey });
        }
    }
}
