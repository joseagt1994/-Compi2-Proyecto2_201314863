using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _Compi2_Proyecto2_201314863
{
    public class Display : LinkedList<Ciclo>
    {
        public Display()
        {

        }

        public Ciclo getCiclo()
        {
            return this.First();
        }

        public void agregarCiclo(int tipo,String nombre,String inicio,String salida)
        {
            Ciclo ciclo = new Ciclo();
            ciclo.tipo = tipo;
            ciclo.nombre = nombre;
            ciclo.etqInicio = inicio;
            ciclo.etqSalida = salida;
            ciclo.interrupciones = 0;
            this.AddFirst(ciclo);
        }

        public void removerCiclo()
        {
            this.RemoveFirst();
        }
        
        public String buscarSalidaLoopID(String id)
        {
            foreach(Ciclo ciclo in this)
            {
                if(ciclo.tipo == (int)Ciclo.TipoCiclo.LOOP)
                {
                    if (ciclo.nombre.Equals(id))
                    {
                        ciclo.interrupciones++;
                        return ciclo.etqSalida;
                    }
                }
            }
            return null;
        }

        public String buscarSalida()
        {
            if(this.Count > 0)
            {
                this.ElementAt(0).interrupciones++;
                return this.ElementAt(0).etqSalida;
            }
            return null;
        }

        public String buscarInicio()
        {
            if (this.Count > 0)
            {
                return this.ElementAt(0).etqInicio;
            }
            return null;
        }
    }
}