using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace _Compi2_Proyecto2_201314863
{
    public class Expresion
    {/*
        public static Nodo expresionC3D(ParseTreeNode nodo)
        {
            switch (nodo.ChildNodes.Count)
            {
                #region "3 hijos"
                case 3:
                    //EXP -> 3 hijos
                    String operador = nodo.ChildNodes.ElementAt(1).Token.Value.ToString();
                    if (operador.Equals("+") || operador.Equals("-") || operador.Equals("*") || operador.Equals("/") || operador.Equals("pow") || operador.Equals("^"))
                    {
                        //ARITMETICAS!
                        Nodo aritmetica = Aritmetica.generarC3D(nodo.ChildNodes.ElementAt(0), operador, nodo.ChildNodes.ElementAt(2));
                        if (aritmetica != null)
                        {
                            aritmetica.referencia = "error";
                        }
                        return aritmetica;
                    }
                    else if (operador.Equals("==") || operador.Equals("!=") || operador.Equals(">") || operador.Equals(">=") || operador.Equals("<") || operador.Equals("<="))
                    {
                        //RELACIONALES!
                        Nodo relacional = relacionalC3D(nodo.ChildNodes.ElementAt(0), operador, nodo.ChildNodes.ElementAt(2));
                        if (relacional != null)
                        {
                            relacional.referencia = "error";
                        }
                        return relacional;
                    }
                    else
                    {
                        //LOGICAS!
                        Nodo logica = logicaC3D(nodo.ChildNodes.ElementAt(0), operador, nodo.ChildNodes.ElementAt(2));
                        if (logica != null)
                        {
                            logica.referencia = "error";
                        }
                        return logica;
                    }
                #endregion
                #region "2 hijos"
                case 2:
                    break;
                #endregion
                #region "1 hijo"
                default:
                    //EXP -> 1 hijo
                    String tipo = nodo.ChildNodes.ElementAt(0).Term.Name;
                    Nodo nval = new Nodo();
                    switch (tipo)
                    {
                        case "LLAMADA":
                            // Generar codigo 3D de llamada a metodo!
                            return generarC3DMetodo(nodo.ChildNodes.ElementAt(0), true);
                        case "NATIVAS":
                            // getBool, getNum, getRandom, getLength(str), getLength(str id,num)
                            return generarC3DNativas(nodo.ChildNodes.ElementAt(0));
                        case "numero":
                            nval.tipo = (int)Simbolo.Tipo.NUMERO;
                            nval.cadena = nodo.ChildNodes.ElementAt(0).Token.Value.ToString();
                            if (nval.cadena.Contains("."))
                            {
                                nval.tipo = (int)Simbolo.Tipo.DECIMAL;
                            }
                            return nval;
                        case "ACCESO":
                            return null;
                        case "CRECE":
                            return null;
                        case "cadena":
                            return guardarCadenaC3D(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Token.Value.ToString());
                        case "caracter":
                            return null;
                        case "BANDERA":
                            return banderaC3D(nodo.ChildNodes.ElementAt(0));
                        default:
                            return expresionC3D(nodo.ChildNodes.ElementAt(0));
                    }
                    #endregion
            }
            return null;
        }

        //GENERAR CODIGO 3D DE EXPRESION!
        /*public Nodo expresionC3D(ParseTreeNode nodo)
        {
            switch (nodo.ChildNodes.Count)
            {
                case 3:
                    //EXP -> 3 hijos
                    String operador = nodo.ChildNodes.ElementAt(1).Token.Value.ToString();
                    if (operador.Equals("+") || operador.Equals("-") || operador.Equals("*") || operador.Equals("/") || operador.Equals("%") || operador.Equals("^"))
                    {
                        //ARITMETICAS!
                        Nodo aritmetica = aritmeticaC3D(nodo.ChildNodes.ElementAt(0), operador, nodo.ChildNodes.ElementAt(2));
                        if (aritmetica != null)
                        {
                            aritmetica.referencia = "error";
                        }
                        return aritmetica;
                    }
                    else if (operador.Equals("==") || operador.Equals("!=") || operador.Equals(">") || operador.Equals(">=") || operador.Equals("<") || operador.Equals("<="))
                    {
                        //RELACIONALES!
                        Nodo relacional = relacionalC3D(nodo.ChildNodes.ElementAt(0), operador, nodo.ChildNodes.ElementAt(2));
                        if (relacional != null)
                        {
                            relacional.referencia = "error";
                        }
                        return relacional;
                    }
                    else
                    {
                        //LOGICAS!
                        Nodo logica = logicaC3D(nodo.ChildNodes.ElementAt(0), operador, nodo.ChildNodes.ElementAt(2));
                        if (logica != null)
                        {
                            logica.referencia = "error";
                        }
                        return logica;
                    }
                case 2:
                    //EXP -> 2 hijos
                    String valor = nodo.ChildNodes.ElementAt(0).Term.Name;
                    switch (valor)
                    {
                        case "-":
                            Nodo nexpu = expresionC3D(nodo.ChildNodes.ElementAt(1));
                            if (nexpu.tipo == (int)Simbolo.Tipo.BOOLEAN)
                            {
                                if (nexpu.etqFalsa != null && nexpu.etqVerdadera != null)
                                {
                                    nexpu = castearC3D((int)Simbolo.Tipo.NUMERO, nexpu,
                                        nodo.ChildNodes.ElementAt(0).Span.Location.Line,
                                        nodo.ChildNodes.ElementAt(0).Span.Location.Column);
                                }
                            }
                            else if (nexpu.tipo == (int)Simbolo.Tipo.CADENA)
                            {
                                Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                                    "No de puede operar negacion a str", nodo.ChildNodes.ElementAt(0).Span.Location.Line,
                                    nodo.ChildNodes.ElementAt(0).Span.Location.Column));
                                return null;
                            }
                            Nodo una = new Nodo();
                            una.cadena = getTemporal();
                            una.referencia = "error";
                            instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, una.cadena, "-1", "*", nexpu.cadena));
                            return una;
                        case "!":
                            Nodo nuevo = new Nodo();
                            Nodo nexp = expresionC3D(nodo.ChildNodes.ElementAt(1));
                            if (nexp.tipo == (int)Simbolo.Tipo.BOOLEAN)
                            {
                                if (nexp.etqFalsa == null && nexp.etqVerdadera == null)
                                {
                                    nexp.tipo = (int)Simbolo.Tipo.NUMERO;
                                }
                            }
                            else if (nexp.tipo == (int)Simbolo.Tipo.NUMERO)
                            {
                                nexp = castearC3D((int)Simbolo.Tipo.BOOLEAN, nexp, nodo.ChildNodes.ElementAt(1).Span.Location.Line, nodo.ChildNodes.ElementAt(0).Span.Location.Column);
                            }
                            else
                            {
                                // Error! No puede ser de tipo cadena!
                                Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                                    "No de puede operar NOT a str", nodo.ChildNodes.ElementAt(0).Span.Location.Line,
                                    nodo.ChildNodes.ElementAt(0).Span.Location.Column));
                                return null;
                            }
                            nuevo.etqFalsa = nexp.etqVerdadera;
                            nuevo.etqVerdadera = nexp.etqFalsa;
                            nuevo.referencia = "error";
                            return nuevo;
                        case "id":
                            // EXP ->  id INDICES
                            if (nodo.ChildNodes.ElementAt(1).Term.Name.Equals("ACCESO"))
                            {
                                return getIDC3D(nodo);
                            }
                            else
                            {
                                Simbolo arreglo = tablaSimbolos.buscarVariable(nodo.ChildNodes.ElementAt(0).
                                Token.Value.ToString(), nombreMetodo);
                                if (arreglo == null)
                                {
                                    arreglo = tablaSimbolos.buscarGlobal(nodo.ChildNodes.ElementAt(0).Token.Value.ToString());
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
                                String eError = getEtiqueta();
                                String eSal = getEtiqueta();
                                Nodo narr = guardarArregloC3D(arreglo, nodo.ChildNodes.ElementAt(1), eError);
                                instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Obtener valor en " + narr.cadena));
                                String temp = getTemporal();
                                String temp2 = getTemporal();
                                if (arreglo.ambito == (int)Simbolo.Tipo.GLOBAL)
                                {
                                    instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Posicion del arreglo global " + arreglo.nombre));
                                    instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, temp, "0", "+", arreglo.pos.ToString()));
                                    instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, temp2, temp, "+", narr.cadena));
                                }
                                else
                                {
                                    instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Posicion del arreglo local " + arreglo.nombre));
                                    instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, temp, "P", "+", arreglo.pos.ToString()));
                                    instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, temp2, temp, "+", narr.cadena));
                                }
                                String temp3 = getTemporal();
                                instrucciones.Add(new C3D((int)C3D.TipoC3D.ACCESO, "Stack", temp3, temp2));
                                narr.cadena = temp3;
                                narr.tipo = arreglo.tipo;
                                narr.posicion = temp2;
                                narr.pila = "Stack";
                                generarEtiquetas((eError));
                                instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Error en arreglo"));
                                generarEtiquetas((eSal));
                                return narr;
                            }
                        default:
                            //NUEVO.. EXP -> create id
                            Nodo instancia = new Nodo();
                            instancia.cadena = nodo.ChildNodes.ElementAt(1).Token.Value.ToString();
                            Simbolo estructura = tablaSimbolos.buscarEstructura(instancia.cadena, "");
                            if (estructura == null)
                            {
                                Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                                    "No se puede instanciar, no existe la estructura " + instancia.cadena,
                                    nodo.ChildNodes.ElementAt(1).Token.Location.Line,
                                    nodo.ChildNodes.ElementAt(1).Token.Location.Column));
                                return null;
                            }
                            instancia.tipo = (int)Simbolo.Tipo.ESTRUCTURA;
                            instancia.padre = "instancia";
                            return instancia;
                    }
                case 1:
                    //EXP -> 1 hijo
                    String tipo = nodo.ChildNodes.ElementAt(0).Term.Name;
                    Nodo nval = new Nodo();
                    switch (tipo)
                    {
                        case "LLAMADA":
                            // Generar codigo 3D de llamada a metodo!
                            return generarC3DMetodo(nodo.ChildNodes.ElementAt(0), true);
                        case "NATIVAS":
                            // getBool, getNum, getRandom, getLength(str), getLength(str id,num)
                            return generarC3DNativas(nodo.ChildNodes.ElementAt(0));
                        case "numero":
                            nval.tipo = (int)Simbolo.Tipo.NUMERO;
                            nval.cadena = nodo.ChildNodes.ElementAt(0).Token.Value.ToString();
                            nval.referencia = "error";
                            return nval;
                        case "CADENA":
                            return guardarCadenaC3D(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Token.Value.ToString());
                        case "BANDERA":
                            Nodo b = banderaC3D(nodo.ChildNodes.ElementAt(0));
                            b.referencia = "error";
                            return b;
                        case "NULL":
                            nval.tipo = (int)Simbolo.Tipo.VACIO;
                            nval.cadena = "-201314863.241094";
                            nval.referencia = "error";
                            return nval;
                        default:
                            return expresionC3D(nodo.ChildNodes.ElementAt(0));
                    }
                    break;
            }
            return null;
        }

        */
        /*
        public Nodo logicaC3D(ParseTreeNode izq, String operador, ParseTreeNode der)
        {
            Nodo nodo = new Nodo();
            nodo.cadena = getEtiqueta();
            nodo.tipo = (int)Simbolo.Tipo.BOOLEAN;
            generarEtiquetas((nodo.cadena));
            if (operador.Equals("||"))
            {
                Nodo nizq = expresionC3D(izq);
                if (nizq == null)
                {
                    return null;
                }
                if (nizq.tipo == (int)Simbolo.Tipo.BOOLEAN)
                {
                    if (nizq.etqVerdadera == null && nizq.etqFalsa == null)
                    {
                        nizq.tipo = (int)Simbolo.Tipo.NUMERO;
                    }
                }
                nizq = castearC3D((int)Simbolo.Tipo.BOOLEAN, nizq, izq.Span.Location.Line, izq.Span.Location.Column);
                if (nizq != null)
                {
                    generarEtiquetas((nizq.etqFalsa));
                    Nodo nder = expresionC3D(der);
                    if (nder == null)
                    {
                        return null;
                    }
                    if (nder.tipo == (int)Simbolo.Tipo.BOOLEAN)
                    {
                        if (nder.etqVerdadera == null && nder.etqFalsa == null)
                        {
                            nder.tipo = (int)Simbolo.Tipo.NUMERO;
                        }
                    }
                    nder = castearC3D((int)Simbolo.Tipo.BOOLEAN, nder, der.Span.Location.Line, der.Span.Location.Column);
                    if (nder != null)
                    {
                        nodo.etqVerdadera = nizq.etqVerdadera + "," + nder.etqVerdadera;
                        nodo.etqFalsa = nder.etqFalsa;
                    }
                    else
                    {
                        //Ocurrio Error!
                        return null;
                    }
                }
                else
                {
                    //Ocurrio Error!
                    return null;
                }
            }
            else if (operador.Equals("&&"))
            {
                Nodo nizq = expresionC3D(izq);
                if (nizq == null)
                {
                    return null;
                }
                if (nizq.tipo == (int)Simbolo.Tipo.BOOLEAN)
                {
                    if (nizq.etqVerdadera == null && nizq.etqFalsa == null)
                    {
                        nizq.tipo = (int)Simbolo.Tipo.NUMERO;
                    }
                }
                nizq = castearC3D((int)Simbolo.Tipo.BOOLEAN, nizq, izq.Span.Location.Line, izq.Span.Location.Column);
                if (nizq != null)
                {
                    generarEtiquetas((nizq.etqVerdadera));
                    Nodo nder = expresionC3D(der);
                    if (nder == null)
                    {
                        return null;
                    }
                    if (nder.tipo == (int)Simbolo.Tipo.BOOLEAN)
                    {
                        if (nder.etqVerdadera == null && nder.etqFalsa == null)
                        {
                            nder.tipo = (int)Simbolo.Tipo.NUMERO;
                        }
                    }
                    nder = castearC3D((int)Simbolo.Tipo.BOOLEAN, nder, der.Span.Location.Line, der.Span.Location.Column);
                    if (nder != null)
                    {
                        nodo.etqVerdadera = nder.etqVerdadera;
                        nodo.etqFalsa = nizq.etqFalsa + "," + nder.etqFalsa;
                    }
                    else
                    {
                        //Ocurrio un error!
                        return null;
                    }
                }
                else
                {
                    //Ocurrio un Error!
                    return null;
                }
            }
            else if (operador.Equals("|&"))
            {
                Nodo nizq = expresionC3D(izq);
                if (nizq == null)
                {
                    return null;
                }
                if (nizq.tipo == (int)Simbolo.Tipo.BOOLEAN)
                {
                    if (nizq.etqVerdadera == null && nizq.etqFalsa == null)
                    {
                        nizq.tipo = (int)Simbolo.Tipo.NUMERO;
                    }
                }
                nizq = castearC3D((int)Simbolo.Tipo.BOOLEAN, nizq, izq.Span.Location.Line, izq.Span.Location.Column);
                if (nizq != null)
                {
                    generarEtiquetas((nizq.etqVerdadera));
                    Nodo nder = expresionC3D(der);
                    if (nder == null)
                    {
                        return null;
                    }
                    if (nder.tipo == (int)Simbolo.Tipo.BOOLEAN)
                    {
                        if (nder.etqVerdadera == null && nder.etqFalsa == null)
                        {
                            nder.tipo = (int)Simbolo.Tipo.NUMERO;
                        }
                    }
                    nder = castearC3D((int)Simbolo.Tipo.BOOLEAN, nder, der.Span.Location.Line, der.Span.Location.Column);
                    if (nder != null)
                    {
                        generarEtiquetas((nizq.etqFalsa));
                        Nodo n3 = expresionC3D(der);
                        nodo.etqFalsa = nder.etqVerdadera + "," + n3.etqFalsa;
                        nodo.etqVerdadera = nder.etqFalsa + "," + n3.etqVerdadera;
                    }
                    else
                    {
                        //Ocurrio un error!
                        return null;
                    }
                }
                else
                {
                    //Ocurrio un error!
                    return null;
                }
            }
            else if (operador.Equals("&?"))
            {
                Nodo nizq = expresionC3D(izq);
                if (nizq == null)
                {
                    return null;
                }
                if (nizq.tipo == (int)Simbolo.Tipo.BOOLEAN)
                {
                    if (nizq.etqVerdadera == null && nizq.etqFalsa == null)
                    {
                        nizq.tipo = (int)Simbolo.Tipo.NUMERO;
                    }
                }
                nizq = castearC3D((int)Simbolo.Tipo.BOOLEAN, nizq, izq.Span.Location.Line, izq.Span.Location.Column);
                if (nizq != null)
                {
                    generarEtiquetas((nizq.etqVerdadera));
                    Nodo nder = expresionC3D(der);
                    if (nder == null)
                    {
                        return null;
                    }
                    if (nder.tipo == (int)Simbolo.Tipo.BOOLEAN)
                    {
                        if (nder.etqVerdadera == null && nder.etqFalsa == null)
                        {
                            nder.tipo = (int)Simbolo.Tipo.NUMERO;
                        }
                    }
                    nder = castearC3D((int)Simbolo.Tipo.BOOLEAN, nder, der.Span.Location.Line, der.Span.Location.Column);
                    if (nder != null)
                    {
                        nodo.etqVerdadera = nizq.etqFalsa + "," + nder.etqFalsa;
                        nodo.etqFalsa = nder.etqVerdadera;
                    }
                    else
                    {
                        //Ocurrio un error!
                        return null;
                    }
                }
                else
                {
                    //Ocurrio un error!
                    return null;
                }
            }
            else
            {
                //|?
                Nodo nizq = expresionC3D(izq);
                if (nizq == null)
                {
                    return null;
                }
                if (nizq.tipo == (int)Simbolo.Tipo.BOOLEAN)
                {
                    if (nizq.etqVerdadera == null && nizq.etqFalsa == null)
                    {
                        nizq.tipo = (int)Simbolo.Tipo.NUMERO;
                    }
                }
                nizq = castearC3D((int)Simbolo.Tipo.BOOLEAN, nizq, izq.Span.Location.Line, izq.Span.Location.Column);
                if (nizq != null)
                {
                    generarEtiquetas((nizq.etqFalsa));
                    Nodo nder = expresionC3D(der);
                    if (nder == null)
                    {
                        return null;
                    }
                    if (nder.tipo == (int)Simbolo.Tipo.BOOLEAN)
                    {
                        if (nder.etqVerdadera == null && nder.etqFalsa == null)
                        {
                            nder.tipo = (int)Simbolo.Tipo.NUMERO;
                        }
                    }
                    nder = castearC3D((int)Simbolo.Tipo.BOOLEAN, nder, der.Span.Location.Line, der.Span.Location.Column);
                    if (nder != null)
                    {
                        nodo.etqVerdadera = nder.etqFalsa;
                        nodo.etqFalsa = nizq.etqVerdadera + "," + nder.etqVerdadera;
                    }
                    else
                    {
                        //Ocurrio un error!
                        return null;
                    }
                }
                else
                {
                    //Ocurrio un error!
                    return null;
                }
            }
            return nodo;
        }

        public Nodo banderaC3D(ParseTreeNode bandera)
        {
            Nodo nuevo = new Nodo();
            nuevo.tipo = (int)Simbolo.Tipo.BOOLEAN; //BOOLEAN
            //nuevo.cadena = getEtiqueta();
            //generarEtiquetas((nuevo.cadena));
            //nuevo.etqVerdadera = getEtiqueta();
            //nuevo.etqFalsa = getEtiqueta();
            if (bandera.ChildNodes.ElementAt(0).Token.Value.ToString().Equals("true"))
            {
                nuevo.cadena = "1";
                //instrucciones.Add(new C3D((int)C3D.TipoC3D.CONDICIONAL, nuevo.etqVerdadera, "1", "==", "1"));
            }
            else
            {
                nuevo.cadena = "0";
                //instrucciones.Add(new C3D((int)C3D.TipoC3D.CONDICIONAL, nuevo.etqVerdadera, "1", "==", "0"));
            }
            //instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, nuevo.etqFalsa));
            return nuevo;
        }

        //GENERAR CADENAS!
        public Nodo guardarCadenaC3D(String cadena)
        {
            //CADENA -> tchar | tstring
            Nodo nuevo = new Nodo();
            nuevo.tipo = (int)Simbolo.Tipo.CADENA;
            nuevo.cadena = getTemporal();
            instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, nuevo.cadena, "H", "+", "0"));
            aumentarHeap("1");
            instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Reservar espacio para nueva cadena"));
            instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Heap", nuevo.cadena, "S"));
            foreach (char c in cadena)
            {
                instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Guardar " + c));
                instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Pool", "S", Convert.ToString((int)c)));
                aumentarPool();
            }
            instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Guardar fin de cadena"));
            instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Pool", "S", "0"));
            aumentarPool();
            return nuevo;
        }

        //ARREGLOS!
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

        //CASTEO DE EXPRESIONES!
        public Nodo castearC3D(int tipo, Nodo nodo, int f, int c)
        {
            if (nodo == null)
            {
                return null;
            }
            if (nodo.tipo == tipo)
            {
                return nodo;
            }
            else
            {
                Nodo nuevo = new Nodo();
                if (nodo.tipo == (int)Simbolo.Tipo.BOOLEAN)
                {
                    if (nodo.etqFalsa == null && nodo.etqVerdadera == null)
                    {
                        nodo.tipo = (int)Simbolo.Tipo.NUMERO;
                    }
                }
                switch (tipo)
                {
                    case (int)Simbolo.Tipo.NUMERO:
                        if (nodo.tipo == (int)Simbolo.Tipo.BOOLEAN)
                        {
                            nuevo.tipo = tipo;
                            nuevo.cadena = getTemporal();
                            String eSal = getEtiqueta();
                            instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Castear bool a num"));
                            generarEtiquetas((nodo.etqVerdadera));
                            instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Devolver 1"));
                            instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, nuevo.cadena, "1", "", ""));
                            instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, eSal));
                            generarEtiquetas((nodo.etqFalsa));
                            instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Devolver 0"));
                            instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, nuevo.cadena, "0", "", ""));
                            generarEtiquetas((eSal));
                            return nuevo;
                        }
                        else
                        {
                            // CADENA
                            Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                                "No se puede castear implicitamente str a num", f, c));
                            return null;
                        }
                    case (int)Simbolo.Tipo.CADENA:
                        if (nodo.tipo == (int)Simbolo.Tipo.BOOLEAN)
                        {
                            nuevo.cadena = getTemporal();
                            nuevo.tipo = tipo;
                            instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Castear bool a str"));
                            instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, nuevo.cadena, "H", "+", "0"));
                            instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Reservar espacio para nueva cadena"));
                            aumentarHeap("1");
                            instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Heap", nuevo.cadena, "S"));
                            // GENERAR CADENA DE BOOL!
                            String eSal = getEtiqueta();
                            generarEtiquetas((nodo.etqVerdadera));
                            instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Guardar palabra true"));
                            Nodo ncad = guardarCadenaC3D("true");
                            concatenar(ncad.tipo, ncad.cadena);
                            instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, eSal));
                            generarEtiquetas((nodo.etqFalsa));
                            instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Guardar palabra false"));
                            Nodo ncad2 = guardarCadenaC3D("false");
                            concatenar(ncad2.tipo, ncad2.cadena);
                            generarEtiquetas((eSal));
                            instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Guardar fin de cadena"));
                            instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Pool", "S", "0"));
                            aumentarPool();
                            return nuevo;
                        }
                        else
                        {
                            // NUMERO
                            nuevo.cadena = getTemporal();
                            nuevo.tipo = tipo;
                            instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Castear num a str"));
                            instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, nuevo.cadena, "H", "+", "0"));
                            instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Reservar espacio para nueva cadena"));
                            aumentarHeap("1");
                            instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Heap", nuevo.cadena, "S"));
                            // Guardar numero en cadena
                            concatenar(nodo.tipo, nodo.cadena);
                            instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Guardar fin de cadena"));
                            instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Pool", "S", "0"));
                            aumentarPool();
                            return nuevo;
                        }
                    default:
                        // BOOLEAN
                        if (nodo.tipo == (int)Simbolo.Tipo.NUMERO)
                        {
                            nuevo.tipo = tipo;
                            nuevo.etqVerdadera = getEtiqueta();
                            nuevo.etqFalsa = getEtiqueta();
                            instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Castear num a bool"));
                            instrucciones.Add(new C3D((int)C3D.TipoC3D.CONDICIONAL, nuevo.etqFalsa, nodo.cadena, "==", "0"));
                            instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, nuevo.etqVerdadera));
                            return nuevo;
                        }
                        else
                        {
                            // CADENA
                            Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                                "No se puede castear implicitamente str a bool", f, c));
                            return null;
                        }
                }
            }
        }*/

    }
}
