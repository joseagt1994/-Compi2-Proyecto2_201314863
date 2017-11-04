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
    public partial class EditorRelacion : Form
    {
        public UML.Relacion relacion;
        public List<UML.Relacion> relaciones;
        private int tipo;
        public EditorRelacion()
        {
            InitializeComponent();
        }

        public void setDatos(List<Clase> clases, int tipo, ref List<UML.Relacion> relaciones)
        {
            foreach(Clase clase in clases)
            {
                clase1.Items.Add(clase.nombre);
                clase2.Items.Add(clase.nombre);
            }
            this.tipo = tipo;
            this.relaciones = relaciones;
            lblRelacion.Text = UML.Relacion.getTipo(tipo);
        }

        private void lblRelacion_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Agregar nueva relacion
            if (clase1.SelectedIndex >= 0 && clase2.SelectedIndex >= 0)
            {
                String c1 = clase1.SelectedItem.ToString();
                String c2 = clase2.SelectedItem.ToString();
                if (c1.Equals(c2))
                {
                    MessageBox.Show("Error: Seleccione dos distintas clases!");
                }
                else
                {
                    MessageBox.Show("Relacion agregada!");
                    relacion = new UML.Relacion(c1, c2, tipo);
                    relaciones.Add(relacion);
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Error: Debes seleccionar dos clases!");
            }
        }
    }
}
