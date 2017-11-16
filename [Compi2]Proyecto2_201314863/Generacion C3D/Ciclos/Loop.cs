using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace _Compi2_Proyecto2_201314863
{
    class Loop
    {
        public static void evaluarLoopC3D(ParseTreeNode sentencias)
        {
            //LOOP -> Sentencias
            String eInicio = GeneradorC3D.getEtiqueta();
            String eSal = GeneradorC3D.getEtiqueta();
            GeneradorC3D.generarEtiquetas((eInicio));
            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Sentencias de loop"));

            GeneradorC3D.display.agregarCiclo((int)Ciclo.TipoCiclo.LOOP, "", eInicio, eSal);
            // Sentencias de Loop
            C3DSentencias.generarC3D(sentencias);
            Ciclo loop = GeneradorC3D.display.getCiclo();
            if (loop.interrupciones == 0)
            {
                Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                    "El loop no contiene sentencia de escape!", sentencias.Span.Location.Line,
                    sentencias.Span.Location.Column));
            }
            GeneradorC3D.display.removerCiclo();

            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, eInicio));
            GeneradorC3D.generarEtiquetas((eSal));
        }
    }
}
