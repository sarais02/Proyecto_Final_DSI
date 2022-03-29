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
    public sealed partial class SinAzucar : Page
    {
        public SinAzucar()
        {
            this.InitializeComponent();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Pausa));
        }
        protected override void OnNavigatedTo(NavigationEventArgs e){  
            
        }

        private void Grid_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if(e.Key==VirtualKey.Escape) 
                Frame.Navigate(typeof(Pausa));
        }
      
    }
}
