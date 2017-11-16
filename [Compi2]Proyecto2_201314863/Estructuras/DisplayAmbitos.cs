using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _Compi2_Proyecto2_201314863
{
    public class DisplayAmbitos : LinkedList<Ambito>
    {
        public void aumentarAmbito(int salida, int ambito, String nombre)
        {
            this.AddFirst(new Ambito(salida,ambito,nombre));
        }

        public void disminuirAmbito()
        {
            this.RemoveFirst();
        }

        public void agregarTemporal(String temporal, double valor)
        {
            this.First().agregarTemporal(temporal, valor);
        }

        public double buscarTemporal(String nombre)
        {
            double valor = 0;
            this.First().temporales.TryGetValue(nombre, out valor);
            return valor;
        }

        public int buscarSalida()
        {
            return this.First().salida;
        }
        
    }
}