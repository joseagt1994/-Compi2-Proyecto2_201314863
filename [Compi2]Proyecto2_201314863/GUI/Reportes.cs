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
    public partial class Reportes : Form
    {
        public Reportes()
        {
            InitializeComponent();
        }

        public void setReporteTS(TablaSimbolos ts)
        {
            this.txtNombre.Text = "Reporte de Tabla de Simbolos";
            Reporte.getReporteTS(ts, ref this.tablaReporte);
        }

        private void txtNombre_Click(object sender, EventArgs e)
        {

        }
    }
}
