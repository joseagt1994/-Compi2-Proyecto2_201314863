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
            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, 
                "// Inicio de la sentencia Si"));
            Nodo nsi = Expresion.castearC3D((int)Simbolo.Tipo.BOOLEAN,
                Expresion.expresionC3D(expresion),expresion.Span.Location.Line,
                expresion.Span.Location.Column);
            if(nsi != null)
            {
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// La condicion verdadera"));
                GeneradorC3D.generarEtiquetas((nsi.etqVerdadera));
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Sentencias de Si"));
                C3DSentencias.generarC3D(sentencias);
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, eSalida));
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// La condicion falsa"));
                GeneradorC3D.generarEtiquetas((nsi.etqFalsa));
            }
        }

        public static void evaluarSinoSi(ParseTreeNode expresion, ParseTreeNode sentencias, String eSalida)
        {
            // Sino Si (Expresion) { Sentencias }
            // Si (Expresion) { Sentencias }
            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                "// Inicio de la sentencia Sino Si"));
            Nodo nsi = Expresion.castearC3D((int)Simbolo.Tipo.BOOLEAN,
                Expresion.expresionC3D(expresion), expresion.Span.Location.Line,
                expresion.Span.Location.Column);
            if (nsi != null)
            {
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// La condicion verdadera"));
                GeneradorC3D.generarEtiquetas((nsi.etqVerdadera));
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Sentencias de Sino Si"));
                C3DSentencias.generarC3D(sentencias);
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, eSalida));
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// La condicion falsa"));
                GeneradorC3D.generarEtiquetas((nsi.etqFalsa));
            }
        }

        public static void evaluarSino(ParseTreeNode sentencias, String eSalida)
        {
            // Sino { Sentencias }
            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Sentencias de Sino"));
            C3DSentencias.generarC3D(sentencias);
            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, eSalida));
        }

        public static void evaluarIF4(ParseTreeNode nodo, String eSalida)
        {
            // IF4 -> IF4 EXP Sentencias | EXP Sentencias
            if(nodo.ChildNodes.Count == 3)
            {
                evaluarIF4(nodo.ChildNodes[0], eSalida);
                evaluarSinoSi(nodo.ChildNodes[1], nodo.ChildNodes[2], eSalida);
            }
            else
            {
                evaluarSinoSi(nodo.ChildNodes[0], nodo.ChildNodes[1], eSalida);
            }
        }
    }
}
