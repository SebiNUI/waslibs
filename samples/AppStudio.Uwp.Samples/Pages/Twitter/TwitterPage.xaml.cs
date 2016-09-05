﻿using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

using AppStudio.Uwp.Commands;
using AppStudio.DataProviders.Twitter;


namespace AppStudio.Uwp.Samples
{
    [SamplePage(Category = "DataProviders", Name = "Twitter", Order = 20)]
    public sealed partial class TwitterPage : SamplePage
    {
        private const string DefaultConsumerKey = "29TPMHBW0QcFIWvNrfWxUGmlV";
        private const string DefaultConsumerSecret = "7cp8HDzES42iAFGgE5yxJ3wAxsrDdu5uEHwhoOKPlN6Q2P8k6s";
        private const string DefaultAccessToken = "275442106-OdbhPuGr8biRdQsHbtzNSMVvHRrX9acsLbiyYgCF";
        private const string DefaultAccessTokenSecret = "GA4Uw2sMgvSayjWTw9qdejB8LzNfNS2cAaQPimVDVhdIP";
        private const string DefaultTwitterQueryParam = "Windows";
        private const TwitterQueryType DefaultQueryType = TwitterQueryType.Search;
        private const int DefaultMaxRecordsParam = 20;

        TwitterDataProvider twitterDataProvider;
        TwitterDataProvider rawDataProvider;

        public TwitterPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
            commandBar.DataContext = this;
            paneHeader.DataContext = this;

            InitializeDataProvider();
        }
        
        public override string Caption
        {
            get { return "Twitter Data Provider"; }
        }

        #region DataProvider Config
        public string ConsumerKey
        {
            get { return (string)GetValue(ConsumerKeyProperty); }
            set { SetValue(ConsumerKeyProperty, value); }
        }

        public static readonly DependencyProperty ConsumerKeyProperty = DependencyProperty.Register(nameof(ConsumerKey), typeof(string), typeof(TwitterPage), new PropertyMetadata(DefaultConsumerKey));

        public string ConsumerSecret
        {
            get { return (string)GetValue(ConsumerSecretProperty); }
            set { SetValue(ConsumerSecretProperty, value); }
        }

        public static readonly DependencyProperty ConsumerSecretProperty = DependencyProperty.Register(nameof(ConsumerSecret), typeof(string), typeof(TwitterPage), new PropertyMetadata(DefaultConsumerSecret));

        public string AccessToken
        {
            get { return (string)GetValue(AccessTokenProperty); }
            set { SetValue(AccessTokenProperty, value); }
        }

        public static readonly DependencyProperty AccessTokenProperty = DependencyProperty.Register(nameof(AccessToken), typeof(string), typeof(TwitterPage), new PropertyMetadata(DefaultAccessToken));

        public string AccessTokenSecret
        {
            get { return (string)GetValue(AccessTokenSecretProperty); }
            set { SetValue(AccessTokenSecretProperty, value); }
        }

        public static readonly DependencyProperty AccessTokenSecretProperty = DependencyProperty.Register(nameof(AccessTokenSecret), typeof(string), typeof(TwitterPage), new PropertyMetadata(DefaultAccessTokenSecret));


        public string TwitterQueryParam
        {
            get { return (string)GetValue(YouTubeQueryParamProperty); }
            set { SetValue(YouTubeQueryParamProperty, value); }
        }

        public static readonly DependencyProperty YouTubeQueryParamProperty = DependencyProperty.Register(nameof(TwitterQueryParam), typeof(string), typeof(TwitterPage), new PropertyMetadata(DefaultTwitterQueryParam));


        public TwitterQueryType TwitterQueryTypeSelectedItem
        {
            get { return (TwitterQueryType)GetValue(TwitterQueryTypeSelectedItemProperty); }
            set { SetValue(TwitterQueryTypeSelectedItemProperty, value); }
        }

        public static readonly DependencyProperty TwitterQueryTypeSelectedItemProperty = DependencyProperty.Register(nameof(TwitterQueryTypeSelectedItemProperty), typeof(TwitterQueryType), typeof(TwitterPage), new PropertyMetadata(DefaultQueryType));


        public int MaxRecordsParam
        {
            get { return (int)GetValue(MaxRecordsParamProperty); }
            set { SetValue(MaxRecordsParamProperty, value); }
        }

        public static readonly DependencyProperty MaxRecordsParamProperty = DependencyProperty.Register(nameof(MaxRecordsParam), typeof(int), typeof(TwitterPage), new PropertyMetadata(DefaultMaxRecordsParam));

        #endregion

