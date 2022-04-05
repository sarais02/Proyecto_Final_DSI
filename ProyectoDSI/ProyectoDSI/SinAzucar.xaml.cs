using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace ProyectoDSI
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class SinAzucar : Page
    {
        bool[,] Tablero;//PARA VER EN QUE CASILLAS HAY FICHAS
        List<PanelFicha> listPanelFichas;//PANEL IZQUIERDO
        public ObservableCollection<FichaInicial> listFichasIniciales1 { get; } = new ObservableCollection<FichaInicial>(); //PANEL INICIAL LISTA ARRIBA

        List<Ficha> FichasJugador;
        List<Ficha> FichasEnemigo;

        private DispatcherTimer timer;
        private DispatcherTimer textTimer_;
        private int minutes;
        private int seconds;
        private int initialTime = 2;
        private int currentTime;
        private bool playersTurn;

        
        public SinAzucar() {
            this.InitializeComponent();
            currentTime = initialTime;
            textTimer_ = new DispatcherTimer();
            textTimer_.Interval = new TimeSpan(0, 0, 1);

            textTimer_.Tick += textTimer_Tick;
            textTimer_.Start();
            playersTurn = true;
            Add();
            CrearLista();           
            inicializarPartida();
        }
        void timer_Tick(object sender, object e)
        {
            if (minutes > 0)
            {
                if (seconds == 0) { seconds = 59; minutes--; }
                else seconds--;

                if (seconds < 10) CountDown.Text = "0" + minutes.ToString() + ":0" + seconds.ToString();
                else CountDown.Text = "0" + minutes.ToString() + ":" + seconds.ToString();
            }
            else if (minutes == 0 && seconds <= 59)
            {
                if (seconds == 0) { seconds = 30; minutes = 1; Add(); textTimer_.Start(); }
                else seconds--;

                if (seconds < 10) CountDown.Text = "0" + minutes.ToString() + ":0" + seconds.ToString();
                else CountDown.Text = "0" + minutes.ToString() + ":" + seconds.ToString();
            }

        }
        void textTimer_Tick(object sender, object e)
        {
            if (currentTime > 0)
            {
                currentTime--;
            }
            else
            {
                textTimer_.Stop();
                currentTime = initialTime;
                clearStackPanel();
                playersTurn = !playersTurn;
            }

        }
        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Pausa));         
        }
        protected override void OnNavigatedTo(NavigationEventArgs e){
            minutes = 1;
            seconds = 30;
            this.NavigationCacheMode = NavigationCacheMode.Required;
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);

            timer.Tick += timer_Tick;
            CountDown.Text = "0" + minutes.ToString() + ":" + seconds.ToString();
            timer.Start();         
        }
        private void Grid_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if(e.Key==VirtualKey.Escape) 
                Frame.Navigate(typeof(Pausa));
        }
        private void CrearLista(){
            listPanelFichas = new List<PanelFicha>();
            PanelFicha panelFicha;
            //regaliz
            panelFicha.ficha_ = new Ficha(-1, "Regaliz", -1, -1);
            panelFicha.num_ = 1;
            panelFicha.rango_ = "7";
            panelFicha.numFichas_ = "x1";
            listPanelFichas.Add(panelFicha);
            //baston
            panelFicha.ficha_ = new Ficha(-1, "Baston", -1, -1);
            panelFicha.num_ = 2;
            panelFicha.rango_ = "6";
            panelFicha.numFichas_ = "x2";
            listPanelFichas.Add(panelFicha);
            //dedos
            panelFicha.ficha_ = new Ficha(-1, "Dedo", -1, -1);
            panelFicha.num_ = 3;
            panelFicha.rango_ = "5";
            panelFicha.numFichas_ = "x3";
            listPanelFichas.Add(panelFicha);
            //huevo
            panelFicha.ficha_ = new Ficha(-1, "Huevo", -1, -1);
            panelFicha.num_ = 4;
            panelFicha.rango_ = "4";
            panelFicha.numFichas_ = "x4";
            listPanelFichas.Add(panelFicha);
            //sandia
            panelFicha.ficha_ = new Ficha(-1, "Sandia", -1, -1);
            panelFicha.num_ = 4;
            panelFicha.rango_ = "3";
            panelFicha.numFichas_ = "x4";
            listPanelFichas.Add(panelFicha);
            //cocacola
            panelFicha.ficha_ = new Ficha(-1, "Cocacola", -1, -1);
            panelFicha.num_ = 8;
            panelFicha.rango_ = "2";
            panelFicha.numFichas_ = "x8";
            listPanelFichas.Add(panelFicha);
            //chicle sandia
            panelFicha.ficha_ = new Ficha(-1, "BombaSandia", -1, -1);
            panelFicha.num_ = 1;
            panelFicha.rango_ = "1";
            panelFicha.numFichas_ = "x1";
            listPanelFichas.Add(panelFicha);
            //petazetas
            panelFicha.ficha_ = new Ficha(-1, "Petazeta", -1, -1);
            panelFicha.num_ = 6;
            panelFicha.rango_ = "B";
            panelFicha.numFichas_ = "x6";
            listPanelFichas.Add(panelFicha);
            //fresa
            panelFicha.ficha_ = new Ficha(-1, "Fresa", -1, -1);
            panelFicha.num_ = 1;
            panelFicha.rango_ = "F";
            panelFicha.numFichas_ = "x1";
            listPanelFichas.Add(panelFicha);
        }
        void inicializarPartida()
        {
            Tablero = new bool[10, 10];
            FichasEnemigo = new List<Ficha>();
            FichasJugador= new List<Ficha>();
           
            
            FichaInicial fichainicial = new FichaInicial();

            fichainicial.ficha_=new Ficha(0, "Regaliz", -1, -1);
            fichainicial.cantidad_ = 1;
            listFichasIniciales1.Add(fichainicial);

            fichainicial = new FichaInicial();
            fichainicial.ficha_ = new Ficha(1, "Baston", -1, -1);
            fichainicial.cantidad_ = 2;
            listFichasIniciales1.Add(fichainicial);
            fichainicial = new FichaInicial();
            fichainicial.ficha_ = new Ficha(2, "Dedo", -1, -1);
            fichainicial.cantidad_ = 3;
            listFichasIniciales1.Add(fichainicial);
            fichainicial = new FichaInicial();
            fichainicial.ficha_ = new Ficha(3, "Huevo", -1, -1);
            fichainicial.cantidad_ = 4;
            listFichasIniciales1.Add(fichainicial);
            fichainicial = new FichaInicial();
            fichainicial.ficha_ = new Ficha(4, "Sandia", -1, -1);
            fichainicial.cantidad_ = 4;
            listFichasIniciales1.Add(fichainicial);
            fichainicial = new FichaInicial();
            fichainicial.ficha_ = new Ficha(5, "Cocacola", -1, -1);
            fichainicial.cantidad_ = 8;
            listFichasIniciales1.Add(fichainicial);
            fichainicial = new FichaInicial();
            fichainicial.ficha_ = new Ficha(6, "BombaSandia", -1, -1);
            fichainicial.cantidad_ = 1;
            listFichasIniciales1.Add(fichainicial);
            fichainicial = new FichaInicial();
            fichainicial.ficha_ = new Ficha(7, "Petazeta", -1, -1);
            fichainicial.cantidad_ = 6;
            listFichasIniciales1.Add(fichainicial);
            fichainicial = new FichaInicial();
            fichainicial.ficha_ = new Ficha(8, "Fresa", -1, -1);
            fichainicial.cantidad_ = 1;
            listFichasIniciales1.Add(fichainicial);
        }     
        void Add()
        {
            if (playersTurn)
            {
                EntranceStackPanel.Children.Add(new Border()
                {
                    Width = 150,
                    Height = 30,
                    BorderThickness = new Thickness(3),
                    Background = new SolidColorBrush(Windows.UI.Colors.CornflowerBlue),
                    BorderBrush = new SolidColorBrush(Windows.UI.Colors.MidnightBlue),
                    Child = new TextBlock()
                    {
                        TextAlignment = TextAlignment.Center,
                        Foreground = new SolidColorBrush(Windows.UI.Colors.White),
                        TextWrapping = TextWrapping.Wrap,
                        FontSize = 15,
                        FontFamily = new FontFamily("MV Boli"),
                        FontWeight = FontWeights.Bold,
                        Text = "¡TU TURNO!"
                    }
                });
            }
            else
            {
                EntranceStackPanel.Children.Add(new Border()
                {
                    Width = 150,
                    Height = 30,
                    BorderThickness = new Thickness(3),
                    Background = new SolidColorBrush(Windows.UI.Colors.White),
                    BorderBrush = new SolidColorBrush(Windows.UI.Colors.Red),
                    Child = new TextBlock()
                    {
                        TextAlignment = TextAlignment.Center,
                        Foreground = new SolidColorBrush(Windows.UI.Colors.Red),
                        TextWrapping = TextWrapping.Wrap,
                        FontSize = 15,
                        FontFamily = new FontFamily("MV Boli"),
                        FontWeight = FontWeights.Bold,
                        Text = "¡TURNO ENEMIGO!"
                    }
                });
            }

        }
        void clearStackPanel()
        {
            EntranceStackPanel.Children.Clear();
        }
        private void GridView_DragItemsStarting(object sender, DragItemsStartingEventArgs e){

            FichaInicial aux = e.Items[0] as FichaInicial;
            string id = aux.ficha_.id_.ToString();        
            e.Data.SetText(id);
            e.Data.RequestedOperation = DataPackageOperation.Copy;
        }
        private void Image_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Copy;
        }
        private async void Image_Drop(object sender, DragEventArgs e){
            if (playersTurn) return;
            var id = await e.DataView.GetTextAsync();
            int aux = int.Parse(id);
            Image Receptor = sender as Image;
            Ficha x;
            Receptor.Visibility = Visibility.Visible;
                      
            x = listFichasIniciales1[aux].ficha_;
            listFichasIniciales1[aux].cantidad_--;
            if (listFichasIniciales1[aux].cantidad_ < 1) {
                for (int i = 1; i+aux < listFichasIniciales1.Count(); i++)
                {
                    //Ficha f = listPanelFichas[i].ficha_;
                    //f.id_--;                  
                    listPanelFichas[i+aux].ficha_.id_--;
                    
                }
                listFichasIniciales1.Remove(listFichasIniciales1[aux]);                
            }
                  
            
            x.id_ = FichasJugador.Count();
            string aux2 = Receptor.Name;
                     
            x.Y_ = (int)Char.GetNumericValue(aux2[1]);
            x.X_ = (int)Char.GetNumericValue(aux2[2]);
            FichasJugador.Add(x);
            Receptor.Source = x.img_.Source;
            Tablero[ x.Y_, x.X_] = true;
        }
        private void Image_PointerPressed(object sender, PointerRoutedEventArgs e){
            Windows.UI.Xaml.Input.Pointer ptr = e.Pointer;            
            if (ptr.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse && !playersTurn)
            {
                Image x= sender as Image;
                string aux2 = x.Name;
                int filCol = (int)Char.GetNumericValue(aux2[2]) + (int)Char.GetNumericValue(aux2[1]) * 10;
                Windows.UI.Input.PointerPoint ptrPt = e.GetCurrentPoint(sender as Image);              
                if (ptrPt.Properties.IsRightButtonPressed){
                    ImagenCartaJugador.Source = x.Source;
                    for (int i = 0; i < FichasJugador.Count; i++) {
                        if (FichasJugador[i].Y_*10+ FichasJugador[i].X_==filCol){
                            PanelInfoJugador.Visibility = Visibility.Visible;
                            InfoCartaJugador.Text = FichasJugador[i].info_;
                            RangoCartaJugador.Text = FichasJugador[i].rango_;
                            CartaInfoJugador.Text = FichasJugador[i].tipo_;                           
                        }
                    }
                }
                if (ptrPt.Properties.IsLeftButtonPressed){
                   //seleccionar la ficha como seleccionada y acto depues guardarnos una referencia a ella
                }
            }

            // Prevent most handlers along the event route from handling the same event again.
            e.Handled = true;
        }
    }
}
