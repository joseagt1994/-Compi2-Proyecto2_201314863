using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Irony.Parsing;

namespace _Compi2_Proyecto2_201314863
{
    public class GeneradorC3D
    {
        #region "Atributos"
        //LISTA DE C3D
        public static List<C3D> instrucciones;
        RichTextBox txtCodigo;
        //ATRIBUTOS HEREDADOS DE INTERPRETE
        TablaSimbolos tablaSimbolos;
        ParseTreeNode instruccionesMain;
        //CONTADORES 
        static int temporales = 1;
        static int etiquetas = 1;
        //DISPLAY DE CICLOS!
        //Display display;
        //DISPLAY DE AMBITOS
        //DisplayAmbitos ambitos;
        //AMBITO DE FUNCIONES O METODOS
        String nombreMetodo; //Nombre de funcion o metodo
        int tipo;       //Funcion o metodo
        int retornos;   //contador de retornos encontrados en los ambitos mencionados anteriormente
        int retornosMetodo;
        int retornosFuncion;
        int tamMetodo;
        bool hayRetorno, si, selector;
        bool enRetorno = false;
        bool esParametro = false;
        String eSalidaMetodo;

        public GeneradorC3D()
        {
            instrucciones = new List<C3D>();
            //this.display = new Display();
            //this.ambitos = new DisplayAmbitos();
            temporales = 1;
            etiquetas = 1;
        }
        #endregion

        #region "Temporales, etiquetas y punteros"

        //Genera los temporales
        public static String getTemporal()
        {
            String temp = "t" + temporales;
            temporales++;
            return temp;
        }

        //Genera las etiquetas necesarias
        public static String getEtiqueta()
        {
            String temp = "L" + etiquetas;
            etiquetas++;
            return temp;
        }

        //Generar las etiquetas necesarias
        public static void generarEtiquetas(String lista)
        {
            if (lista == null)
            {
                return;
            }
            String[] etiquetas = lista.Split(',');
            foreach (String e in etiquetas)
            {
                instrucciones.Add(new C3D((int)C3D.TipoC3D.ETIQUETA, e));
            }
        }

        //Aumentar Heap
        public static void aumentarHeap(String valor)
        {
            instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, "H", "H", "+", valor));
        }

        #endregion
    }
}
