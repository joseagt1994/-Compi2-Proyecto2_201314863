using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace _Compi2_Proyecto2_201314863
{
    class Hacer
    {
        public static void evaluarHacerC3D(ParseTreeNode sentencias, ParseTreeNode cond)
        {
            //DO_WHILE -> Sentencias EXP
            String eAux = GeneradorC3D.getEtiqueta();
            String eInicio = GeneradorC3D.getEtiqueta();
            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, eAux));
            GeneradorC3D.generarEtiquetas((eInicio));
            Nodo nhacer = Expresion.castearC3D((int)Simbolo.Tipo.BOOLEAN,
                Expresion.expresionC3D(cond), cond.Span.Location.Line,
                cond.Span.Location.Column);
            if (nhacer != null)
            {
                GeneradorC3D.generarEtiquetas((eAux));
                GeneradorC3D.generarEtiquetas((nhacer.etqVerdadera));
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Sentencias de do-while"));

                GeneradorC3D.display.agregarCiclo((int)Ciclo.TipoCiclo.DO_WHILE, "", eInicio, nhacer.etqFalsa);
                // Sentencias de HacerMientras
                C3DSentencias.generarC3D(sentencias);
                GeneradorC3D.display.removerCiclo();

                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, eInicio));
                GeneradorC3D.generarEtiquetas((nhacer.etqFalsa));
            }
        }
    }
}
