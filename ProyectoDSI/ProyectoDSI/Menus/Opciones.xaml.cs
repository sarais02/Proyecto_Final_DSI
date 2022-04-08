﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace ProyectoDSI
{
    public sealed partial class Opciones : Page
    {
        CoreCursor CursorArrow = null;
        CoreCursor CursorHand = null;

        PointerPoint ptrPt;       
        public Opciones()
        {
            this.InitializeComponent();
            CursorHand = new CoreCursor(CoreCursorType.Hand, 0);
            CursorArrow = new CoreCursor(CoreCursorType.Arrow, 0);
        }
        private void ButtonReglamento_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Reglas_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Reglamento));
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Controles));
        }

        private void Grid_KeyDown(object sender, KeyRoutedEventArgs e) {
            switch (e.Key){
                case VirtualKey.Escape: Volver(); break;               
            }
        }
        private void Volver(){
            if (Frame.CanGoBack){
                Frame.GoBack();
            }
        }

        private void VolverButton_Click(object sender, RoutedEventArgs e){
            Volver();
        }

        private void BackImage_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            ptrPt = e.GetCurrentPoint(BackImage);
            Window.Current.CoreWindow.PointerCursor = CursorHand;
            SpecialText.Visibility = Visibility.Visible;
        }

        private void BackImage_PointerExited(object sender, PointerRoutedEventArgs e)
        {

            Window.Current.CoreWindow.PointerCursor = CursorArrow;
            SpecialText.Visibility = Visibility.Collapsed;
        }

        private void BackImage_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Volver();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            string language = comboBox.SelectedValue.ToString();
            var resourceContext = new Windows.ApplicationModel.Resources.Core.ResourceContext(); // not using ResourceContext.GetForCurrentView

            if (language == "Español") 
            {
                //resourceContext.Languages = new string[] { "es-Es" }; 
                
                Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "es-ES";

                Windows.ApplicationModel.Resources.Core.ResourceContext.GetForViewIndependentUse().Reset();
                Windows.ApplicationModel.Resources.Core.ResourceContext.GetForCurrentView().Reset();
            }

            else if (language == "Ingles") 
            {
                //resourceContext.Languages = new string[] { "en-Us" }; 
                
                Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "en-US";

                Windows.ApplicationModel.Resources.Core.ResourceContext.GetForViewIndependentUse().Reset();
                Windows.ApplicationModel.Resources.Core.ResourceContext.GetForCurrentView().Reset();

            }
            //await Task.Delay(100);
            //Frame.Navigate(this.GetType());
        }
    }
}
