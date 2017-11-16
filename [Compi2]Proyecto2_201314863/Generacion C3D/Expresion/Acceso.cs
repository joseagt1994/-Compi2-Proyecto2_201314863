using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace _Compi2_Proyecto2_201314863
{
    public class Acceso
    {
        public Clase clase = null;
        public static Simbolo actual = null; // VARIABLE QUE PUEDE SER ARREGLO, VAR, FUNCION
        public enum Tipo : int
        {
            NINGUNO, ESTE, SUPER
        }
        public static Nodo generarC3DAcceso(ParseTreeNode acceso, Tipo tipo, Nodo nExp)
        {
            actual = null;
            Nodo nodo = null;
            String puntero = "P";
            String estructura = "Stack";
            foreach(ParseTreeNode objeto in acceso.ChildNodes)
            {
                if (objeto.Term.Name.Equals("id"))
                {
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                        "// Accediendo a un id "+objeto.Token.Text));
                    nodo = generarC3DID(objeto.Token.Text, tipo, puntero, estructura);
                }
                else if (objeto.Term.Name.Equals("NARREGLO"))
                {
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                        "// Accediendo a un arreglo"));
                    nodo = Arreglo.generarAsignacionC3D(objeto.ChildNodes[0].Token.Text,
                        objeto.ChildNodes[1], tipo);
                }
                else
                {
                    // llamada con retorno
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                        "// Accediendo a una llamada"));
                    nodo = Llamada.llamadaC3D(objeto, tipo);
                }
                tipo = Tipo.ESTE;
                if(nodo != null)
                {
                    estructura = nodo.estructura;
                    puntero = nodo.cadena;
                }
                    
            }
            if(nExp != null)
            {
                // Asignar el valor a la variable!
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                    "// Asignar el valor a la variable del acceso"));
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR,
                            nodo.estructura, nodo.referencia, nExp.cadena));
            }
            return nodo;
        }

        public static Nodo generarC3DID(String id, Tipo tipo, String puntero, String estructura)
        {
            // id
            Simbolo sid;
            Nodo nodo = new Nodo();
            nodo.estructura = estructura;

            if (C3DSentencias.procedimientoActual == null || actual != null)
            {
                if(actual != null)
                {
                    if(actual.tipo != (int)Simbolo.Tipo.CLASE)
                    {
                        // Error! No es una clase, no se puede acceder!
                        return null;
                    }
                    estructura = "Heap";
                    sid = TablaSimbolos.getInstance.buscarVariable(id, actual.clase,
                    null, tipo);
                }
                else
                {
                    sid = TablaSimbolos.getInstance.buscarVariable(id, C3DSentencias.claseActual.nombre,
                    null, tipo);
                }
            }
            else
            {
                sid = TablaSimbolos.getInstance.buscarVariable(id, C3DSentencias.claseActual.nombre,
                    C3DSentencias.procedimientoActual.nombre, tipo);
            }
            if(sid != null)
            {
                String pos = "1";
                if(sid.ambito == (int)Simbolo.Tipo.GLOBAL)
                {
                    // Global
                    if((actual == null && C3DSentencias.claseActual.nombre.Equals(sid.padre)) 
                        || (actual != null && sid.padre.Equals(actual.clase)))
                    {
                        pos = "0";
                    }
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                        "// El " + id + "es global!"));
                    String t1 = GeneradorC3D.getTemporal();
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                        "// Obtener posicion de este o super"));
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION,
                        t1, puntero, "+", pos));
                    String t2 = t1;
                    if (estructura.Equals("Stack"))
                    {
                        t2 = GeneradorC3D.getTemporal();
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                        "// Obtener posicion del ambito self o super en el Heap"));
                        GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ACCESO,
                            estructura, t2, t1));
                    }
                    estructura = "Heap";
                    String t9 = GeneradorC3D.getTemporal();
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                        "// Obtener posicion del ambito self o super"));
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ACCESO,
                        estructura, t9, t2));
                    puntero = t9;
                }
                // Generar C3D para buscar la posicion de ID y luego el valor
                String t3 = GeneradorC3D.getTemporal();
                String t4 = GeneradorC3D.getTemporal();
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                    "// Obtener posicion del "+id));
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION,
                                t3, puntero, "+", Convert.ToString(sid.pos)));
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                    "// Obtener valor del " + id));
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ACCESO,
                    estructura, t4, t3));
                actual = sid;
                nodo.tipo = sid.tipo;
                nodo.referencia = t3;
                nodo.cadena = t4;
                nodo.estructura = estructura;
                return nodo;
            }
            else
            {
                // Error: La variable id no existe!
                return null;
            }
        }
    }
}
