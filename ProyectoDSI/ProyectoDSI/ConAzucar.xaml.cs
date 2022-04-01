using System;
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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace ProyectoDSI
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class ConAzucar : Page
    {
        List<PanelFicha> listPanelFichas;
        bool[,] Tablero;
        private DispatcherTimer timer;
        private DispatcherTimer textTimer_;
        private int minutes;
        private int seconds;
        private int initialTime=2;
        private int currentTime;
        private bool playersTurn;
        public ConAzucar()
        {
            this.InitializeComponent();
            currentTime = initialTime;
            textTimer_ = new DispatcherTimer();
            textTimer_.Interval = new TimeSpan(0, 0, 1);

            textTimer_.Tick += textTimer_Tick;
            textTimer_.Start();
            playersTurn = true;
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Pausa));
        }

        void timer_Tick(object sender, object e)
        {
            if (minutes > 0)
            {
                if (seconds == 0) { seconds = 59; minutes--; }
                else seconds--;

                if(seconds<10) CountDown.Text = "0" + minutes.ToString() + ":0" + seconds.ToString();
                else CountDown.Text = "0" + minutes.ToString() + ":" +  seconds.ToString();
            }
            else if(minutes==0 && seconds <= 59)
            {
                if (seconds == 0) { timer.Stop();  Add(); }
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
                currentTime = initialTime;
                clearStackPanel();
                playersTurn = !playersTurn;
            }
            
        }
        private void Grid_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Escape)
                Frame.Navigate(typeof(Pausa));
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            minutes = 1;
            seconds = 30;
            this.NavigationCacheMode = NavigationCacheMode.Required;
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);

            timer.Tick += timer_Tick;
            CountDown.Text = "0" + minutes.ToString() + ":0" + seconds.ToString();
            timer.Start();
           

            CrearLista();
            inicializateTablero();

            base.OnNavigatedTo(e);
        }
        private void CrearLista()
        {
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
                    Background = new SolidColorBrush(Windows.UI.Colors.LightBlue),
                    BorderBrush = new SolidColorBrush(Windows.UI.Colors.MediumPurple),
                    Child = new TextBlock() 
                    { 
                        TextAlignment = TextAlignment.Center, 
                        Foreground= new SolidColorBrush(Windows.UI.Colors.MediumPurple),
                        TextWrapping=TextWrapping.Wrap,
                        FontSize=15,
                        FontFamily=new FontFamily("MV Boli"),
                        Text="YOUR TURN"
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
                    Background = new SolidColorBrush(Windows.UI.Colors.LightBlue),
                    BorderBrush = new SolidColorBrush(Windows.UI.Colors.MediumPurple),
                    Child = new TextBlock()
                    {
                        TextAlignment = TextAlignment.Center,
                        Foreground = new SolidColorBrush(Windows.UI.Colors.MediumPurple),
                        TextWrapping = TextWrapping.Wrap,
                        FontSize = 15,
                        FontFamily = new FontFamily("MV Boli"),
                        Text = "YOUR TURN"
                    }
                });
            }
            
       }
        void clearStackPanel()
        {
            EntranceStackPanel.Children.Clear();
        }
    }
}
