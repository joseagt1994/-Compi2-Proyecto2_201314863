using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Irony.Parsing;

namespace _Compi2_Proyecto2_201314863
{
    public class GeneradorC3D
    {
        #region "Atributos"
        //LISTA DE C3D
        public static List<C3D> instrucciones;
        //CONTADORES 
        static int temporales = 1;
        static int etiquetas = 1;
        // LISTA DE CLASES
        public static List<Clase> clases;
        //DISPLAY DE CICLOS!
        //Display display;
        //DISPLAY DE AMBITOS
        //DisplayAmbitos ambitos;
        //AMBITO DE FUNCIONES O METODOS
        String nombreMetodo; //Nombre de funcion o metodo
        int tipo;       //Funcion o metodo
        public static int tamMain;
        String eSalidaMetodo;
        #endregion

        #region "Generacion de C3D"

        public static List<C3D> generarC3D(Clase main_clase)
        {
            instrucciones = new List<C3D>();
            //this.display = new Display();
            //this.ambitos = new DisplayAmbitos();
            temporales = 1;
            etiquetas = 1;
            // Generar codigo 3D del metodo principal
            generarC3DMain(main_clase);
            // Generar codigo 3D de los demas metodos
            generarC3DClases();
            return instrucciones;
        }

        public static Clase getClasePadre(String nombre)
        {
            foreach(Clase clase in clases)
            {
                if (clase.nombre.Equals(nombre))
                {
                    return clase;
                }
            }
            return null;
        }

