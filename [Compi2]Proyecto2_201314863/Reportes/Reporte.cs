using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _Compi2_Proyecto2_201314863
{
    public class Reporte
    {
        public static void getReporteTS(TablaSimbolos ts, ref DataGridView tabla)
        {
            /*public String nombre, padre, clase;
            public int ambito, tipo, rol, pos, tam;
            //public int fila, columna;
            public List<int> dimensiones;*/

            tabla.Columns.Add(crearColumna("#"));
            tabla.Columns.Add(crearColumna("Rol"));
            tabla.Columns.Add(crearColumna("Nombre"));
            tabla.Columns.Add(crearColumna("Padre"));
            tabla.Columns.Add(crearColumna("Tipo"));
            tabla.Columns.Add(crearColumna("Clase"));
            tabla.Columns.Add(crearColumna("Ambito"));
            tabla.Columns.Add(crearColumna("Visibilidad"));
            tabla.Columns.Add(crearColumna("Posicion"));
            tabla.Columns.Add(crearColumna("Tamaño"));
            tabla.Columns.Add(crearColumna("Dimensiones"));
            int id = 1;
            foreach(Simbolo simbolo in ts)
            {
                object[] fila = new object[11];
                fila[0] = id;
                fila[1] = Simbolo.getValor(simbolo.rol);
                fila[2] = simbolo.nombre;
                fila[3] = simbolo.padre;
                fila[4] = Simbolo.getValor(simbolo.tipo);
                fila[5] = simbolo.clase;
                if(simbolo.clase == null)
                {
                    fila[5] = "--------";
                }
                fila[6] = Simbolo.getValor(simbolo.ambito);
                fila[7] = Simbolo.getVisibilidad(simbolo.visibilidad);
                fila[8] = simbolo.pos;
                fila[9] = simbolo.tam;
                fila[10] = "--------";
                if(simbolo.dimensiones != null)
                {
                    fila[10] = "";
                    foreach (int v in simbolo.dimensiones)
                    {
                        fila[10] += v+" ";
                    }
                }
                tabla.Rows.Add(fila);
                id++;
            }
        }

        public static DataGridViewColumn crearColumna(string nombre)
        {
            DataGridViewColumn col = new DataGridViewColumn();
            DataGridViewCell cell = new DataGridViewTextBoxCell();
            col.Width = 100;
            col.ReadOnly = true;
            col.Name = "Columna";
            col.HeaderText = nombre;
            col.CellTemplate = cell;
            return col;
        }
    }
}
