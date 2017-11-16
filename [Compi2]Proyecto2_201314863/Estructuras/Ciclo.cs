using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _Compi2_Proyecto2_201314863
{
    public class Ciclo
    {
        public enum TipoCiclo : int
        {
            SWITCH,FOR,WHILE,WHILEX,DO_WHILE,DO_WHILEX,COUNT,LOOP,REPEAT,WHILEXORAND
        }
        public int tipo; //TipoCiclo
        public String nombre, etqSalida, etqInicio;
        public int interrupciones;
    }
}