        private static void generarC3DMain(Clase clase)
        {
            C3DSentencias.claseActual = clase;
            Simbolo smain = TablaSimbolos.getInstance.getProcedimiento(clase.nombre,"main");
            // Guardar this de la clase main
            instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Reservar espacio para this y super de "+clase.nombre));
            String t1 = getTemporal();
            instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Guardar inicio -> pos 0"));
            instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, t1, "H", "+", "0"));
            instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Stack", "0", "0"));
            instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Stack", "1", "1"));
            GeneradorC3D.aumentarHeap("2");
            // Generar this y super
            generarC3DThis(clase, t1, "Heap");
            // Guardar super de la clase main
            generarC3DSuper(clase, t1, "Heap");
            instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Llamada al main"));
            instrucciones.Add(new C3D((int)C3D.TipoC3D.LLAMADA, clase.nombre+"_main"));
            instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Terminar la ejecucion"));
            instrucciones.Add(new C3D((int)C3D.TipoC3D.EXIT, ""));
        }

        private static void generarC3DSuper(Clase clase, String puntero, String estructura)
        {
            Simbolo sclase = TablaSimbolos.getInstance.getClase(clase.nombre);
            if (sclase != null)
            {
                String t1 = getTemporal();
                instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Guardar apuntador del super"));
                instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, t1, puntero, "+", "1"));
                instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Guardar posicion donde empieza super"));
                instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, estructura, t1, "H"));
                // Buscar clase padre
                Clase padre;
                if(clase.padre != null)
                {
                    padre = getClasePadre(clase.padre);
                    if(padre == null)
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
                sclase = TablaSimbolos.getInstance.getClase(clase.padre);
                if(sclase == null)
                {
                    return;
                }
                instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Reservar espacio para la clase padre"+sclase.nombre));
                aumentarHeap(Convert.ToString(sclase.tam));
                // Recorrer las variables globales y asignarles
                foreach (Atributo atr in padre.atributos)
                {
                    if(atr.visibilidad == (int)Simbolo.Visibilidad.PUBLICO || 
                        atr.visibilidad == (int)Simbolo.Visibilidad.PROTEGIDO)
                    {
                        if (atr.esArreglo)
                        {
                            Arreglo.guardarC3D(atr.nombre, atr.valor, Acceso.Tipo.SUPER);
                        }
                        else
                        {
                            if (atr.valor != null)
                            {
                                instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Obtener valor del atributo "+atr.nombre));
                                Nodo exp = Expresion.expresionC3D(atr.valor);
                                Acceso.actual = null;
                                instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Obtener posicion del atributo "+atr.nombre));
                                Nodo nodo = Acceso.generarC3DID(atr.nombre, Acceso.Tipo.SUPER, 
                                    "P", "Stack");
                                if (nodo != null)
                                {
                                    // Asignar la expresion
                                    instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Guardar valor del atributo "+atr.nombre));
                                    instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR,
                                        nodo.estructura, nodo.referencia, exp.cadena));
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void generarC3DThis(Clase clase, String puntero, String estructura)
        {
            Simbolo sclase = TablaSimbolos.getInstance.getClase(clase.nombre);
            if(sclase != null)
            {
                String t1 = getTemporal();
                instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Guardar apuntador del this"));
                instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, t1, puntero, "+", "0"));
                instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Guardar apuntador donde incia el this"));
                instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, estructura, t1, "H"));
                instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Reservar espacio para globales de "+clase.nombre));
                aumentarHeap(Convert.ToString(sclase.tam));
                // Recorrer las variables globales y asignarles
                foreach (Atributo atr in clase.atributos)
                {
                    if (atr.esArreglo)
                    {
                        Arreglo.guardarC3D(atr.nombre, atr.valor, Acceso.Tipo.ESTE);
                    }
                    else
                    {
                        if(atr.valor != null)
                        {
                            instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Obtener valor del atributo "+atr.nombre));
                            Nodo exp = Expresion.expresionC3D(atr.valor);
                            Acceso.actual = null;
                            instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Obtener posicion del atributo "+atr.nombre));
                            Nodo nodo = Acceso.generarC3DID(atr.nombre, Acceso.Tipo.ESTE, "P", "Stack");
                            if(nodo != null)
                            {
                                // Asignar la expresion
                                instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Asignar expresion al atributo "+atr.nombre));
                                instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR,
                                    nodo.estructura, nodo.referencia, exp.cadena));
                            }
                        }
                    }
                }
            }
        }

        private static void generarC3DClases()
        {
            // Recorrer cada una de las clases
            foreach(Clase clase in clases)
            {
                C3DSentencias.claseActual = clase;
                // Recorrer cada uno de los constructores
                foreach (Procedimiento constructor in clase.constructores)
                {
                    C3DSentencias.procedimientoActual = TablaSimbolos.getInstance.getProcedimiento(clase.nombre,
                        constructor.completo);
                    //C3DSentencias.procedimientoActual = constructor.completo;
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.INICIO_METODO, 
                        clase.nombre+"_"+constructor.completo));
                    // Guardar H en t1
                    String t1 = GeneradorC3D.getTemporal();
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Guardar apuntador de la instancia"));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, t1, "H", "+", "0"));
                    GeneradorC3D.aumentarHeap("2");
                    // Generar this y super
                    String t2 = getTemporal();
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, t2, "P", "+", "0"));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Stack", t2, t1));
                    generarC3DThis(clase, t1, "Heap");
                    t2 = getTemporal();
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, t2, "P", "+", "1"));
                    String t5 = getTemporal();
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, t5, t1, "+", "1"));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Stack", t2, t5));
                    generarC3DSuper(clase, t1, "Heap");
                    String t3 = getTemporal();
                    String t4 = getTemporal();
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, t3, t1, "+", "1"));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, t4, "P", "+", "1"));
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Stack", t4, t3));
                    // Sentencias del constructor
                    C3DSentencias.generarC3D(constructor.sentencias);
                    // Guardar en retorno la posicion del objeto creado
                    String temp = C3DSentencias.retornarC3D(constructor.linea, constructor.columna);
                    if (!temp.Equals(""))
                    {
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO,
                            "// Guardar el retorno en la posicion"));
                        instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Stack", temp,
                            t1));
                    }
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.FIN_METODO, "}"));
                }
                // Recorrer cada uno de los procedimientos
                foreach (Procedimiento procedimiento in clase.procedimientos)
                {
                    //C3DSentencias.procedimientoActual = procedimiento.completo;
                    C3DSentencias.procedimientoActual = TablaSimbolos.getInstance.getProcedimiento(clase.nombre,
                        procedimiento.completo);
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.INICIO_METODO, 
                        clase.nombre + "_" + procedimiento.completo));
                    // Sentencias del procedimiento
                    C3DSentencias.generarC3D(procedimiento.sentencias);
                    instrucciones.Add(new C3D((int)C3D.TipoC3D.FIN_METODO, "}"));
                }
            }
        }

        #endregion

        #region "Temporales, etiquetas y punteros"

        //Genera los temporales
        public static String getTemporal()
        {
            String temp = "t" + temporales;
            temporales++;
            return temp;
        }

        //Genera las etiquetas necesarias
        public static String getEtiqueta()
        {
            String temp = "L" + etiquetas;
            etiquetas++;
            return temp;
        }

        //Generar las etiquetas necesarias
        public static void generarEtiquetas(String lista)
        {
            if (lista == null)
            {
                return;
            }
            String[] etiquetas = lista.Split(',');
            foreach (String e in etiquetas)
            {
                instrucciones.Add(new C3D((int)C3D.TipoC3D.ETIQUETA, e));
            }
        }

        //Aumentar Heap
        public static void aumentarHeap(String valor)
        {
            instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, "H", "H", "+", valor));
        }

        #endregion
    }
}
