using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi2_Proyecto2_201314863
{
    public class Error
    {
        public enum tipoError : int
        {
            LEXICO, SINTACTICO, SEMANTICO
        }

        public int fila, columna, tipo;
        public String descripcion;

        public Error(int tipo, String des, int f, int c)
        {
            this.tipo = tipo;
            this.descripcion = des;
            this.fila = f;
            this.columna = c;
        }

        public String getTipoError(int tipo)
        {
            switch (tipo)
            {
                case (int)tipoError.LEXICO:
                    return "Lexico";
                case (int)tipoError.SINTACTICO:
                    return "Sintactico";
                default:
                    return "Semantico";
            }
        }
    }
}
