using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace _Compi2_Proyecto2_201314863
{
    public class Arreglo
    {
        public static void guardarC3D(string id, ParseTreeNode arreglo, Acceso.Tipo tipo)
        {
            Acceso.actual = null;
            Nodo nodo = Acceso.generarC3DID(id, tipo, "P", "Stack");
            if(nodo == null)
            {
                return;
            }
            Simbolo sid;
            if(C3DSentencias.procedimientoActual == null)
            {
                sid = TablaSimbolos.getInstance.buscarVariable(id, C3DSentencias.claseActual.nombre,
                null, tipo);
            }
            else
            {
                sid = TablaSimbolos.getInstance.buscarVariable(id, C3DSentencias.claseActual.nombre,
                C3DSentencias.procedimientoActual.nombre, tipo);
            }
            // Recorrer los valores del arreglo y guardarlos en la posicion empezando en 0
            if (arreglo != null && sid.dimensiones != null)
            {
                List<int> dimensiones = new List<int>();
                dimensionesArreglo(arreglo, ref dimensiones);
                for(int i = 0; i < sid.dimensiones.Count; i++)
                {
                    if(dimensiones[i] != sid.dimensiones[i])
                    {
                        // Error semantico! No es de las dimensiones que solicita!
                        return;
                    }
                }
                // Todo bien!
                /* String t1 = GeneradorC3D.getTemporal();
                 GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                     "// Obtener puntero al heap del arreglo " + id));
                 GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ACCESO, nodo.estructura, t1, nodo.referencia));
                 */
                int tamH = sid.getTamanioTotal();
                String t1 = GeneradorC3D.getTemporal();
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                     "// Guardar H disponible en " + id));
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, 
                    nodo.estructura, nodo.referencia, "H"));
                recorrerDimensiones(arreglo, "H");
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                     "// Incrementar H para reservar tam del arreglo "+id));
                GeneradorC3D.aumentarHeap(Convert.ToString(tamH));
            }
        }

        public static void recorrerDimensiones(ParseTreeNode arreglo,String pos)
        {
            if (arreglo.Term.Name.Equals("ARREGLO"))
            {
                foreach(ParseTreeNode arr in arreglo.ChildNodes)
                {
                    recorrerDimensiones(arr,pos);
                }
            }
            else
            {
                Nodo exp = Expresion.expresionC3D(arreglo);
                if(exp != null)
                {
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                    "// Guardar expresion en el arreglo"));
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR,
                            "Heap", pos, exp.cadena));
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                    "// Incrementar "+pos));
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION,
                        pos, pos, "+", "1"));
                }
            }
        }
        public static void dimensionesArreglo(ParseTreeNode arreglo, ref List<int> dimensiones)
        {
            if(arreglo.Term.Name.Equals("ARREGLO"))
            {
                dimensionesArreglo(arreglo.ChildNodes[0], ref dimensiones);
                dimensiones.Add(arreglo.ChildNodes.Count);
            }
        }

        public static Nodo generarAsignacionC3D(string id, ParseTreeNode indices, Acceso.Tipo tipo)
        {
            Simbolo sid;
            if (C3DSentencias.procedimientoActual == null)
            {
                sid = TablaSimbolos.getInstance.buscarVariable(id, C3DSentencias.claseActual.nombre,
                null, tipo);
            }
            else
            {
                sid = TablaSimbolos.getInstance.buscarVariable(id, C3DSentencias.claseActual.nombre,
                C3DSentencias.procedimientoActual.nombre, tipo);
            }
            if(sid == null)
            {
                // Error Semantico!
                return null;
            }
            if(sid.dimensiones == null)
            {
                // Error Semantico! No es un arreglo!
                return null;
            }
            Nodo nodo = Acceso.generarC3DID(id, tipo, "P", "Stack");
            String tpos = generarParametrizacionC3D(indices, sid);
            if(nodo != null && tpos != null)
            {
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                    "// Sumar la posicion del Heap del arreglo y la posicion del arreglo"));
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, nodo.referencia,
                    nodo.cadena, "+", tpos));
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                    "// Obtener el valor en la posicion del arreglo "+id));
                nodo.cadena = GeneradorC3D.getTemporal();
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ACCESO, "Heap", nodo.cadena, 
                    nodo.referencia));
            }
            return nodo;
        }

        public static String generarParametrizacionC3D(ParseTreeNode indices, Simbolo sid)
        {
            int con = 1;
            String temp = null;
            foreach(ParseTreeNode hijo in indices.ChildNodes)
            {
                Nodo exp = Expresion.expresionC3D(hijo);
                if(exp == null)
                {
                    break;
                }
                int tam = sid.getTamanio(con);
                String t1 = GeneradorC3D.getTemporal();
                String t2 = GeneradorC3D.getTemporal();
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                    "// Restar uno a la posicion en la dimension " + con));
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, t1, exp.cadena,
                    "-", "1"));
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                    "// Multiplicarla por el tamanio de la dimension"));
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, t2, t1, "*",
                    Convert.ToString(tam)));
                if(con > 1)
                {
                    String t3 = GeneradorC3D.getTemporal();
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                        "// Sumar las dos posiciones"));
                    GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, t3, temp, "+", t2));
                    temp = t3;
                }
                else
                {
                    temp = t2;
                }
                con++;
            }
            return temp;
        }

    }
}
