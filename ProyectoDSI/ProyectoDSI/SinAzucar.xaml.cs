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
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace ProyectoDSI
{
    struct PanelFicha{
        public Ficha ficha_;
        public int num_;         
    }
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class SinAzucar : Page
    {
        List<PanelFicha> listPanelFichas;
        public SinAzucar()
        {
            this.InitializeComponent();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Pausa));
        }
        protected override void OnNavigatedTo(NavigationEventArgs e){
            listPanelFichas = new List<PanelFicha>();
            PanelFicha panelFicha;
            //regaliz
            panelFicha.ficha_ = new Ficha(-1, "Regaliz", -1, -1);
            panelFicha.num_ = 1;
            listPanelFichas.Add(panelFicha);
            //baston
            panelFicha.ficha_ = new Ficha(-1, "Baston", -1, -1);
            panelFicha.num_ = 2;
            listPanelFichas.Add(panelFicha);
            //dedos
            panelFicha.ficha_ = new Ficha(-1, "Dedo", -1, -1);
            panelFicha.num_ = 3;
            listPanelFichas.Add(panelFicha);
            //huevo
            panelFicha.ficha_ = new Ficha(-1, "Huevo", -1, -1);
            panelFicha.num_ = 4;
            listPanelFichas.Add(panelFicha);
            //sandia
            panelFicha.ficha_ = new Ficha(-1, "Sandia", -1, -1);
            panelFicha.num_ = 4;
            listPanelFichas.Add(panelFicha);
            //cocacola
            panelFicha.ficha_ = new Ficha(-1, "Cocacola", -1, -1);
            panelFicha.num_ = 8;
            listPanelFichas.Add(panelFicha);
            //chicle sandia
            panelFicha.ficha_ = new Ficha(-1, "BombaSandia", -1, -1);
            panelFicha.num_ = 1;
            listPanelFichas.Add(panelFicha);
            //petazetas
            panelFicha.ficha_ = new Ficha(-1, "Petazeta", -1, -1);
            panelFicha.num_ = 6;
            listPanelFichas.Add(panelFicha);
            //fresa
            panelFicha.ficha_ = new Ficha(-1, "Fresa", -1, -1);
            panelFicha.num_ = 1;
            listPanelFichas.Add(panelFicha);
          
        }

        private void Grid_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if(e.Key==VirtualKey.Escape) 
                Frame.Navigate(typeof(Pausa));
        }
      
    }
}
