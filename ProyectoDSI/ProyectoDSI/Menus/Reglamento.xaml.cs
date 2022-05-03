using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;
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
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class Reglamento : Page
    {
        CoreCursor CursorArrow = null;
        CoreCursor CursorHand = null;
        MediaPlayer clickSound;
        MediaPlayer backgroundSound;
        PointerPoint ptrPt;
        public Reglamento()
        {
            this.InitializeComponent();
            CursorHand = new CoreCursor(CoreCursorType.Hand, 0);
            CursorArrow = new CoreCursor(CoreCursorType.Arrow, 0);
            clickSound = new MediaPlayer();
            backgroundSound = new MediaPlayer();
        }

        private void Button_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            ptrPt = e.GetCurrentPoint(BackImage);
            Window.Current.CoreWindow.PointerCursor = CursorHand;
            SpecialText.Visibility = Visibility.Visible;
        }

        private void BackButton_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = CursorArrow;
            SpecialText.Visibility = Visibility.Collapsed;
        }
        private void Grid_KeyDown(object sender, KeyRoutedEventArgs e){
            // First, check that it's safe to ask the Frame to go backward.
            switch (e.Key)
            {
                case VirtualKey.Escape: Volver(); break;
            }
        }
        void Volver(){
            // First, check that it's safe to ask the Frame to go backward.
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
                backgroundSound.Pause();
            }
        }

        private void BackImage_Click(object sender, RoutedEventArgs e)
        {
            clickSound.Play();
            Volver();
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            Windows.Storage.StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets");
            Windows.Storage.StorageFile file = await folder.GetFileAsync("bottonclick.wav");
            clickSound.Source = MediaSource.CreateFromStorageFile(file);
            file = await folder.GetFileAsync("menu03.wav");
            backgroundSound.Source = MediaSource.CreateFromStorageFile(file);
            backgroundSound.IsLoopingEnabled = true;
            backgroundSound.Play();
        }
    }
}
