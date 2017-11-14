using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace _Compi2_Proyecto2_201314863
{
    public class Relacional
    {
        /*public Nodo relacionalC3D(ParseTreeNode izq, String operador, ParseTreeNode der)
        {
            switch (operador)
            {
                case "==":
                    return generarIgualC3D(izq, der);
                case "!=":
                    return generarNoIgualC3D(izq, der);
                case ">=":
                    return generarMayorIgualC3D(izq, der);
                case ">":
                    return generarMayorC3D(izq, der);
                case "<=":
                    return generarMenorIgualC3D(izq, der);
                default:
                    //<
                    return generarMenorC3D(izq, der);
            }
        }

        public Nodo generarIgualC3D(ParseTreeNode izq, ParseTreeNode der)
        {
            Nodo nizq = expresionC3D(izq);
            Nodo nder = expresionC3D(der);
            if (nizq == null || nder == null)
            {
                return null;
            }
            Nodo nodo = new Nodo();
            nodo.tipo = (int)Simbolo.Tipo.BOOLEAN;
            nodo.etqVerdadera = getEtiqueta();
            nodo.etqFalsa = getEtiqueta();
            if (nizq.tipo == (int)Simbolo.Tipo.VACIO)
            {
                // NULL == DER
                if (nder.tipo == (int)Simbolo.Tipo.NUMERO || nder.tipo == (int)Simbolo.Tipo.CADENA
                    || nder.tipo == (int)Simbolo.Tipo.ESTRUCTURA)
                {
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Comparar si es NULL"));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.CONDICIONAL, nodo.etqVerdadera, nizq.cadena, "==", nder.cadena));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, nodo.etqFalsa));
                    return nodo;
                }
                else
                {
                    return null;
                }
            }
            if (nder.tipo == (int)Simbolo.Tipo.VACIO)
            {
                // IZQ == NULL
                if (nizq.tipo == (int)Simbolo.Tipo.NUMERO || nizq.tipo == (int)Simbolo.Tipo.CADENA
                    || nizq.tipo == (int)Simbolo.Tipo.ESTRUCTURA)
                {
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Comparar si es NULL"));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.CONDICIONAL, nodo.etqVerdadera, nizq.cadena, "==", nder.cadena));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, nodo.etqFalsa));
                    return nodo;
                }
                else
                {
                    return null;
                }
            }
            switch (nizq.tipo)
            {
                case (int)Simbolo.Tipo.NUMERO:
                    if (nder.tipo == (int)Simbolo.Tipo.NUMERO)
                    {
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Comparar si los dos numeros son iguales"));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.CONDICIONAL, nodo.etqVerdadera, nizq.cadena, "==", nder.cadena));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, nodo.etqFalsa));
                        return nodo;
                    }
                    break;
                case (int)Simbolo.Tipo.BOOLEAN:
                    if (nizq.etqVerdadera != null && nizq.etqFalsa != null)
                    {
                        nizq = castearC3D((int)Simbolo.Tipo.NUMERO, nizq, izq.Span.Location.Line, izq.Span.Location.Column);
                    }
                    if (nder.tipo == (int)Simbolo.Tipo.BOOLEAN)
                    {
                        if (nder.etqVerdadera != null && nder.etqFalsa != null)
                        {
                            nder = castearC3D((int)Simbolo.Tipo.NUMERO, nder, der.Span.Location.Line, der.Span.Location.Column);
                        }
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Comparar si los dos bools son iguales"));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.CONDICIONAL, nodo.etqVerdadera, nizq.cadena, "==", nder.cadena));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, nodo.etqFalsa));
                        return nodo;
                    }
                    break;
                case (int)Simbolo.Tipo.CADENA:
                    if (nder.tipo == (int)Simbolo.Tipo.CADENA)
                    {

                        String temp1 = getTemporal();
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Guardar los punteros a Heap de las cadenas"));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.ACCESO, "Heap", temp1, nizq.cadena));
                        String temp2 = getTemporal();
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.ACCESO, "Heap", temp2, nder.cadena));
                        String taux = getTemporal();
                        String eInicio = getEtiqueta();
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Iniciar un contador en 0"));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, taux, "0", "", ""));
                        generarEtiquetas((eInicio));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Obtener posicion de la cadena 1"));
                        String tpos1 = getTemporal();
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, tpos1, temp1, "+", taux));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Obtener valor Pool[posicion] de cadena 1"));
                        String tval1 = getTemporal();
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.ACCESO, "Pool", tval1, tpos1));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Obtener posicion de la cadena 2"));
                        String tpos2 = getTemporal();
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, tpos2, temp2, "+", taux));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Obtener valor Pool[posicion] de cadena 2"));
                        String tval2 = getTemporal();
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.ACCESO, "Pool", tval2, tpos2));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.CONDICIONAL, nodo.etqFalsa, tval1, "!=", tval2));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.CONDICIONAL, nodo.etqVerdadera, tval1, "==", "0"));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, taux, taux, "+", "1"));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, eInicio));
                        return nodo;
                    }
                    break;
                case (int)Simbolo.Tipo.VACIO:
                    // Si es NULL
                    break;
            }
            Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                "No se puede comparar si son iguales el " + getTipo(nizq.tipo) + " con " + getTipo(nder.tipo) + ".",
                izq.Span.Location.Line, izq.Span.Location.Column));
            return null;
        }

        public Nodo generarNoIgualC3D(ParseTreeNode izq, ParseTreeNode der)
        {
            Nodo nizq = expresionC3D(izq);
            Nodo nder = expresionC3D(der);
            if (nizq == null || nder == null)
            {
                return null;
            }
            Nodo nodo = new Nodo();
            nodo.tipo = (int)Simbolo.Tipo.BOOLEAN;
            nodo.etqVerdadera = getEtiqueta();
            nodo.etqFalsa = getEtiqueta();
            if (nizq.tipo == (int)Simbolo.Tipo.VACIO)
            {
                // NULL != DER
                if (nder.tipo == (int)Simbolo.Tipo.NUMERO || nder.tipo == (int)Simbolo.Tipo.CADENA
                    || nder.tipo == (int)Simbolo.Tipo.ESTRUCTURA)
                {
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Comparar si no es NULL"));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.CONDICIONAL, nodo.etqVerdadera, nizq.cadena, "!=", nder.cadena));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, nodo.etqFalsa));
                    return nodo;
                }
                else
                {
                    return null;
                }
            }
            if (nder.tipo == (int)Simbolo.Tipo.VACIO)
            {
                // IZQ != NULL
                if (nder.tipo == (int)Simbolo.Tipo.NUMERO || nder.tipo == (int)Simbolo.Tipo.CADENA
                    || nder.tipo == (int)Simbolo.Tipo.ESTRUCTURA)
                {
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Comparar si no es NULL"));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.CONDICIONAL, nodo.etqVerdadera, nizq.cadena, "!=", nder.cadena));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, nodo.etqFalsa));
                    return nodo;
                }
                else
                {
                    return null;
                }
            }
            switch (nizq.tipo)
            {
                case (int)Simbolo.Tipo.NUMERO:
                    if (nder.tipo == (int)Simbolo.Tipo.NUMERO)
                    {
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Comparar si los dos numeros son desiguales"));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.CONDICIONAL, nodo.etqVerdadera, nizq.cadena, "!=", nder.cadena));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, nodo.etqFalsa));
                        return nodo;
                    }
                    break;
                case (int)Simbolo.Tipo.BOOLEAN:
                    if (nizq.etqVerdadera != null && nizq.etqFalsa != null)
                    {
                        nizq = castearC3D((int)Simbolo.Tipo.NUMERO, nizq, izq.Span.Location.Line, izq.Span.Location.Column);
                    }
                    if (nder.tipo == (int)Simbolo.Tipo.BOOLEAN)
                    {
                        if (nder.etqVerdadera != null && nder.etqFalsa != null)
                        {
                            nder = castearC3D((int)Simbolo.Tipo.NUMERO, nder, der.Span.Location.Line, der.Span.Location.Column);
                        }
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Comparar si los dos bools son desiguales"));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.CONDICIONAL, nodo.etqVerdadera, nizq.cadena, "!=", nder.cadena));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, nodo.etqFalsa));
                        return nodo;
                    }
                    break;
                case (int)Simbolo.Tipo.CADENA:
                    if (nder.tipo == (int)Simbolo.Tipo.CADENA)
                    {

                        String temp1 = getTemporal();
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Guardar los punteros a Heap de las cadenas"));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.ACCESO, "Heap", temp1, nizq.cadena));
                        String temp2 = getTemporal();
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.ACCESO, "Heap", temp2, nder.cadena));
                        String taux = getTemporal();
                        String eInicio = getEtiqueta();
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Iniciar un contador en 0"));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, taux, "0", "", ""));
                        generarEtiquetas((eInicio));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Obtener posicion de la cadena 1"));
                        String tpos1 = getTemporal();
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, tpos1, temp1, "+", taux));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Obtener valor Pool[posicion] de cadena 1"));
                        String tval1 = getTemporal();
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.ACCESO, "Pool", tval1, tpos1));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Obtener posicion de la cadena 2"));
                        String tpos2 = getTemporal();
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, tpos2, temp2, "+", taux));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Obtener valor Pool[posicion] de cadena 2"));
                        String tval2 = getTemporal();
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.ACCESO, "Pool", tval2, tpos2));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.CONDICIONAL, nodo.etqVerdadera, tval1, "!=", tval2));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.CONDICIONAL, nodo.etqFalsa, tval1, "==", "0"));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, taux, taux, "+", "1"));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, eInicio));
                        return nodo;
                    }
                    break;
                case (int)Simbolo.Tipo.VACIO:
                    // Si es NULL
                    break;
            }
            Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                "No se puede comparar si son desiguales el " + getTipo(nizq.tipo) + " con " + getTipo(nder.tipo) + ".",
                izq.Span.Location.Line, izq.Span.Location.Column));
            return null;
        }

        public Nodo generarMayorC3D(ParseTreeNode izq, ParseTreeNode der)
        {
            Nodo nizq = expresionC3D(izq);
            Nodo nder = expresionC3D(der);
            if (nizq == null || nder == null)
            {
                return null;
            }
            if (nizq.tipo == (int)Simbolo.Tipo.VACIO)
            {
                //Error: NullPointerException
            }
            if (nder.tipo == (int)Simbolo.Tipo.VACIO)
            {
                //Error: NullPointerException
            }
            Nodo nodo = new Nodo();
            nodo.tipo = (int)Simbolo.Tipo.BOOLEAN;
            nodo.etqVerdadera = getEtiqueta();
            nodo.etqFalsa = getEtiqueta();
            switch (nizq.tipo)
            {
                case (int)Simbolo.Tipo.NUMERO:
                    if (nder.tipo == (int)Simbolo.Tipo.NUMERO)
                    {
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Comparar si num > num"));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.CONDICIONAL, nodo.etqVerdadera, nizq.cadena, ">", nder.cadena));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, nodo.etqFalsa));
                        return nodo;
                    }
                    break;
                case (int)Simbolo.Tipo.CADENA:
                    if (nder.tipo == (int)Simbolo.Tipo.CADENA)
                    {

                        String temp1 = getTemporal();
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Guardar los punteros a Heap de las cadenas"));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.ACCESO, "Heap", temp1, nizq.cadena));
                        String temp2 = getTemporal();
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.ACCESO, "Heap", temp2, nder.cadena));
                        String taux = getTemporal();
                        String eInicio = getEtiqueta();
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Iniciar un contador en 0"));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, taux, "0", "", ""));
                        generarEtiquetas((eInicio));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Obtener posicion de la cadena 1"));
                        String tpos1 = getTemporal();
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, tpos1, temp1, "+", taux));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Obtener valor Pool[posicion] de cadena 1"));
                        String tval1 = getTemporal();
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.ACCESO, "Pool", tval1, tpos1));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Obtener posicion de la cadena 2"));
                        String tpos2 = getTemporal();
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, tpos2, temp2, "+", taux));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Obtener valor Pool[posicion] de cadena 2"));
                        String tval2 = getTemporal();
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.ACCESO, "Pool", tval2, tpos2));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.CONDICIONAL, nodo.etqVerdadera, tval1, ">", tval2));
                        String e2 = getEtiqueta();
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.CONDICIONAL, e2, tval1, "==", tval2));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, nodo.etqFalsa));
                        generarEtiquetas((e2));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.CONDICIONAL, nodo.etqFalsa, tval1, "==", "0"));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, taux, taux, "+", "1"));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, eInicio));
                        return nodo;
                    }
                    break;
            }
            Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                "No se puede comparar si el " + getTipo(nizq.tipo) + " es mayor a " + getTipo(nder.tipo) + ".",
                izq.Span.Location.Line, izq.Span.Location.Column));
            return null;
        }

        public Nodo generarMenorC3D(ParseTreeNode izq, ParseTreeNode der)
        {
            Nodo nizq = expresionC3D(izq);
            Nodo nder = expresionC3D(der);
            if (nizq == null || nder == null)
            {
                return null;
            }
            if (nizq.tipo == (int)Simbolo.Tipo.VACIO)
            {
                //Error: NullPointerException
            }
            if (nder.tipo == (int)Simbolo.Tipo.VACIO)
            {
                //Error: NullPointerException
            }
            Nodo nodo = new Nodo();
            nodo.tipo = (int)Simbolo.Tipo.BOOLEAN;
            nodo.etqVerdadera = getEtiqueta();
            nodo.etqFalsa = getEtiqueta();
            switch (nizq.tipo)
            {
                case (int)Simbolo.Tipo.NUMERO:
                    if (nder.tipo == (int)Simbolo.Tipo.NUMERO)
                    {
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Comparar si num < num"));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.CONDICIONAL, nodo.etqVerdadera, nizq.cadena, "<", nder.cadena));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, nodo.etqFalsa));
                        return nodo;
                    }
                    break;
                case (int)Simbolo.Tipo.CADENA:
                    if (nder.tipo == (int)Simbolo.Tipo.CADENA)
                    {

                        String temp1 = getTemporal();
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Guardar los punteros a Heap de las cadenas"));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.ACCESO, "Heap", temp1, nizq.cadena));
                        String temp2 = getTemporal();
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.ACCESO, "Heap", temp2, nder.cadena));
                        String taux = getTemporal();
                        String eInicio = getEtiqueta();
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Iniciar un contador en 0"));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, taux, "0", "", ""));
                        generarEtiquetas((eInicio));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Obtener posicion de la cadena 1"));
                        String tpos1 = getTemporal();
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, tpos1, temp1, "+", taux));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Obtener valor Pool[posicion] de cadena 1"));
                        String tval1 = getTemporal();
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.ACCESO, "Pool", tval1, tpos1));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Obtener posicion de la cadena 2"));
                        String tpos2 = getTemporal();
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, tpos2, temp2, "+", taux));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Obtener valor Pool[posicion] de cadena 2"));
                        String tval2 = getTemporal();
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.ACCESO, "Pool", tval2, tpos2));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.CONDICIONAL, nodo.etqVerdadera, tval1, "<", tval2));
                        String e2 = getEtiqueta();
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.CONDICIONAL, e2, tval1, "==", tval2));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, nodo.etqFalsa));
                        generarEtiquetas((e2));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.CONDICIONAL, nodo.etqFalsa, tval1, "==", "0"));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, taux, taux, "+", "1"));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, eInicio));
                        return nodo;
                    }
                    break;
            }
            Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                "No se puede comparar si el " + getTipo(nizq.tipo) + " es menor a " + getTipo(nder.tipo) + ".",
                izq.Span.Location.Line, izq.Span.Location.Column));
            return null;
        }

        public Nodo generarMayorIgualC3D(ParseTreeNode izq, ParseTreeNode der)
        {
            Nodo nizq = expresionC3D(izq);
            Nodo nder = expresionC3D(der);
            if (nizq == null || nder == null)
            {
                return null;
            }
            if (nizq.tipo == (int)Simbolo.Tipo.VACIO)
            {
                //Error: NullPointerException
            }
            if (nder.tipo == (int)Simbolo.Tipo.VACIO)
            {
                //Error: NullPointerException
            }
            Nodo nodo = new Nodo();
            nodo.tipo = (int)Simbolo.Tipo.BOOLEAN;
            nodo.etqVerdadera = getEtiqueta();
            nodo.etqFalsa = getEtiqueta();
            switch (nizq.tipo)
            {
                case (int)Simbolo.Tipo.NUMERO:
                    if (nder.tipo == (int)Simbolo.Tipo.NUMERO)
                    {
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Comparar si num >= num"));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.CONDICIONAL, nodo.etqVerdadera, nizq.cadena, ">", nder.cadena));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, nodo.etqFalsa));
                        return nodo;
                    }
                    break;
            }
            Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                "No se puede comparar si el " + getTipo(nizq.tipo) + " es mayor o igual a " + getTipo(nder.tipo) + ".",
                izq.Span.Location.Line, izq.Span.Location.Column));
            return null;
        }

        public Nodo generarMenorIgualC3D(ParseTreeNode izq, ParseTreeNode der)
        {
            Nodo nizq = expresionC3D(izq);
            Nodo nder = expresionC3D(der);
            if (nizq == null || nder == null)
            {
                return null;
            }
            if (nizq.tipo == (int)Simbolo.Tipo.VACIO)
            {
                //Error: NullPointerException
            }
            if (nder.tipo == (int)Simbolo.Tipo.VACIO)
            {
                //Error: NullPointerException
            }
            Nodo nodo = new Nodo();
            nodo.tipo = (int)Simbolo.Tipo.BOOLEAN;
            nodo.etqVerdadera = getEtiqueta();
            nodo.etqFalsa = getEtiqueta();
            switch (nizq.tipo)
            {
                case (int)Simbolo.Tipo.NUMERO:
                    if (nder.tipo == (int)Simbolo.Tipo.NUMERO)
                    {
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Comparar si num <= num"));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.CONDICIONAL, nodo.etqVerdadera, nizq.cadena, "<=", nder.cadena));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, nodo.etqFalsa));
                        return nodo;
                    }
                    break;
            }
            Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                "No se puede comparar si el " + getTipo(nizq.tipo) + " es menor o igual a " + getTipo(nder.tipo) + ".",
                izq.Span.Location.Line, izq.Span.Location.Column));
            return null;
        }*/

    }
}
