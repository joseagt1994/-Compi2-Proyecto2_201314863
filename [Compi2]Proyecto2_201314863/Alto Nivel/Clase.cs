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
        public int linea, columna;

        public Clase(String nombre, String padre, int l, int c)
        {
            this.nombre = nombre;
            this.padre = padre;
            this.atributos = new List<Atributo>();
            this.constructores = new List<Procedimiento>();
            this.procedimientos = new List<Procedimiento>();
            this.linea = l;
            this.columna = c;
        }

        public Clase(String nombre, int l, int c)
        {
            this.nombre = nombre;
            this.padre = null;
            this.atributos = new List<Atributo>();
            this.constructores = new List<Procedimiento>();
            this.procedimientos = new List<Procedimiento>();
            this.linea = l;
            this.columna = c;
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
                    Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                        "La variable global "+atr.nombre+" ya existe en la clase "+nombre, 
                        atr.linea, atr.columna));
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
                String n1 = nuevo.completo;
                String n2 = constructor.completo;
                if (n1.Equals(n2))
                {
                    Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                        "El constructor " + n1 + " ya existe en la clase " + nombre,
                        nuevo.linea, nuevo.columna));
                    return true;
                }
            }
            return false;
        }

        public void agregarProcedimiento(Procedimiento nuevo)
        {
            if (!existeProcedimiento(nuevo))
            {
                procedimientos.Add(nuevo);
            }
        }

        public bool existeProcedimiento(Procedimiento nuevo)
        {
            foreach (Procedimiento proc in procedimientos)
            {
                String n1 = nuevo.completo;
                String n2 = proc.completo;
                if (n1.Equals(n2))
                {
                    Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                        "El procedimiento " + n1 + " ya existe en la clase " + nombre,
                        nuevo.linea, nuevo.columna));
                    return true;
                }
            }
            return false;
        }
        
    }
}
