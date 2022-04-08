using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;

namespace ProyectoDSI
{
    struct PanelFicha
    {
        public Ficha ficha_;
        public int num_;
        public string rango_;
        public string numFichas_;
    }
   public  class FichaInicial{
       
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
            //info
            if(tipo == resourceLoader.GetString("NameFresa"))
            {
                info_ = resourceLoader.GetString("InfoFresa");
                rango_ = "F";
            }
            else if(tipo== resourceLoader.GetString("NameCocacola"))
            {
                info_ = resourceLoader.GetString("InfoCola");
                rango_ = "1";
            }
            else if(tipo== resourceLoader.GetString("NameDedo"))
            {
                info_ = resourceLoader.GetString("InfoDedo");
                rango_ = "5";
            }
            else if(tipo== resourceLoader.GetString("NamePtazeta"))
            {
                info_ = resourceLoader.GetString("InfoPtazetas");
                rango_ = "B";
            }
            else if(tipo== resourceLoader.GetString("NameSandia"))
            {
                info_ = resourceLoader.GetString("InfoSandia");
                rango_ = "3";
            }
            else if(tipo== resourceLoader.GetString("NameBombaSandia"))
            {
                info_ = resourceLoader.GetString("InfoBomba");
                rango_ = "1";
            }
            else if (tipo== resourceLoader.GetString("NameHuevo"))
            {
                info_ = resourceLoader.GetString("InfoHuevo");
                rango_ = "4";
            }
            else if(tipo== resourceLoader.GetString("NameBaston"))
            {
                info_ = resourceLoader.GetString("InfoBaston");
                rango_ = "6";
            }
            else if(tipo== resourceLoader.GetString("NameRegaliz"))
            {
                info_ = resourceLoader.GetString("InfoRegaliz");
                rango_ = "7";
            }
            //imagen
            img_ = new Image();
            dirImg_ = "ms-appx:///Assets/" + tipo+".png";
            //string s = System.IO.Directory.GetCurrentDirectory() + "\\" + dirImg_;           
            img_.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(dirImg_));
            //contentcontrol
            ccImg_ = new ContentControl();
            ccImg_.Content = img_;
            ccImg_.UseSystemFocusVisuals = true;          
        }
       public  void setID()
       {
            id_--;
       }
    }
}
