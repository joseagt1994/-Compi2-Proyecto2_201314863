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
    public partial class Opciones : Form
    {
        static Opciones cuadro;
        static Pagina.Lenguaje tipoArchivo;
        public Opciones()
        {
            InitializeComponent();
        }

        public static int ObtenerArchivo()
        {
            cuadro = new Opciones();
            cuadro.ShowDialog();
            return (int)tipoArchivo;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Seleccionar OLC++
            tipoArchivo = Pagina.Lenguaje.OLC;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Seleccionar Tree
            tipoArchivo = Pagina.Lenguaje.TREE;
            this.Close();
        }
    }
}
