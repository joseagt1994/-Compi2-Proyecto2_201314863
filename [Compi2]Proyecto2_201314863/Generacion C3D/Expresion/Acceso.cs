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
        private static Simbolo actual = null; // VARIABLE QUE PUEDE SER ARREGLO, VAR, FUNCION
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
                    nodo = generarC3DID(objeto.Token.Text, tipo, puntero, estructura);
                }
                else if (objeto.Term.Name.Equals("NARREGLO"))
                {
                    nodo = Arreglo.generarAsignacionC3D(objeto.ChildNodes[0].Token.Text,
                        objeto.ChildNodes[1], tipo);
                }
                else
                {
                    // llamada con retorno
                }
                tipo = Tipo.ESTE;
                estructura = nodo.estructura;
            }
            if(nExp != null)
            {
                // Asignar el valor a la variable!
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
                    if(!sid.padre.Equals(C3DSentencias.claseActual.padre))
                    {
                        pos = "0";
                    }
                    String t1 = GeneradorC3D.getTemporal();
                    String t2 = GeneradorC3D.getTemporal();
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION,
                        t1, puntero, "+", pos));
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ACCESO,
                        estructura, t2, t1));
                    puntero = t2;
                    nodo.estructura = "Heap";
                }
                // Generar C3D para buscar la posicion de ID y luego el valor
                String t3 = GeneradorC3D.getTemporal();
                String t4 = GeneradorC3D.getTemporal();
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION,
                                t3, puntero, "+", Convert.ToString(sid.pos)));
                //GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ACCESO,
                //estructura, t4, t3));
                actual = sid;
                nodo.referencia = t3;
                nodo.cadena = t4;
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
