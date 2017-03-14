﻿using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

using AppStudio.Uwp.EventArguments;

namespace AppStudio.Uwp.Controls
{
    partial class PivoramaPanel
    {
        public event EventHandler<IntEventArgs> SelectedIndexChanged;

        #region ItemTemplate
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        private static void ItemTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as PivoramaPanel;
            control.InvalidateMeasure();
        }

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(PivoramaPanel), new PropertyMetadata(null, ItemTemplateChanged));
        #endregion

        #region ItemWidth
        public double ItemWidth
        {
            get { return (double)GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }

        private static void ItemWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as PivoramaPanel;
            control.InvalidateMeasure();
        }

        public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register("ItemWidth", typeof(double), typeof(PivoramaPanel), new PropertyMetadata(440.0, ItemWidthChanged));
        #endregion

        private void OnItemTapped(object sender, TappedRoutedEventArgs e)
        {
            if (SelectedIndexChanged != null)
            {
                var contentControl = sender as ContentControl;
                if (contentControl.Tag != null)
                {
                    SelectedIndexChanged(this, new IntEventArgs((int)contentControl.Tag));
                }
            }
        }
    }
}
