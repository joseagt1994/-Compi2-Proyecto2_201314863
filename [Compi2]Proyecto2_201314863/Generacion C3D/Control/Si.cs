using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace _Compi2_Proyecto2_201314863
{
    public class Si
    {
        public static void evaluarSi(ParseTreeNode expresion, ParseTreeNode sentencias, String eSalida)
        {
            // Si (Expresion) { Sentencias }
        }

        public static void evaluarSinoSi(ParseTreeNode expresion, ParseTreeNode sentencias, String eSalida)
        {
            // Sino Si (Expresion) { Sentencias }
        }

        public static void evaluarSino(ParseTreeNode sentencias, String eSalida)
        {
            // Sino { Sentencias }
        }
    }
}
