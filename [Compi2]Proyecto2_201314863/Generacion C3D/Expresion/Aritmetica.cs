using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace _Compi2_Proyecto2_201314863
{
    public class Aritmetica
    {
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
        }

        public static Nodo generarSumaC3D(ParseTreeNode izq, ParseTreeNode der)
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
                    if (nder.tipo == (int)Simbolo.Tipo.NUMERO || 
                        nder.tipo == (int)Simbolo.Tipo.CARACTER ||
                        nder.tipo == (int)Simbolo.Tipo.BOOLEAN ||
                        nder.tipo == (int)Simbolo.Tipo.DECIMAL)
                    {
                        String temp = GeneradorC3D.getTemporal();
                        // temp = nizq.cad operador nder.der
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Sumar dos temporales, num + num"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, temp, nizq.cadena, "+", nder.cadena));
                        nodo.cadena = temp;
                        nodo.tipo = nizq.tipo;
                        if (nder.tipo == (int)Simbolo.Tipo.DECIMAL)
                        {
                            nodo.tipo = nder.tipo;
                        }
                        return nodo;
                    }
                    else if (nder.tipo == (int)Simbolo.Tipo.CADENA)
                    {
                        nodo.cadena = GeneradorC3D.getTemporal();
                        nodo.tipo = nder.tipo;
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Concatenar , num + str"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, nodo.cadena, "H", "+", "0"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Reservar espacio para nueva cadena"));
                        concatenar(nizq.tipo, nizq.cadena);
                        concatenar(nder.tipo, nder.cadena);
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Guardar fin de cadena"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Heap", "H", "0"));
                        GeneradorC3D.aumentarHeap("1");
                        return nodo;
                    }
                    break;
                case (int)Simbolo.Tipo.DECIMAL:
                    if (nder.tipo == (int)Simbolo.Tipo.NUMERO ||
                        nder.tipo == (int)Simbolo.Tipo.CARACTER ||
                        nder.tipo == (int)Simbolo.Tipo.BOOLEAN ||
                        nder.tipo == (int)Simbolo.Tipo.DECIMAL)
                    {
                        String temp = GeneradorC3D.getTemporal();
                        // temp = nizq.cad operador nder.der
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Sumar dos temporales, decimal + num"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, temp, nizq.cadena, "+", nder.cadena));
                        nodo.cadena = temp;
                        nodo.tipo = nizq.tipo;
                        return nodo;
                    }
                    else if (nder.tipo == (int)Simbolo.Tipo.CADENA)
                    {
                        nodo.cadena = GeneradorC3D.getTemporal();
                        nodo.tipo = nder.tipo;
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Concatenar , decimal + str"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, nodo.cadena, "H", "+", "0"));
                        concatenar(nizq.tipo, nizq.cadena);
                        concatenar(nder.tipo, nder.cadena);
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Guardar fin de cadena"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Heap", "H", "0"));
                        GeneradorC3D.aumentarHeap("1");
                        return nodo;
                    }
                    break;
                case (int)Simbolo.Tipo.CARACTER:
                    if (nder.tipo == (int)Simbolo.Tipo.NUMERO ||
                        nder.tipo == (int)Simbolo.Tipo.DECIMAL)
                    {
                        String temp = GeneradorC3D.getTemporal();
                        // temp = nizq.cad operador nder.der
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Sumar dos temporales, caracter + num"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, temp, nizq.cadena, "+", nder.cadena));
                        nodo.cadena = temp;
                        nodo.tipo = nder.tipo;
                        return nodo;
                    }
                    else if (nder.tipo == (int)Simbolo.Tipo.CADENA)
                    {
                        nodo.cadena = GeneradorC3D.getTemporal();
                        nodo.tipo = nder.tipo;
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Concatenar , caracter + str"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, nodo.cadena, "H", "+", "0"));
                        concatenar(nizq.tipo, nizq.cadena);
                        concatenar(nder.tipo, nder.cadena);
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Guardar fin de cadena"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Heap", "H", "0"));
                        GeneradorC3D.aumentarHeap("1");
                        return nodo;
                    }
                    break;
                case (int)Simbolo.Tipo.BOOLEAN:
                    if (nder.tipo == (int)Simbolo.Tipo.NUMERO || nder.tipo == (int)Simbolo.Tipo.DECIMAL)
                    {
                        String temp = GeneradorC3D.getTemporal();
                        if(nder.tipo == (int)Simbolo.Tipo.NUMERO)
                        {
                            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Sumar , bool + num"));
                        }
                        else
                        {
                            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Sumar , bool + decimal"));
                        }
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, temp, nizq.cadena, "+", nder.cadena));
                        nodo.cadena = temp;
                        nodo.tipo = nder.tipo;
                        return nodo;
                    }
                    else if (nder.tipo == (int)Simbolo.Tipo.BOOLEAN)
                    {
                        return Logica.generarC3D(izq, "||", der);
                    }
                    break;
                case (int)Simbolo.Tipo.CADENA:
                    if (nder.tipo == (int)Simbolo.Tipo.NUMERO ||
                        nder.tipo == (int)Simbolo.Tipo.DECIMAL ||
                        nder.tipo == (int)Simbolo.Tipo.CARACTER)
                    {
                        nodo.cadena = GeneradorC3D.getTemporal();
                        nodo.tipo = nizq.tipo;
                        if (nder.tipo == (int)Simbolo.Tipo.NUMERO)
                        {
                            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Concatenar , str + num"));
                        }
                        else if (nder.tipo == (int)Simbolo.Tipo.DECIMAL)
                        {
                            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Concatenar , str + decimal"));
                        }
                        else
                        {
                            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Concatenar , str + caracter"));
                        }
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, nodo.cadena, "H", "+", "0"));
                        concatenar(nizq.tipo, nizq.cadena);
                        concatenar(nder.tipo, nder.cadena);
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Guardar fin de cadena"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Heap", "H", "0"));
                        GeneradorC3D.aumentarHeap("1");
                        return nodo;
                    }
                    else if (nder.tipo == (int)Simbolo.Tipo.CADENA)
                    {
                        nodo.cadena = GeneradorC3D.getTemporal();
                        nodo.tipo = nizq.tipo;
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Concatenar , str + str"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, nodo.cadena, "H", "+", "0"));
                        concatenar(nizq.tipo, nizq.cadena);
                        concatenar(nder.tipo, nder.cadena);
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Guardar fin de cadena"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Heap", "H", "0"));
                        GeneradorC3D.aumentarHeap("1");
                        return nodo;
                    }
                    break;
            }
            // Error semantico!
            Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                 "No se puede sumar " + Simbolo.getValor(nizq.tipo) + " con " + Simbolo.getValor(nder.tipo) + ".",
                 izq.Span.Location.Line, izq.Span.Location.Column));
            return null;
        }

        internal static Nodo unarioC3D(ParseTreeNode n2)
        {
            Nodo nodo = Expresion.expresionC3D(n2);
            if(nodo.tipo == (int)Simbolo.Tipo.NUMERO || nodo.tipo == (int)Simbolo.Tipo.DECIMAL)
            {
                String temp = GeneradorC3D.getTemporal();
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Multiplicar -1 * "+Simbolo.getValor(nodo.tipo)));
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, temp, "-1", "*", nodo.cadena));
                nodo.cadena = temp;
            }
            else
            {
                Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                 "No se puede cambiar signo a " + Simbolo.getValor(nodo.tipo)+".",
                 n2.Span.Location.Line, n2.Span.Location.Column));
            }
            return nodo;
        }

        public static Nodo generarRestaC3D(ParseTreeNode izq, ParseTreeNode der)
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
            nodo.cadena = GeneradorC3D.getTemporal();
            nodo.tipo = (int)Simbolo.Tipo.NUMERO;
            switch (nizq.tipo)
            {
                case (int)Simbolo.Tipo.NUMERO:
                    if (nder.tipo == (int)Simbolo.Tipo.NUMERO ||
                        nder.tipo == (int)Simbolo.Tipo.CARACTER ||
                        nder.tipo == (int)Simbolo.Tipo.BOOLEAN ||
                        nder.tipo == (int)Simbolo.Tipo.DECIMAL)
                    {
                        String temp = GeneradorC3D.getTemporal();
                        // temp = nizq.cad operador nder.der
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Restar dos temporales, num + num"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, temp, nizq.cadena, "-", nder.cadena));
                        nodo.cadena = temp;
                        nodo.tipo = nizq.tipo;
                        if (nder.tipo == (int)Simbolo.Tipo.DECIMAL)
                        {
                            nodo.tipo = nder.tipo;
                        }
                        return nodo;
                    }
                    break;
                case (int)Simbolo.Tipo.DECIMAL:
                    if (nder.tipo == (int)Simbolo.Tipo.NUMERO ||
                        nder.tipo == (int)Simbolo.Tipo.CARACTER ||
                        nder.tipo == (int)Simbolo.Tipo.BOOLEAN ||
                        nder.tipo == (int)Simbolo.Tipo.DECIMAL)
                    {
                        String temp = GeneradorC3D.getTemporal();
                        // temp = nizq.cad operador nder.der
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Restar dos temporales, decimal - num"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, temp, nizq.cadena, "-", nder.cadena));
                        nodo.cadena = temp;
                        nodo.tipo = nizq.tipo;
                        return nodo;
                    }
                    break;
                case (int)Simbolo.Tipo.BOOLEAN:
                    // bool - num
                    if (nder.tipo == (int)Simbolo.Tipo.NUMERO ||
                        nder.tipo == (int)Simbolo.Tipo.DECIMAL)
                    {
                        String temp = GeneradorC3D.getTemporal();
                        // temp = nizq.cad operador nder.der
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Restar dos temporales, bool - num"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, temp, nizq.cadena, "-", nder.cadena));
                        nodo.cadena = temp;
                        nodo.tipo = nizq.tipo;
                        if (nder.tipo == (int)Simbolo.Tipo.DECIMAL)
                        {
                            nodo.tipo = nder.tipo;
                        }
                        return nodo;
                    }
                    break;
                case (int)Simbolo.Tipo.CARACTER:
                    // bool - num
                    if (nder.tipo == (int)Simbolo.Tipo.NUMERO ||
                        nder.tipo == (int)Simbolo.Tipo.DECIMAL)
                    {
                        String temp = GeneradorC3D.getTemporal();
                        // temp = nizq.cad operador nder.der
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Restar dos temporales, caracter - num"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, temp, nizq.cadena, "-", nder.cadena));
                        nodo.cadena = temp;
                        nodo.tipo = nizq.tipo;
                        if (nder.tipo == (int)Simbolo.Tipo.DECIMAL)
                        {
                            nodo.tipo = nder.tipo;
                        }
                        return nodo;
                    }
                    break;
            }
            //Error semantico! 
            Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                 "No se puede restar " + Simbolo.getValor(nizq.tipo) + " con " + Simbolo.getValor(nder.tipo) + ".",
                 izq.Span.Location.Line, izq.Span.Location.Column));
            return null;
        }

        public static Nodo generarMultiplicarC3D(ParseTreeNode izq, ParseTreeNode der)
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
            nodo.cadena = GeneradorC3D.getTemporal();
            nodo.tipo = (int)Simbolo.Tipo.NUMERO;
            switch (nizq.tipo)
            {
                case (int)Simbolo.Tipo.NUMERO:
                    if (nder.tipo == (int)Simbolo.Tipo.NUMERO ||
                        nder.tipo == (int)Simbolo.Tipo.CARACTER ||
                        nder.tipo == (int)Simbolo.Tipo.BOOLEAN ||
                        nder.tipo == (int)Simbolo.Tipo.DECIMAL)
                    {
                        String temp = GeneradorC3D.getTemporal();
                        // temp = nizq.cad operador nder.der
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Multiplicar dos temporales, num * num"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, temp, nizq.cadena, "*", nder.cadena));
                        nodo.cadena = temp;
                        nodo.tipo = nizq.tipo;
                        if (nder.tipo == (int)Simbolo.Tipo.DECIMAL)
                        {
                            nodo.tipo = nder.tipo;
                        }
                        return nodo;
                    }
                    break;
                case (int)Simbolo.Tipo.DECIMAL:
                    if (nder.tipo == (int)Simbolo.Tipo.NUMERO ||
                        nder.tipo == (int)Simbolo.Tipo.CARACTER ||
                        nder.tipo == (int)Simbolo.Tipo.BOOLEAN ||
                        nder.tipo == (int)Simbolo.Tipo.DECIMAL)
                    {
                        String temp = GeneradorC3D.getTemporal();
                        // temp = nizq.cad operador nder.der
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Multiplicar dos temporales, decimal * num"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, temp, nizq.cadena, "*", nder.cadena));
                        nodo.cadena = temp;
                        nodo.tipo = nizq.tipo;
                        return nodo;
                    }
                    break;
                case (int)Simbolo.Tipo.BOOLEAN:
                    // bool - num
                    if (nder.tipo == (int)Simbolo.Tipo.NUMERO ||
                        nder.tipo == (int)Simbolo.Tipo.DECIMAL)
                    {
                        String temp = GeneradorC3D.getTemporal();
                        // temp = nizq.cad operador nder.der
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Multiplicar dos temporales, bool * num"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, temp, nizq.cadena, "*", nder.cadena));
                        nodo.cadena = temp;
                        nodo.tipo = nizq.tipo;
                        if (nder.tipo == (int)Simbolo.Tipo.DECIMAL)
                        {
                            nodo.tipo = nder.tipo;
                        }
                        return nodo;
                    }
                    else if(nder.tipo == (int)Simbolo.Tipo.BOOLEAN)
                    {
                        return Logica.generarC3D(izq, "&&", der);
                    }
                    break;
                case (int)Simbolo.Tipo.CARACTER:
                    // bool - num
                    if (nder.tipo == (int)Simbolo.Tipo.NUMERO ||
                        nder.tipo == (int)Simbolo.Tipo.DECIMAL)
                    {
                        String temp = GeneradorC3D.getTemporal();
                        // temp = nizq.cad operador nder.der
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Restar dos temporales, caracter * num"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, temp, nizq.cadena, "*", nder.cadena));
                        nodo.cadena = temp;
                        nodo.tipo = nizq.tipo;
                        if (nder.tipo == (int)Simbolo.Tipo.DECIMAL)
                        {
                            nodo.tipo = nder.tipo;
                        }
                        return nodo;
                    }
                    break;
            }
            //Error semantico! 
            Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                 "No se puede multiplicar " + Simbolo.getValor(nizq.tipo) + " con " + Simbolo.getValor(nder.tipo) + ".",
                 izq.Span.Location.Line, izq.Span.Location.Column));
            return null;
        }

        public static Nodo generarDividirC3D(ParseTreeNode izq, ParseTreeNode der)
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
            nodo.cadena = GeneradorC3D.getTemporal();
            nodo.tipo = (int)Simbolo.Tipo.DECIMAL;
            if (nizq.tipo == (int)Simbolo.Tipo.NUMERO ||
                nizq.tipo == (int)Simbolo.Tipo.CARACTER ||
                nizq.tipo == (int)Simbolo.Tipo.BOOLEAN ||
                nizq.tipo == (int)Simbolo.Tipo.DECIMAL)
            {
                if (nder.tipo == (int)Simbolo.Tipo.NUMERO ||
                nder.tipo == (int)Simbolo.Tipo.CARACTER ||
                nder.tipo == (int)Simbolo.Tipo.BOOLEAN ||
                nder.tipo == (int)Simbolo.Tipo.DECIMAL)
                {
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
            }
            //Error semantico! 
            Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                "No se puede dividir " + Simbolo.getValor(nizq.tipo) + " con " + Simbolo.getValor(nder.tipo) + ".",
                izq.Span.Location.Line, izq.Span.Location.Column));
            return null;
        }
        
        public static Nodo generarPotenciaC3D(ParseTreeNode izq, ParseTreeNode der)
        {
            /*Nodo nodo = new Nodo();
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
            */return null;
        }

        public static void concatenar(int tipo, String cadena)
        {
            switch (tipo)
            {
                case (int)Simbolo.Tipo.NUMERO:
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Heap", "H", "-241094.22"));
                    GeneradorC3D.aumentarHeap("1");
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Guardar numero"));
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Heap", "H", cadena));
                    GeneradorC3D.aumentarHeap("1");
                    break;
                case (int)Simbolo.Tipo.DECIMAL:
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Heap", "H", "-241094.22"));
                    GeneradorC3D.aumentarHeap("1");
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Guardar decimal"));
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Heap", "H", cadena));
                    GeneradorC3D.aumentarHeap("1");
                    break;
                case (int)Simbolo.Tipo.CARACTER:
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Guardar caracter"));
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Heap", "H", cadena));
                    GeneradorC3D.aumentarHeap("1");
                    break;
                case (int)Simbolo.Tipo.CADENA:
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "//Guardar cadena de " + cadena));
                    String taux = GeneradorC3D.getTemporal();
                    String eFin = GeneradorC3D.getEtiqueta();
                    String eInicio = GeneradorC3D.getEtiqueta();
                    GeneradorC3D.generarEtiquetas((eInicio));
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ACCESO, "Heap", taux, cadena));
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "//Si no es fin de cadena guarda el valor"));
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.CONDICIONAL, eFin, taux, "==", "0"));
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Heap", "H", taux));
                    GeneradorC3D.aumentarHeap("1");
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Aumentar el contador que lleva la posicion del Heap"));
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, cadena, cadena, "+", "1"));
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL, eInicio));
                    GeneradorC3D.generarEtiquetas((eFin));
                    break;
            }
        }

        public static Nodo generarCrecimientoC3D(ParseTreeNode crecer)
        {
            // Es incremento o decremento y tiene que ser de tipo entero
            // CRECER -> EXP (++ | --)
            Nodo nodo = Expresion.expresionC3D(crecer.ChildNodes[0]);
            if(nodo == null)
            {
                return null;
            }
            ParseTreeNode varC = crecer.ChildNodes[1];
            String ope;
            if (varC.Term.Name.Equals("++"))
            {
                ope = "+";
            }
            else
            {
                ope = "-";
            }
            if(nodo.tipo == (int)Simbolo.Tipo.NUMERO || nodo.tipo == (int)Simbolo.Tipo.DECIMAL ||
                nodo.tipo == (int)Simbolo.Tipo.CARACTER)
            {
                String temp = GeneradorC3D.getTemporal();
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// expresion "+ope+" expresion"));
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, temp, nodo.cadena, ope, "1"));
                if (!nodo.referencia.Equals(""))
                {
                    // Guardar en la variable
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "//guardar valor en id"));
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, nodo.estructura, nodo.referencia, temp));
                }
                nodo.cadena = temp;
                nodo.tipo = (int)Simbolo.Tipo.DECIMAL;
            }
            else
            {
                Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                "No se puede incrementar o decrementar el tipo " + Simbolo.getValor(nodo.tipo) + ".",
                crecer.Token.Location.Line, crecer.Token.Location.Column));
            }
            return nodo;
        }
        
    }
}
