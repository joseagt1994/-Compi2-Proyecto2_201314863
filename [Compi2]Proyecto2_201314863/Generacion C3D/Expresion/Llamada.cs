using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace _Compi2_Proyecto2_201314863
{
    public class Llamada
    {
        public static Nodo llamadaC3D(ParseTreeNode llamada, Acceso.Tipo tipo)
        {
            // LLAMADA.Rule = id + apar + EXPS + CPAR
            Nodo nodo = new Nodo();
            String id = llamada.ChildNodes[0].Token.Text;
            // Evaluar las expresiones
            List<Nodo> expresiones = new List<Nodo>();
            String llave = getExpresiones(llamada.ChildNodes[1], ref expresiones);
            if(llave != null)
            {
                llave = id + llave;
                Simbolo procedimiento;
                // Buscar en la clase que se necesita
                List<Simbolo> sparametros;
                if(tipo == Acceso.Tipo.SUPER)
                {
                    // Buscar en el padre
                    if(C3DSentencias.claseActual.padre == null)
                    {
                        return null;
                    }
                    procedimiento = TablaSimbolos.getInstance.getProcedimiento(
                    C3DSentencias.claseActual.padre, llave);
                    if(procedimiento == null)
                    {
                        // Error! No existe el procedimiento en la clase padre
                        return null;
                    }
                    else
                    {
                        if(procedimiento.visibilidad == (int)Simbolo.Visibilidad.PRIVADO)
                        {
                            // Error! No se puede acceder a un procedimiento privado!
                            return null;
                        }
                    }
                    sparametros = TablaSimbolos.getInstance.getParametros(
                        C3DSentencias.claseActual.padre, procedimiento);
                }
                else
                {
                    // Buscar en la clase actual
                    procedimiento = TablaSimbolos.getInstance.getProcedimiento(
                    C3DSentencias.claseActual.nombre, llave);
                    if (procedimiento == null)
                    {
                        // Error! No existe el procedimiento en la clase
                    }
                    sparametros = TablaSimbolos.getInstance.getParametros(
                        C3DSentencias.claseActual.nombre, procedimiento);
                }
                if(expresiones.Count == sparametros.Count)
                {
                    int tam = GeneradorC3D.tamMain;
                    if(C3DSentencias.procedimientoActual != null)
                    {
                        // Esta en el main!
                        tam = C3DSentencias.procedimientoActual.tam;
                    }
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                        "// Pasar los parametros"));
                    String taux = GeneradorC3D.getTemporal();
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                            "// Crear un temporal auxiliar para el incremento del ambito"));
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, taux,
                        "P", "+", Convert.ToString(tam)));
                    parametrosC3D(taux, sparametros, expresiones);
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                        "// Incrementar el ambito"));
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, "P",
                        "P", "+", Convert.ToString(tam)));
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                        "// Llamar al procedimiento " + llave));
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.LLAMADA, procedimiento.padre+"_"+llave));
                    // Ver si hay retorno Y guardarlo
                    if (procedimiento.rol == (int)Simbolo.Tipo.FUNCION)
                    {
                        String temp = GeneradorC3D.getTemporal();
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                            "// Obtener posicion del retorno en el ambito actual "));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, temp, "P",
                            "+", "2"));
                        nodo.cadena = GeneradorC3D.getTemporal();
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                            "// Obtener valor del retorno de la funcion " + llave));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ACCESO, "Stack", 
                            nodo.cadena, temp));
                        nodo.tipo = procedimiento.tipo;
                    }
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                            "// Disminuir el ambito "));
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, "P",
                        "P", "-", Convert.ToString(tam)));
                }
                else
                {
                    // Error! Faltan parametros!
                    return null;
                }
            }
            return nodo;
        }

        public static Nodo instanciaC3D(ParseTreeNode nuevo, Acceso.Tipo tipo)
        {
            // NUEVO -> new id EXPS
            // SUPER -> super [ EXPS ]
            Nodo nodo = new Nodo();
            // NUEVO
                String idC = nuevo.ChildNodes[1].Token.Text;
                // Buscar que la clase exista
                Clase instancia = GeneradorC3D.getClasePadre(idC);
                if (instancia != null)
                {
                List<Nodo> expresiones = new List<Nodo>();
                String llave = getExpresiones(nuevo.ChildNodes[2], ref expresiones);
                if (llave != null)
                {
                    llave = "constructor" + llave;
                    Simbolo procedimiento = TablaSimbolos.getInstance.getProcedimiento(
                    instancia.nombre, llave);
                    if (procedimiento == null || procedimiento.visibilidad == (int)Simbolo.Visibilidad.PRIVADO)
                    {
                        // Error Semantico! No se puede hacer instancia por ser privado
                        return null;
                    }
                    // Get parametros
                    List<Simbolo> sparametros = TablaSimbolos.getInstance.getParametros(
                    instancia.nombre, procedimiento);
                    if (expresiones.Count == sparametros.Count)
                    {
                        int tam = GeneradorC3D.tamMain;
                        if (C3DSentencias.procedimientoActual != null)
                        {
                            // Esta en el main!
                            tam = C3DSentencias.procedimientoActual.tam;
                        }
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                            "// Pasar los parametros"));
                        String taux = GeneradorC3D.getTemporal();
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                                "// Crear un temporal auxiliar para el incremento del ambito"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, taux,
                            "P", "+", Convert.ToString(tam)));
                        parametrosC3D(taux, sparametros, expresiones);
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                            "// Incrementar el ambito"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, "P",
                            "P", "+", Convert.ToString(tam)));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                            "// Llamar al procedimiento " + llave));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.LLAMADA, instancia.nombre+"_"+llave));
                        // Ver si hay retorno Y guardarlo
                        String temp = GeneradorC3D.getTemporal();
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                            "// Obtener posicion del retorno en el ambito actual "));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, temp, "P",
                            "+", "2"));
                        nodo.cadena = GeneradorC3D.getTemporal();
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                            "// Obtener valor del retorno de la funcion " + llave));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ACCESO, "Stack",
                            nodo.cadena , temp));
                        nodo.tipo = (int)Simbolo.Tipo.CLASE;
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                                "// Disminuir el ambito "));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, "P",
                            "P", "-", Convert.ToString(tam)));
                    }
                }
            }
            
            return nodo;
        }

        private static void parametrosC3D(String temp, List<Simbolo> parametros, List<Nodo> expresiones)
        {
            for(int i = 0; i < parametros.Count; i++)
            {
                // Generar C3D de parametro
                String t1 = GeneradorC3D.getTemporal();
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                            "// Obtener posicion del parametro"));
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, t1,
                    temp, "+", Convert.ToString(parametros[i].pos)));
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                            "// Guardar expresion como parametro en la posicion " + i));
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Stack", t1, 
                    expresiones[i].cadena));
            }
        }

        private static String getExpresiones(ParseTreeNode expresiones, ref List<Nodo> exps)
        {
            String cadena = "";
            foreach(ParseTreeNode nexp in expresiones.ChildNodes)
            {
                Nodo exp = Expresion.expresionC3D(nexp);
                if(exp == null)
                {
                    return null;
                }
                exps.Add(exp);
                cadena += "_" + Simbolo.getValor(exp.tipo);
            }
            return cadena;
        }
    }
}
