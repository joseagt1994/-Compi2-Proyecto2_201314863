using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace _Compi2_Proyecto2_201314863
{
    public class Procedimiento
    {
        #region Atributos del procedimiento
        public String nombre;
        public int tipo, visibilidad;
        public bool sobreescrito;

        public List<Atributo> parametros;
        public ParseTreeNode sentencias;

        #endregion

        #region "Constructores"
        // Crear procedimiento con nombre, tipo, parametros y sentencias
        public Procedimiento(String nombre,int tipo,List<Atributo> parametros,
            ParseTreeNode sentencias,bool escritura)
        {
            this.nombre = nombre;
            this.tipo = tipo;
            this.visibilidad = (int)Simbolo.Visibilidad.PUBLICO;
            this.parametros = parametros;
            this.sentencias = sentencias;
            this.sobreescrito = escritura;
        }

        // Crear procedimiento con nombre, tipo, parametros, sentencias y visibilidad
        public Procedimiento(String nombre, int tipo, int vis, List<Atributo> parametros,
            ParseTreeNode sentencias, bool escritura)
        {
            this.nombre = nombre;
            this.tipo = tipo;
            this.visibilidad = vis;
            this.parametros = parametros;
            this.sentencias = sentencias;
            this.sobreescrito = escritura;
        }
        #endregion
    }
}
