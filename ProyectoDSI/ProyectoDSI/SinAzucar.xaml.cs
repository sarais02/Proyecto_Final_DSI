using System;
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
using Windows.Media.Core;
using Windows.Media.Playback;
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
        MediaPlayer clickSound;
        MediaPlayer attackSound;
        MediaPlayer pressedSound;
        casillaTablero[,] Tablero;//PARA VER EN QUE CASILLAS HAY FICHAS
        public ObservableCollection<PanelFicha> PanelFichasIzquierda { get; } = new ObservableCollection<PanelFicha>();//PANEL IZQUIERDO
        public ObservableCollection<FichaInicial> ListaPanelFichasIniciales { get; } = new ObservableCollection<FichaInicial>(); //PANEL INICIAL LISTA ARRIBA

        public event PropertyChangedEventHandler PropertyChanged;
        List<Ficha> FichasJugador;
        List<Ficha> FichasEnemigo;

        //Movimiento
        int seleccion;
        //Ficha fichaSeleccionada;
        bool hayAlgunaFichaSeleccionada_MOUSE = false;
        bool hayAlgunaFichaSeleccionada_KEYBOARD_GAMEPAD = false;
        List<coords> posiblesMovimientos = new List<coords>();

        private DispatcherTimer timer;
        private DispatcherTimer textTimer_;
        private DispatcherTimer attackTimer_;
        private int minutes;
        private int seconds;
        private int initialTime = 2, initTime=2;
        private int currentTime, actualTime;
        private bool playersTurn;
        private bool isStarted = false;
        private bool estadoinicial=true;
        private bool atack = false;

        
        Image mi;
        coords Pos;
        public SinAzucar() {
            this.InitializeComponent();
            minutes = 1;
            seconds = 30;

            timer = new DispatcherTimer();
            currentTime = initialTime;
            textTimer_ = new DispatcherTimer();
            actualTime = initTime;
            attackTimer_ = new DispatcherTimer();
            textTimer_.Interval = new TimeSpan(0, 0, 1);
            attackTimer_.Interval = new TimeSpan(0, 0, 1);
            timer.Interval = new TimeSpan(0, 0, 1);

            clickSound = new MediaPlayer();
            attackSound = new MediaPlayer();
            pressedSound = new MediaPlayer();
            textTimer_.Tick += textTimer_Tick;
            attackTimer_.Tick += attackTimer_Tick;
            timer.Tick += timer_Tick;
            CountDown.Text = "0" + minutes.ToString() + ":" + seconds.ToString();
            textTimer_.Start();

            playersTurn = true;
            Add();
            CrearLista();
            inicializarPartida();

            this.NavigationCacheMode = NavigationCacheMode.Enabled;
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
                if (seconds == 0) { playersTurn = !playersTurn; seconds = 30; minutes = 1; Add(); textTimer_.Start(); }
                else seconds--;

                if (seconds < 10) CountDown.Text = "0" + minutes.ToString() + ":0" + seconds.ToString();
                else CountDown.Text = "0" + minutes.ToString() + ":" + seconds.ToString();
            }
        }
        void textTimer_Tick(object sender, object e)
        {
            if (currentTime > 0)
                currentTime--;
            
            else
            {
                textTimer_.Stop();
                currentTime = initialTime;
                clearStackPanel();
            }
        }
        void attackTimer_Tick(object sender, object e)
        {
            if (actualTime > 0)
                actualTime--;
            
            else
            {
                attackTimer_.Stop();
                actualTime = initTime;
                clearAttackStackPanel();
                PanelInfoEnemigo.Visibility = Visibility.Collapsed;
                PanelInfoJugador.Visibility = Visibility.Collapsed;
            }
        }
        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            clickSound.Play();
            timer.Stop();
            Frame.Navigate(typeof(Pausa));
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e) {
            if(isStarted) timer.Start();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            //SONIDOS
            Windows.Storage.StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets");
            Windows.Storage.StorageFile file = await folder.GetFileAsync("bottonclick.wav");
            clickSound.Source = MediaSource.CreateFromStorageFile(file);
            file = await folder.GetFileAsync("attacksound.wav");
            attackSound.Source = MediaSource.CreateFromStorageFile(file);
            file = await folder.GetFileAsync("click.wav");
            pressedSound.Source = MediaSource.CreateFromStorageFile(file);
        }
        private void Grid_KeyDown(object sender, KeyRoutedEventArgs e){
            if (estadoinicial) return;
            bool mover = false;
           
                switch (e.Key){
                case VirtualKey.Escape:
                    Frame.Navigate(typeof(Pausa));
                    break;
                case VirtualKey.Space:
                case VirtualKey.Enter:
                    MoverCasillaSeleccionada(Pos.y_, Pos.x_, ref hayAlgunaFichaSeleccionada_KEYBOARD_GAMEPAD);
                    e.Handled = true;
                    break;
                case VirtualKey.GamepadA:
                    MoverCasillaSeleccionada(Pos.y_, Pos.x_, ref hayAlgunaFichaSeleccionada_KEYBOARD_GAMEPAD);
                    e.Handled = true;
                    break;
                case VirtualKey.Left:
                    mover = true;
                    Pos.x_--;
                    if (Pos.x_ < 0) Pos.x_ = 0;
                    e.Handled = true;
                    break;
                case VirtualKey.Right:
                    mover = true;
                    Pos.x_++;
                    if (Pos.x_ > Tablero.GetLength(1)) Pos.x_ = Tablero.GetLength(1) - 1;
                    e.Handled = true;
                    break;
                case VirtualKey.Up:
                    mover = true;
                    Pos.y_--;
                    if (Pos.y_ < 0) Pos.y_ = 0;
                    e.Handled = true;
                    break;
                case VirtualKey.Down:
                    mover = true;
                    Pos.y_++;
                    if (Pos.y_ > Tablero.GetLength(0)) Pos.y_ = Tablero.GetLength(0) - 1;
                    e.Handled = true;
                    break;
                default:
                    e.Handled = true;
                    break;
            }
            if (mover){
                miContentControl.Visibility = Visibility.Visible;
                miContentControl.SetValue(Grid.RowProperty, Pos.y_);
                miContentControl.SetValue(Grid.ColumnProperty, Pos.x_);
                seleccionarCasillaKey_GamePad(Pos.y_, Pos.x_);
            }
        }
        private void CrearLista() {

            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();

            //regaliz
            PanelFichasIzquierda.Add(new PanelFicha(new Ficha(-1, resourceLoader.GetString("NameRegaliz"), -1, -1), 1, "7", "x1"));
            //baston
            PanelFichasIzquierda.Add(new PanelFicha(new Ficha(-1, resourceLoader.GetString("NameBaston"), -1, -1), 2, "6", "x2"));
            //dedos           
            PanelFichasIzquierda.Add(new PanelFicha(new Ficha(-1, resourceLoader.GetString("NameDedo"), -1, -1), 3, "5", "x3"));
            //huevo         
            PanelFichasIzquierda.Add(new PanelFicha(new Ficha(-1, resourceLoader.GetString("NameHuevo"), -1, -1), 4, "4", "x4"));
            //sandia           
            PanelFichasIzquierda.Add(new PanelFicha(new Ficha(-1, resourceLoader.GetString("NameSandia"), -1, -1), 4, "3", "x4"));
            //cocacola
            PanelFichasIzquierda.Add(new PanelFicha(new Ficha(-1, resourceLoader.GetString("NameCocacola"), -1, -1), 8, "2", "x8"));
            //chicle sandia
            PanelFichasIzquierda.Add(new PanelFicha(new Ficha(-1, resourceLoader.GetString("NameBombaSandia"), -1, -1), 1, "1", "x1"));
            //petazetas          
            PanelFichasIzquierda.Add(new PanelFicha(new Ficha(-1, resourceLoader.GetString("NamePtazeta"), -1, -1), 6, "B", "x1"));
            //fresa
            PanelFichasIzquierda.Add(new PanelFicha(new Ficha(-1, resourceLoader.GetString("NameBombaSandia"), -1, -1), 1, "F", "x1"));
        }
        void inicializarPartida()
        {
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();

            Tablero = new casillaTablero[10, 10];
            FichasEnemigo = new List<Ficha>();
            FichasJugador = new List<Ficha>();
            
            //miContentControl.Visibility = Visibility.Collapsed;         
            Pos = new coords(7, 0);
           

            FichaInicial fichainicial = new FichaInicial(new Ficha(0, resourceLoader.GetString("NameRegaliz"), -1, -1),1);         
            ListaPanelFichasIniciales.Add(fichainicial);

            fichainicial = new FichaInicial(new Ficha(1, resourceLoader.GetString("NameBaston"), -1, -1),2);
            ListaPanelFichasIniciales.Add(fichainicial);

            fichainicial = new FichaInicial(new Ficha(2, resourceLoader.GetString("NameDedo"), -1, -1),3);            
            ListaPanelFichasIniciales.Add(fichainicial);

            fichainicial = new FichaInicial(new Ficha(3, resourceLoader.GetString("NameHuevo"), -1, -1),4);         
            ListaPanelFichasIniciales.Add(fichainicial);

            fichainicial = new FichaInicial(new Ficha(4, resourceLoader.GetString("NameSandia"), -1, -1),4);            
            ListaPanelFichasIniciales.Add(fichainicial);

            fichainicial = new FichaInicial(new Ficha(5, resourceLoader.GetString("NameCocacola"), -1, -1),8);           
            ListaPanelFichasIniciales.Add(fichainicial);

            fichainicial = new FichaInicial(new Ficha(6, resourceLoader.GetString("NameBombaSandia"), -1, -1),1);           
            ListaPanelFichasIniciales.Add(fichainicial);

            fichainicial = new FichaInicial(new Ficha(7, resourceLoader.GetString("NamePtazeta"), -1, -1),6);          
            ListaPanelFichasIniciales.Add(fichainicial);

            fichainicial = new FichaInicial(new Ficha(8, resourceLoader.GetString("NameFresa"), -1, -1),1);          
            ListaPanelFichasIniciales.Add(fichainicial);

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
                PanelDeFichas.Visibility = Visibility.Collapsed;
            }
        }
        void Add()
        {
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
            //TURNO NORMAL
            if (!atack)
            {
                if (playersTurn){
                    if (EntranceStackPanel.Children[0] != null) clearStackPanel();
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
                //ATAQUE
                else{
                    if (EntranceStackPanel.Children[0] != null) clearStackPanel();
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
                if (AttackStackPanel.Children[0] != null) clearAttackStackPanel();
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
            Ficha nueva = new Ficha(FichasJugador.Count(), ListaPanelFichasIniciales[aux].ficha_.tipo_, x, y);

            RemoverFichaInicial(aux);
            Receptor.Source = nueva.img_.Source;
           
            FichasJugador.Add(nueva);
            Tablero[nueva.Y_, nueva.X_].hayFicha = true;
            Tablero[nueva.Y_, nueva.X_].esJug = true;

            //si ya ha puesto todas sus fichas se inicia la partida
            ComprobacionEstadoInicial();
            pressedSound.Play();
        }
        private void RemoverFichaInicial(int aux){
            ListaPanelFichasIniciales[aux].cantidad_--;
            if (ListaPanelFichasIniciales[aux].cantidad_ < 1)
            {
                for (int i = 1; i + aux < ListaPanelFichasIniciales.Count(); i++)
                    ListaPanelFichasIniciales[i + aux].ficha_.id_--;
                ListaPanelFichasIniciales.Remove(ListaPanelFichasIniciales[aux]);
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs((ListaPanelFichasIniciales[aux].cantidad_).ToString()));
            
        }
        private void ComprobacionEstadoInicial(){
            if (FichasJugador.Count() >= 30){
                EstadoInicial.Visibility = Visibility.Collapsed;
                PanelDeFichas.Visibility = Visibility.Visible;
                ImagenInicial.Visibility = Visibility.Collapsed;
               
                estadoinicial = false;
                Grid_Tablero.Children.Remove(EstadoInicial);
                timer.Start();
                isStarted = true;
            }
        }
        private void Image_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Windows.UI.Xaml.Input.Pointer ptr = e.Pointer;
            if (ptr.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse && playersTurn && !estadoinicial)
            {//si es el turno del jugador y ha clickeado
                Image x = sender as Image;//acedo a la imagen en la que se ha dropeado la ficha
                string Name = x.Name;//accedo a su nombre              

                //cacheo la fila y columna
                int fil = (int)Char.GetNumericValue(Name[1]);
                int col = (int)Char.GetNumericValue(Name[2]);
                Windows.UI.Input.PointerPoint ptrPt = e.GetCurrentPoint(sender as Image);

                //SI NO ES UNA CASILLA DEL JUGADOR Y HAY UNA CASILLA SELECCIONADA
                if (hayAlgunaFichaSeleccionada_MOUSE && ptrPt.Properties.IsLeftButtonPressed){
                    MoverCasillaSeleccionada(fil, col, ref hayAlgunaFichaSeleccionada_MOUSE);
                    minutes = 1;
                    seconds = 30;
                    playersTurn = !playersTurn;
                    Add();
                    textTimer_.Start();
                    if (!hayAlgunaFichaSeleccionada_MOUSE) return;
                }

                //MIRO SI HE SELECCIONADO ALGUNA CASILLA DEL JUGADOR
                for (int i = 0; i < FichasJugador.Count; i++) {//recorro la lista de fichas del jugador
                    if (FichasJugador[i].Y_ ==fil && FichasJugador[i].X_ == col) {//si es la ficha del jugador
                       // miContentControl.Visibility = Visibility.Collapsed;//quito el marco de teclado
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
                            hayAlgunaFichaSeleccionada_MOUSE = true;
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
        private void seleccionarCasillaKey_GamePad(int fil,int col){
            //MIRO SI HE SELECCIONADO ALGUNA CASILLA DEL JUGADOR
            for (int i = 0; i < FichasJugador.Count; i++)
            {//recorro la lista de fichas del jugador
                if (FichasJugador[i].Y_ == fil && FichasJugador[i].X_ == col)
                {//si es la ficha del jugador

                    //CAMBIAR LA IMAGEN DE LA FICHA POR LA DE CONTORNO DORADITO
                    eliminarCasillaSeleccionadas();
                    //Nos guardamos lo q hemos seleccionado este indice nos servira para luego acceder a el en la lista
                    seleccion = i;
                    posiblesMovimientos.Clear();
                    //eliminarCasillaSeleccionadas();
                    hayAlgunaFichaSeleccionada_KEYBOARD_GAMEPAD = true;
                    //cambiamos la imagen de las posibles casillas de movimientos
                    //petazeta
                    if (FichasJugador[i].rango_ == "B") ;//como no se mueve pues no hace nada
                                                            //cocacola
                    else if (FichasJugador[i].rango_ == "2"){
                        seleccionarCasillasCocacola();
                    }
                    //resto de fichas que tienen un movimiento normal
                    else{
                        seleccionarCasillasNormal();
                    }
                    
                }
            }
        }
        private void MoverCasillaSeleccionada(int fil,int col, ref bool cambio){
            //SI NO ES UNA CASILLA DEL JUGADOR Y HAY UNA CASILLA SELECCIONADA
            if (cambio){
                //recorrer las lista de coordenasdas con los posibles movimientos de la ficha seleccionada y
                //si se corresponde con la casilla en la que se ha hecho click mover la fichas
                for (int i = 0; i < posiblesMovimientos.Count(); i++)
                {
                    if (posiblesMovimientos[i].x_ == col && posiblesMovimientos[i].y_ == fil)
                    {
                        int enemigo = -1;
                        pressedSound.Play();

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
                            attackSound.Play();
                            atack = true;
                            Add();
                            attackTimer_.Start();
                            atack = false;

                            CartaInfoJugador.Text = FichasJugador[seleccion].tipo_;
                            RangoCartaJugador.Text = FichasJugador[seleccion].rango_;
                            ImagenCartaJugador.Source = FichasJugador[seleccion].img_.Source;
                            InfoCartaJugador.Text = FichasJugador[seleccion].info_;

                            CartaInfoEnemigo.Text = FichasEnemigo[enemigo].tipo_;
                            RangoCartaEnemigo.Text = FichasEnemigo[enemigo].rango_;
                            ImagenCartaEnemigo.Source = FichasEnemigo[enemigo].img_.Source;
                            InfoCartaEnemigo.Text = FichasEnemigo[enemigo].info_;

                            PanelInfoJugador.Visibility = Visibility.Visible;//activo el panel
                            PanelInfoEnemigo.Visibility = Visibility.Visible;
                        }
                        //quito la antigua casilla
                        Tablero[FichasJugador[seleccion].Y_, FichasJugador[seleccion].X_].hayFicha = false;
                        Tablero[FichasJugador[seleccion].Y_, FichasJugador[seleccion].X_].esJug = false;
                        eliminarCasillaSeleccionadas();
                        //gana el enemigo
                        if (enemigo != -1 && (FichasEnemigo[enemigo].rango_ == "B" || FichasJugador[seleccion].rango_ == "B" || FichasEnemigo[enemigo].rango_[0] > FichasJugador[seleccion].rango_[0]))
                        {
                            Image aux = Grid_Tablero.FindName("_" + FichasJugador[seleccion].Y_.ToString() + FichasJugador[seleccion].X_.ToString()) as Image;
                            int num;
                            aux.Source = new BitmapImage();//le quito la imagen
                            if (FichasJugador[seleccion].rango_ == "F") { num = 8; }
                            else
                            {

                                num = FichasJugador[seleccion].rango_[0] - 48;
                                num = Math.Abs(num - 7);
                            }
                            int resta = PanelFichasIzquierda[num].numFichas_[1] - 48;
                            resta--;
                            PanelFichasIzquierda[num].numFichas_ = PanelFichasIzquierda[num].numFichas_[0] + resta.ToString();
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
                            Tablero[FichasEnemigo[enemigo].Y_, FichasEnemigo[enemigo].X_].hayFicha = false;//quito la marca del tablero
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
                        cambio= false;//ya no hay ninguna ficha seleccionada
                        double total = FichasEnemigo.Count() + FichasJugador.Count();
                        double valor = FichasJugador.Count() / total;
                        ProgressBar.Value = valor * 100;
                        return;
                    }
                }
            }
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
                    ListaPanelFichasIniciales.Add(new FichaInicial(new Ficha(ListaPanelFichasIniciales.Count(), FichasJugador[i].tipo_,-1,-1), 1));
                    FichasJugador.Remove(FichasJugador[i]);                   
                }
            }
        }
        private void GridView_ItemClick(object sender, ItemClickEventArgs e){
            FichaInicial ficha=e.ClickedItem as FichaInicial;
            pressedSound.Play();
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
        private void PanelIzquierdo_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Tab)
            {
                e.Handled = true;
                DependencyObject candidate = null;
                candidate = FocusManager.FindNextFocusableElement(FocusNavigationDirection.Up);
               
            }
        }
    }   
}
