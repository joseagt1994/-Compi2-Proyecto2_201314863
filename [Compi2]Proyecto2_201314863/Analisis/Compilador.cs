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
        int tamanio;

        public Compilador()
        {
            clases = new List<Clase>();
            tablaSimbolos = TablaSimbolos.getInstance;
        }
        #endregion

        #region "Fases del Compilador"
        public List<C3D> compilar(String texto, RichTextBox consola, int lenguaje)
        {
            
            // ****************** FASE 1 Analisis Alto Nivel ********************//
            if (lenguaje == (int)Pagina.Lenguaje.OLC)
            {
                // OLC++
                InterpreteOLC interprete = new InterpreteOLC();
                clases = interprete.analizarOLC(texto);
            }
            else
            {
                // Tree
                InterpreteTree interprete = new InterpreteTree();
                clases = interprete.analizarTree(texto);
            }
            // **************** FASE 2 Llenar Tabla de Simbolos *****************//
            llenarTablaSimbolos();
            
            // **************** FASE 3 Analisis Semantico y Codigo Intermedio ***//


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
                    tamanio++;
                }
                // Agregar clase a la TS
                simbolos.AddFirst(guardarSimboloClase(clase));
                tablaSimbolos.insertarLista(simbolos);
                // Recorrer constructores -> parametros -> declaraciones
                foreach(Procedimiento constructor in clase.constructores)
                {
                    guardarConstructor(clase.nombre, constructor);
                }
                // Recorrer procedimientos -> parametros -> declaraciones
                foreach (Procedimiento procedimiento in clase.procedimientos)
                {

                }
            }
        }

        public void guardarConstructor(String clase, Procedimiento constructor)
        {
            simbolos = new LinkedList<Simbolo>();
            tamanio = 0;
            // Recorrer parametros
            foreach(Atributo parametro in constructor.parametros)
            {
                simbolos.AddLast(guardarSimboloParametro(constructor.completo, parametro));
            }
            // Recorrer sentencias en busca de declaraciones

        }

        public Simbolo guardarSimboloProcedimiento(String padre, Procedimiento procedimiento)
        {
            Simbolo sprocedimiento = new Simbolo();
            sprocedimiento.ambito = (int)Simbolo.Tipo.GLOBAL;
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
            sconstructor.clase = "";
            sconstructor.dimensiones = null;
            sconstructor.nombre = constructor.nombre;
            sconstructor.padre = padre;
            sconstructor.pos = tamanio;
            sconstructor.rol = (int)Simbolo.Tipo.CONSTRUCTOR;
            sconstructor.tam = 1;
            sconstructor.tipo = -1;
            return sconstructor;
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
            sglobal.ambito = (int)Simbolo.Tipo.GLOBAL;
            sglobal.clase = "";
            sglobal.nombre = atributo.nombre;
            sglobal.padre = padre;
            sglobal.pos = tamanio;
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
        #endregion
    }
}
