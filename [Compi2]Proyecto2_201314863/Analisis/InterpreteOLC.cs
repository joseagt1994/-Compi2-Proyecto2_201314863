using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace _Compi2_Proyecto2_201314863
{
    class InterpreteOLC
    {
        #region "Variables"
        List<Clase> clases;
        #endregion

        #region "Interprete"
        public InterpreteOLC()
        {
            clases = new List<Clase>();
        }

        public List<Clase> analizarOLC(String entrada)
        {
            AnalizadorOLC gramatica = new AnalizadorOLC();
            Parser parser = new Parser(gramatica);

            ParseTree arbol = parser.Parse(entrada);
            ParseTreeNode raiz = arbol.Root;
            AST ast = new AST();
            ast.graficarAST(raiz);
            if (raiz == null || arbol.ParserMessages.Count > 0 || arbol.HasErrors())
            {
                //Hay Errores      
                //MessageBox.Show("Hay Errores");
                //errores.Append("Se encontraron errores:\n");
                foreach (var item in arbol.ParserMessages)
                {
                    Errores.getInstance.agregar(new Error((int)Error.tipoError.SINTACTICO,
                        item.Message, item.Location.Line, item.Location.Column));
                }
            }
            //Guardar metodos,funciones y variables; y ver donde empieza el main
            if (raiz != null)
            {
                guardar(raiz);
            }
            return clases;
        }

        public void guardar(ParseTreeNode nodo)
        {
            // INICIO -> IMPORTACIONES CLASES
            if (nodo.ChildNodes.Count == 2)
            {
                guardarImportaciones(nodo.ChildNodes.ElementAt(0));
                guardarClases(nodo.ChildNodes.ElementAt(1));
            }
            else
            {
                guardarClases(nodo.ChildNodes.ElementAt(0));
            }
        }

        #endregion

        #region "Importaciones y Llamadas"

        public void guardarImportaciones(ParseTreeNode nodo)
        {

        }

        #endregion

        #region "Clases"

        public void guardarClases(ParseTreeNode nodo)
        {
            // CLASES -> Lista de CLASE
            foreach (ParseTreeNode clase in nodo.ChildNodes)
            {
                Clase nueva = guardarClase(clase);
                if (!existeClase(nueva))
                {
                    clases.Add(nueva);
                }
            }
        }

        public bool existeClase(Clase nueva)
        {
            foreach (Clase c in clases)
            {
                if (c.nombre.Equals(nueva.nombre))
                {
                    Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                        "La clase " + nueva.nombre + " ya existe", nueva.linea,
                        nueva.columna));
                    return true;
                }
            }
            return false;
        }

        public Clase guardarClase(ParseTreeNode nodo)
        {
            // CLASE -> id id CUERPOS
            //| id CUERPOS
            Clase nueva;
            int idCuerpos = 1;
            if(nodo.ChildNodes.Count == 3)
            {
                // Tiene padre
                nueva = new Clase(nodo.ChildNodes[0].Token.Text, 
                    nodo.ChildNodes[1].Token.Text, nodo.ChildNodes[0].Token.Location.Line,
                    nodo.ChildNodes[0].Token.Location.Column);
                idCuerpos = 2;
            }
            else
            {
                // No tiene padre
                nueva = new Clase(nodo.ChildNodes[0].Token.Text,
                    nodo.ChildNodes[0].Token.Location.Line,
                    nodo.ChildNodes[0].Token.Location.Column);
            }
            // Recorrer cuerpos
            foreach(ParseTreeNode cuerpo in nodo.ChildNodes[idCuerpos].ChildNodes)
            {
                // Recorrer cada uno de los cuerpos en busca de Declaraciones, Metodos, Funciones o Constructores
                /*
                 * CUERPO.Rule = VISIBILIDAD + TPROC + id + TCUERPO? -> 4 , 3
                        | TPROC + id + TCUERPO? -> 3 , 2
                        | sobreescribir + METODO -> 2
                        | PRINCIPAL -> 1
                        | VISIBILIDAD + CONSTRUCTOR -> 2
                        | CONSTRUCTOR -> 1
                        | ASIGNACION + fin; -> 1
                 */
                switch (cuerpo.ChildNodes.Count)
                {
                    case 1:
                        // CONSTRUCTOR o PRINCIPAL
                        if (cuerpo.ChildNodes[0].Term.Name.Equals("CONSTRUCTOR"))
                        {
                            Procedimiento cons = guardarConstructor(cuerpo.ChildNodes[0],
                                (int)Simbolo.Visibilidad.PUBLICO);
                            cons.llenarDeclaraciones(cons.sentencias, nueva.atributos);
                            nueva.agregarConstructor(cons);
                        }
                        else if (cuerpo.ChildNodes[0].Term.Name.Equals("PRINCIPAL"))
                        {

                            Procedimiento proc = new Procedimiento("main", 
                                (int)Simbolo.Tipo.VACIO, new List<Atributo>(),cuerpo.ChildNodes[0].ChildNodes[0],
                                cuerpo.Span.Location.Line, cuerpo.Span.Location.Column);
                            proc.llenarDeclaraciones(proc.sentencias, nueva.atributos);
                            nueva.agregarProcedimiento(proc);
                        }
                        break;
                    case 2:
                        // sobreescribir METODO
                        // VISIBILIDAD CONSTRUCTOR
                        if (cuerpo.ChildNodes[1].Term.Name.Equals("CONSTRUCTOR"))
                        {
                            int vis = Simbolo.getVisibilidad(cuerpo.ChildNodes[0].ChildNodes[0].Token.Text);
                            Procedimiento cons = guardarConstructor(cuerpo.ChildNodes[1], vis);
                            cons.llenarDeclaraciones(cons.sentencias, nueva.atributos);
                            nueva.agregarConstructor(cons);
                        }
                        else if (cuerpo.ChildNodes[1].Term.Name.Equals("METODO"))
                        {
                            Procedimiento proc = guardarProcedimiento
                                (cuerpo.ChildNodes[1], true);
                            proc.llenarDeclaraciones(proc.sentencias, nueva.atributos);
                            nueva.agregarProcedimiento(proc);
                        }
                        else
                        {
                            // TPROC id
                            ParseTreeNode tproc = cuerpo.ChildNodes[0];
                            int tipo = Simbolo.getTipo(tproc.ChildNodes[0].Token.Text);
                            nueva.agregarAtributo(new Atributo(cuerpo.ChildNodes[1].Token.Text,
                                tipo, cuerpo.ChildNodes[1].Token.Location.Line,
                                cuerpo.ChildNodes[1].Token.Location.Column));
                        }
                        break;
                    case 3:
                        // TPROC id CMETODO
                        // TPROC id DARREGLO
                        // TPROC id EXP
                        // VISIBILIDAD TPROC id
                        if (cuerpo.ChildNodes[0].Term.Name.Equals("TPROC"))
                        {
                            if (cuerpo.ChildNodes[2].Term.Name.Equals("CMETODO"))
                            {
                                // Es procedimiento
                                Procedimiento proc = guardarProcedimiento
                                (cuerpo, true);
                                proc.llenarDeclaraciones(proc.sentencias, nueva.atributos);
                                nueva.agregarProcedimiento(proc);
                            }
                            else
                            {
                                // Es atributo
                                ParseTreeNode tproc = cuerpo.ChildNodes[0];
                                String ctipo = tproc.ChildNodes[0].Token.Text;
                                int tipo = Simbolo.getTipo(ctipo);
                                Atributo a = new Atributo(cuerpo.ChildNodes[1].Token.Text,
                                    tipo, cuerpo.ChildNodes[1].Token.Location.Line,
                                    cuerpo.ChildNodes[1].Token.Location.Column);
                                // Ver si el tipo de la variable es una clase
                                if (tipo == (int)Simbolo.Tipo.CLASE)
                                {
                                    a.asignarClase(ctipo);
                                }
                                // Verificar si es un arreglo o tiene valor
                                if (cuerpo.ChildNodes[2].Term.Name.Equals("EXP"))
                                {
                                    // Tiene valor
                                    a.asignarValor(cuerpo.ChildNodes[2]);
                                }
                                else
                                {
                                    // Es arreglo
                                    ParseTreeNode darr = cuerpo.ChildNodes[2];
                                    a.asignarArreglo(darr.ChildNodes[0]);
                                    if(darr.ChildNodes.Count == 2)
                                    {
                                        a.asignarValor(darr.ChildNodes[1]);
                                    }
                                }
                                nueva.agregarAtributo(a);
                            }
                        }
                        else if (cuerpo.ChildNodes[0].Term.Name.Equals("VISIBILIDAD"))
                        {
                            // Es atributo
                            int vis = Simbolo.getVisibilidad(cuerpo.ChildNodes[0].ChildNodes[0].Token.Text);
                            ParseTreeNode tproc = cuerpo.ChildNodes[1];
                            String ctipo = tproc.ChildNodes[0].Token.Text;
                            int tipo = Simbolo.getTipo(ctipo);
                            Atributo a = new Atributo(cuerpo.ChildNodes[2].Token.Text,
                                tipo, vis, cuerpo.ChildNodes[2].Token.Location.Line,
                                cuerpo.ChildNodes[2].Token.Location.Column);
                            if (tipo == (int)Simbolo.Tipo.CLASE)
                            {
                                a.asignarClase(ctipo);
                            }
                            nueva.agregarAtributo(a);
                        }
                        break;
                    case 4:
                        // VISIBILIDAD TPROC id CMETODO
                        // VISIBILIDAD TPROC id DARREGLO
                        // VISIBILIDAD TPROC id EXP
                        int vis2 = Simbolo.getVisibilidad(cuerpo.ChildNodes[0].ChildNodes[0].Token.Text);
                        if (cuerpo.ChildNodes[3].Term.Name.Equals("CMETODO"))
                        {
                            // Es procedimiento
                            Procedimiento proc = guardarProcedimiento(cuerpo, false);
                            proc.llenarDeclaraciones(proc.sentencias, nueva.atributos);
                            nueva.agregarProcedimiento(proc);
                        }
                        else 
                        {
                            // Es atributo --> DARREGLO o EXP
                            // Es atributo
                            ParseTreeNode tproc = cuerpo.ChildNodes[1];
                            String ctipo = tproc.ChildNodes[0].Token.Text;
                            int tipo = Simbolo.getTipo(ctipo);
                            Atributo a = new Atributo(cuerpo.ChildNodes[2].Token.Text,
                                tipo, vis2, cuerpo.ChildNodes[2].Token.Location.Line,
                                cuerpo.ChildNodes[2].Token.Location.Column);
                            // Ver si el tipo de la variable es una clase
                            if (tipo == (int)Simbolo.Tipo.CLASE)
                            {
                                a.asignarClase(ctipo);
                            }
                            // Verificar si es un arreglo o tiene valor
                            if (cuerpo.ChildNodes[3].Term.Name.Equals("EXP"))
                            {
                                // Tiene valor
                                a.asignarValor(cuerpo.ChildNodes[3]);
                            }
                            else
                            {
                                // Es arreglo
                                ParseTreeNode darr = cuerpo.ChildNodes[3];
                                a.asignarArreglo(darr.ChildNodes[0]);
                                if (darr.ChildNodes.Count == 2)
                                {
                                    a.asignarValor(darr.ChildNodes[1]);
                                }
                            }
                            nueva.agregarAtributo(a);
                        }
                        break;
                }
            }
            return nueva;
        }

        public Procedimiento guardarProcedimiento(ParseTreeNode nodo, bool sobrescribir)
        {
            // VISIBILIDAD TPROC id CMETODO
            // TPROC id CMETODO                     
            /*** CMETODO -> PARAMETROS SENTENCIAS ***/
            Procedimiento nuevo;
            int vis = (int)Simbolo.Visibilidad.PUBLICO;
            int inicio = 0;
            if(nodo.ChildNodes.Count == 4)
            {
                vis = Simbolo.getVisibilidad(nodo.ChildNodes[inicio].ChildNodes[0].Token.Text);
                inicio++;
            }
            ParseTreeNode tproc = nodo.ChildNodes[inicio];
            String id = nodo.ChildNodes[inicio + 1].Token.Text;
            ParseTreeNode cmetodo = nodo.ChildNodes[inicio + 2];
            int tipo = Simbolo.getTipo(tproc.ChildNodes[0].Token.Text);
            List<Atributo> parametros = guardarParametros(cmetodo.ChildNodes[0]);
            nuevo = new Procedimiento(id, tipo, vis, parametros, cmetodo.ChildNodes[1], sobrescribir,
                nodo.ChildNodes[inicio + 1].Token.Location.Line,
                nodo.ChildNodes[inicio + 1].Token.Location.Column);
            if(tproc.ChildNodes[1].ChildNodes.Count > 0)
            {
                nuevo.agregarArreglo(tproc.ChildNodes[1]);
            }
            return nuevo;
        }

        public Procedimiento guardarConstructor(ParseTreeNode nodo, int vis)
        {
            // CONSTRUCTOR -> 
            String id = nodo.ChildNodes[0].Token.Text;
            // Comparar que sea igual que la clase actual
            ParseTreeNode cmetodo = nodo.ChildNodes[1];
            List<Atributo> parametros = guardarParametros(cmetodo.ChildNodes[0]);
            return new Procedimiento("constructor", (int)Simbolo.Tipo.CONSTRUCTOR, 
                vis, parametros, cmetodo.ChildNodes[1], false, nodo.ChildNodes[0].Token.Location.Line,
                nodo.ChildNodes[0].Token.Location.Column);
        }

        public List<Atributo> guardarParametros(ParseTreeNode nodo)
        {
            List<Atributo> parametros = new List<Atributo>();
            foreach (ParseTreeNode pa in nodo.ChildNodes)
            {
                // PARAMETRO -> TIPO NARREGLO -> id INDICES
                int tipo = Simbolo.getTipo(pa.ChildNodes[0].Token.Text);
                Atributo parametro = new Atributo(pa.ChildNodes[1].Token.Text, tipo,
                    pa.ChildNodes[1].Token.Location.Line,
                    pa.ChildNodes[1].Token.Location.Column);
                if (tipo == (int)Simbolo.Tipo.CLASE)
                {
                    parametro.asignarClase(pa.ChildNodes[0].Token.Text);
                }
                if (pa.ChildNodes[2].ChildNodes.Count > 0)
                {
                    parametro.asignarArreglo(pa.ChildNodes[2]);
                }
                parametros.Add(parametro);
            }
            return parametros;
        }

        #endregion
    }
}
