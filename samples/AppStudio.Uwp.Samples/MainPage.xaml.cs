﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using AppStudio.Uwp.Controls;
using AppStudio.Uwp.Commands;
using AppStudio.Uwp.Navigation;

namespace AppStudio.Uwp.Samples
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.TopContentTemplate = this.Resources["WideTopTemplate"] as DataTemplate;
            this.DataContext = this;
            SizeChanged += OnSizeChanged;
            ShellControl.ClearCommandBar();
        }

        public static string Caption
        {
            get { return ""; }
        }

        public static bool HideCommandBar
        {
            get { return true; }
        }

        public static DataTemplate HeaderTemplate
        {
            get { return App.Current.Resources["HomeHeaderTemplate"] as DataTemplate; }
        }

        #region PrimaryCommands, SecondaryCommands, CommandBarBackground
        public static IEnumerable<ICommandBarElement> PrimaryCommands
        {
            get { return null; }
        }

        public static IEnumerable<ICommandBarElement> SecondaryCommands
        {
            get { return null; }
        }

        public static Brush CommandBarBackground
        {
            get { return null; }
        }
        #endregion

        #region TopContentTemplate
        public DataTemplate TopContentTemplate
        {
            get { return (DataTemplate)GetValue(TopContentTemplateProperty); }
            set { SetValue(TopContentTemplateProperty, value); }
        }

        public static readonly DependencyProperty TopContentTemplateProperty = DependencyProperty.Register("TopContentTemplate", typeof(DataTemplate), typeof(MainPage), new PropertyMetadata(null));
        #endregion

        #region Items
        public ObservableCollection<object> Items
        {
            get { return (ObservableCollection<object>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(ObservableCollection<object>), typeof(MainPage), new PropertyMetadata(null));
        #endregion
        
        public static ICommand ItemClickCommand
        {
            get
            {
                return new RelayCommand<ControlDataItem>((control) =>
                {
                    NavigationService.NavigateToPage(control.DetailPageName);
                });
            }
        }        

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.Items = new ObservableCollection<object>(FeaturedControlsDataSource.GetItems());
            AppShell.Current.Shell.SelectItem("Home");
            base.OnNavigatedTo(e);
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (AppShell.Current.Shell.DisplayMode == SplitViewDisplayMode.CompactOverlay)
            {
                this.TopContentTemplate = this.Resources["WideTopTemplate"] as DataTemplate;
            }
            else
            {
                this.TopContentTemplate = this.Resources["TinyTopTemplate"] as DataTemplate;
            }
        }
    }
}
