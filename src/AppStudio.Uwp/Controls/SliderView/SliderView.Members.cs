﻿using System.Windows.Input;

using Windows.UI.Xaml;

namespace AppStudio.Uwp.Controls
{
    partial class SliderView
    {
        #region ItemsSource
        public object ItemsSource
        {
            get { return (object)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(object), typeof(SliderView), new PropertyMetadata(null));
        #endregion

        #region Index
        public int Index
        {
            get { return (int)GetValue(IndexProperty); }
            set { SetValue(IndexProperty, value); }
        }

        public static readonly DependencyProperty IndexProperty = DependencyProperty.Register("Index", typeof(int), typeof(SliderView), new PropertyMetadata(0));
        #endregion

        #region ItemTemplate
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(SliderView), new PropertyMetadata(null));
        #endregion

        #region ItemWidth
        public double ItemWidth
        {
            get { return (double)GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }

        public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register("ItemWidth", typeof(double), typeof(SliderView), new PropertyMetadata(200.0));
        #endregion

        #region ItemClickCommand
        public ICommand ItemClickCommand
        {
            get { return (ICommand)GetValue(ItemClickCommandProperty); }
            set { SetValue(ItemClickCommandProperty, value); }
        }

        public static readonly DependencyProperty ItemClickCommandProperty = DependencyProperty.Register("ItemClickCommand", typeof(ICommand), typeof(SliderView), new PropertyMetadata(null));
        #endregion

        #region ArrowsVisibility
        public Visibility ArrowsVisibility
        {
            get { return (Visibility)GetValue(ArrowsVisibilityProperty); }
            set { SetValue(ArrowsVisibilityProperty, value); }
        }

        public static readonly DependencyProperty ArrowsVisibilityProperty = DependencyProperty.Register("ArrowsVisibility", typeof(Visibility), typeof(SliderView), new PropertyMetadata(Visibility.Collapsed));
        #endregion
    }
}
