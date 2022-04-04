using System;
using System.Collections.Generic;
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
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace ProyectoDSI
{ 
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class SinAzucar : Page
    {
        bool[,] Tablero;
        List<PanelFicha> listPanelFichas;
        List<FichaInicial>listFichasIniciales1;
        List<FichaInicial>listFichasIniciales2;

        List<Ficha> FichasJugador;
        List<Ficha> FichasEnemigo;

        private DispatcherTimer timer;
        private DispatcherTimer textTimer_;
        private int minutes;
        private int seconds;
        private int initialTime = 2;
        private int currentTime;
        private bool playersTurn;
        public SinAzucar()
        {
            this.InitializeComponent();
            currentTime = initialTime;
            textTimer_ = new DispatcherTimer();
            textTimer_.Interval = new TimeSpan(0, 0, 1);

            textTimer_.Tick += textTimer_Tick;
            textTimer_.Start();
            playersTurn = true;
            Add();
            CrearLista();
            inicializateTablero();
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
            FichasEnemigo = new List<Ficha>();
            FichasJugador= new List<Ficha>();
            listFichasIniciales1 = new List<FichaInicial>();
            listFichasIniciales2 = new List<FichaInicial>();
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
            fichainicial.ficha_ = new Ficha(10, "Sandia", -1, -1);
            fichainicial.cantidad_ = 4;
            listFichasIniciales2.Add(fichainicial);
            fichainicial = new FichaInicial();
            fichainicial.ficha_ = new Ficha(11, "Cocacola", -1, -1);
            fichainicial.cantidad_ = 8;
            listFichasIniciales2.Add(fichainicial);
            fichainicial = new FichaInicial();
            fichainicial.ficha_ = new Ficha(12, "BombaSandia", -1, -1);
            fichainicial.cantidad_ = 1;
            listFichasIniciales2.Add(fichainicial);
            fichainicial = new FichaInicial();
            fichainicial.ficha_ = new Ficha(13, "Petazeta", -1, -1);
            fichainicial.cantidad_ = 6;
            listFichasIniciales2.Add(fichainicial);
            fichainicial = new FichaInicial();
            fichainicial.ficha_ = new Ficha(14, "Fresa", -1, -1);
            fichainicial.cantidad_ = 1;
            listFichasIniciales2.Add(fichainicial);
        }
        private void inicializateTablero()
        {
            Tablero = new bool[10, 10];          
            for (int i = 0; i < Tablero.GetLength(0); i++)
            {
                for (int j = 0; j < Tablero.GetLength(1); j++)
                {
                    Tablero[i, j] = false;
                }
            }
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
            var id = await e.DataView.GetTextAsync();
            int aux = int.Parse(id);
            Image Receptor = sender as Image;
            Ficha x;
            Receptor.Visibility = Visibility.Visible;
            if (aux >= 10) {
                aux -= 10;             
                x = listFichasIniciales2[aux].ficha_;
               if (listFichasIniciales2[aux].cantidad_ <= 1)  listFichasIniciales2.Remove(listFichasIniciales2[aux]);
                listFichasIniciales2[aux].cantidad_--;
            }
            else{                
                x = listFichasIniciales1[aux].ficha_;
                if (listFichasIniciales1[aux].cantidad_ <= 1) listFichasIniciales2.Remove(listFichasIniciales2[aux]);
                 listFichasIniciales1[aux].cantidad_--;
            }
            x.id_ = FichasJugador.Count();
            x.X_ = 0;
            x.Y_ = 0;
            FichasJugador.Add(x);
            Receptor.Source = x.img_.Source;
        }
    }
}
