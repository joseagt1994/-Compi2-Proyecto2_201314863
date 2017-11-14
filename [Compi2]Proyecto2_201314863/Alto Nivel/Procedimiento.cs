using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace _Compi2_Proyecto2_201314863
{
    public class Procedimiento : Auxiliar
    {
        #region Atributos del procedimiento
        public String nombre, completo, clase;
        public int tipo, visibilidad, linea, columna;
        public bool sobreescrito;

        public List<Atributo> parametros;
        public ParseTreeNode sentencias;
        public List<int> dimensiones;

        #endregion

        #region "Constructores"
        // Crear Constructor
        public Procedimiento(String nombre, int tipo, List<Atributo> parametros,
            ParseTreeNode sentencias, int l, int c)
        {
            this.nombre = nombre;
            this.tipo = tipo;
            this.visibilidad = (int)Simbolo.Visibilidad.PUBLICO;
            this.parametros = parametros;
            this.sentencias = sentencias;
            this.linea = l;
            this.columna = c;
            setNombreCompleto();
        }

        // Crear procedimiento con nombre, tipo, parametros y sentencias
        public Procedimiento(String nombre, int tipo, List<Atributo> parametros,
            ParseTreeNode sentencias, bool escritura, int l, int c)
        {
            this.nombre = nombre;
            this.tipo = tipo;
            this.visibilidad = (int)Simbolo.Visibilidad.PUBLICO;
            this.parametros = parametros;
            this.sentencias = sentencias;
            this.sobreescrito = escritura;
            this.linea = l;
            this.columna = c;
            setNombreCompleto();
        }

        // Crear procedimiento con nombre, tipo, parametros, sentencias y visibilidad
        public Procedimiento(String nombre, int tipo, int vis, List<Atributo> parametros,
            ParseTreeNode sentencias, bool escritura, int l, int c)
        {
            this.nombre = nombre;
            this.tipo = tipo;
            this.visibilidad = vis;
            this.parametros = parametros;
            this.sentencias = sentencias;
            this.sobreescrito = escritura;
            this.linea = l;
            this.columna = c;
            setNombreCompleto();
        }

        public void setNombreCompleto()
        {
            this.completo = getNombreProcedimiento(this);
        }
        
        public void agregarClase(String clase)
        {
            this.clase = clase;
        }

        public void agregarArreglo(ParseTreeNode dims)
        {
            if (dims != null)
            {
                this.dimensiones = new List<int>();
                foreach (ParseTreeNode dim in dims.ChildNodes)
                {
                    this.dimensiones.Add(-1);
                }
            }
        }

        public static String getNombreProcedimiento(Procedimiento proc)
        {
            String nombre = proc.nombre;
            if(proc.parametros != null)
            {
                if (proc.parametros.Count > 0)
                {
                    foreach (Atributo p in proc.parametros)
                    {
                        nombre += "_" + Simbolo.getValor(p.tipo);
                    }
                }
            }
            return nombre;
        }

        public String getValor()
        {
            String val = "";
            switch (visibilidad)
            {
                case (int)Simbolo.Visibilidad.PUBLICO:
                    val = "+ " + nombre;
                    break;
                case (int)Simbolo.Visibilidad.PRIVADO:
                    val = "- " + nombre;
                    break;
                default:
                    val = "# " + nombre;
                    break;
            }
            return val + "() : " + Simbolo.getValor(tipo);
        }
        #endregion
    }
}
