﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;
using System.Windows.Forms;

namespace _Compi2_Proyecto2_201314863
{
    public class InterpreteTree
    {
        #region "Variables"
        List<Clase> clases;
        #endregion

        #region "Interprete"
        public InterpreteTree()
        {
            clases = new List<Clase>();
        }

        public List<Clase> analizar(String entrada)
        {
            AnalizadorTree gramatica = new AnalizadorTree();
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
            //listaMetodos = new Dictionary<string, Cuerpo>();
            if (raiz != null)
            {
                guardar(raiz);
            }
            return clases;
        }

        public void guardar(ParseTreeNode nodo)
        {
            // INICIO -> IMPORTACIONES CLASES
            guardarImportaciones(nodo.ChildNodes.ElementAt(0));
            guardarClases(nodo.ChildNodes.ElementAt(1));
        }
        #endregion

        #region "Importaciones"
        public List<Clase> guardarImportaciones(ParseTreeNode nodo)
        {
            // IMPORTACIONES -> LISTA_ARCHIVOS 
            // LISTA_ARCHIVOS -> archivo | url | ruta
            foreach (ParseTreeNode archivo in nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).ChildNodes)
            {
                String texto = "";
                if (archivo.Term.Name.Equals("archivo"))
                {
                    // El archivo se encuentra en la carpeta actual
                }
                else if (archivo.Term.Name.Equals("ruta"))
                {
                    // El archivo se encuentra en otra carpeta
                }
                else
                {
                    // El archivo se encuentra en el repositorio
                }
                InterpreteTree ntree = new InterpreteTree();
                return ntree.analizar(texto);
            }
            return null;
        }
        #endregion

        #region "Clases"
        public void guardarClases(ParseTreeNode nodo)
        {
            // CLASES -> Lista de CLASE
            foreach(ParseTreeNode clase in nodo.ChildNodes)
            {
                clases.Add(guardarClase(clase));
            }
        }

        public Clase guardarClase(ParseTreeNode clase)
        {
            // CLASE -> id BCUERPO | id id BCUERPO
            Clase nueva;
            String nombre_clase = clase.ChildNodes.ElementAt(0).Token.Value.ToString();
            int pos = 1;
            if (clase.ChildNodes.Count == 2)
            {
                nueva = new Clase(nombre_clase);
            }
            else
            {
                // Tiene padre
                String padre = clase.ChildNodes.ElementAt(1).Token.Value.ToString();
                nueva = new Clase(nombre_clase, padre);
                pos++;
            }
            // Recorrer atributos, constructores y procedimientos
            ParseTreeNode cuerpos = clase.ChildNodes.ElementAt(pos).ChildNodes.ElementAt(0);
            foreach(ParseTreeNode cuerpo in cuerpos.ChildNodes)
            {
                if (cuerpo.ChildNodes.ElementAt(0).Term.Name.Equals("METODO"))
                {
                    nueva.agregarProcedimiento(guardarProcedimiento(cuerpo.ChildNodes[0]));
                }
                else if (cuerpo.ChildNodes.ElementAt(0).Term.Name.Equals("__constructor"))
                {
                    nueva.agregarConstructor(guardarConstructor(cuerpo));
                }
                else if (cuerpo.ChildNodes.ElementAt(0).Term.Name.Equals("DECLARACION"))
                {
                    ParseTreeNode declara = cuerpo.ChildNodes[0];
                    if (declara.Term.Name.Equals("DECLARACION"))
                    {
                        /* DECLARACION -> TIPO + LISTA_IDS + asignarR
                                        | TIPO + LISTA_IDS
                                        | TIPO + id + INDICES */
                        int tipo = Simbolo.getTipo(declara.ChildNodes[0].Token.Text);
                        ParseTreeNode ids = declara.ChildNodes[1];
                        if (declara.ChildNodes.Count == 3 && ids.Term.Name.Equals("id"))
                        {
                            nueva.agregarAtributo(new Atributo(ids.Token.Text, tipo, declara.ChildNodes[2]));
                        }
                        else
                        {
                            foreach (ParseTreeNode var in ids.ChildNodes)
                            {
                                Atributo a = new Atributo(var.Token.Text, tipo, null);
                                if (declara.ChildNodes.Count == 3)
                                {
                                    a.asignarValor(declara.ChildNodes[2]);
                                }
                                nueva.agregarAtributo(a);
                            }
                        }
                    }
                }
                else if (cuerpo.ChildNodes[0].Term.Name.Equals("VISIBILIDAD"))
                {
                    int vis = Simbolo.getVisibilidad(cuerpo.ChildNodes[0].ChildNodes[0].Token.Text);
                    ParseTreeNode declara = cuerpo.ChildNodes[1];
                    if (declara.Term.Name.Equals("DECLARACION"))
                    {
                        int tipo = Simbolo.getTipo(declara.ChildNodes[0].Token.Text);
                        ParseTreeNode ids = declara.ChildNodes[1];
                        if (declara.ChildNodes.Count == 4 && ids.Term.Name.Equals("id"))
                        {
                            nueva.agregarAtributo(new Atributo(ids.Token.Text, tipo, declara.ChildNodes[2]));
                        }
                        else
                        {
                            foreach (ParseTreeNode var in ids.ChildNodes)
                            {
                                Atributo a = new Atributo(var.Token.Text, tipo, null);
                                if (declara.ChildNodes.Count == 4)
                                {
                                    a.asignarValor(declara.ChildNodes[2]);
                                }
                                nueva.agregarAtributo(a);
                            }
                        }
                    }
                }
            }
            return nueva;
        }

