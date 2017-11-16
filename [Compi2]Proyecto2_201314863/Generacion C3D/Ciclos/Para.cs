using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace _Compi2_Proyecto2_201314863
{
    class Para
    {
        public static void evaluarParaC3D(ParseTreeNode tipo, ParseTreeNode cond, 
            ParseTreeNode control, ParseTreeNode sentencias)
        {
            //FOR -> DECLARACION/ASIGNACION EXP ASIGNACION Sentencias
            C3DSentencias.generarC3D(tipo);
            String eInicio = GeneradorC3D.getEtiqueta();
            String eAux = GeneradorC3D.getEtiqueta();
            GeneradorC3D.generarEtiquetas((eInicio));
            Nodo npara = Expresion.castearC3D((int)Simbolo.Tipo.BOOLEAN,
                Expresion.expresionC3D(cond), cond.Span.Location.Line,
                cond.Span.Location.Column);
            if (npara != null)
            {
                GeneradorC3D.generarEtiquetas((npara.etqVerdadera));
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Sentencias de for"));
                GeneradorC3D.display.agregarCiclo((int)Ciclo.TipoCiclo.FOR, "", eAux, npara.etqFalsa);
                // Sentencias de Para
                C3DSentencias.generarC3D(sentencias);
                GeneradorC3D.display.removerCiclo();
                GeneradorC3D.generarEtiquetas(eAux);
                Expresion.expresionC3D(control);
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, eInicio));
                GeneradorC3D.generarEtiquetas((npara.etqFalsa));
            }
        }
    }
}
