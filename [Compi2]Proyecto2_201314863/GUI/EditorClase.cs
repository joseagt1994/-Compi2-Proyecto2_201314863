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
    public partial class EditorClase : Form
    {
        private Clase clase;
        public EditorClase()
        {
            InitializeComponent();
        }

        public void setClase(ref Clase nueva)
        {
            this.clase = nueva;
            txtClase.Text = nueva.nombre;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Guardar clase
            MessageBox.Show("Clase " + txtClase.Text + " guardada!");
            clase.nombre = txtClase.Text;
            this.Close();
        }

        private void btnAgregarAtributo_Click(object sender, EventArgs e)
        {
            // Guardar datos de atributo
            int vis = Simbolo.getVisibilidad(visibilidad.SelectedItem.ToString().ToLower());
            String stipo = tipo.SelectedItem.ToString().ToLower();
            int tipo_valor = Simbolo.getTipo(stipo);
            String nombre = txtNombre.Text;
            // Guardar el atributo en la lista de atributos
            listAtributos.Items.Add(getValor(nombre, vis) + " : " + stipo);
            // Guardar el atributo en la clase
            clase.agregarAtributo(new Atributo(nombre, tipo_valor, vis, listAtributos.Items.Count-1, 0));
            // Limpiar
            visibilidad.ResetText();
            tipo.ResetText();
            txtNombre.Text = "";
        }

        public String getValor(String nombre,int vis)
        {
            switch (vis)
            {
                case (int)Simbolo.Visibilidad.PUBLICO:
                    return "+ " + nombre;
                case (int)Simbolo.Visibilidad.PRIVADO:
                    return "- " + nombre;
                default:
                    return "# " + nombre;
            }
        }

        private void btnEliminarAtributo_Click(object sender, EventArgs e)
        {
            // Obtener el indice seleccionado
            int indice = listAtributos.SelectedIndex;
            // Eliminar item seleccionado
            listAtributos.Items.RemoveAt(indice);
            // Eliminar atributo de la clase
            foreach(Atributo a in clase.atributos)
            {
                if(a.linea == indice)
                {
                    clase.atributos.Remove(a);
                    break;
                }
            }
            int contador = 0;
            foreach (Atributo a in clase.atributos)
            {
                a.linea = contador;
                contador++;
            }
        }

        private void btnAgregarProcedimiento_Click(object sender, EventArgs e)
        {
            // Guardar datos de procedimiento
            int vis = Simbolo.getVisibilidad(visibilidad.SelectedItem.ToString().ToLower());
            String stipo = tipo.SelectedItem.ToString().ToLower();
            int tipo_valor = Simbolo.getTipo(stipo);
            String nombre = txtNombre.Text;
            // Guardar el procedimiento en la lista de procedimientos
            listProcedimientos.Items.Add(getValor(nombre, vis) + "() : " + stipo);
            // Guardar el procedimiento en la clase
            clase.agregarProcedimiento(new Procedimiento(nombre, tipo_valor, vis, null, null, false, listProcedimientos.Items.Count, 0));
            // Limpiar
            visibilidad.ResetText();
            tipo.ResetText();
            txtNombre.Text = "";
        }

        private void btnEliminarProcedimiento_Click(object sender, EventArgs e)
        {
            // Obtener el indice seleccionado
            int indice = listProcedimientos.SelectedIndex;
            // Eliminar item seleccionado
            listProcedimientos.Items.RemoveAt(indice);
            // Eliminar atributo de la clase
            foreach (Procedimiento a in clase.procedimientos)
            {
                if (a.linea == indice)
                {
                    clase.procedimientos.Remove(a);
                    break;
                }
            }
            int contador = 0;
            foreach (Procedimiento a in clase.procedimientos)
            {
                a.linea = contador;
                contador++;
            }
        }
    }
}
