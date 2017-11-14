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
        public static Nodo generarC3D(ParseTreeNode izq, String operador, ParseTreeNode der)
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

        public static Nodo generarIgualC3D(ParseTreeNode izq, ParseTreeNode der)
        {
            Nodo nizq = Expresion.expresionC3D(izq);
            Nodo nder = Expresion.expresionC3D(der);
            if (nizq == null || nder == null)
            {
                return null;
            }
            return generarRelacional("==", "iguales", nizq, nder, izq, der);
        }

        public static Nodo generarNoIgualC3D(ParseTreeNode izq, ParseTreeNode der)
        {
            Nodo nizq = Expresion.expresionC3D(izq);
            Nodo nder = Expresion.expresionC3D(der);
            if (nizq == null || nder == null)
            {
                return null;
            }
            return generarRelacional("!=", "diferentes", nizq, nder, izq, der);
        }

        public static Nodo generarMayorC3D(ParseTreeNode izq, ParseTreeNode der)
        {
            Nodo nizq = Expresion.expresionC3D(izq);
            Nodo nder = Expresion.expresionC3D(der);
            if (nizq == null || nder == null)
            {
                return null;
            }
            return generarRelacional(">", "mayor", nizq, nder, izq, der);
        }

        public static Nodo generarMenorC3D(ParseTreeNode izq, ParseTreeNode der)
        {
            Nodo nizq = Expresion.expresionC3D(izq);
            Nodo nder = Expresion.expresionC3D(der);
            if (nizq == null || nder == null)
            {
                return null;
            }
            return generarRelacional("<", "menor", nizq, nder, izq, der);
        }

        public static Nodo generarMayorIgualC3D(ParseTreeNode izq, ParseTreeNode der)
        {
            Nodo nizq = Expresion.expresionC3D(izq);
            Nodo nder = Expresion.expresionC3D(der);
            if (nizq == null || nder == null)
            {
                return null;
            }
            return generarRelacional(">=", "mayor o iguales", nizq, nder, izq, der);
        }

        public static Nodo generarMenorIgualC3D(ParseTreeNode izq, ParseTreeNode der)
        {
            Nodo nizq = Expresion.expresionC3D(izq);
            Nodo nder = Expresion.expresionC3D(der);
            if (nizq == null || nder == null)
            {
                return null;
            }
            return generarRelacional("<=", "menor o iguales", nizq, nder, izq, der);
        }

        public static Nodo generarRelacional(String ope, String texto, Nodo nizq, Nodo nder, ParseTreeNode izq, ParseTreeNode der)
        {
            Nodo nodo = new Nodo();
            nodo.tipo = (int)Simbolo.Tipo.BOOLEAN;
            nodo.etqVerdadera = GeneradorC3D.getEtiqueta();
            nodo.etqFalsa = GeneradorC3D.getEtiqueta();
            if (nizq.tipo == (int)Simbolo.Tipo.NUMERO ||
                nizq.tipo == (int)Simbolo.Tipo.DECIMAL)
            {
                if (nder.tipo == (int)Simbolo.Tipo.NUMERO ||
                    nder.tipo == (int)Simbolo.Tipo.DECIMAL ||
                nder.tipo == (int)Simbolo.Tipo.CARACTER)
                {
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Comparar si los dos valores son "+ope));
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.CONDICIONAL, nodo.etqVerdadera, nizq.cadena, ope, nder.cadena));
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, nodo.etqFalsa));
                    return nodo;
                }
            }
            else if (nizq.tipo == (int)Simbolo.Tipo.CARACTER)
            {
                if (nder.tipo == (int)Simbolo.Tipo.CADENA)
                {
                    nder = sumarCaracteres(nder.cadena);
                }
                if (nder.tipo == (int)Simbolo.Tipo.NUMERO ||
                    nder.tipo == (int)Simbolo.Tipo.DECIMAL ||
                    nder.tipo == (int)Simbolo.Tipo.CARACTER || 
                    nder.tipo == (int)Simbolo.Tipo.CADENA)
                {
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Comparar si los dos valores son "+ope));
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.CONDICIONAL, nodo.etqVerdadera, nizq.cadena, ope, nder.cadena));
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, nodo.etqFalsa));
                    return nodo;
                }
            }
            else if (nizq.tipo == (int)Simbolo.Tipo.CADENA)
            {
                nizq = sumarCaracteres(nizq.cadena);
                if (nder.tipo == (int)Simbolo.Tipo.CADENA)
                {
                    nder = sumarCaracteres(nder.cadena);
                }
                if (nder.tipo == (int)Simbolo.Tipo.CARACTER || nder.tipo == (int)Simbolo.Tipo.CADENA)
                {
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Comparar si los dos valores son " + ope));
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.CONDICIONAL, nodo.etqVerdadera, nizq.cadena, ope, nder.cadena));
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, nodo.etqFalsa));
                    return nodo;
                }
            }
            Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                "No se puede comparar si son "+texto+" el tipo " + Simbolo.getValor(nizq.tipo) + " con " + Simbolo.getValor(nder.tipo) + ".",
                izq.Span.Location.Line, izq.Span.Location.Column));
            return null;
        }

        public static Nodo sumarCaracteres(String tcadena)
        {
            Nodo nodo = new Nodo();
            // temporales y etiquetas
            nodo.tipo = (int)Simbolo.Tipo.NUMERO;
            nodo.cadena = GeneradorC3D.getTemporal();
            String t2 = GeneradorC3D.getTemporal();
            String eInicio = GeneradorC3D.getEtiqueta();
            String eFin = GeneradorC3D.getEtiqueta();

            // Codigo 3D
            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Variable de control de suma de caracteres"));
            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, nodo.cadena, "0", "", ""));
            GeneradorC3D.generarEtiquetas(eInicio);
            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Acceder a la posicion de la cadena en "+tcadena));
            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ACCESO, "Heap", t2, tcadena));
            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Diferente de fin de cadena"));
            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.CONDICIONAL, eFin, t2, "==", "0"));
            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Sumar el caracter con la var de control"));
            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, nodo.cadena, nodo.cadena, "+", t2));
            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Incrementar "+tcadena));
            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, tcadena, tcadena, "+", "1"));
            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, eInicio));
            GeneradorC3D.generarEtiquetas(eFin);

            return nodo;
        }
    }
}
