using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _Compi2_Proyecto2_201314863
{
    class Pagina : TabPage
    {
        public enum Lenguaje
        {
            TREE, OLC
        }
        public int tipo;
        public String nombre, ruta;
        public RichTextBox cuerpo;
        
        public Pagina(int tipo, String nombre)
        {
            this.tipo = tipo;
            if(tipo == (int)Lenguaje.TREE)
            {
                this.Text = nombre + ".tree";
            }
            else
            {
                this.Text = nombre + ".olc";
            }
            this.nombre = nombre;
            this.cuerpo = new RichTextBox();
            this.cuerpo.SetBounds(0, 0, 662, 270);
            this.Controls.Add(this.cuerpo);
        }

        public Pagina(int tipo, String nombre, String texto, String ruta)
        {
            this.tipo = tipo;
            if (tipo == (int)Lenguaje.TREE)
            {
                this.Text = nombre + ".tree";
            }
            else
            {
                this.Text = nombre + ".olc";
            }
            this.nombre = nombre;
            this.cuerpo = new RichTextBox();
            this.ruta = ruta;
            this.cuerpo.Text = texto;
            this.cuerpo.SetBounds(0, 0, 662, 270);
            this.Controls.Add(this.cuerpo);
        }
    }
}
