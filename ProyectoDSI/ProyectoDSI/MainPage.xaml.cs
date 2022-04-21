using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
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

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0xc0a

namespace ProyectoDSI
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        MediaPlayer clickSound;
        public MainPage()
        {
            this.InitializeComponent();
            clickSound = new MediaPlayer();
        }
        private  void Button_Click(object sender, RoutedEventArgs e)
        {
            clickSound.Play();
            Frame.Navigate(typeof(Opciones));
        }

        private void OnExit_Click(object sender, RoutedEventArgs e)
        {
            CoreApplication.Exit();
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            clickSound.Play();
            Frame.Navigate(typeof(SeleccionNivel));
        }

        protected override  async void OnNavigatedTo(NavigationEventArgs e)
        {
            Windows.Storage.StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets");
            Windows.Storage.StorageFile file = await folder.GetFileAsync("bottonclick.wav");
            clickSound.Source = MediaSource.CreateFromStorageFile(file);
        }
    }
}
