using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace _Compi2_Proyecto2_201314863
{
    class X
    {
        public static void evaluarXC3D(ParseTreeNode exp1, ParseTreeNode exp2, 
            ParseTreeNode sentencias)
        {
            //WHILEX -> EXP EXP Sentencias
            String eInicio = GeneradorC3D.getEtiqueta();
            String eSen = GeneradorC3D.getEtiqueta();
            String eCond = GeneradorC3D.getEtiqueta();
            String eSalida = GeneradorC3D.getEtiqueta();
            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                "// Inicializar temporal auxiliar"));
            String taux = GeneradorC3D.getTemporal();
            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION,
                taux, "0", "", ""));
            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                "// Etiqueta de Inicio del ciclo X"));
            GeneradorC3D.generarEtiquetas((eInicio));
            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                "// Evaluar condicion 1"));
            Nodo cond1 = Expresion.castearC3D((int)Simbolo.Tipo.BOOLEAN,
                Expresion.expresionC3D(exp1), exp1.Span.Location.Line, exp1.Span.Location.Column);
            if(cond1 != null)
            {
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                    "// Etiqueta verdadera primera condicion"));
                GeneradorC3D.generarEtiquetas(cond1.etqVerdadera);
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                    "// Evaluar condicion por primera vez"));
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.CONDICIONAL,
                    eSen, taux, "==", "0"));
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL,
                    eCond));
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                    "// Etiqueta falsa primera condicion"));
                GeneradorC3D.generarEtiquetas(cond1.etqFalsa);
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.CONDICIONAL,
                    eSalida, taux, "==", "1"));
                GeneradorC3D.generarEtiquetas(eCond);
                Nodo cond2 = Expresion.castearC3D((int)Simbolo.Tipo.BOOLEAN,
                    Expresion.expresionC3D(exp2), exp2.Span.Location.Line, exp2.Span.Location.Column);
                if(cond2 != null)
                {
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                        "// Etiqueta verdadera segunda condicion"));
                    GeneradorC3D.generarEtiquetas(cond2.etqVerdadera);
                    GeneradorC3D.generarEtiquetas(eSen);
                    GeneradorC3D.display.agregarCiclo((int)Ciclo.TipoCiclo.WHILEX, "", eInicio, eSalida);
                    // Sentencias de X
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                        "// Sentencia del ciclo X"));
                    C3DSentencias.generarC3D(sentencias);
                    GeneradorC3D.display.removerCiclo();
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION,
                        taux, "1", "", ""));
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                        "// Salta al inicio"));
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL,
                        eInicio));
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                        "// Etiqueta falsa segunda condicion y salida"));
                    GeneradorC3D.generarEtiquetas(cond2.etqFalsa);
                    GeneradorC3D.generarEtiquetas(eSalida);
                }
            }
        }
    }
}
