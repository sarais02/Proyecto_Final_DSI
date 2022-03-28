﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
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
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class Opciones : Page
    {
        int indice;
        public Opciones()
        {
            this.InitializeComponent();
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
    }
}
