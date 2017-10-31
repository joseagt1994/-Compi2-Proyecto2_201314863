using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi2_Proyecto2_201314863
{
    public class Clase
    {
        public String nombre, padre;
        public List<Atributo> atributos;
        public List<Procedimiento> procedimientos;

        public Clase(String nombre, String padre)
        {
            this.nombre = nombre;
            this.padre = padre;
            this.atributos = new List<Atributo>();
            this.procedimientos = new List<Procedimiento>();
        }

        public Clase(String nombre)
        {
            this.nombre = nombre;
            this.padre = null;
            this.atributos = new List<Atributo>();
            this.procedimientos = new List<Procedimiento>();
        }

        public void agregarAtributo(Atributo nuevo)
        {

        }

        public void agregarProcedimiento(Procedimiento nuevo)
        {

        }
    }
}
