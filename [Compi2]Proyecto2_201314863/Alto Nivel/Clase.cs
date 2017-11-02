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
        public List<Procedimiento> constructores;
        public List<Procedimiento> procedimientos;

        public Clase(String nombre, String padre)
        {
            this.nombre = nombre;
            this.padre = padre;
            this.atributos = new List<Atributo>();
            this.constructores = new List<Procedimiento>();
            this.procedimientos = new List<Procedimiento>();
        }

        public Clase(String nombre)
        {
            this.nombre = nombre;
            this.padre = null;
            this.atributos = new List<Atributo>();
            this.constructores = new List<Procedimiento>();
            this.procedimientos = new List<Procedimiento>();
        }

        public void agregarAtributo(Atributo nuevo)
        {
            if (!existeAtributo(nuevo))
            {
                atributos.Add(nuevo);
            }
        }

        public bool existeAtributo(Atributo nuevo)
        {
            foreach (Atributo atr in atributos)
            {
                if (atr.nombre.Equals(nuevo.nombre))
                {
                    return true;
                }
            }
            return false;
        }

        public void agregarConstructor(Procedimiento nuevo)
        {
            if (!existeConstructor(nuevo))
            {
                constructores.Add(nuevo);
            }
        }
        
        public bool existeConstructor(Procedimiento nuevo)
        {
            foreach(Procedimiento constructor in constructores)
            {
                String n1 = getNombreProcedimiento(nuevo);
                String n2 = getNombreProcedimiento(constructor);
                if (n1.Equals(n2))
                {
                    return true;
                }
            }
            return false;
        }

        public void agregarProcedimiento(Procedimiento nuevo)
        {
            if (!existeConstructor(nuevo))
            {
                procedimientos.Add(nuevo);
            }
        }

        public static String getNombreProcedimiento(Procedimiento proc)
        {
            String nombre = proc.nombre;
            if(proc.parametros.Count > 0)
            {
                foreach(Atributo p in proc.parametros)
                {
                    nombre += "_" + Simbolo.getValor(p.tipo);
                }
            }
            return nombre;
        }
    }
}
