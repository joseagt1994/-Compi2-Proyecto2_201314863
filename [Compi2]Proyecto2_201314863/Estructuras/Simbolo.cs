using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi2_Proyecto2_201314863
{
    public class Simbolo
    {
        public enum Tipo : int
        {
            GLOBAL, LOCAL, VARIABLE, METODO, FUNCION, PARAMETRO, RETORNO, NUMERO,
            DECIMAL, CARACTER, CADENA, BOOLEAN, VACIO, ETIQUETA, CLASE
        }

        public enum Visibilidad : int
        {
            PUBLICO, PROTEGIDO, PRIVADO
        }

        //ATRIBUTOS DE SIMBOLO
        public String nombre, padre, estructura;
        public int ambito, tipo, rol, pos, tam, acceso, dims;
        public int fila, columna;
        //public List<Dimension> dimensiones;
        //public ParseTreeNode sentencias;
        //public C3D instruccion;

        public Simbolo()
        {

        }

        public String getValor(int val)
        {
            switch (val)
            {
                case (int)Tipo.GLOBAL:
                    return "Global";
                case (int)Tipo.LOCAL:
                    return "Local";
                case (int)Tipo.VARIABLE:
                    return "Variable";
                case (int)Tipo.METODO:
                    return "Metodo";
                case (int)Tipo.FUNCION:
                    return "Funcion";
                case (int)Tipo.PARAMETRO:
                    return "Parametro";
                case (int)Tipo.CLASE:
                    return "Clase";
                case (int)Tipo.DECIMAL:
                    return "Decimal";
                case (int)Tipo.CARACTER:
                    return "Caracter";
                case (int)Tipo.NUMERO:
                    return "Numero";
                case (int)Tipo.CADENA:
                    return "Cadena";
                case (int)Tipo.BOOLEAN:
                    return "Boolean";
                case (int)Tipo.VACIO:
                    return "Vacio";
                case (int)Tipo.RETORNO:
                    return "Retorno";
            }
            return "--------";
        }
    }
}
