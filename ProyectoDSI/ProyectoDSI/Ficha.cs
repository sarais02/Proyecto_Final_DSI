using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;

namespace ProyectoDSI
{
    class Ficha{

        public int id_; //todas las fichas estaran en una lista y este sera su indicide en ekl que se encuentra en dicha lista
        public string tipo_;
        public string info_;
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
                case "fresas":
                    info_ ="";
                    break;

                case "cocacolas":
                    info_ = "Va absolutamente loca puede moverse mucho y " +
                        "muy rápido en un corto periodo de tiempo.Sin embargo " +
                        "es muy débil porque no está muy fuerte";
                    break;

                case "dedos":
                    info_ = "Tiene ciertos parecidos con los humanos," +
                        " muchas chuches se asustan al verlos aparecer y " +
                        "más si se agrupan de 5 en 5.";
                    break;

                case "petazetas":
                    break;

                case "sandias":
                    break;

                case "bastones":
                    break;

                case "bombassandia":
                    break;

                case "regalices":
                    break;
            }
            //imagen
            img_ = new Image();
            dirImg_ = "Assets\\" + tipo;
            string s = System.IO.Directory.GetCurrentDirectory() + "\\" + dirImg_;
            img_.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(s));
            //contentcontrol
            ccImg_ = new ContentControl();
            ccImg_.Content = img_;
            ccImg_.UseSystemFocusVisuals = true;

        }
    }
}
