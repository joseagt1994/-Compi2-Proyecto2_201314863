using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _Compi2_Proyecto2_201314863
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            agregarPaginas();
        }

        private void agregarPaginas()
        {
            Pagina nueva = new Pagina((int)Pagina.Lenguaje.OLC, "nuevo");
            paginas.Controls.Add(nueva);
            Pagina nueva2 = new Pagina((int)Pagina.Lenguaje.TREE, "nuevo");
            paginas.Controls.Add(nueva2);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Compilar
            // Reiniciar TS
            TablaSimbolos.reiniciar();
            int actual = paginas.SelectedIndex;
            if(actual >= 0)
            {
                Compilador compilador = new Compilador();
                Pagina pag = (Pagina)paginas.SelectedTab;
                compilador.compilar(pag.cuerpo.Text, txtConsola, pag.tipo);
            }
            else
            {
                MessageBox.Show("Seleccione un archivo antes para compilar!");
            }
        }

        private void generarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UML nuevo = new UML();
            nuevo.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Nuevo archivo
            Pagina.Lenguaje tipoArchivo = (Pagina.Lenguaje)Opciones.ObtenerArchivo();
            Pagina nueva = new Pagina((int)tipoArchivo, "nuevo");
            paginas.Controls.Add(nueva);
            MessageBox.Show(tipoArchivo.ToString());
        }

        private void tablaDeSimbolosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Ver el reporte de Simbolos
            Reportes rpts = new Reportes();
            rpts.setReporteTS(TablaSimbolos.getInstance);
            rpts.Show();
        }
    }
}
