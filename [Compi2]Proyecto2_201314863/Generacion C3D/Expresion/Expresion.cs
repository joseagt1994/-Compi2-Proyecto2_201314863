using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace _Compi2_Proyecto2_201314863
{
    public class Expresion
    {
        public static Nodo expresionC3D(ParseTreeNode nodo)
        {
            switch (nodo.ChildNodes.Count)
            {
                #region "3 hijos"
                case 3:
                    //EXP -> 3 hijos
                    ParseTreeNode nder = nodo.ChildNodes[1];
                    String operador;
                    if (nder.Term.Name.Equals("EXP"))
                    {
                        operador = nodo.ChildNodes[2].Token.Value.ToString();
                    }
                    else
                    {
                        operador = nodo.ChildNodes[1].Token.Value.ToString();
                        nder = nodo.ChildNodes[2];
                    }
                    if (operador.Equals("+") || operador.Equals("-") || operador.Equals("*") || operador.Equals("/") || operador.Equals("pow") || operador.Equals("^"))
                    {
                        //ARITMETICAS!
                        Nodo aritmetica = Aritmetica.generarC3D(nodo.ChildNodes.ElementAt(0), operador, nder);
                        if (aritmetica != null)
                        {
                            aritmetica.referencia = "error";
                        }
                        return aritmetica;
                    }
                    else if (operador.Equals("==") || operador.Equals("!=") || operador.Equals(">") || operador.Equals(">=") || operador.Equals("<") || operador.Equals("<="))
                    {
                        //RELACIONALES!
                        Nodo relacional = Relacional.generarC3D(nodo.ChildNodes.ElementAt(0), operador, nder);
                        if (relacional != null)
                        {
                            relacional.referencia = "error";
                        }
                        return relacional;
                    }
                    else
                    {
                        //LOGICAS!
                        Nodo logica = Logica.generarC3D(nodo.ChildNodes.ElementAt(0), operador, nder);
                        if (logica != null)
                        {
                            logica.referencia = "error";
                        }
                        return logica;
                    }
                #endregion
                #region "2 hijos"
                case 2:
                    ParseTreeNode n1 = nodo.ChildNodes[0];
                    ParseTreeNode n2 = nodo.ChildNodes[1];
                    /*
                    | EXP + ToTerm("-")
                    | EXP + ToTerm("not")
                    | ToTerm("-") + EXP
                    | ToTerm("not") + EXP
                    | super + punto + ACCESO
                    | self + punto + ACCESO
                    */
                    if (n1.Term.Name.Equals("super") || n1.Term.Name.Equals("este") || n1.Term.Name.Equals("this"))
                    {
                        // (super | self)  ACCESO)
                        if (n1.Term.Name.Equals("super"))
                        {
                            return Acceso.generarC3DAcceso(n2, Acceso.Tipo.SUPER, null);
                        }
                        else
                        {
                            return Acceso.generarC3DAcceso(n2, Acceso.Tipo.NINGUNO, null);
                        }
                    }
                    else
                    {
                        // Ver si es de OLC++ o Tree
                        if (n1.Term.Name.Equals("EXP"))
                        {
                            ParseTreeNode aux = n2;
                            n2 = n1;
                            n1 = aux;
                        }
                        if(n1.Term.Name.Equals("not") || n1.Term.Name.Equals("!"))
                        {
                            // NEGACION
                            return Logica.negacionC3D(n2);
                        }
                        else
                        {
                            // UNARIO
                            return Aritmetica.unarioC3D(n2);
                        }
                    }
                    break;
                #endregion
                #region "1 hijo"
                default:
                    //EXP -> 1 hijo
                    String tipo = nodo.ChildNodes.ElementAt(0).Term.Name;
                    Nodo nval = new Nodo();
                    switch (tipo)
                    {
                        case "NUEVO":
                            // Generar codigo 3D de llamara a constructor
                            return Llamada.instanciaC3D(nodo.ChildNodes[0], Acceso.Tipo.NINGUNO);
                        case "NATIVAS":
                            // getBool, getNum, getRandom, getLength(str), getLength(str id,num)
                            //return generarC3DNativas(nodo.ChildNodes.ElementAt(0));
                        case "numero":
                            nval.tipo = (int)Simbolo.Tipo.NUMERO;
                            nval.cadena = nodo.ChildNodes.ElementAt(0).Token.Value.ToString();
                            if (nval.cadena.Contains("."))
                            {
                                nval.tipo = (int)Simbolo.Tipo.DECIMAL;
                            }
                            return nval;
                        case "ACCESO":
                            return Acceso.generarC3DAcceso(nodo.ChildNodes[0], Acceso.Tipo.NINGUNO, null);
                        case "CRECE":
                            return Aritmetica.generarCrecimientoC3D(nodo.ChildNodes[0]);
                        case "cadena":
                            return guardarCadenaC3D(nodo.ChildNodes[0].Token.Value.ToString().Replace("\"", ""));
                        case "caracter":
                            nval.tipo = (int)Simbolo.Tipo.CARACTER;
                            String cadena = nodo.ChildNodes[0].Token.Value.ToString().Replace("'", "");
                            int val = (int)cadena.ElementAt(0);
                            nval.cadena = Convert.ToString(val);
                            return nval;
                        case "BANDERA":
                            return Logica.banderaC3D(nodo.ChildNodes.ElementAt(0));
                        default:
                            return expresionC3D(nodo.ChildNodes.ElementAt(0));
                    }
                    #endregion
            }
            return null;
        }

        //GENERAR CADENAS!
        public static Nodo guardarCadenaC3D(String cadena)
        {
            //CADENA -> tchar | tstring
            Nodo nuevo = new Nodo();
            nuevo.tipo = (int)Simbolo.Tipo.CADENA;
            nuevo.cadena = GeneradorC3D.getTemporal();
            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, nuevo.cadena, "H", "+", "0"));
            foreach (char c in cadena)
            {
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Guardar " + c));
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Heap", "H", Convert.ToString((int)c)));
                GeneradorC3D.aumentarHeap("1");
            }
            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Guardar fin de cadena"));
            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Heap", "H", "0"));
            GeneradorC3D.aumentarHeap("1");
            return nuevo;
        }

       /* //ARREGLOS!
        public Nodo guardarArregloC3D(Simbolo arr, ParseTreeNode indices, String eError)
        {
            // INDICES -> INDICES INDICE
            //          | INDICE
            Nodo nuevo = new Nodo();
            if (indices.ChildNodes.Count == 2)
            {
                Nodo indiceE = guardarArregloC3D(arr, indices.ChildNodes.ElementAt(0), eError);
                Nodo dimInterna = getInternasC3D(arr, indices.ChildNodes.ElementAt(1), eError, indiceE.externa);
                instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// " + indiceE.externa.ToString() + " dimension externa!"));
                String tam = tablaSimbolos.getTamanio(arr, indiceE.externa, dimInterna.interna).ToString();
                String temp1 = getTemporal();
                instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, temp1, dimInterna.cadena, "*", tam));
                nuevo.cadena = getTemporal();
                instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, nuevo.cadena, indiceE.cadena, "+", temp1));
                nuevo.externa = indiceE.externa + 1;
            }
            else
            {
                Nodo dimInterna = getInternasC3D(arr, indices.ChildNodes.ElementAt(0), eError, 1);
                instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// 1 dimension externa!"));
                nuevo.cadena = getTemporal();
                String tam = tablaSimbolos.getTamanio(arr, 1, dimInterna.interna).ToString();
                instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, nuevo.cadena, dimInterna.cadena, "*", tam));
                nuevo.externa = 2;
            }
            return nuevo;
        }

        public Nodo getInternasC3D(Simbolo arr, ParseTreeNode indice, String eError, int externa)
        {
            // INDICE -> INDICE EXP
            //         | EXP
            Nodo nuevo = new Nodo();
            if (indice.ChildNodes.Count == 2)
            {
                Nodo indice1 = getInternasC3D(arr, indice.ChildNodes.ElementAt(0), eError, externa);
                Nodo exp = expresionC3D(indice.ChildNodes.ElementAt(1));
                String inf = tablaSimbolos.getIndiceInferior(arr, externa, indice1.interna).ToString();
                String sup = tablaSimbolos.getIndiceSuperior(arr, externa, indice1.interna).ToString();
                instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Comparar si es menor que el indice inferior"));
                instrucciones.Add(new C3D((int)C3D.TipoC3D.CONDICIONAL, eError, exp.cadena, "<", inf));
                instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Comparar si es menor que el superior"));
                instrucciones.Add(new C3D((int)C3D.TipoC3D.CONDICIONAL, eError, exp.cadena, ">=", sup));
                instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// El indice esta correcto, se procede"));
                String temp1 = getTemporal();
                instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, temp1, exp.cadena, "-", inf));
                String tam = tablaSimbolos.getTamanio(arr, externa, indice1.interna).ToString();
                String temp2 = getTemporal();
                instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, temp2, temp1, "*", tam));
                nuevo.cadena = getTemporal();
                instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, nuevo.cadena, indice1.cadena, "+", temp2));
                nuevo.interna = indice1.interna + 1;
            }
            else
            {
                //ext -> 1, int -> 1
                Nodo exp = expresionC3D(indice.ChildNodes.ElementAt(0));
                String inf = tablaSimbolos.getIndiceInferior(arr, externa, 1).ToString();
                String sup = tablaSimbolos.getIndiceSuperior(arr, externa, 1).ToString();
                instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Comparar si es menor que el indice inferior"));
                instrucciones.Add(new C3D((int)C3D.TipoC3D.CONDICIONAL, eError, exp.cadena, "<", inf));
                instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Comparar si es menor que el superior"));
                instrucciones.Add(new C3D((int)C3D.TipoC3D.CONDICIONAL, eError, exp.cadena, ">=", sup));
                instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// El indice esta correcto, se procede"));
                nuevo.cadena = getTemporal();
                instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, nuevo.cadena, exp.cadena, "-", inf));
                nuevo.interna = 2;
            }
            return nuevo;
        }

        //ID!
        public Nodo getIDC3D(ParseTreeNode nodo)
        {
            Nodo nuevo = new Nodo();
            nuevo.pila = "Stack";
            Simbolo objeto = tablaSimbolos.buscarVariable(nodo.ChildNodes.ElementAt(0).
                                    Token.Value.ToString(), nombreMetodo);
            if (objeto == null)
            {
                objeto = tablaSimbolos.buscarGlobal(nodo.ChildNodes.ElementAt(0).Token.Value.ToString());
                if (objeto == null)
                {
                    Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                        "La variable " + nodo.ChildNodes.ElementAt(0).Token.Value.ToString() +
                        " no ha sido declarada!", nodo.ChildNodes.ElementAt(0).Token.Location.Line,
                        nodo.ChildNodes.ElementAt(0).Token.Location.Column));
                    return null;
                }
            }
            nuevo.tipo = objeto.tipo;

            String eError = getEtiqueta();
            String eSalida = getEtiqueta();
            ParseTreeNode acceso = nodo.ChildNodes.ElementAt(1);
            // Generacion de codigo de referencia y valor...
            String temp1 = getTemporal();
            nuevo.posicion = temp1;
            if (objeto.ambito == (int)Simbolo.Tipo.GLOBAL)
            {
                nuevo.referencia = "error";
            }
            else
            {
                if (objeto.dimensiones != null)
                {
                    //ES ARREGLO!
                }
                else
                {
                    if (objeto.tipo == (int)Simbolo.Tipo.CADENA || objeto.tipo == (int)Simbolo.Tipo.ESTRUCTURA)
                    {
                        nuevo.referencia = temp1;
                    }
                    else
                    {
                        nuevo.referencia = "error";
                    }
                }
            }
            String val = "P";
            if (objeto.ambito == (int)Simbolo.Tipo.GLOBAL)
            {
                val = "0";
            }
            instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Buscar posicion en pila o heap (global) de " + objeto.nombre));
            C3D ins1 = new C3D((int)C3D.TipoC3D.ASIGNACION, temp1, val, "+", objeto.pos.ToString());
            instrucciones.Add(ins1);
            Simbolo ter1 = new Simbolo();
            ter1.tipo = (int)Simbolo.Tipo.ETIQUETA;
            ter1.nombre = temp1;
            ter1.padre = nombreMetodo;
            ter1.instruccion = ins1;
            nuevo.cadena = getTemporal();
            instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Buscar valor de " + objeto.nombre));
            C3D ins2 = new C3D((int)C3D.TipoC3D.ACCESO, nuevo.pila, nuevo.cadena, temp1);
            instrucciones.Add(ins2);
            Simbolo ter2 = new Simbolo();
            ter2.tipo = (int)Simbolo.Tipo.ETIQUETA;
            ter2.nombre = nuevo.cadena;
            ter2.padre = nombreMetodo;
            ter2.instruccion = ins2;
            if (enRetorno && !esParametro)
            {
                tablaSimbolos.Add(ter1);
                tablaSimbolos.Add(ter2);
            }
            if (acceso.ChildNodes.Count > 0)
            {
                // SE HACE EL ACCESO!
                if (objeto.tipo == (int)Simbolo.Tipo.ESTRUCTURA)
                {
                    nuevo.padre = objeto.estructura;
                    nuevo = getAccesoC3D(nuevo, acceso, eError);
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Salto a salida"));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, eSalida));
                    generarEtiquetas((eError));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// NullPointerException"));
                    String taux = getTemporal();
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Apuntar al proximo ambito"));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, taux, "P", "+", tamMetodo.ToString()));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Envio del primer parametro, referencia"));
                    String tpar1 = getTemporal();
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, tpar1, taux, "+", "102"));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Stack", tpar1, "0"));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Aumentar ambito de exit"));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, "P", "P", "+", tamMetodo.ToString()));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.EXIT, "exit"));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Disminuir ambito de exit"));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, "P", "P", "-", tamMetodo.ToString()));
                    generarEtiquetas((eSalida));
                }
                else
                {
                    //Error!
                    Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                        "El objeto " + objeto.nombre + " no es una estructura, no se puede acceder!",
                        nodo.ChildNodes.ElementAt(0).Token.Location.Line,
                        nodo.ChildNodes.ElementAt(0).Token.Location.Column));
                    return null;
                }
            }
            return nuevo;
        }

        //ACCESO!
        public Nodo getAccesoC3D(Nodo objeto, ParseTreeNode accesos, String eError)
        {
            // ID (ACCESO -> .ID.ID.ID...)
            // En objeto trae el valor en cadena y en padre trae el nombre del objeto a accesar

            foreach (ParseTreeNode acceso in accesos.ChildNodes)
            {
                int tipo = objeto.tipo;
                String padre = objeto.padre;
                if (tipo == (int)Simbolo.Tipo.ESTRUCTURA)
                {
                    Simbolo atributo = tablaSimbolos.buscarAtributo(acceso.Token.Value.ToString(), padre);
                    if (atributo == null)
                    {
                        Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                            "El atributo " + acceso.Token.Value.ToString() +
                            " no existe en " + objeto.padre + "!", acceso.Token.Location.Line,
                            acceso.Token.Location.Column));
                        return null;
                    }
                    objeto.tipo = atributo.tipo;
                    objeto.padre = atributo.estructura;
                    // Realizar C3D
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Verificar que no sea NULL"));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.CONDICIONAL, eError, objeto.cadena, "==", "-201314863.102494"));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Obtener posicion de " + atributo.nombre));
                    objeto.referencia = getTemporal();
                    objeto.posicion = objeto.referencia;
                    objeto.pila = "Heap";
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, objeto.referencia, objeto.cadena, "+", atributo.pos.ToString()));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Obtener valor de " + atributo.nombre));
                    objeto.cadena = getTemporal();
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.ACCESO, "Heap", objeto.cadena, objeto.referencia));
                    if (!(objeto.tipo == (int)Simbolo.Tipo.CADENA || objeto.tipo == (int)Simbolo.Tipo.ESTRUCTURA))
                    {
                        objeto.referencia = "error";
                    }
                }
                else
                {
                    //Error! No se puede acceder algo que no sea una estructura!
                    Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                        "El atributo " + padre + " no es una estructura, no se puede acceder!",
                        acceso.Token.Location.Line,
                        acceso.Token.Location.Column));
                    return null;
                }

            }
            return objeto;
        }

        // NATIVAS
        public Nodo generarC3DNativas(ParseTreeNode nodo)
        {
            Nodo retorno = new Nodo();
            switch (nodo.ChildNodes.ElementAt(0).Term.Name)
            {
                case "inNum":
                    Nodo expN1 = expresionC3D(nodo.ChildNodes.ElementAt(1));
                    Nodo expN2 = expresionC3D(nodo.ChildNodes.ElementAt(2));
                    if (expN1 == null || expN2 == null)
                    {
                        return null;
                    }
                    String taux = getTemporal();
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Apuntar al proximo ambito"));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, taux, "P", "+", tamMetodo.ToString()));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Envio del primer parametro, referencia"));
                    String tpar1 = getTemporal();
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, tpar1, taux, "+", "1"));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Stack", tpar1, expN1.cadena));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Envio del segundo parametro, num"));
                    String tpar2 = getTemporal();
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, tpar2, taux, "+", "2"));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Stack", tpar2, expN2.cadena));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Aumentar ambito de inNum"));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, "P", "P", "+", tamMetodo.ToString()));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.NATIVA, "$$_inNum"));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Guardar el retorno"));
                    retorno.referencia = "error";
                    retorno.tipo = (int)Simbolo.Tipo.NUMERO;
                    String tpos = getTemporal();
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, tpos, "P", "+", "0"));
                    retorno.cadena = getTemporal();
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.ACCESO, "Stack", retorno.cadena, tpos));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Disminuir ambito de inNum"));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, "P", "P", "-", tamMetodo.ToString()));
                    break;
                case "getBool":
                    Nodo expB1 = expresionC3D(nodo.ChildNodes.ElementAt(1));
                    if (expB1 == null)
                    {
                        return null;
                    }
                    String tauxB = getTemporal();
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Apuntar al proximo ambito"));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, tauxB, "P", "+", tamMetodo.ToString()));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Envio del primer parametro, referencia"));
                    String tpar1B = getTemporal();
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, tpar1B, tauxB, "+", "1"));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Stack", tpar1B, expB1.cadena));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Aumentar ambito de getBool"));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, "P", "P", "+", tamMetodo.ToString()));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.NATIVA, "$$_getBool"));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Guardar el retorno"));
                    retorno.referencia = "error";
                    retorno.tipo = (int)Simbolo.Tipo.BOOLEAN;
                    String tposB = getTemporal();
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, tposB, "P", "+", "0"));
                    retorno.cadena = getTemporal();
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.ACCESO, "Stack", retorno.cadena, tposB));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Disminuir ambito de getBool"));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, "P", "P", "-", tamMetodo.ToString()));
                    break;
                case "getNum":
                    Nodo expgN1 = expresionC3D(nodo.ChildNodes.ElementAt(1));
                    Nodo expgN2 = expresionC3D(nodo.ChildNodes.ElementAt(2));
                    Nodo expgN3 = expresionC3D(nodo.ChildNodes.ElementAt(3));
                    if (expgN1 == null || expgN2 == null || expgN3 == null)
                    {
                        return null;
                    }
                    String tauxg = getTemporal();
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Apuntar al proximo ambito"));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, tauxg, "P", "+", tamMetodo.ToString()));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Envio del primer parametro, referencia"));
                    String tpar1g = getTemporal();
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, tpar1g, tauxg, "+", "1"));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Stack", tpar1g, expgN1.cadena));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Envio del segundo parametro, referencia"));
                    String tpar2g = getTemporal();
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, tpar2g, tauxg, "+", "2"));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Stack", tpar2g, expgN2.cadena));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Envio del tercer parametro, num"));
                    String tpar3g = getTemporal();
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, tpar3g, tauxg, "+", "3"));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Stack", tpar3g, expgN3.cadena));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Aumentar ambito de getNum"));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, "P", "P", "+", tamMetodo.ToString()));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.NATIVA, "$$_getNum"));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Guardar el retorno"));
                    retorno.referencia = "error";
                    retorno.tipo = (int)Simbolo.Tipo.NUMERO;
                    String tposg = getTemporal();
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, tposg, "P", "+", "0"));
                    retorno.cadena = getTemporal();
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.ACCESO, "Stack", retorno.cadena, tposg));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Disminuir ambito de getNum"));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, "P", "P", "-", tamMetodo.ToString()));
                    break;
                case "getLength":
                    if (nodo.ChildNodes.Count == 2)
                    {
                        //getLength
                        Nodo expLN1 = expresionC3D(nodo.ChildNodes.ElementAt(1));
                        if (expLN1 == null)
                        {
                            return null;
                        }
                        String tauxl = getTemporal();
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Apuntar al proximo ambito"));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, tauxl, "P", "+", tamMetodo.ToString()));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Envio del primer parametro, referencia"));
                        String tpar1l = getTemporal();
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, tpar1l, tauxl, "+", "1"));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Stack", tpar1l, expLN1.cadena));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Aumentar ambito de getStrLength"));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, "P", "P", "+", tamMetodo.ToString()));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.NATIVA, "$$_getStrLength"));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Guardar el retorno"));
                        retorno.referencia = "error";
                        retorno.tipo = (int)Simbolo.Tipo.NUMERO;
                        String tposl = getTemporal();
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, tposl, "P", "+", "0"));
                        retorno.cadena = getTemporal();
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.ACCESO, "Stack", retorno.cadena, tposl));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Disminuir ambito de getStrLength"));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, "P", "P", "-", tamMetodo.ToString()));

                    }
                    else
                    {
                        //getLengthArreglo   -> id num
                        String id = nodo.ChildNodes.ElementAt(1).Token.Value.ToString();
                        Simbolo arreglo = tablaSimbolos.buscarVariable(id, nombreMetodo);
                        if (arreglo == null)
                        {
                            arreglo = tablaSimbolos.buscarGlobal(id);
                            if (arreglo == null)
                            {
                                Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                                    "El arreglo " + nodo.ChildNodes.ElementAt(0).Token.Value.ToString() +
                                    " no ha sido declarado!", nodo.ChildNodes.ElementAt(0).Token.Location.Line,
                                    nodo.ChildNodes.ElementAt(0).Token.Location.Column));
                                return null;
                            }
                        }
                        if (arreglo.dims == -1)
                        {
                            Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                                "La variable " + nodo.ChildNodes.ElementAt(0).Token.Value.ToString() +
                                " no es de tipo areglo!", nodo.ChildNodes.ElementAt(0).Token.Location.Line,
                                nodo.ChildNodes.ElementAt(0).Token.Location.Column));
                            return null;
                        }
                        int dimension = Convert.ToInt32(nodo.ChildNodes.ElementAt(2).Token.Value.ToString());
                        int tam = tablaSimbolos.getTamanio(arreglo, dimension);
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Aumentar ambito de getArrLength"));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, "P", "P", "+", tamMetodo.ToString()));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.NATIVA, "getArrLength"));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Guardar el retorno"));
                        retorno.referencia = "error";
                        retorno.tipo = (int)Simbolo.Tipo.NUMERO;
                        retorno.cadena = getTemporal();
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, retorno.cadena, tam.ToString(), "", ""));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Disminuir ambito de getArrLength"));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, "P", "P", "-", tamMetodo.ToString()));
                    }
                    break;
                default:
                    // getRandom
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Aumentar ambito de getRandom"));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, "P", "P", "+", tamMetodo.ToString()));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.NATIVA, "$$_getRandom"));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Guardar el retorno"));
                    retorno.referencia = "error";
                    retorno.tipo = (int)Simbolo.Tipo.NUMERO;
                    String tposr = getTemporal();
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, tposr, "P", "+", "0"));
                    retorno.cadena = getTemporal();
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.ACCESO, "Stack", retorno.cadena, tposr));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Disminuir ambito de getRandom"));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, "P", "P", "-", tamMetodo.ToString()));
                    break;
            }

            return retorno;
        }
        */

                //CASTEO DE EXPRESIONES!
        public static Nodo castearC3D(int tipo, Nodo nodo, int f, int c)
        {
            if (nodo == null)
            {
                return null;
            }
            if (nodo.tipo == (int)Simbolo.Tipo.BOOLEAN)
            {
                if (nodo.etqFalsa == null && nodo.etqVerdadera == null)
                {
                    nodo.tipo = (int)Simbolo.Tipo.NUMERO;
                }
            }
            if (nodo.tipo == tipo)
            {
                return nodo;
            }
            else
            {
                Nodo nuevo = new Nodo();
                switch (tipo)
                {
                    case (int)Simbolo.Tipo.NUMERO:
                        if (nodo.tipo == (int)Simbolo.Tipo.CARACTER || nodo.tipo == (int)Simbolo.Tipo.DECIMAL)
                        {
                            nodo.tipo = tipo;
                            return nodo;
                        }
                        else
                        {
                            // CADENA o BOOLEAN
                            if(nodo.tipo == (int)Simbolo.Tipo.CADENA)
                            {
                                Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                                "No se puede castear implicitamente cadena a entero", f, c));
                            }
                            else
                            {
                                Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                                "No se puede castear implicitamente booleano a entero", f, c));
                            }
                            return null;
                        }
                    case (int)Simbolo.Tipo.CARACTER:
                        if (nodo.tipo == (int)Simbolo.Tipo.NUMERO || nodo.tipo == (int)Simbolo.Tipo.DECIMAL)
                        {
                            nodo.tipo = tipo;
                            return nodo;
                        }
                        else
                        {
                            // CADENA o BOOLEAN
                            if (nodo.tipo == (int)Simbolo.Tipo.CADENA)
                            {
                                Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                                "No se puede castear implicitamente cadena a caracter", f, c));
                            }
                            else
                            {
                                Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                                "No se puede castear implicitamente booleano a caracter", f, c));
                            }
                            return null;
                        }
                    case (int)Simbolo.Tipo.DECIMAL:
                        if (nodo.tipo == (int)Simbolo.Tipo.CARACTER || nodo.tipo == (int)Simbolo.Tipo.NUMERO)
                        {
                            nodo.tipo = tipo;
                            return nodo;
                        }
                        else
                        {
                            // CADENA o BOOLEAN
                            if (nodo.tipo == (int)Simbolo.Tipo.CADENA)
                            {
                                Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                                "No se puede castear implicitamente cadena a decimal", f, c));
                            }
                            else
                            {
                                Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                                "No se puede castear implicitamente booleano a decimal", f, c));
                            }
                            return null;
                        }
                    case (int)Simbolo.Tipo.BOOLEAN:
                        // BOOLEAN
                        if (nodo.tipo == (int)Simbolo.Tipo.NUMERO)
                        {
                            nuevo.tipo = tipo;
                            nuevo.etqVerdadera = GeneradorC3D.getEtiqueta();
                            nuevo.etqFalsa = GeneradorC3D.getEtiqueta();
                            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Castear num a bool"));
                            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.CONDICIONAL, nuevo.etqFalsa, nodo.cadena, "==", "0"));
                            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, nuevo.etqVerdadera));
                            return nuevo;
                        }
                        else
                        {
                            Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                                "No se puede castear implicitamente "+Simbolo.getValor(nodo.tipo)+" a bool", f, c));
                            return null;
                        }
                    default:
                        // CADENA
                        Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                                "No se puede castear implicitamente " + Simbolo.getValor(nodo.tipo) + " a cadena", f, c));
                        return null;
                }
            }
        }

    }
}
