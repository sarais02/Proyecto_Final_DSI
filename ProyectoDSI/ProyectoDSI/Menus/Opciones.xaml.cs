using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Media.Core;
using Windows.Media.Playback;
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
        MediaPlayer clickSound;
        MediaPlayer backgroundSound;
        PointerPoint ptrPt;       
        public Opciones()
        {
            this.InitializeComponent();
            CursorHand = new CoreCursor(CoreCursorType.Hand, 0);
            CursorArrow = new CoreCursor(CoreCursorType.Arrow, 0);
            clickSound = new MediaPlayer();
            backgroundSound = new MediaPlayer();
            ElementSoundPlayer.State = ElementSoundPlayerState.On;
            sliderAudio.Value = ElementSoundPlayer.Volume * 100;
        }
        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
        private void Reglas_Click(object sender, RoutedEventArgs e)
        {
            clickSound.Play();
            backgroundSound.Pause();
            Frame.Navigate(typeof(Reglamento));
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            clickSound.Play();
            backgroundSound.Pause();
            Frame.Navigate(typeof(Controles));
        }

        private void Grid_KeyDown(object sender, KeyRoutedEventArgs e) {
            switch (e.Key){
                case VirtualKey.Escape: Volver(); break;               
            }
        }
        private void Volver(){
            if (Frame.CanGoBack){
                backgroundSound.Pause();
                Frame.GoBack();
            }
        }

        private void VolverButton_Click(object sender, RoutedEventArgs e){
            clickSound.Play();
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

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            TextBlock text = comboBox.SelectedValue as TextBlock;
            string language = text.Text;
            var resourceContext = new Windows.ApplicationModel.Resources.Core.ResourceContext(); // not using ResourceContext.GetForCurrentView

            if (language == "Español" || language=="Spanish") 
            {
                Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "es-ES";

                Windows.ApplicationModel.Resources.Core.ResourceContext.GetForViewIndependentUse().Reset();
                Windows.ApplicationModel.Resources.Core.ResourceContext.GetForCurrentView().Reset();
            }

            else if (language == "Inglés" || language=="English") 
            {
                Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "en-US";

                Windows.ApplicationModel.Resources.Core.ResourceContext.GetForViewIndependentUse().Reset();
                Windows.ApplicationModel.Resources.Core.ResourceContext.GetForCurrentView().Reset();

            }
            backgroundSound.Pause();
            Frame.Navigate(this.GetType());
            backgroundSound.Pause();
            Frame.GoBack();
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

        private void sliderAudio_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            double volume = sliderAudio.Value / 100.0;
            ElementSoundPlayer.Volume = volume;
        }
    }
}
