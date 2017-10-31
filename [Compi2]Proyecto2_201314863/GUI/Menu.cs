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
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Compilar
            InterpreteTree analiza = new InterpreteTree();
            analiza.analizar(txtCuerpo.Text, txtConsola);
            //InterpreteOLC analizaO = new InterpreteOLC();
            //analizaO.analizar(txtCuerpo.Text, txtConsola);
        }
    }
}
