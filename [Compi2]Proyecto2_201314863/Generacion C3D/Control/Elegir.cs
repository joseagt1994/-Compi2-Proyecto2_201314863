using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace _Compi2_Proyecto2_201314863
{
    class Elegir
    {
        /*
         * SWITCH.Rule = selector + caso + EXP + dosp + Eos + BCASO;

            BCASO.Rule = Indent + CASOS + DEFECTO + Dedent
                | Indent + CASOS + Dedent;

            CASOS.Rule = MakeStarRule(CASOS, CASO);

            CASO.Rule = EXP + dosp + Eos + BLOQUE;
            
            DEFECTO.Rule = defecto + dosp + Eos + BLOQUE;
         */
        public static void evaluarElegirC3D(ParseTreeNode expresion, ParseTreeNode bcaso)
        {
            //SWITCH -> EXP BCASO
            List<string> etiquetasCaso = new List<string>();
            String eTest = GeneradorC3D.getEtiqueta();
            String eSal = GeneradorC3D.getEtiqueta();
            //Recorrer EXP
            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ETIQUETA, eTest));
            Nodo nexp = Expresion.expresionC3D(expresion);
            if(nexp != null)
            {
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, eTest));
                //Recorrer cada caso
                foreach (ParseTreeNode caso in bcaso.ChildNodes[0].ChildNodes)
                {
                    //Caso -> ECASO Sentencias
                    String eCaso = GeneradorC3D.getEtiqueta();
                    etiquetasCaso.Add(eCaso);
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Inicio de caso "));
                    GeneradorC3D.generarEtiquetas((eCaso));
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Sentencias de caso "));

                    GeneradorC3D.display.agregarCiclo((int)Ciclo.TipoCiclo.SWITCH, "", "", eSal);
                    // Sentencias de Switch
                    C3DSentencias.generarC3D(caso.ChildNodes[1].ChildNodes[0]);
                    GeneradorC3D.display.removerCiclo();
                }
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, eSal));
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ETIQUETA, eTest));
                int contar = 0;
                foreach (ParseTreeNode caso in bcaso.ChildNodes[0].ChildNodes)
                {
                    evaluarCasoC3D(caso.ChildNodes[0], nexp, etiquetasCaso[contar]);
                    contar++;
                }
                //Defecto?
                if (bcaso.ChildNodes.Count == 2)
                {
                    //Sentencias de Defecto
                    GeneradorC3D.display.agregarCiclo((int)Ciclo.TipoCiclo.SWITCH, "", "", eSal);
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Sentencias de defecto"));
                    GeneradorC3D.display.removerCiclo();
                    C3DSentencias.generarC3D(bcaso.ChildNodes[0].ChildNodes[0]);
                }
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Etiqueta de salida del switch"));
                GeneradorC3D.generarEtiquetas((eSal));
            }
        }

        public static void evaluarCasoC3D(ParseTreeNode exp, Nodo pivote, String eCaso)
        {
            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                "// Evaluar expresion del caso"));
            Nodo ncaso = Expresion.expresionC3D(exp);
            if(ncaso != null)
            {
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                    "// Evaluar condicion del caso"));
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.CONDICIONAL,
                    eCaso, ncaso.cadena, "==", pivote.cadena));
            }
        }
    }
}
