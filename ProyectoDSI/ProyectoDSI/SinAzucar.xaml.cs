﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
    struct coords {
        public coords(int y, int x){
            x_ = x;
            y_ = y;
        }
       public int x_;
       public int y_;
    }
    struct casillaTablero{
        public casillaTablero(bool ficha = false, bool jug = false){
            hayFicha = ficha;
            esJug = jug;
        }
        public bool hayFicha;
        public bool esJug;
    }
    public interface INotifyPropertyChanged
    {
        event PropertyChangedEventHandler PropertyChanged;
    }
    public sealed partial class SinAzucar : Page, INotifyPropertyChanged
    {
        casillaTablero[,] Tablero;//PARA VER EN QUE CASILLAS HAY FICHAS
        List<PanelFicha> listPanelFichas;//PANEL IZQUIERDO
        public ObservableCollection<FichaInicial> listFichasIniciales1 { get; } = new ObservableCollection<FichaInicial>(); //PANEL INICIAL LISTA ARRIBA
        public event PropertyChangedEventHandler PropertyChanged;
        List<Ficha> FichasJugador;
        List<Ficha> FichasEnemigo;

        //Movimiento
        int seleccion;
        //Ficha fichaSeleccionada;
        bool hayAlgunaFichaSeleccionada = false;
        List<coords> posiblesMovimientos = new List<coords>();

        private DispatcherTimer timer;
        private DispatcherTimer textTimer_;
        private int minutes;
        private int seconds;
        private int initialTime = 2;
        private int currentTime;
        private bool playersTurn;
        private bool estadoinicial=true;
        private bool atack = false;
       

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
        protected override void OnNavigatedTo(NavigationEventArgs e) {
            minutes = 1;
            seconds = 30;
            this.NavigationCacheMode = NavigationCacheMode.Required;
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);

            timer.Tick += timer_Tick;
            CountDown.Text = "0" + minutes.ToString() + ":" + seconds.ToString();
           
        }
        private void Grid_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Escape)
                Frame.Navigate(typeof(Pausa));
        }
        private void CrearLista() {

            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();

            listPanelFichas = new List<PanelFicha>();
            PanelFicha panelFicha;
            //regaliz
            panelFicha.ficha_ = new Ficha(-1, resourceLoader.GetString("NameRegaliz"), -1, -1);
            panelFicha.num_ = 1;
            panelFicha.rango_ = "7";
            panelFicha.numFichas_ = "x1";
            listPanelFichas.Add(panelFicha);
            //baston
            panelFicha.ficha_ = new Ficha(-1, resourceLoader.GetString("NameBaston"), -1, -1);
            panelFicha.num_ = 2;
            panelFicha.rango_ = "6";
            panelFicha.numFichas_ = "x2";
            listPanelFichas.Add(panelFicha);
            //dedos
            panelFicha.ficha_ = new Ficha(-1, resourceLoader.GetString("NameDedo"), -1, -1);
            panelFicha.num_ = 3;
            panelFicha.rango_ = "5";
            panelFicha.numFichas_ = "x3";
            listPanelFichas.Add(panelFicha);
            //huevo
            panelFicha.ficha_ = new Ficha(-1, resourceLoader.GetString("NameHuevo"), -1, -1);
            panelFicha.num_ = 4;
            panelFicha.rango_ = "4";
            panelFicha.numFichas_ = "x4";
            listPanelFichas.Add(panelFicha);
            //sandia
            panelFicha.ficha_ = new Ficha(-1, resourceLoader.GetString("NameSandia"), -1, -1);
            panelFicha.num_ = 4;
            panelFicha.rango_ = "3";
            panelFicha.numFichas_ = "x4";
            listPanelFichas.Add(panelFicha);
            //cocacola
            panelFicha.ficha_ = new Ficha(-1, resourceLoader.GetString("NameCocacola"), -1, -1);
            panelFicha.num_ = 8;
            panelFicha.rango_ = "2";
            panelFicha.numFichas_ = "x8";
            listPanelFichas.Add(panelFicha);
            //chicle sandia
            panelFicha.ficha_ = new Ficha(-1, resourceLoader.GetString("NameBombaSandia"), -1, -1);
            panelFicha.num_ = 1;
            panelFicha.rango_ = "1";
            panelFicha.numFichas_ = "x1";
            listPanelFichas.Add(panelFicha);
            //petazetas
            panelFicha.ficha_ = new Ficha(-1, resourceLoader.GetString("NamePtazeta"), -1, -1);
            panelFicha.num_ = 6;
            panelFicha.rango_ = "B";
            panelFicha.numFichas_ = "x6";
            listPanelFichas.Add(panelFicha);
            //fresa
            panelFicha.ficha_ = new Ficha(-1, resourceLoader.GetString("NameFresa"), -1, -1);
            panelFicha.num_ = 1;
            panelFicha.rango_ = "F";
            panelFicha.numFichas_ = "x1";
            listPanelFichas.Add(panelFicha);
        }
        void inicializarPartida()
        {
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();

            Tablero = new casillaTablero[10, 10];
            FichasEnemigo = new List<Ficha>();
            FichasJugador = new List<Ficha>();


            FichaInicial fichainicial = new FichaInicial(new Ficha(0, resourceLoader.GetString("NameRegaliz"), -1, -1),1);         
            listFichasIniciales1.Add(fichainicial);

            fichainicial = new FichaInicial(new Ficha(1, resourceLoader.GetString("NameBaston"), -1, -1),2);
            listFichasIniciales1.Add(fichainicial);

            fichainicial = new FichaInicial(new Ficha(2, resourceLoader.GetString("NameDedo"), -1, -1),3);            
            listFichasIniciales1.Add(fichainicial);

            fichainicial = new FichaInicial(new Ficha(3, resourceLoader.GetString("NameHuevo"), -1, -1),4);         
            listFichasIniciales1.Add(fichainicial);

            fichainicial = new FichaInicial(new Ficha(4, resourceLoader.GetString("NameSandia"), -1, -1),4);            
            listFichasIniciales1.Add(fichainicial);

            fichainicial = new FichaInicial(new Ficha(5, resourceLoader.GetString("NameCocacola"), -1, -1),8);           
            listFichasIniciales1.Add(fichainicial);

            fichainicial = new FichaInicial(new Ficha(6, resourceLoader.GetString("NameBombaSandia"), -1, -1),1);           
            listFichasIniciales1.Add(fichainicial);

            fichainicial = new FichaInicial(new Ficha(7, resourceLoader.GetString("NamePtazeta"), -1, -1),6);          
            listFichasIniciales1.Add(fichainicial);

            fichainicial = new FichaInicial(new Ficha(8, resourceLoader.GetString("NameFresa"), -1, -1),1);          
            listFichasIniciales1.Add(fichainicial);

            //OBSTACULOS DEL MAPA
            Tablero[4, 2].hayFicha = true;
            Tablero[4, 2].esJug = true;
            Tablero[4, 3].hayFicha = true;
            Tablero[4, 3].esJug = true;
            Tablero[5, 2].hayFicha = true;
            Tablero[5, 2].esJug = true;
            Tablero[5, 3].hayFicha = true;
            Tablero[5, 3].esJug = true;

            Tablero[4, 6].hayFicha = true;
            Tablero[4, 6].esJug = true;
            Tablero[4, 7].hayFicha = true;
            Tablero[4, 7].esJug = true;
            Tablero[5, 6].hayFicha = true;
            Tablero[5, 6].esJug = true;
            Tablero[5, 7].hayFicha = true;
            Tablero[5, 7].esJug = true;

            //FICHAS DEL ENEMIGO
            Random RND = new Random();
            int x, y;
            string type;
            for (int i = 0; i < 30; i++) {
                if (i <= 0) type = resourceLoader.GetString("NameRegaliz");
                else if (i < 3) type = resourceLoader.GetString("NameBaston");
                else if (i < 6) type = resourceLoader.GetString("NameDedo");
                else if (i < 10) type = resourceLoader.GetString("NameHuevo");
                else if (i < 14) type = resourceLoader.GetString("NameSandia");
                else if (i < 22) type = resourceLoader.GetString("NameCocacola");
                else if (i < 23) type = resourceLoader.GetString("NameBombaSandia");
                else if (i < 29) type = resourceLoader.GetString("NamePtazeta");
                else type = resourceLoader.GetString("NameFresa");

                x = RND.Next(0, 10);
                y = RND.Next(0, 3);
                while (Tablero[y, x].hayFicha) {
                    x = RND.Next(0, 10);
                    y = RND.Next(0, 3);
                }
                Tablero[y, x].hayFicha =true;
                Tablero[y, x].esJug = false;
                
                Ficha eng = new Ficha(FichasEnemigo.Count(), type, x, y);
                string nombre = "_" + y.ToString() + x.ToString();
                Image aux = Grid_Tablero.FindName(nombre) as Image;
                aux.Source = new BitmapImage(new Uri("ms-appx:///Assets/fichaEnemiga.png", UriKind.RelativeOrAbsolute)); 
                FichasEnemigo.Add(eng);
            }
        }
        void Add()
        {
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();

            if (!atack)
            {
                if (playersTurn){
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
                            Text = resourceLoader.GetString("LetreroTurnoJugador")
                        }
                    });
                }
                else{
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
                            Text = resourceLoader.GetString("LetreroTurnoEnemigo")
                        }
                    });
                }
            }
            else
            {
                AttackStackPanel.Children.Add(new Image() 
                { 
                    Source = new BitmapImage(new Uri("ms-appx:///Assets/fire.png", UriKind.RelativeOrAbsolute)),
                    Height=50
                });

                AttackStackPanel.Children.Add(new Border() 
                {
                    Width = 150,
                    Height = 30,
                    BorderThickness = new Thickness(3),
                    Background = new SolidColorBrush(Windows.UI.Colors.Yellow),
                    BorderBrush = new SolidColorBrush(Windows.UI.Colors.Orange),
                    Child = new TextBlock()
                    {
                        TextAlignment = TextAlignment.Center,
                        Foreground = new SolidColorBrush(Windows.UI.Colors.Orange),
                        TextWrapping = TextWrapping.Wrap,
                        FontSize = 15,
                        FontFamily = new FontFamily("MV Boli"),
                        FontWeight = FontWeights.Bold,
                        Text = resourceLoader.GetString("LetreroAtaque")
                    }
                });

                AttackStackPanel.Children.Add(new Image()
                {
                    Source = new BitmapImage(new Uri("ms-appx:///Assets/fire.png", UriKind.RelativeOrAbsolute)),
                    Height = 50
                });
            }
        }
        void clearStackPanel(){
            EntranceStackPanel.Children.Clear();
        }
        void clearAttackStackPanel()
        {
            AttackStackPanel.Children.Clear();
        }
        private void GridView_DragItemsStarting(object sender, DragItemsStartingEventArgs e) {
            FichaInicial aux = e.Items[0] as FichaInicial;
            string id = aux.ficha_.id_.ToString();
            e.Data.SetText(id);
            e.Data.RequestedOperation = DataPackageOperation.Copy;
        }
        private void Image_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Copy;
        }
        private async void Image_Drop(object sender, DragEventArgs e) {
            var id = await e.DataView.GetTextAsync();
            int aux = int.Parse(id);
            Image Receptor = sender as Image;
            
            string aux2 = Receptor.Name;
            Receptor.Visibility = Visibility.Visible;
            int y= (int)Char.GetNumericValue(aux2[1]),x= (int)Char.GetNumericValue(aux2[2]);

            if (Tablero[y, x].esJug) {
                VolverAcasillaInicial(x, y);
            }
            Ficha nueva = new Ficha(FichasJugador.Count(), listFichasIniciales1[aux].ficha_.tipo_, x, y);

            RemoverFichaInicial(aux);
            Receptor.Source = nueva.img_.Source;
           
            FichasJugador.Add(nueva);
            Tablero[nueva.Y_, nueva.X_].hayFicha = true;
            Tablero[nueva.Y_, nueva.X_].esJug = true;

            //si ya ha puesto todas sus fichas se inicia la partida
            ComprobacionEstadoInicial();

        }
        private void RemoverFichaInicial(int aux){
            listFichasIniciales1[aux].cantidad_--;
            if (listFichasIniciales1[aux].cantidad_ < 1)
            {
                for (int i = 1; i + aux < listFichasIniciales1.Count(); i++)
                    listFichasIniciales1[i + aux].ficha_.id_--;
                listFichasIniciales1.Remove(listFichasIniciales1[aux]);
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs((listFichasIniciales1[aux].cantidad_).ToString()));
            
        }
        private void ComprobacionEstadoInicial(){
            if (FichasJugador.Count() >= 30){
                EstadoInicial.Visibility = Visibility.Collapsed;
                estadoinicial = false;

                timer.Start();
            }
        }
        private void Image_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Windows.UI.Xaml.Input.Pointer ptr = e.Pointer;
            if (ptr.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse && !playersTurn && !estadoinicial)
            {//si es el turno del jugador y ha clickeado
                Image x = sender as Image;//acedo a la imagen en la que se ha dropeado la ficha
                string Name = x.Name;//accedo a su nombre              

                //cacheo la fila y columna
                int fil = (int)Char.GetNumericValue(Name[1]);
                int col = (int)Char.GetNumericValue(Name[2]);
                Windows.UI.Input.PointerPoint ptrPt = e.GetCurrentPoint(sender as Image);

                //SI NO ES UNA CASILLA DEL JUGADOR Y HAY UNA CASILLA SELECCIONADA
                if (hayAlgunaFichaSeleccionada && ptrPt.Properties.IsLeftButtonPressed)
                {
                    //recorrer las lista de coordenasdas con los posibles movimientos de la ficha seleccionada y
                    //si se corresponde con la casilla en la que se ha hecho click mover la fichas
                    for (int i = 0; i < posiblesMovimientos.Count(); i++)
                    {
                        if (posiblesMovimientos[i].x_ == col && posiblesMovimientos[i].y_ == fil)
                        {
                            int enemigo = -1;

                            for (int k = 0; k < FichasEnemigo.Count(); k++)
                            {
                                if (FichasEnemigo[k].X_ == col && FichasEnemigo[k].Y_ == fil)
                                {
                                    // FichasEnemigo.Remove(FichasEnemigo[k]);
                                    enemigo = k;
                                    break;
                                }
                            }
                            if (enemigo != -1)
                            {
                                atack = true;
                                Add();
                                atack = false;
                            }
                            //quito la antigua casilla
                            Tablero[FichasJugador[seleccion].Y_, FichasJugador[seleccion].X_].hayFicha = false;
                            Tablero[FichasJugador[seleccion].Y_, FichasJugador[seleccion].X_].esJug = false;
                            eliminarCasillaSeleccionadas();
                            //gana el enemigo
                            if (enemigo != -1 && (FichasEnemigo[enemigo].rango_ == "B" || FichasEnemigo[enemigo].rango_[0] > FichasJugador[seleccion].rango_[0]))
                            {
                                Image aux = Grid_Tablero.FindName("_" + FichasJugador[seleccion].Y_.ToString() + FichasJugador[seleccion].X_.ToString()) as Image;
                                aux.Source = new BitmapImage();//le quito la imagen                                                              
                                FichasJugador.Remove(FichasJugador[seleccion]);
                            }
                            //empate
                            else if (enemigo != -1 && FichasEnemigo[enemigo].rango_ == FichasJugador[seleccion].rango_)
                            {
                                Image aux = Grid_Tablero.FindName("_" + FichasJugador[seleccion].Y_.ToString() + FichasJugador[seleccion].X_.ToString()) as Image;
                                aux.Source = new BitmapImage();//le quito la imagen
                                FichasJugador.Remove(FichasJugador[seleccion]);

                                aux = Grid_Tablero.FindName("_" + FichasEnemigo[enemigo].Y_.ToString() + FichasEnemigo[enemigo].X_.ToString()) as Image;
                                aux.Source = new BitmapImage();//le quito la imagen
                                FichasEnemigo.Remove(FichasEnemigo[enemigo]);
                            }
                            //gana jugador
                            else
                            {
                                Image aux = Grid_Tablero.FindName("_" + FichasJugador[seleccion].Y_.ToString() + FichasJugador[seleccion].X_.ToString()) as Image;
                                aux.Source = new BitmapImage();//le quito la imagen
                                                               //establezco las nuevas coordenadas de la ficha
                                FichasJugador[seleccion].X_ = col;
                                FichasJugador[seleccion].Y_ = fil;

                                aux = Grid_Tablero.FindName("_" + fil.ToString() + col.ToString()) as Image;
                                aux.Source = FichasJugador[seleccion].img_.Source;//pongo la imagen en las nuevas coordenadas

                                //actualizo el tablero
                                Tablero[fil, col].hayFicha = true;
                                Tablero[fil, col].esJug = true;

                                if (enemigo != -1)
                                {
                                    FichasEnemigo.Remove(FichasEnemigo[enemigo]);
                                }
                            }
                            posiblesMovimientos.Clear();//limpio los posibles movimientos
                            hayAlgunaFichaSeleccionada = false;//ya no hay ninguna ficha seleccionada
                            double total = FichasEnemigo.Count() + FichasJugador.Count();
                            double valor = FichasJugador.Count() / total;
                            ProgressBar.Value = valor * 100;

                            return;
                        }
                    }
                }

                //MIRO SI HE SELECCIONADO ALGUNA CASILLA DEL JUGADOR
                for (int i = 0; i < FichasJugador.Count; i++) {//recorro la lista de fichas del jugador
                    if (FichasJugador[i].Y_ ==fil && FichasJugador[i].X_ == col) {//si es la ficha del jugador
                       
                        //CLICK DERECHO
                        if (ptrPt.Properties.IsRightButtonPressed) {
                            ImagenCartaJugador.Source = x.Source;//pongo la imagend e la ficha
                            PanelInfoJugador.Visibility = Visibility.Visible;//activo el panel
                            //actualizo la info
                            InfoCartaJugador.Text = FichasJugador[i].info_;
                            RangoCartaJugador.Text = FichasJugador[i].rango_;
                            CartaInfoJugador.Text = FichasJugador[i].tipo_;
                        }
                        //CLICK IZQUIERDO
                        if (ptrPt.Properties.IsLeftButtonPressed) {

                            //CAMBIAR LA IMAGEN DE LA FICHA POR LA DE CONTORNO DORADITO
                            eliminarCasillaSeleccionadas();
                            //Nos guardamos lo q hemos seleccionado este indice nos servira para luego acceder a el en la lista
                            seleccion = i;
                            posiblesMovimientos.Clear();
                            //eliminarCasillaSeleccionadas();
                            hayAlgunaFichaSeleccionada = true;
                            //cambiamos la imagen de las posibles casillas de movimientos
                            //petazeta
                            if (FichasJugador[i].rango_ == "B") ;//como no se mueve pues no hace nada
                            //cocacola
                            else if (FichasJugador[i].rango_ == "2") {
                                seleccionarCasillasCocacola();
                            }
                            //resto de fichas que tienen un movimiento normal
                            else {
                                seleccionarCasillasNormal();
                            }
                        }
                    }
                }              
            }
            e.Handled = true;
        }
        private void seleccionarCasillasCocacola() {
            coords posFichaSelecionada = new coords(FichasJugador[seleccion].Y_, FichasJugador[seleccion].X_);           
            int i = posFichaSelecionada.x_ - 1; bool salir = false;
            //izquierda
            while (i >=0&&!salir){
                if (!Tablero[posFichaSelecionada.y_, i].esJug){
                    if (Tablero[posFichaSelecionada.y_, i].hayFicha) salir = true;

                    posiblesMovimientos.Add(new coords(posFichaSelecionada.y_, i));
                    seleccionarCasilla(i, posFichaSelecionada.y_);
                }
                else salir = true;
                i--;
            }
            i = posFichaSelecionada.x_ + 1;  salir = false;
            //derecha
            while (i < Tablero.GetLength(1) && !salir)
            {
                if (!Tablero[posFichaSelecionada.y_, i].esJug){
                    if (Tablero[posFichaSelecionada.y_, i].hayFicha) salir = true;

                    posiblesMovimientos.Add(new coords(posFichaSelecionada.y_, i));
                    seleccionarCasilla(i, posFichaSelecionada.y_);
                }
                else salir = true;
                i++;
            }
            i = posFichaSelecionada.y_ - 1; salir = false;
            //arriba
            while (i >= 0 && !salir)
            {
                if (!Tablero[i, posFichaSelecionada.x_].esJug)
                {
                    if (Tablero[i, posFichaSelecionada.x_].hayFicha) salir = true;

                    posiblesMovimientos.Add(new coords(i, posFichaSelecionada.x_));
                    seleccionarCasilla(posFichaSelecionada.x_, i);
                }
                else salir = true;
                i--;
            }
            i = posFichaSelecionada.y_ + 1; salir = false;
            //abajo
            while (i < Tablero.GetLength(0) && !salir)
            {
                if (!Tablero[i, posFichaSelecionada.x_].esJug){
                    if (Tablero[i, posFichaSelecionada.x_].hayFicha) salir = true;

                    posiblesMovimientos.Add(new coords(i, posFichaSelecionada.x_));
                    seleccionarCasilla(posFichaSelecionada.x_, i);
                }
                else salir = true;
                i++;
            }
        }
        private void seleccionarCasillasNormal() {
            coords posFichaSelecionada = new coords( FichasJugador[seleccion].Y_, FichasJugador[seleccion].X_);
            //arriba
            if (posFichaSelecionada.y_-1>=0&& //para que no se salga del tablero             
                !Tablero[posFichaSelecionada.y_-1, posFichaSelecionada.x_].esJug) {//si hay alguna ficha del jugador no queremos que sea un posible movimiento
                posiblesMovimientos.Add(new coords(posFichaSelecionada.y_ - 1,posFichaSelecionada.x_));
                seleccionarCasilla(posFichaSelecionada.x_, posFichaSelecionada.y_ - 1);
            }
            //abajo
            if (posFichaSelecionada.y_ + 1 < Tablero.GetLength(0) && //para que no se salga del tablero             
                 !Tablero[posFichaSelecionada.y_+1, posFichaSelecionada.x_].esJug){//si hay alguna ficha del jugador no queremos que sea un posible movimiento
                posiblesMovimientos.Add(new coords(posFichaSelecionada.y_ +1, posFichaSelecionada.x_));
                seleccionarCasilla(posFichaSelecionada.x_, posFichaSelecionada.y_ + 1);
            }
            //derecha
            if (posFichaSelecionada.x_-1>=0&&
                !Tablero[posFichaSelecionada.y_, posFichaSelecionada.x_-1].esJug){
                posiblesMovimientos.Add(new coords(posFichaSelecionada.y_ , posFichaSelecionada.x_-1));
                seleccionarCasilla(posFichaSelecionada.x_-1, posFichaSelecionada.y_);
            }
            //izquierda
            if (posFichaSelecionada.x_ + 1 <Tablero.GetLength(1) &&
                !Tablero[posFichaSelecionada.y_, posFichaSelecionada.x_ + 1].esJug){
                posiblesMovimientos.Add(new coords(posFichaSelecionada.y_, posFichaSelecionada.x_ + 1));
                seleccionarCasilla(posFichaSelecionada.x_ + 1, posFichaSelecionada.y_ );
            }
        }
        private void eliminarCasillaSeleccionadas(int i = -1){
            for (int j = 0; j < posiblesMovimientos.Count(); j++){                
                DeseleccionarCasilla(posiblesMovimientos[j].x_, posiblesMovimientos[j].y_);
            }
        }
        private void seleccionarCasilla(int x,int y) {
            Image aux = Grid_Tablero.FindName("_" + y.ToString() + x.ToString()) as Image;
            if (Tablero[y, x].hayFicha) //es enemiga
                aux.Source = new BitmapImage(new Uri("ms-appx:///Assets/fichaEnemiga2.png", UriKind.RelativeOrAbsolute));
            else//No hay ficha
                aux.Source = new BitmapImage(new Uri("ms-appx:///Assets/borde.png", UriKind.RelativeOrAbsolute));
        }
        private void DeseleccionarCasilla(int x,int y){
            Image aux = Grid_Tablero.FindName("_" + y.ToString() + x.ToString()) as Image;
            if (Tablero[y, x].hayFicha) //es enemiga
                aux.Source = new BitmapImage(new Uri("ms-appx:///Assets/fichaEnemiga.png", UriKind.RelativeOrAbsolute));
            else//No hay ficha
                aux.Source = new BitmapImage();
        }
        private void VolverAcasillaInicial(int x,int y){
            for (int i = 0; i < FichasJugador.Count(); i++){
                if(FichasJugador[i].X_==x && FichasJugador[i].Y_ == y){
                    listFichasIniciales1.Add(new FichaInicial(new Ficha(listFichasIniciales1.Count(), FichasJugador[i].tipo_,-1,-1), 1));
                    FichasJugador.Remove(FichasJugador[i]);                   
                }
            }
        }
        private void GridView_ItemClick(object sender, ItemClickEventArgs e){
            FichaInicial ficha=e.ClickedItem as FichaInicial;
            for (int i = 7; i < Tablero.GetLength(0); i++){
                for (int j = 0; j < Tablero.GetLength(1); j++){
                    if (!Tablero[i, j].esJug){
                        Image aux = Grid_Tablero.FindName("_" + i.ToString() + j.ToString()) as Image;                     
                        aux.Source = ficha.ficha_.img_.Source;                       
                        RemoverFichaInicial(ficha.ficha_.id_);
                        Tablero[i, j].esJug = true;
                        Tablero[i, j].hayFicha = true;
                        FichasJugador.Add(new Ficha(FichasJugador.Count,ficha.ficha_.tipo_, j, i));
                        ComprobacionEstadoInicial();
                        return;
                    }
                }
            }
        }
    }
    
}
