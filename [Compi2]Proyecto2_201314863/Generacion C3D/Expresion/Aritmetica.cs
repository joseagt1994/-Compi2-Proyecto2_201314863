using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace _Compi2_Proyecto2_201314863
{
    public class Aritmetica
    {/*
        public static Nodo generarC3D(ParseTreeNode izq, String operador, ParseTreeNode der)
        {
            switch (operador)
            {
                case "+":
                    return generarSumaC3D(izq, der);
                case "-":
                    return generarRestaC3D(izq, der);
                case "*":
                    return generarMultiplicarC3D(izq, der);
                case "/":
                    return generarDividirC3D(izq, der);
                default:
                    // '^' Potencia
                    return generarPotenciaC3D(izq, der);
            }
        }*/

     /*   public static Nodo generarSumaC3D(ParseTreeNode izq, ParseTreeNode der)
        {
            Nodo nodo = new Nodo();
            Nodo nizq = Expresion.expresionC3D(izq);
            Nodo nder = Expresion.expresionC3D(der);
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
            switch (nizq.tipo)
            {
                case (int)Simbolo.Tipo.NUMERO:
                    if (nder.tipo == (int)Simbolo.Tipo.NUMERO)
                    {
                        String temp = GeneradorC3D.getTemporal();
                        // temp = nizq.cad operador nder.der
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Sumar dos temporales, num + num"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, temp, nizq.cadena, "+", nder.cadena));
                        nodo.cadena = temp;
                        nodo.tipo = nder.tipo;
                    }
                    else if (nder.tipo == (int)Simbolo.Tipo.BOOLEAN)
                    {
                        if (nder.etqVerdadera != null && nder.etqFalsa != null)
                        {
                            nder = castearC3D((int)Simbolo.Tipo.NUMERO, nder, der.Span.Location.Line, der.Span.Location.Column);
                        }
                        String temp = GeneradorC3D.getTemporal();
                        // temp = nizq.cad operador nder.der
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Sumar , num + bool"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, temp, nizq.cadena, "+", nder.cadena));
                        nodo.cadena = temp;
                        nodo.tipo = nder.tipo;
                    }
                    else if (nder.tipo == (int)Simbolo.Tipo.CADENA)
                    {
                        nodo.cadena = GeneradorC3D.getTemporal();
                        nodo.tipo = nder.tipo;
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Concatenar , num + str"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, nodo.cadena, "H", "+", "0"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Reservar espacio para nueva cadena"));
                        GeneradorC3D.aumentarHeap("1");
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Heap", nodo.cadena, "S"));
                        concatenar(nizq.tipo, nizq.cadena);
                        concatenar(nder.tipo, nder.cadena);
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Guardar fin de cadena"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Pool", "S", "0"));
                        aumentarPool();
                    }
                    break;
                case (int)Simbolo.Tipo.BOOLEAN:
                    if (nder.tipo == (int)Simbolo.Tipo.NUMERO)
                    {
                        if (nizq.etqVerdadera != null && nizq.etqFalsa != null)
                        {
                            nizq = castearC3D((int)Simbolo.Tipo.NUMERO, nizq, izq.Span.Location.Line, izq.Span.Location.Column);
                        }
                        String temp = GeneradorC3D.getTemporal();
                        // temp = nizq.cad operador nder.der
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Sumar , bool + num"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, temp, nizq.cadena, "+", nder.cadena));
                        nodo.cadena = temp;
                        nodo.tipo = nder.tipo;
                    }
                    else if (nder.tipo == (int)Simbolo.Tipo.BOOLEAN)
                    {
                        if (nizq.etqVerdadera != null && nizq.etqFalsa != null)
                        {
                            nizq = castearC3D((int)Simbolo.Tipo.NUMERO, nizq, izq.Span.Location.Line, izq.Span.Location.Column);
                        }
                        if (nder.etqVerdadera != null && nder.etqFalsa != null)
                        {
                            nder = castearC3D((int)Simbolo.Tipo.NUMERO, nder, der.Span.Location.Line, der.Span.Location.Column);
                        }
                        String temp = GeneradorC3D.getTemporal();
                        // temp = nizq.cad operador nder.der
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Sumar , bool + bool"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, temp, nizq.cadena, "+", nder.cadena));
                        nodo.cadena = temp;
                        nodo.tipo = (int)Simbolo.Tipo.NUMERO;
                    }
                    else if (nder.tipo == (int)Simbolo.Tipo.CADENA)
                    {
                        if (nizq.etqVerdadera == null && nizq.etqFalsa == null)
                        {
                            nizq.tipo = (int)Simbolo.Tipo.NUMERO;
                            nizq = castearC3D((int)Simbolo.Tipo.BOOLEAN, nizq, izq.Span.Location.Line, izq.Span.Location.Column);
                        }
                        nodo.cadena = GeneradorC3D.getTemporal();
                        nodo.tipo = nder.tipo;
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Concatenar , bool + str"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, nodo.cadena, "H", "+", "0"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Reservar espacio para nueva cadena"));
                        GeneradorC3D.aumentarHeap("1");
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Heap", nodo.cadena, "S"));
                        // GENERAR CADENA DE BOOL!
                        String eSal = GeneradorC3D.getEtiqueta();
                        GeneradorC3D.generarEtiquetas((nizq.etqVerdadera));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Guardar palabra true"));
                        Nodo ncad = guardarCadenaC3D("true");
                        concatenar(ncad.tipo, ncad.cadena);
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, eSal));
                        GeneradorC3D.generarEtiquetas((nizq.etqFalsa));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Guardar palabra false"));
                        Nodo ncad2 = guardarCadenaC3D("false");
                        concatenar(ncad2.tipo, ncad2.cadena);
                        GeneradorC3D.generarEtiquetas((eSal));
                        concatenar(nder.tipo, nder.cadena);
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Guardar fin de cadena"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Pool", "S", "0"));
                        aumentarPool();
                    }
                    break;
                case (int)Simbolo.Tipo.CADENA:
                    if (nder.tipo == (int)Simbolo.Tipo.NUMERO)
                    {
                        nodo.cadena = GeneradorC3D.getTemporal();
                        nodo.tipo = nizq.tipo;
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Concatenar , str + num"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, nodo.cadena, "H", "+", "0"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Reservar espacio para nueva cadena"));
                        GeneradorC3D.aumentarHeap("1");
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Heap", nodo.cadena, "S"));
                        concatenar(nizq.tipo, nizq.cadena);
                        concatenar(nder.tipo, nder.cadena);
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Guardar fin de cadena"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Pool", "S", "0"));
                        aumentarPool();
                    }
                    else if (nder.tipo == (int)Simbolo.Tipo.BOOLEAN)
                    {
                        if (nder.etqVerdadera == null && nder.etqFalsa == null)
                        {
                            nder.tipo = (int)Simbolo.Tipo.NUMERO;
                            nder = castearC3D((int)Simbolo.Tipo.BOOLEAN, nder, der.Span.Location.Line, der.Span.Location.Column);
                        }
                        nodo.cadena = GeneradorC3D.getTemporal();
                        nodo.tipo = nizq.tipo;
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Concatenar , str + bool"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, nodo.cadena, "H", "+", "0"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Reservar espacio para nueva cadena"));
                        GeneradorC3D.aumentarHeap("1");
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Heap", nodo.cadena, "S"));
                        // GENERAR CADENA DE BOOL!
                        String eSal = GeneradorC3D.getEtiqueta();
                        String temporal = GeneradorC3D.getTemporal();
                        GeneradorC3D.generarEtiquetas((nder.etqVerdadera));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Guardar palabra true"));
                        Nodo ncad = guardarCadenaC3D("true");
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Guardar el puntero de true"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, temporal, ncad.cadena, "", ""));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, eSal));
                        GeneradorC3D.generarEtiquetas((nder.etqFalsa));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Guardar palabra false"));
                        Nodo ncad2 = guardarCadenaC3D("false");
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Guardar el puntero de false"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, temporal, ncad2.cadena, "", ""));
                        GeneradorC3D.generarEtiquetas((eSal));
                        concatenar(nizq.tipo, nizq.cadena);
                        concatenar(nodo.tipo, temporal);
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Guardar fin de cadena"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Pool", "S", "0"));
                        aumentarPool();
                    }
                    else if (nder.tipo == (int)Simbolo.Tipo.CADENA)
                    {
                        nodo.cadena = GeneradorC3D.getTemporal();
                        nodo.tipo = nizq.tipo;
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Concatenar , str + str"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, nodo.cadena, "H", "+", "0"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Reservar espacio para nueva cadena"));
                        GeneradorC3D.aumentarHeap("1");
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Heap", nodo.cadena, "S"));
                        concatenar(nizq.tipo, nizq.cadena);
                        concatenar(nder.tipo, nder.cadena);
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Guardar fin de cadena"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Pool", "S", "0"));
                        aumentarPool();
                    }
                    break;
            }
            return nodo;
        }

        public static Nodo generarRestaC3D(ParseTreeNode izq, ParseTreeNode der)
        {
            Nodo nodo = new Nodo();
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
            nodo.cadena = GeneradorC3D.getTemporal();
            nodo.tipo = (int)Simbolo.Tipo.NUMERO;
            switch (nizq.tipo)
            {
                case (int)Simbolo.Tipo.NUMERO:
                    if (nder.tipo == (int)Simbolo.Tipo.NUMERO)
                    {
                        // num - num
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Restar num - num"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, nodo.cadena, nizq.cadena, "-", nder.cadena));
                        return nodo;
                    }
                    else if (nder.tipo == (int)Simbolo.Tipo.BOOLEAN)
                    {
                        // num - bool
                        if (nder.etqVerdadera != null && nder.etqFalsa != null)
                        {
                            nder = castearC3D((int)Simbolo.Tipo.NUMERO, nder, der.Span.Location.Line, der.Span.Location.Column);
                        }
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Restar num - bool"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, nodo.cadena, nizq.cadena, "-", nder.cadena));
                        return nodo;
                    }
                    break;
                case (int)Simbolo.Tipo.BOOLEAN:
                    if (nder.tipo == (int)Simbolo.Tipo.NUMERO)
                    {
                        // bool - num
                        if (nizq.etqVerdadera != null && nizq.etqFalsa != null)
                        {
                            nizq = castearC3D((int)Simbolo.Tipo.NUMERO, nizq, izq.Span.Location.Line, izq.Span.Location.Column);
                        }
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Restar bool - num"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, nodo.cadena, nizq.cadena, "-", nder.cadena));
                        return nodo;
                    }
                    break;
            }
            //Error semantico! 
            Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                "No se puede restar " + getTipo(nizq.tipo) + " con " + getTipo(nder.tipo) + ".",
                izq.Span.Location.Line, izq.Span.Location.Column));
            return null;
        }

        public static Nodo generarMultiplicarC3D(ParseTreeNode izq, ParseTreeNode der)
        {
            Nodo nodo = new Nodo();
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
            nodo.cadena = GeneradorC3D.getTemporal();
            nodo.tipo = (int)Simbolo.Tipo.NUMERO;
            switch (nizq.tipo)
            {
                case (int)Simbolo.Tipo.NUMERO:
                    if (nder.tipo == (int)Simbolo.Tipo.NUMERO)
                    {
                        // num * num
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Multiplicar num * num"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, nodo.cadena, nizq.cadena, "*", nder.cadena));
                        return nodo;
                    }
                    else if (nder.tipo == (int)Simbolo.Tipo.BOOLEAN)
                    {
                        // num * bool
                        if (nder.etqVerdadera != null && nder.etqFalsa != null)
                        {
                            nder = castearC3D((int)Simbolo.Tipo.NUMERO, nder, der.Span.Location.Line, der.Span.Location.Column);
                        }
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Multiplicar num * bool"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, nodo.cadena, nizq.cadena, "*", nder.cadena));
                        return nodo;
                    }
                    break;
                case (int)Simbolo.Tipo.BOOLEAN:
                    if (nder.tipo == (int)Simbolo.Tipo.NUMERO)
                    {
                        // bool * num
                        if (nizq.etqVerdadera != null && nizq.etqFalsa != null)
                        {
                            nizq = castearC3D((int)Simbolo.Tipo.NUMERO, nizq, izq.Span.Location.Line, izq.Span.Location.Column);
                        }
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Multiplicar bool * num"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, nodo.cadena, nizq.cadena, "*", nder.cadena));
                        return nodo;
                    }
                    else if (nder.tipo == (int)Simbolo.Tipo.BOOLEAN)
                    {
                        if (nizq.etqVerdadera != null && nizq.etqFalsa != null)
                        {
                            nizq = castearC3D((int)Simbolo.Tipo.NUMERO, nizq, izq.Span.Location.Line, izq.Span.Location.Column);
                        }
                        if (nder.etqVerdadera != null && nder.etqFalsa != null)
                        {
                            nder = castearC3D((int)Simbolo.Tipo.NUMERO, nder, der.Span.Location.Line, der.Span.Location.Column);
                        }
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Multiplicar bool * bool"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, nodo.cadena, nizq.cadena, "*", nder.cadena));
                        return nodo;
                    }
                    break;
            }
            //Error semantico! 
            Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                "No se puede multiplicar " + getTipo(nizq.tipo) + " con " + getTipo(nder.tipo) + ".",
                izq.Span.Location.Line, izq.Span.Location.Column));
            return null;
        }

        public static Nodo generarDividirC3D(ParseTreeNode izq, ParseTreeNode der)
        {
            Nodo nodo = new Nodo();
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
            nodo.cadena = GeneradorC3D.getTemporal();
            nodo.tipo = (int)Simbolo.Tipo.NUMERO;
            switch (nizq.tipo)
            {
                case (int)Simbolo.Tipo.NUMERO:
                    if (nder.tipo == (int)Simbolo.Tipo.NUMERO)
                    {
                        // num - num
                        String eError = GeneradorC3D.getEtiqueta();
                        String eSalida = GeneradorC3D.getEtiqueta();
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Dividir num / num"));
                        // Verificar si num en nder no es 0
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.CONDICIONAL, eError, nder.cadena, "==", "0"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, nodo.cadena, nizq.cadena, "/", nder.cadena));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, eSalida));
                        GeneradorC3D.generarEtiquetas((eError));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Error: No se puede dividir entre 0"));
                        GeneradorC3D.generarEtiquetas((eSalida));
                        return nodo;
                    }
                    else if (nder.tipo == (int)Simbolo.Tipo.BOOLEAN)
                    {
                        // num - bool
                        if (nder.etqVerdadera != null && nder.etqFalsa != null)
                        {
                            nder = castearC3D((int)Simbolo.Tipo.NUMERO, nder, der.Span.Location.Line, der.Span.Location.Column);
                        }
                        String eError = GeneradorC3D.getEtiqueta();
                        String eSalida = GeneradorC3D.getEtiqueta();
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Dividir num / bool"));
                        // Verificar si num en nder no es 0
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.CONDICIONAL, eError, nder.cadena, "==", "0"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, nodo.cadena, nizq.cadena, "/", nder.cadena));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, eSalida));
                        GeneradorC3D.generarEtiquetas((eError));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Error: No se puede dividir entre 0"));
                        GeneradorC3D.generarEtiquetas((eSalida));
                        return nodo;
                    }
                    break;
                case (int)Simbolo.Tipo.BOOLEAN:
                    if (nder.tipo == (int)Simbolo.Tipo.NUMERO)
                    {
                        // bool - num
                        if (nizq.etqVerdadera != null && nizq.etqFalsa != null)
                        {
                            nizq = castearC3D((int)Simbolo.Tipo.NUMERO, nizq, izq.Span.Location.Line, izq.Span.Location.Column);
                        }
                        String eError = GeneradorC3D.getEtiqueta();
                        String eSalida = GeneradorC3D.getEtiqueta();
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Dividir bool / num"));
                        // Verificar si num en nder no es 0
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.CONDICIONAL, eError, nder.cadena, "==", "0"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, nodo.cadena, nizq.cadena, "/", nder.cadena));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, eSalida));
                        GeneradorC3D.generarEtiquetas((eError));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Error: No se puede dividir entre 0"));
                        GeneradorC3D.generarEtiquetas((eSalida));
                        return nodo;
                    }
                    break;
            }
            //Error semantico! 
            Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                "No se puede dividir " + getTipo(nizq.tipo) + " con " + getTipo(nder.tipo) + ".",
                izq.Span.Location.Line, izq.Span.Location.Column));
            return null;
        }
        
        public static Nodo generarPotenciaC3D(ParseTreeNode izq, ParseTreeNode der)
        {
            Nodo nodo = new Nodo();
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
            nodo.cadena = GeneradorC3D.getTemporal();
            nodo.tipo = (int)Simbolo.Tipo.NUMERO;
            switch (nizq.tipo)
            {
                case (int)Simbolo.Tipo.NUMERO:
                    if (nder.tipo == (int)Simbolo.Tipo.NUMERO)
                    {
                        // num ^ num
                        String eSalida = GeneradorC3D.getEtiqueta();
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Operar potencia num ^ num"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, nodo.cadena, nizq.cadena, "^", nder.cadena));
                        return nodo;
                    }
                    else if (nder.tipo == (int)Simbolo.Tipo.BOOLEAN)
                    {
                        // num ^ bool
                        if (nder.etqVerdadera != null && nder.etqFalsa != null)
                        {
                            nder = castearC3D((int)Simbolo.Tipo.NUMERO, nder, der.Span.Location.Line, der.Span.Location.Column);
                        }
                        String eSalida = GeneradorC3D.getEtiqueta();
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Operar potencia num ^ bool"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, nodo.cadena, nizq.cadena, "^", nder.cadena));
                        return nodo;
                    }
                    break;
                case (int)Simbolo.Tipo.BOOLEAN:
                    if (nder.tipo == (int)Simbolo.Tipo.NUMERO)
                    {
                        // bool ^ num
                        if (nizq.etqVerdadera != null && nizq.etqFalsa != null)
                        {
                            nizq = castearC3D((int)Simbolo.Tipo.NUMERO, nizq, izq.Span.Location.Line, izq.Span.Location.Column);
                        }
                        String eSalida = GeneradorC3D.getEtiqueta();
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Operar potencia bool ^ num"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, nodo.cadena, nizq.cadena, "^", nder.cadena));
                        return nodo;
                    }
                    break;
            }
            //Error semantico! 
            Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                "No se puede operar potencia de " + getTipo(nizq.tipo) + " con " + getTipo(nder.tipo) + ".",
                izq.Span.Location.Line, izq.Span.Location.Column));
            return null;
        }

        public static void concatenar(int tipo, String cadena)
        {
            switch (tipo)
            {
                case (int)Simbolo.Tipo.NUMERO:
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Pool", "S", "-241094.22"));
                    aumentarPool();
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Guardar numero"));
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Pool", "S", cadena));
                    aumentarPool();
                    break;
                case (int)Simbolo.Tipo.CADENA:
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "//Guardar cadena en " + cadena));
                    String taux = GeneradorC3D.getTemporal();
                    String eFin = GeneradorC3D.getEtiqueta();
                    String eInicio = GeneradorC3D.getEtiqueta();
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ACCESO, "Heap", taux, cadena));
                    GeneradorC3D.generarEtiquetas((eInicio));
                    String tval = GeneradorC3D.getTemporal();
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Guardar el valor de Pool en " + tval));
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ACCESO, "Pool", tval, taux));
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "//Si no es fin de cadena guarda el valor"));
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.CONDICIONAL, eFin, tval, "==", "0"));
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Pool", "S", tval));
                    aumentarPool();
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Aumentar el contador que lleva la posicion del Pool"));
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, taux, taux, "+", "1"));
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, eInicio));
                    GeneradorC3D.generarEtiquetas((eFin));
                    break;
            }
        }*/

    }
}
