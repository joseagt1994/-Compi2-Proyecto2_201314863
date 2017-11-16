using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _Compi2_Proyecto2_201314863
{
    public class Ambito
    {
        // Guarda el temporal, nombre y  su valor
        public Dictionary<String, double> temporales;
        public int salida,ambito; // Guarda el puntero del anterior ambito antes de hacer call
        public String nombre;

        public Ambito(int salida,int ambito,String nombre)
        {
            this.temporales = new Dictionary<string, double>();
            this.salida = salida;
            this.ambito = ambito;
            this.nombre = nombre;
        }

        public void agregarTemporal(String temporal, double valor)
        {
            if (temporales.ContainsKey(temporal))
            {
                temporales.Remove(temporal);
            }
            temporales.Add(temporal, valor);
        }

        public double buscarTemporal(String nombre)
        {
            double valor = 0;
            this.temporales.TryGetValue(nombre, out valor);
            return valor;
        }
    }
}