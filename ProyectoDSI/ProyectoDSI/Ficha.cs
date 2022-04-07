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
            switch (tipo)
            {
                case "Fresa":
                    info_ = resourceLoader.GetString("InfoFresa");
                    rango_ = "F";
                    break;

                case "Cocacola":
                    info_ = resourceLoader.GetString("InfoCola");
                    rango_ = "1";
                    break;

                case "Dedo":
                    info_ = "Tiene ciertos parecidos con los humanos," +
                        " muchas chuches se asustan al verlos aparecer y " +
                        "más si se agrupan de 5 en 5";
                    rango_ = "5";
                    break;

                case "Petazeta":
                    info_ = "Muy buenos es su trabajo, explotan y arrasan todo a su paso, dicen que algunos han dejado tuertos a humanos";
                    rango_ = "B";
                    break;

                case "Sandia":
                    info_ = "Arma de doble filo, si ataca por la punta podrá causar mucho daño,en cambio si lo hace por la parte de la cáscara será fácil acabar con ella";
                    rango_ = "3";
                    break;

                case "Baston":
                    info_ = "Está afilado y puede atravesar a casi todas las cuches, acabando con ellas brutalmente";
                    rango_ = "6";
                    break;

                case "BombaSandia":
                    info_ = " Destruye todo a su paso , en cuanto detecta a algún enemigo en su área de explosion , no deja nada a su paso";
                    rango_ = "1";
                    break;

                case "Huevo":
                    info_ = "Tiene su punto débil expuesto, la yema, pero para poder acabar con él deberás cogerlo primero y gracias a su disposición de la clara puede arrastrarse muy rápido";
                    rango_ = "4";
                    break;

                case "Regaliz":
                    info_ = "Es flexible y largo, puede envolver a todas las chuches y asfixiarlas";
                    rango_ = "7";
                    break;

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
