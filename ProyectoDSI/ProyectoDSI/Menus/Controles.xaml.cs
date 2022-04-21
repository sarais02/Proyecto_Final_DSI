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
    public sealed partial class Controles : Page
    {
        CoreCursor CursorArrow = null;
        CoreCursor CursorHand = null;
        PointerPoint ptrPt;
        MediaPlayer clickSound;
        public Controles()
        {
            this.InitializeComponent();
            CursorArrow = new CoreCursor(CoreCursorType.Arrow, 0);
            CursorHand = new CoreCursor(CoreCursorType.Hand, 0);
            clickSound = new MediaPlayer();
        }

        private void Grid_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
                case VirtualKey.Escape: Volver(); break;
            }
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

        
        private void Volver()
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
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
        }
    }
}
