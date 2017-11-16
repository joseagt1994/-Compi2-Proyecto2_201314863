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
        public FastColoredTextBoxNS.IronyFCTB cuerpo;
        
        public Pagina(int tipo, String nombre)
        {
            this.tipo = tipo;
            if(tipo == (int)Lenguaje.TREE)
            {
                this.Text = nombre + ".tree";
                this.cuerpo = new FastColoredTextBoxNS.IronyFCTB();
                this.cuerpo.SetParser(new AnalizadorTree());
            }
            else
            {
                this.Text = nombre + ".olc";
                this.cuerpo = new FastColoredTextBoxNS.IronyFCTB();
                this.cuerpo.SetParser(new AnalizadorOLC());
            }
            this.nombre = nombre;
            this.cuerpo.SetBounds(0, 0, 662, 270);
            this.Controls.Add(this.cuerpo);
        }

        public Pagina(int tipo, String nombre, String texto, String ruta)
        {
            this.tipo = tipo;
            if (tipo == (int)Lenguaje.TREE)
            {
                this.Text = nombre + ".tree";
                this.cuerpo = new FastColoredTextBoxNS.IronyFCTB();
                this.cuerpo.SetParser(new AnalizadorTree());
            }
            else
            {
                this.Text = nombre + ".olc";
                this.cuerpo = new FastColoredTextBoxNS.IronyFCTB();
                this.cuerpo.SetParser(new AnalizadorOLC());
            }
            this.nombre = nombre;
            this.ruta = ruta;
            this.cuerpo.Text = texto;
            this.cuerpo.SetBounds(0, 0, 662, 270);
            this.Controls.Add(this.cuerpo);
        }
    }
}
