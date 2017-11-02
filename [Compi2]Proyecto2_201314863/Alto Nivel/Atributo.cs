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
        public String nombre;
        public int tipo, visibilidad;
        public bool esArreglo = false;
        public ParseTreeNode dimensiones = null;
        public ParseTreeNode valor = null;

        public Atributo(String nombre, int tipo, ParseTreeNode dims)
        {
            this.nombre = nombre;
            this.tipo = tipo;
            this.visibilidad = (int)Simbolo.Visibilidad.PUBLICO;
            if (dims != null)
            {
                this.esArreglo = true;
                this.dimensiones = dims;
            }
        }

        public Atributo(String nombre, int tipo, int vis, ParseTreeNode dims)
        {
            this.nombre = nombre;
            this.tipo = tipo;
            this.visibilidad = vis;
            if (dims != null)
            {
                this.esArreglo = true;
                this.dimensiones = dims;
            }
        }

        public void asignarValor(ParseTreeNode valor)
        {
            this.valor = valor;
        }
    }
}