        public Procedimiento guardarProcedimiento(ParseTreeNode nodo)
        {
            /*
             METODO -> sobreescribir VISIBILIDAD TPROC id LISTA_PARAMETROS BLOQUE
                     | VISIBILIDAD TPROC id LISTA_PARAMETROS BLOQUE
                     | sobreescribir TPROC id LISTA_PARAMETROS BLOQUE
                     | TPROC id LISTA_PARAMETROS BLOQUE
            */
            List<Atributo> parametros;
            if(nodo.ChildNodes.Count == 6)
            {
                // METODO -> sobreescribir VISIBILIDAD TPROC id LISTA_PARAMETROS BLOQUE
                parametros = guardarParametros(nodo.ChildNodes[4]);
                String id = nodo.ChildNodes[3].Token.Text;
                int vis = Simbolo.getVisibilidad(nodo.ChildNodes[1].ChildNodes[0].Token.Text);
                if(nodo.ChildNodes[2].ChildNodes.Count == 2)
                {
                    return new Procedimiento(id, 
                        Simbolo.getTipo(nodo.ChildNodes[2].ChildNodes[1].Token.Text), vis, 
                        parametros, nodo.ChildNodes[5].ChildNodes[0], true);
                }
                else
                {
                    return new Procedimiento(id, (int)Simbolo.Tipo.VACIO, vis, parametros, 
                        nodo.ChildNodes[5].ChildNodes[0], true);
                }
            }
            else if(nodo.ChildNodes.Count == 5)
            {
                /*
                 METODO -> VISIBILIDAD TPROC id LISTA_PARAMETROS BLOQUE
                         | sobreescribir TPROC id LISTA_PARAMETROS BLOQUE
                */
                parametros = guardarParametros(nodo.ChildNodes[3]);
                String id = nodo.ChildNodes[2].Token.Text;
                if (nodo.ChildNodes[0].Term.Name.Equals("VISIBILIDAD"))
                {
                    int vis = Simbolo.getVisibilidad(nodo.ChildNodes[0].ChildNodes[0].Token.Text);
                    if (nodo.ChildNodes[2].ChildNodes.Count == 2)
                    {
                        return new Procedimiento(id,
                            Simbolo.getTipo(nodo.ChildNodes[2].ChildNodes[1].Token.Text), vis,
                            parametros, nodo.ChildNodes[4].ChildNodes[0], false);
                    }
                    else
                    {
                        return new Procedimiento(id, (int)Simbolo.Tipo.VACIO, vis, parametros,
                            nodo.ChildNodes[4].ChildNodes[0], false);
                    }
                }
                else
                {
                    if (nodo.ChildNodes[2].ChildNodes.Count == 2)
                    {
                        return new Procedimiento(id,
                            Simbolo.getTipo(nodo.ChildNodes[2].ChildNodes[1].Token.Text),
                            parametros, nodo.ChildNodes[4].ChildNodes[0], true);
                    }
                    else
                    {
                        return new Procedimiento(id, (int)Simbolo.Tipo.VACIO, parametros,
                            nodo.ChildNodes[4].ChildNodes[0], true);
                    }
                }
            }
            else
            {
                // METODO -> TPROC id LISTA_PARAMETROS BLOQUE
                parametros = guardarParametros(nodo.ChildNodes[2]);
                String id = nodo.ChildNodes[1].Token.Text;
                if (nodo.ChildNodes[0].ChildNodes.Count == 2)
                {
                    return new Procedimiento(id,
                        Simbolo.getTipo(nodo.ChildNodes[0].ChildNodes[1].Token.Text),
                        parametros, nodo.ChildNodes[3].ChildNodes[0], false);
                }
                else
                {
                    return new Procedimiento(id, (int)Simbolo.Tipo.VACIO, parametros,
                        nodo.ChildNodes[3].ChildNodes[0], false);
                }
            }
        }

        public Procedimiento guardarConstructor(ParseTreeNode nodo)
        {
            // CUERPO -> constructor PARAMETROS? BLOQUE
            if(nodo.ChildNodes.Count == 3)
            {
                // Con parametros
                List<Atributo> parametros = guardarParametros(nodo.ChildNodes[1]);
                return new Procedimiento("constructor", (int)Simbolo.Tipo.CONSTRUCTOR,
                    parametros, nodo.ChildNodes[1]);
            }
            else
            {
                // Sin parametros
                return new Procedimiento("constructor", (int)Simbolo.Tipo.CONSTRUCTOR, 
                    new List<Atributo>(), nodo.ChildNodes[1]);
            }
        }

        public List<Atributo> guardarParametros(ParseTreeNode nodo)
        {
            List<Atributo> parametros = new List<Atributo>();
            foreach(ParseTreeNode pa in nodo.ChildNodes)
            {
                // PARAMETRO -> TIPO id INDICES
                int tipo = Simbolo.getTipo(pa.ChildNodes[0].Token.Text);
                parametros.Add(new Atributo(pa.ChildNodes[1].Token.Text, tipo, pa.ChildNodes[2]));
            }
            return parametros;
        }
        #endregion
    }
}
