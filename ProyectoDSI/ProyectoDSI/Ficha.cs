using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;

namespace ProyectoDSI
{
    public class PanelFicha
    {
        public PanelFicha(){

        }
        public PanelFicha(Ficha ficha,int num,string rango,string numFichas){
            ficha_ = ficha;
            num_ = num;
            rango_ = rango;
            numFichas_ = numFichas;
        }
        public Ficha ficha_;
        public int num_;
        public string rango_;
        public string numFichas_;
    }
   public  class FichaInicial{
        public FichaInicial() {
            
        }
        public FichaInicial(Ficha f,int k){
            ficha_ = f;
            cantidad_ = k;
        }
        public Ficha ficha_;
        public int cantidad_;
    }
    public class Ficha{

        public int id_ { get; set; } //todas las fichas estaran en una lista y este sera su indicide en ekl que se encuentra en dicha lista
        public string tipo_;
        public string info_;
        public string rango_;
        //imagen
        public string dirImg_;
        public Image img_;
        public ContentControl ccImg_;
        //POSICION(DEBEMOS CONSEGUIR ASOCIAR LA X A LAS COLUMNAS Y LAS Y A LAS FILAS)
        public int X_ { get; set; }
        public int Y_ { get; set; }
        public int Angulo_;      
        public CompositeTransform Transformation_;

        public Ficha(int id,string tipo,int posX, int posY){
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
            id_ = id;
            tipo_ = tipo;
            //inicializo posicion que en un principio colocara la imagen en su respectiva columna y fila
            X_ = posX;
            Y_ = posY;
            Angulo_ = 0;
            Transformation_ = new CompositeTransform();
            img_ = new Image();
            //info
            if (tipo == resourceLoader.GetString("NameFresa"))
            {
                info_ = resourceLoader.GetString("InfoFresa");
                rango_ = "F";
                dirImg_ = "ms-appx:///Assets/fresa.png";
                img_.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(dirImg_));
            }
            else if(tipo== resourceLoader.GetString("NameCocacola"))
            {
                info_ = resourceLoader.GetString("InfoCola");
                rango_ = "2";
                dirImg_ = "ms-appx:///Assets/Cocacola.png";
                img_.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(dirImg_));
            }
            else if(tipo== resourceLoader.GetString("NameDedo"))
            {
                info_ = resourceLoader.GetString("InfoDedo");
                rango_ = "5";
                dirImg_ = "ms-appx:///Assets/Dedo.png";
                img_.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(dirImg_));
            }
            else if(tipo== resourceLoader.GetString("NamePtazeta"))
            {
                info_ = resourceLoader.GetString("InfoPtazetas");
                rango_ = "B";
                dirImg_ = "ms-appx:///Assets/Petazeta.png";
                img_.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(dirImg_));
            }
            else if(tipo== resourceLoader.GetString("NameSandia"))
            {
                info_ = resourceLoader.GetString("InfoSandia");
                rango_ = "3";
                dirImg_ = "ms-appx:///Assets/Sandia.png";
                img_.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(dirImg_));
            }
            else if(tipo== resourceLoader.GetString("NameBombaSandia"))
            {
                info_ = resourceLoader.GetString("InfoBomba");
                rango_ = "1";
                dirImg_ = "ms-appx:///Assets/BombaSandia.png";
                img_.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(dirImg_));
            }
            else if (tipo== resourceLoader.GetString("NameHuevo"))
            {
                info_ = resourceLoader.GetString("InfoHuevo");
                rango_ = "4";
                dirImg_ = "ms-appx:///Assets/Huevo.png";
                img_.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(dirImg_));
            }
            else if(tipo== resourceLoader.GetString("NameBaston"))
            {
                info_ = resourceLoader.GetString("InfoBaston");
                rango_ = "6";
                dirImg_ = "ms-appx:///Assets/Baston.png";
                img_.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(dirImg_));
            }
            else if(tipo== resourceLoader.GetString("NameRegaliz"))
            {
                info_ = resourceLoader.GetString("InfoRegaliz");
                rango_ = "7";
                dirImg_ = "ms-appx:///Assets/regaliz.png";
                img_.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(dirImg_));
            }
            //imagen
           
            //contentcontrol
            ccImg_ = new ContentControl();
            ccImg_.Content = img_;
            ccImg_.UseSystemFocusVisuals = true;          
        }      
    }
}
