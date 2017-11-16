using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace _Compi2_Proyecto2_201314863
{
    class Mientras
    {
        public static void evaluarMientrasC3D(ParseTreeNode exp, ParseTreeNode sentencias)
        {
            String eInicio = GeneradorC3D.getEtiqueta();
            GeneradorC3D.generarEtiquetas((eInicio));
            Nodo nmientras = Expresion.castearC3D((int)Simbolo.Tipo.BOOLEAN,
                Expresion.expresionC3D(exp), exp.Span.Location.Line,
                exp.Span.Location.Column);
            if (nmientras != null)
            {
                GeneradorC3D.generarEtiquetas((nmientras.etqVerdadera));
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Sentencias de while"));

                GeneradorC3D.display.agregarCiclo((int)Ciclo.TipoCiclo.WHILE, "", eInicio, nmientras.etqFalsa);
                //Sentencias de while
                C3DSentencias.generarC3D(sentencias);
                GeneradorC3D.display.removerCiclo();

                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, eInicio));
                GeneradorC3D.generarEtiquetas((nmientras.etqFalsa));
            }
        }
    }
}
