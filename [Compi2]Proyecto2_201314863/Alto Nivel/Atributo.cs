using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi2_Proyecto2_201314863
{
    public class Atributo
    {
        public String nombre;
        public int tipo, visibilidad;

        public Atributo(String nombre, int tipo)
        {
            this.nombre = nombre;
            this.tipo = tipo;
            this.visibilidad = (int)Simbolo.Visibilidad.PUBLICO;
        }

        public Atributo(String nombre, int tipo, int vis)
        {
            this.nombre = nombre;
            this.tipo = tipo;
            this.visibilidad = vis;
        }
    }
}
