using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace _Compi2_Proyecto2_201314863
{
    class Repetir
    {
        public static void evaluarRepetirC3D(ParseTreeNode sentencias, ParseTreeNode cond)
        {
            //REPEAT -> Sentencias EXP
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
                GeneradorC3D.generarEtiquetas((nhacer.etqFalsa));
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Sentencias de repeat-until"));

                GeneradorC3D.display.agregarCiclo((int)Ciclo.TipoCiclo.REPEAT, "", eInicio, nhacer.etqFalsa);
                // Sentencias de Repeat
                C3DSentencias.generarC3D(sentencias);
                GeneradorC3D.display.removerCiclo();

                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, eInicio));
                GeneradorC3D.generarEtiquetas((nhacer.etqVerdadera));
            }
        }
    }
}
