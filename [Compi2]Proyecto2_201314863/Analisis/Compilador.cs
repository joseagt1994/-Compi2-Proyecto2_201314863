using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _Compi2_Proyecto2_201314863
{
    public class Compilador
    {
        #region "Variables"
        List<Clase> clases;
        TablaSimbolos tablaSimbolos;
        LinkedList<Simbolo> simbolos;
        public static String rutaEjecutada;
        int tamanio;

        public Compilador()
        {
            clases = new List<Clase>();
            tablaSimbolos = TablaSimbolos.getInstance;
        }
        #endregion

        #region "Fases del Compilador"
        public List<C3D> compilar(String texto, RichTextBox consola, int lenguaje, String clase)
        {
            
            // *************** FASE 1 Analisis Lexico y Sintactico **************//
            if (lenguaje == (int)Pagina.Lenguaje.OLC)
            {
                // OLC++
                InterpreteOLC interprete = new InterpreteOLC();
                clases = interprete.analizarOLC(texto);
                clase = clase.Replace(".olc","");
            }
            else
            {
                // Tree
                InterpreteTree interprete = new InterpreteTree();
                clases = interprete.analizarTree(texto);
                clase = clase.Replace(".tree", "");
            }
            // **************** FASE 2 Llenar Tabla de Simbolos *****************//
            llenarTablaSimbolos();

            // **************** FASE 3 Analisis Semantico y Codigo Intermedio ***//
            if (clases.Count > 0)
            {
                Clase main = buscarMain(clases[0]);
                if (main == null)
                {
                    // Error! No hay procedimiento main en la clase de inicio!
                    return null;
                }
                GeneradorC3D.clases = clases;
                return GeneradorC3D.generarC3D(main);
            }
            return null;
        }

        private Clase buscarMain(Clase clase)
        {
            foreach(Simbolo simbolo in tablaSimbolos)
            {
                if(simbolo.padre != null)
                {
                    if(simbolo.rol == (int)Simbolo.Tipo.METODO && simbolo.nombre.Equals("main"))
                    {
                        return clase;
                    }
                }
            }
            return null;
        }
        #endregion

        #region "Llenar la TS"

        public void llenarTablaSimbolos()
        {
            foreach(Clase clase in clases)
            {
                simbolos = new LinkedList<Simbolo>();
                tamanio = 0;
                // Recorrer atributos
                foreach(Atributo atributo in clase.atributos)
                {
                    simbolos.AddLast(guardarSimboloVariable(clase.nombre, atributo, true));
                }
                // Agregar clase a la TS
                simbolos.AddFirst(guardarSimboloClase(clase));
                tablaSimbolos.insertarLista(simbolos);
                // Recorrer constructores -> parametros -> declaraciones
                foreach(Procedimiento constructor in clase.constructores)
                {
                    tablaSimbolos.insertarLista(guardarConstructor(clase.nombre, constructor));
                }
                // Recorrer procedimientos -> parametros -> declaraciones
                foreach (Procedimiento procedimiento in clase.procedimientos)
                {
                    tablaSimbolos.insertarLista(guardarProcedimiento(clase.nombre, procedimiento));
                }
            }
        }

        public LinkedList<Simbolo> guardarConstructor(String clase, Procedimiento constructor)
        {
            simbolos = new LinkedList<Simbolo>();
            // Agregar this y super a la lista de simbolos
            simbolos.AddLast(guardarEste(constructor.completo));
            simbolos.AddLast(guardarSuper(constructor.completo));
            Atributo retorno = new Atributo("retorno", (int)Simbolo.Tipo.CLASE, constructor.linea, constructor.columna);
            retorno.clase = clase;
            simbolos.AddLast(guardarSimboloRetorno(constructor.completo, retorno));
            tamanio = 3;
            // Recorrer parametros
            foreach(Atributo parametro in constructor.parametros)
            {
                simbolos.AddLast(guardarSimboloParametro(constructor.completo, parametro));
            }
            // Recorrer sentencias en busca de declaraciones
            foreach(Atributo variable in constructor.declaraciones)
            {
                simbolos.AddLast(guardarSimboloVariable(constructor.completo, variable, false));
            }
            // Recorrer al inicio el constructor
            simbolos.AddFirst(guardarSimboloConstructor(clase, constructor));
            return simbolos;
        }

        public LinkedList<Simbolo> guardarProcedimiento(String clase, Procedimiento procedimiento)
        {
            simbolos = new LinkedList<Simbolo>();
            // Agregar this y super a la lista de simbolos
            simbolos.AddLast(guardarEste(procedimiento.completo));
            simbolos.AddLast(guardarSuper(procedimiento.completo));
            tamanio = 2;
            // Ver si es funcion o no
            if(procedimiento.tipo != (int)Simbolo.Tipo.VACIO)
            {
                Atributo retorno = new Atributo("retorno", procedimiento.tipo, procedimiento.linea, procedimiento.columna);
                retorno.clase = procedimiento.clase;
                simbolos.AddLast(guardarSimboloRetorno(procedimiento.completo, retorno));
            }
            // Recorrer parametros
            foreach (Atributo parametro in procedimiento.parametros)
            {
                simbolos.AddLast(guardarSimboloParametro(procedimiento.completo, parametro));
            }
            // Recorrer sentencias en busca de declaraciones
            foreach (Atributo variable in procedimiento.declaraciones)
            {
                simbolos.AddLast(guardarSimboloVariable(procedimiento.completo, variable, false));
            }
            // Recorrer al inicio el procedimiento
            simbolos.AddFirst(guardarSimboloProcedimiento(clase, procedimiento));
            return simbolos;
        }

        public Simbolo guardarSimboloProcedimiento(String padre, Procedimiento procedimiento)
        {
            Simbolo sprocedimiento = new Simbolo();
            sprocedimiento.ambito = (int)Simbolo.Tipo.GLOBAL;
            sprocedimiento.clase = "";
            sprocedimiento.dimensiones = null;
            sprocedimiento.nombre = procedimiento.completo;
            sprocedimiento.visibilidad = procedimiento.visibilidad;
            sprocedimiento.padre = padre;
            sprocedimiento.tam = tamanio;
            sprocedimiento.tipo = procedimiento.tipo;
            sprocedimiento.rol = (int)Simbolo.Tipo.FUNCION;
            if (procedimiento.tipo == (int)Simbolo.Tipo.VACIO)
            {
                sprocedimiento.rol = (int)Simbolo.Tipo.METODO;
            }
            if (procedimiento.tipo == (int)Simbolo.Tipo.CLASE)
            {
                sprocedimiento.clase = procedimiento.clase;
            }
            return sprocedimiento;
        }

        public Simbolo guardarSimboloConstructor(String padre, Procedimiento constructor)
        {
            Simbolo sconstructor = new Simbolo();
            sconstructor.ambito = (int)Simbolo.Tipo.GLOBAL;
            sconstructor.visibilidad = constructor.visibilidad;
            sconstructor.clase = "";
            sconstructor.dimensiones = null;
            sconstructor.nombre = constructor.completo;
            sconstructor.padre = padre;
            sconstructor.pos = -1;
            sconstructor.rol = (int)Simbolo.Tipo.CONSTRUCTOR;
            sconstructor.tam = tamanio;
            sconstructor.tipo = -1;
            return sconstructor;
        }

        public Simbolo guardarSimboloRetorno(String padre, Atributo retorno)
        {
            Simbolo sparametro = new Simbolo();
            sparametro.ambito = (int)Simbolo.Tipo.LOCAL;
            sparametro.clase = "";
            if (retorno.tipo == (int)Simbolo.Tipo.CLASE)
            {
                sparametro.clase = retorno.clase;
            }
            sparametro.dimensiones = retorno.dimensiones;
            sparametro.nombre = retorno.nombre;
            sparametro.padre = padre;
            sparametro.pos = tamanio;
            tamanio++;
            sparametro.rol = (int)Simbolo.Tipo.RETORNO;
            sparametro.tam = 1;
            sparametro.tipo = retorno.tipo;
            return sparametro;
        }

        public Simbolo guardarSimboloParametro(String padre, Atributo parametro)
        {
            Simbolo sparametro = new Simbolo();
            sparametro.ambito = (int)Simbolo.Tipo.LOCAL;
            sparametro.clase = "";
            if(parametro.tipo == (int)Simbolo.Tipo.CLASE)
            {
                sparametro.clase = parametro.clase;
            }
            sparametro.dimensiones = parametro.dimensiones;
            sparametro.nombre = parametro.nombre;
            sparametro.padre = padre;
            sparametro.pos = tamanio;
            tamanio++;
            sparametro.rol = (int)Simbolo.Tipo.PARAMETRO;
            sparametro.tam = 1;
            sparametro.tipo = parametro.tipo;
            return sparametro;
        }

        public Simbolo guardarSimboloClase(Clase clase)
        {
            Simbolo sclase = new Simbolo();
            sclase.nombre = clase.nombre;
            sclase.padre = clase.padre;
            sclase.ambito = -1;
            sclase.dimensiones = null;
            sclase.pos = -1;
            sclase.rol = (int)Simbolo.Tipo.CLASE;
            sclase.tipo = -1;
            sclase.tam = tamanio;
            return sclase;
        }

        public Simbolo guardarSimboloVariable(String padre, Atributo atributo, bool global)
        {
            Simbolo sglobal = new Simbolo();
            sglobal.ambito = (int)Simbolo.Tipo.LOCAL;
            if (global)
            {
                sglobal.ambito = (int)Simbolo.Tipo.GLOBAL;
            }
            sglobal.clase = "";
            sglobal.nombre = atributo.nombre;
            sglobal.padre = padre;
            sglobal.pos = tamanio;
            sglobal.visibilidad = atributo.visibilidad;
            tamanio++;
            sglobal.rol = (int)Simbolo.Tipo.VARIABLE;
            sglobal.tam = 1;
            sglobal.tipo = atributo.tipo;
            if(atributo.tipo == (int)Simbolo.Tipo.CLASE)
            {
                sglobal.clase = atributo.clase;
            }
            sglobal.dimensiones = atributo.dimensiones;
            return sglobal;
        }

        public Simbolo guardarEste(String padre)
        {
            Simbolo seste = new Simbolo();
            seste.ambito = (int)Simbolo.Tipo.LOCAL;
            seste.clase = "";
            seste.nombre = "this";
            seste.padre = padre;
            seste.pos = 0;
            seste.rol = (int)Simbolo.Tipo.VARIABLE;
            seste.tam = 1;
            seste.tipo = -1;
            seste.dimensiones = null;
            return seste;
        }

        public Simbolo guardarSuper(String padre)
        {
            Simbolo seste = new Simbolo();
            seste.ambito = (int)Simbolo.Tipo.LOCAL;
            seste.clase = "";
            seste.nombre = "super";
            seste.padre = padre;
            seste.pos = 1;
            seste.rol = (int)Simbolo.Tipo.VARIABLE;
            seste.tam = 1;
            seste.tipo = -1;
            seste.dimensiones = null;
            return seste;
        }
        #endregion
    }
}
