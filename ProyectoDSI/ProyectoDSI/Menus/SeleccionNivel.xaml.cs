using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;
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
    public sealed partial class SeleccionNivel : Page
    {
        MediaPlayer clickSound;
        MediaPlayer backgroundSound;
        public SeleccionNivel()
        {
            this.InitializeComponent();
            clickSound = new MediaPlayer();
            backgroundSound = new MediaPlayer();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            clickSound.Play();
            MenuIniciarBatalla.Visibility = Visibility.Visible;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            clickSound.Play();
            MenuElegitBando.Visibility = Visibility.Visible;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            clickSound.Play();
            if (Frame.CanGoBack) Frame.GoBack();
            backgroundSound.Pause();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            clickSound.Play();
            Frame.Navigate(typeof(ConAzucar));
            backgroundSound.Pause();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            clickSound.Play();
            Frame.Navigate(typeof(SinAzucar));
            backgroundSound.Pause();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            Windows.Storage.StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets");
            Windows.Storage.StorageFile file = await folder.GetFileAsync("bottonclick.wav");
            clickSound.Source = MediaSource.CreateFromStorageFile(file);
            file = await folder.GetFileAsync("menu02.wav");
            backgroundSound.Source = MediaSource.CreateFromStorageFile(file);
            backgroundSound.IsLoopingEnabled = true;
            backgroundSound.Play();
        }
    }
}
