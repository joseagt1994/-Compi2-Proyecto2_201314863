using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace _Compi2_Proyecto2_201314863
{
    public class C3DSentencias
    {
        public static Clase claseActual;
        public static Simbolo procedimientoActual;
        public static String eFinProcedimiento;

        public static void generarC3D(ParseTreeNode sentencias)
        {
            foreach(ParseTreeNode sentencia in sentencias.ChildNodes)
            {
                switch (sentencia.Term.Name)
                {
                    case "DECLARACION":
                        if(sentencia.ChildNodes.Count == 3)
                        {
                            declararAsignarC3D(sentencia);
                        }
                        break;
                    case "ASIGNACION":
                        asignarC3D(sentencia);
                        break;
                    case "SUPER":
                        if (sentencia.ChildNodes[1].Term.Name.Equals("ASIGNACION"))
                        {
                            asignarC3D(sentencia);
                        }
                        else if (sentencia.ChildNodes[1].Term.Name.Equals("ACCESO"))
                        {
                            // super + ACCESO
                            Acceso.generarC3DAcceso(sentencia.ChildNodes[1], Acceso.Tipo.SUPER, null);
                        }
                        else
                        {
                            // super + [ + EXPS + ]

                        }
                        break;
                    case "ACCESO":
                        Acceso.generarC3DAcceso(sentencia, Acceso.Tipo.NINGUNO, null);
                        break;
                    case "RETORNO":
                        if(procedimientoActual != null)
                        {
                            if(procedimientoActual.rol == (int)Simbolo.Tipo.FUNCION)
                            {
                                // Evaluar expresion
                                Nodo expRetorno = Expresion.expresionC3D(sentencia.ChildNodes[0]);
                                if (expRetorno != null)
                                {
                                    // Guardar en retorno la posicion del objeto creado
                                    String temp = C3DSentencias.retornarC3D(sentencia.Span.Location.Line, sentencia.Span.Location.Column);
                                    if (!temp.Equals(""))
                                    {
                                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                                            "// Guardar el retorno en la posicion"));
                                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Stack", temp,
                                            expRetorno.cadena));
                                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, 
                                            "// Salto al final de la funcion "+procedimientoActual.nombre));
                                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, eFinProcedimiento));
                                    }
                                }
                            }
                            else
                            {
                                if(procedimientoActual.rol == (int)Simbolo.Tipo.METODO)
                                {
                                    Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                                    "Retornar vino dentro de un metodo!", sentencia.Span.Location.Line,
                                    sentencia.Span.Location.Column));
                                }
                                else
                                {
                                    Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                                    "Retornar vino dentro de un constructor!", sentencia.Span.Location.Line,
                                    sentencia.Span.Location.Column));
                                }
                            }
                        }
                        break;
                    case "IMPRIMIR":
                        Nodo exp = Expresion.expresionC3D(sentencia.ChildNodes[1]);
                        // Recorrer e imprimir segun el caso
                        if(exp != null)
                        {
                            Imprimir.imprimirC3D(exp);
                        }
                        break;
                    case "NATIVAS":
                        break;
                    case "CONTINUAR":
                        // continue;
                        String eInicio = GeneradorC3D.display.buscarInicio();
                        if (eInicio == null)
                        {
                            Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                                "Continuar vino fuera de un ciclo.", sentencia.Span.Location.Line,
                                sentencia.Span.Location.Column));
                        }
                        else
                        {
                            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Continue"));
                            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, eInicio));
                        }
                        break;
                    case "INTERRUMPIR":
                        // break;
                        String eSalidaI = GeneradorC3D.display.buscarSalida();
                        if (eSalidaI == null)
                        {
                            Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                                "Interrumpir vino fuera de un ciclo.", sentencia.Span.Location.Line,
                                sentencia.Span.Location.Column));
                        }
                        else
                        {
                            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Break"));
                            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, eSalidaI));
                        }
                        break;
                    case "IF1":
                        // IF1 -> EXP Sentencias
                        String eSalida = GeneradorC3D.getEtiqueta();
                        Si.evaluarSi(sentencia.ChildNodes[0], sentencia.ChildNodes[1], eSalida);
                        GeneradorC3D.generarEtiquetas(eSalida);
                        break;
                    case "IF2":
                        // IF2 -> EXP Sentencias Sentencias
                        String eSalida2 = GeneradorC3D.getEtiqueta();
                        Si.evaluarSi(sentencia.ChildNodes[0], sentencia.ChildNodes[1], eSalida2);
                        Si.evaluarSino(sentencia.ChildNodes[2], eSalida2);
                        GeneradorC3D.generarEtiquetas(eSalida2);
                        break;
                    case "IF3":
                        // IF3 -> EXP Sentencias IF4
                        /*
                         * IF4.Rule = IF4 EXP Sentencias
                                    | EXP Sentencias;
                         * */
                        String eSalida3 = GeneradorC3D.getEtiqueta();
                        Si.evaluarSi(sentencia.ChildNodes[0], sentencia.ChildNodes[1], eSalida3);
                        Si.evaluarIF4(sentencia.ChildNodes[2], eSalida3);
                        GeneradorC3D.generarEtiquetas(eSalida3);
                        break;
                    case "IF5":
                        // IF5 -> EXP Sentencias IF4 Sentencias
                        String eSalida4 = GeneradorC3D.getEtiqueta();
                        Si.evaluarSi(sentencia.ChildNodes[0], sentencia.ChildNodes[1], eSalida4);
                        Si.evaluarIF4(sentencia.ChildNodes[2], eSalida4);
                        Si.evaluarSino(sentencia.ChildNodes[3], eSalida4);
                        GeneradorC3D.generarEtiquetas(eSalida4);
                        break;
                    case "FOR":
                        // FOR.Rule = Fasignar (D o A) EXP EXP Sentencias 
                        // Declarar o asignar!
                        Para.evaluarParaC3D(sentencia.ChildNodes[0], sentencia.ChildNodes[1],
                            sentencia.ChildNodes[2], sentencia.ChildNodes[3]);
                        break;
                    case "WHILE":
                        //WHILE -> EXP Sentencias
                        Mientras.evaluarMientrasC3D(sentencia.ChildNodes[0], sentencia.ChildNodes[1]);
                        break;
                    case "X":
                        X.evaluarXC3D(sentencia.ChildNodes[0], sentencia.ChildNodes[1], sentencia.ChildNodes[2]);
                        break;
                    case "REPEAT":
                        Repetir.evaluarRepetirC3D(sentencia.ChildNodes[0], sentencia.ChildNodes[1]);
                        break;
                    case "DO_WHILE":
                        Hacer.evaluarHacerC3D(sentencia.ChildNodes[0], sentencia.ChildNodes[1]);
                        break;
                    case "SWITCH":
                        Elegir.evaluarElegirC3D(sentencia.ChildNodes[0], sentencia.ChildNodes[1]);
                        break;
                    case "LOOP":
                        Loop.evaluarLoopC3D(sentencia.ChildNodes[0].ChildNodes[0]);
                        break;
                }
            }
        }

        public static String retornarC3D(int f, int c)
        {
            if(procedimientoActual.rol != (int)Simbolo.Tipo.CONSTRUCTOR)
            {
                if(procedimientoActual.rol == (int)Simbolo.Tipo.METODO)
                {
                    // Error!
                    Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                        "El metodo "+procedimientoActual.nombre+"no tiene retorno!", f, c));
                    return "";
                }
            }
            String temp = GeneradorC3D.getTemporal();
            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                    "// Obtener posicion del retorno en el ambito actual "));
            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, temp, "P",
                "+", "2"));
            return temp;
        }

        public static void declararAsignarC3D(ParseTreeNode declaracion)
        {
            /* OLC++
             * DECLARACION.Rule = TIPO + LISTA_IDS + asignarR *---*
                             | TIPO + LISTA_IDS + DARREGLO -> INDICES (ARREGLO)? *---*
                             | TIPO + LISTA_IDS;
             * Tree
             * DECLARACION.Rule = TIPO + LISTA_IDS + asignarR *---*
                             | TIPO + LISTA_IDS
                             | TIPO + NARREGLO;
            */
            if(declaracion.ChildNodes.Count == 3)
            {
                int tipo = Simbolo.getTipo(declaracion.ChildNodes[0].Token.Text);
                ParseTreeNode ids = declaracion.ChildNodes[1];
                foreach (ParseTreeNode id in ids.ChildNodes)
                {
                    if (declaracion.ChildNodes[2].Term.Name.Equals("DARREGLO"))
                    {
                        ParseTreeNode darr = declaracion.ChildNodes[2];
                        // Guardar espacio
                        if (darr.ChildNodes.Count == 2)
                        {
                            // DARREGLO -> INDICES ARREGLO
                            Arreglo.guardarC3D(id.Token.Text, darr.ChildNodes[1], Acceso.Tipo.NINGUNO);
                        }
                        else
                        {
                            Arreglo.guardarC3D(id.Token.Text, null, Acceso.Tipo.NINGUNO);
                        }
                    }
                    else
                    {
                        // EXP
                        Acceso.actual = null;
                        Nodo exp = Expresion.expresionC3D(declaracion.ChildNodes[2]);
                        Nodo nodo = Acceso.generarC3DID(id.Token.Text, Acceso.Tipo.NINGUNO,
                            "P", "Stack");
                        // Asignar la expresion
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR,
                            nodo.estructura, nodo.referencia, exp.cadena));
                    }
                }
            }
            else
            {
                // 2 hijos
                if (declaracion.ChildNodes[1].Term.Name.Equals("NARREGLO"))
                {
                    ParseTreeNode id = declaracion.ChildNodes[1].ChildNodes[0];
                    Arreglo.guardarC3D(id.Token.Text, null, Acceso.Tipo.NINGUNO);
                }
            }
        }
        
        public static void asignarC3D(ParseTreeNode asignacion)
        {
            /* Tree 
                * ASIGNACION.Rule = ACCESO + asignar + EXP;
             * OLC++
                * ASIGNACION.Rule = self + punto + ACCESO + asignar + EXP
                | ACCESO + asignar + EXP;
            */
            int indice = 0;
            Acceso.Tipo tipo = Acceso.Tipo.NINGUNO;
            if(asignacion.ChildNodes[0].Term.Name.Equals("self") || asignacion.ChildNodes[0].Term.Name.Equals("este"))
            {
                tipo = Acceso.Tipo.ESTE;
                indice++;
            }
            else if(asignacion.ChildNodes[0].Term.Name.Equals("super"))
            {
                tipo = Acceso.Tipo.SUPER;
                asignacion = asignacion.ChildNodes[1];
            }
            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Obtener valor de la asignacion"));
            Nodo exp = Expresion.expresionC3D(asignacion.ChildNodes[indice + 1]);
            if(exp != null)
            {
                if (asignacion.ChildNodes[indice].Term.Name.Equals("ACCESO"))
                {
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                        "// Empezar a Realizar el acceso"));
                    Acceso.generarC3DAcceso(asignacion.ChildNodes[indice], tipo, exp);
                }
            }
        }

    }
}
