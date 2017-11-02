using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace _Compi2_Proyecto2_201314863
{
    public class Atributo
    {
        public String nombre, clase;
        public int tipo, visibilidad, linea, columna;
        public bool esArreglo = false;
        public List<int> dimensiones = null;
        public ParseTreeNode valor = null;

        public Atributo(String nombre, int tipo, int l, int c)
        {
            this.nombre = nombre;
            this.tipo = tipo;
            this.visibilidad = (int)Simbolo.Visibilidad.PUBLICO;
            this.linea = l;
            this.columna = c;
        }

        public Atributo(String nombre, int tipo, int vis, int l, int c)
        {
            this.nombre = nombre;
            this.tipo = tipo;
            this.visibilidad = vis;
            this.linea = l;
            this.columna = c;
        }

        public void asignarArreglo(ParseTreeNode dims)
        {
            if (dims != null)
            {
                this.esArreglo = true;
                this.dimensiones = new List<int>();
                foreach(ParseTreeNode dim in dims.ChildNodes)
                {
                    this.dimensiones.Add(Convert.ToInt32(dim.Token.Text));
                }
            }
        }

        public void asignarClase(String clase)
        {
            this.tipo = (int)Simbolo.Tipo.CLASE;
            this.clase = clase;
        }

        public void asignarValor(ParseTreeNode valor)
        {
            this.valor = valor;
        }
    }
}
