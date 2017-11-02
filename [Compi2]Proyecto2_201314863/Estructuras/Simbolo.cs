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
            DECIMAL, CARACTER, CADENA, BOOLEAN, VACIO, ETIQUETA, CLASE, CONSTRUCTOR
        }

        public enum Visibilidad : int
        {
            PUBLICO, PROTEGIDO, PRIVADO
        }

        //ATRIBUTOS DE SIMBOLO
        public String nombre, padre, clase;
        public int ambito, tipo, rol, pos, tam;
        //public int fila, columna;
        public List<int> dimensiones;
        //public C3D instruccion;

        public Simbolo()
        {

        }

        public static int getVisibilidad(String vis)
        {
            switch (vis)
            {
                case "publico":
                    return (int)Visibilidad.PUBLICO;
                case "privado":
                    return (int)Visibilidad.PRIVADO;
                default:
                    return (int)Visibilidad.PROTEGIDO;
            }
        }

        public static int getTipo(String tipo)
        {
            switch (tipo)
            {
                case "entero":
                    return (int)Tipo.NUMERO;
                case "decimal":
                    return (int)Tipo.DECIMAL;
                case "booleano":
                    return (int)Tipo.BOOLEAN;
                case "caracter":
                    return (int)Tipo.CARACTER;
                case "cadena":
                    return (int)Tipo.CADENA;
                default:
                    return (int)Tipo.CLASE;
            }
        }

        public static String getValor(int val)
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
                    return "Entero";
                case (int)Tipo.CADENA:
                    return "Cadena";
                case (int)Tipo.BOOLEAN:
                    return "Boolean";
                case (int)Tipo.VACIO:
                    return "Vacio";
                case (int)Tipo.RETORNO:
                    return "Retorno";
                case (int)Tipo.CONSTRUCTOR:
                    return "Constructor";
            }
            return "--------";
        }

    }
}
