using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace _Compi2_Proyecto2_201314863
{
    public class Logica
    {
        public static Nodo generarC3D(ParseTreeNode izq, String operador, ParseTreeNode der)
        {
            Nodo nodo = new Nodo();
            nodo.cadena = GeneradorC3D.getEtiqueta();
            nodo.tipo = (int)Simbolo.Tipo.BOOLEAN;
            GeneradorC3D.generarEtiquetas((nodo.cadena));
            if (operador.Equals("||"))
            {
                // OR
                Nodo nizq = Expresion.expresionC3D(izq);
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
                nizq = Expresion.castearC3D((int)Simbolo.Tipo.BOOLEAN, nizq, izq.Span.Location.Line, izq.Span.Location.Column);
                if (nizq != null)
                {
                    GeneradorC3D.generarEtiquetas((nizq.etqFalsa));
                    Nodo nder = Expresion.expresionC3D(der);
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
                    nder = Expresion.castearC3D((int)Simbolo.Tipo.BOOLEAN, nder, der.Span.Location.Line, der.Span.Location.Column);
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
                // AND
                Nodo nizq = Expresion.expresionC3D(izq);
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
                nizq = Expresion.castearC3D((int)Simbolo.Tipo.BOOLEAN, nizq, izq.Span.Location.Line, izq.Span.Location.Column);
                if (nizq != null)
                {
                    GeneradorC3D.generarEtiquetas((nizq.etqVerdadera));
                    Nodo nder = Expresion.expresionC3D(der);
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
                    nder = Expresion.castearC3D((int)Simbolo.Tipo.BOOLEAN, nder, der.Span.Location.Line, der.Span.Location.Column);
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
            else
            {
                //|? XOR
                Nodo nizq = Expresion.expresionC3D(izq);
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
                nizq = Expresion.castearC3D((int)Simbolo.Tipo.BOOLEAN, nizq, izq.Span.Location.Line, izq.Span.Location.Column);
                if (nizq != null)
                {
                    GeneradorC3D.generarEtiquetas((nizq.etqFalsa));
                    Nodo nder = Expresion.expresionC3D(der);
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
                    nder = Expresion.castearC3D((int)Simbolo.Tipo.BOOLEAN, nder, der.Span.Location.Line, der.Span.Location.Column);
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

        public static Nodo banderaC3D(ParseTreeNode bandera)
        {
            Nodo nuevo = new Nodo();
            nuevo.tipo = (int)Simbolo.Tipo.BOOLEAN; //BOOLEAN

            if (bandera.ChildNodes.ElementAt(0).Token.Value.ToString().Equals("true"))
            {
                nuevo.cadena = "1";
            }
            else
            {
                nuevo.cadena = "0";
            }
            return nuevo;
        }

        public static Nodo negacionC3D(ParseTreeNode exp)
        {
            Nodo cond = Expresion.castearC3D((int)Simbolo.Tipo.BOOLEAN, Expresion.expresionC3D(exp),
                exp.Span.Location.Line, exp.Span.Location.Column);
            if(cond != null)
            {
                if(cond.tipo == (int)Simbolo.Tipo.BOOLEAN)
                {
                    String eTemp = cond.etqVerdadera;
                    cond.etqVerdadera = cond.etqFalsa;
                    cond.etqFalsa = eTemp;
                }
            }
            return cond;
        }
    }
}
