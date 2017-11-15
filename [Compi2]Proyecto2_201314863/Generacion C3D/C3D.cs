using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi2_Proyecto2_201314863
{
    public class C3D
    {
        public enum TipoC3D : int
        {
            COMENTARIO, ETIQUETA, ASIGNACION, INCONDICIONAL, CONDICIONAL, INICIO_METODO,
            FIN_METODO, LLAMADA, IMPRIMIR, EXIT, ACCESO, VALOR, NATIVA
        }

        //COMENTARIO,INICIO_METODO,FIN_METODO -------> cadena
        //ETIQUETA ------------> cadena:
        //INCONDICIONAL -------> goto cadena
        //ASIGNACION ----------> cadena = id1 ope id2
        //ACCESO --------------> id1 = cadena(stack,heap,pool)[id2]
        //VALOR ---------------> cadena(stack,heap,pool)[id1] = id2
        //CONDICIONAL ---------> if id1 ope id2 then goto cadena
        //LLAMADA -------------> call cadena
        //IMPRIMIR ------------> printf(cadena,id1)
        //EXIT ----------------> cadena
        public String cadena, id1, operacion, id2;
        public int tipo;
        public double valor1, valor2;

        public C3D(int tipo, String cad, String id1, String op, String id2)
        {
            this.tipo = tipo;
            //ASIGNACION,CONDICIONAL
            this.cadena = cad;
            this.id1 = id1;
            this.operacion = op;
            this.id2 = id2;
            double d1, d2;
            this.valor1 = 0;
            if (Double.TryParse(this.id1, out d1))
            {
                this.valor1 = d1;
            }
            this.valor2 = 0;
            if (Double.TryParse(this.id1, out d2))
            {
                this.valor2 = d2;
            }
        }

        public C3D(int tipo, String acceso, String ter1, String ter2)
        {
            this.tipo = tipo;
            //ACCESO,VALOR
            this.cadena = acceso;
            this.id1 = ter1;
            this.id2 = ter2;
        }

        public C3D(int tipo, String cadena)
        {
            this.tipo = tipo;
            //COMENTARIO,ETIQUETA,INCONDICIONAL,LLAMADA,EXIT
            this.cadena = cadena;
        }

        public C3D(int tipo, String param, String id)
        {
            this.tipo = tipo;
            //IMPRIMIR
            this.cadena = param;
            this.id1 = id;
        }

        public String getInstruccion()
        {
            switch (this.tipo)
            {
                case (int)TipoC3D.COMENTARIO:
                    return cadena;
                case (int)TipoC3D.ETIQUETA:
                    return cadena + ":";
                case (int)TipoC3D.ASIGNACION:
                    return cadena + " = " + id1 + " " + operacion + " " + id2;
                case (int)TipoC3D.ACCESO:
                    return id1 + " = " + cadena + "[" + id2 + "]";
                case (int)TipoC3D.VALOR:
                    return cadena + "[" + id1 + "] = " + id2;
                case (int)TipoC3D.INCONDICIONAL:
                    return "goto " + cadena;
                case (int)TipoC3D.CONDICIONAL:
                    return "if(" + id1 + " " + operacion + " " + id2 + ") goto " + cadena;
                case (int)TipoC3D.INICIO_METODO:
                    return "\nvoid " + cadena + "(){\n";
                case (int)TipoC3D.FIN_METODO:
                    return "\n}\n";
                case (int)TipoC3D.LLAMADA:
                    return "call " + cadena + "()";
                case (int)TipoC3D.IMPRIMIR:
                    return "printf(" + cadena + "," + id1 + ")";
                case (int)TipoC3D.NATIVA:
                    return "call " + cadena + "()";
                default:
                    return "call exit() "; //VER QUE SE GENERA!!!!!??????
            }
        }
    }
}