        #region Items
        public ObservableCollection<object> Items
        {
            get { return (ObservableCollection<object>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(nameof(Items), typeof(ObservableCollection<object>), typeof(TwitterPage), new PropertyMetadata(null));

        #endregion        

        #region RawData
        public string DataProviderRawData
        {
            get { return (string)GetValue(DataProviderRawDataProperty); }
            set { SetValue(DataProviderRawDataProperty, value); }
        }

        public static readonly DependencyProperty DataProviderRawDataProperty = DependencyProperty.Register(nameof(DataProviderRawData), typeof(string), typeof(TwitterPage), new PropertyMetadata(string.Empty));

        #endregion    

        #region HasErrors
        public bool HasErrors
        {
            get { return (bool)GetValue(HasErrorsProperty); }
            set { SetValue(HasErrorsProperty, value); }
        }
        public static readonly DependencyProperty HasErrorsProperty = DependencyProperty.Register(nameof(HasErrors), typeof(bool), typeof(TwitterPage), new PropertyMetadata(false));
        #endregion

        #region NoItems
        public bool NoItems
        {
            get { return (bool)GetValue(NoItemsProperty); }
            set { SetValue(NoItemsProperty, value); }
        }
        public static readonly DependencyProperty NoItemsProperty = DependencyProperty.Register(nameof(NoItems), typeof(bool), typeof(TwitterPage), new PropertyMetadata(false));
        #endregion

        #region IsBusy
        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }
        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register(nameof(IsBusy), typeof(bool), typeof(TwitterPage), new PropertyMetadata(false));

        #endregion

        #region ICommands
        public ICommand RefreshDataCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Request();
                });
            }
        }

        public ICommand MoreDataCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    MoreItemsRequest();
                });
            }
        }

        public ICommand RestoreConfigCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    RestoreConfig();
                    Request();
                });
            }
        }

        #endregion

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.Items = new ObservableCollection<object>();
            RestoreConfig();
            Request();

            base.OnNavigatedTo(e);
        }

        protected override void OnSettings()
        {
            AppShell.Current.Shell.ShowRightPane(new TwitterSettings() { DataContext = this });
        }

        private async void Request()
        {
            try
            {
                IsBusy = true;
                HasErrors = false;
                NoItems = false;
                DataProviderRawData = string.Empty;
                Items.Clear();

                var config = new TwitterDataConfig
                {
                    Query = TwitterQueryParam,
                    QueryType = TwitterQueryTypeSelectedItem
                };                

                var items = await twitterDataProvider.LoadDataAsync(config, MaxRecordsParam);

                NoItems = !items.Any();

                foreach (var item in items)
                {
                    Items.Add(item);
                }
               
                var rawParser = new RawParser();
                var rawData = await rawDataProvider.LoadDataAsync(config, MaxRecordsParam, rawParser);
                DataProviderRawData = rawData.FirstOrDefault()?.Raw;
            }
            catch (Exception ex)
            {
                DataProviderRawData += ex.Message;
                DataProviderRawData += ex.StackTrace;
                HasErrors = true;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void MoreItemsRequest()
        {
            try
            {
                IsBusy = true;
                HasErrors = false;
                NoItems = false;
                DataProviderRawData = string.Empty;
                Items.Clear();

                var items = await twitterDataProvider.LoadMoreDataAsync();

                NoItems = !items.Any();

                foreach (var item in items)
                {
                    Items.Add(item);
                }

                var rawData = await rawDataProvider.LoadMoreDataAsync<RawSchema>();
                DataProviderRawData = rawData.FirstOrDefault()?.Raw;
            }
            catch (Exception ex)
            {
                DataProviderRawData += ex.Message;
                DataProviderRawData += ex.StackTrace;
                HasErrors = true;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void RestoreConfig()
        {
            ConsumerKey = DefaultConsumerKey;
            ConsumerSecret = DefaultConsumerSecret;
            AccessToken = DefaultAccessToken;
            AccessTokenSecret = DefaultAccessTokenSecret;
            TwitterQueryParam = DefaultTwitterQueryParam;
            TwitterQueryTypeSelectedItem = DefaultQueryType;
            MaxRecordsParam = DefaultMaxRecordsParam;           
        }

        private void InitializeDataProvider()
        {
            twitterDataProvider = new TwitterDataProvider(new TwitterOAuthTokens
            {
                AccessToken = AccessToken,
                AccessTokenSecret = AccessTokenSecret,
                ConsumerKey = ConsumerKey,
                ConsumerSecret = ConsumerSecret
            });

            rawDataProvider = new TwitterDataProvider(new TwitterOAuthTokens
            {
                AccessToken = AccessToken,
                AccessTokenSecret = AccessTokenSecret,
                ConsumerKey = ConsumerKey,
                ConsumerSecret = ConsumerSecret
            });
        }

    }
}
