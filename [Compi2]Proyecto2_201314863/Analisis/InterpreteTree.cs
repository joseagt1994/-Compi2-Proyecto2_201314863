using System;
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

        public List<Clase> analizarTree(String entrada)
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
            if (raiz != null)
            {
                guardar(raiz);
            }
            return clases;
        }

        public void guardar(ParseTreeNode nodo)
        {
            // INICIO -> IMPORTACIONES CLASES
            if(nodo.ChildNodes.Count == 2)
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
                return ntree.analizarTree(texto);
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
                Clase nueva = guardarClase(clase);
                if (!existeClase(nueva))
                {
                    clases.Add(nueva);
                }
            }
        }

        public bool existeClase(Clase nueva)
        {
            foreach(Clase c in clases)
            {
                if (c.nombre.Equals(nueva.nombre))
                {
                    Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                        "La clase "+nueva.nombre+ " ya existe", nueva.linea, 
                        nueva.columna));
                    return true;
                }
            }
            return false;
        }

        public Clase guardarClase(ParseTreeNode clase)
        {
            // CLASE -> id BCUERPO | id id BCUERPO
            Clase nueva;
            String nombre_clase = clase.ChildNodes.ElementAt(0).Token.Value.ToString();
            int pos = 1;
            if (clase.ChildNodes.Count == 2)
            {
                nueva = new Clase(nombre_clase, clase.ChildNodes[0].Token.Location.Line,
                    clase.ChildNodes[0].Token.Location.Line);
            }
            else
            {
                // Tiene padre
                String padre = clase.ChildNodes.ElementAt(1).Token.Value.ToString();
                nueva = new Clase(nombre_clase, padre, clase.ChildNodes[0].Token.Location.Line,
                    clase.ChildNodes[0].Token.Location.Line);
                pos++;
            }
            // Recorrer atributos, constructores y procedimientos
            ParseTreeNode cuerpos = clase.ChildNodes.ElementAt(pos).ChildNodes.ElementAt(0);
            foreach(ParseTreeNode cuerpo in cuerpos.ChildNodes)
            {
                if (cuerpo.ChildNodes.ElementAt(0).Term.Name.Equals("METODO"))
                {
                    Procedimiento proc = guardarProcedimiento(cuerpo.ChildNodes[0]);
                    proc.llenarDeclaraciones(proc.sentencias, nueva.atributos);
                    nueva.agregarProcedimiento(proc);
                }
                else if (cuerpo.ChildNodes.ElementAt(0).Term.Name.Equals("__constructor"))
                {
                    Procedimiento cons = guardarConstructor(cuerpo);
                    cons.llenarDeclaraciones(cons.sentencias, nueva.atributos);
                    nueva.agregarConstructor(cons);
                }
                else if (cuerpo.ChildNodes.ElementAt(0).Term.Name.Equals("DECLARACION"))
                {
                    ParseTreeNode declara = cuerpo.ChildNodes[0];
                    /* DECLARACION -> TIPO + LISTA_IDS + asignarR
                                    | TIPO + LISTA_IDS
                                    | TIPO + NARREGLO -> id + INDICES */
                    int tipo = Simbolo.getTipo(declara.ChildNodes[0].Token.Text);
                    ParseTreeNode ids = declara.ChildNodes[1];
                    if (ids.Term.Name.Equals("NARREGLO"))
                    {
                        Atributo variable = new Atributo(ids.ChildNodes[0].Token.Text, tipo,
                                ids.ChildNodes[0].Token.Location.Line, ids.ChildNodes[0].Token.Location.Column);
                        variable.asignarArreglo(ids.ChildNodes[1]);
                        if(tipo == (int)Simbolo.Tipo.CLASE)
                        {
                            variable.asignarClase(declara.ChildNodes[0].ChildNodes[0].Token.Text);
                        }
                        nueva.agregarAtributo(variable);
                    }
                    else
                    {
                        foreach (ParseTreeNode var in ids.ChildNodes)
                        {
                            Atributo a = new Atributo(var.Token.Text, tipo,
                                var.Token.Location.Line, var.Token.Location.Column);
                            if (declara.ChildNodes.Count == 3)
                            {
                                a.asignarValor(declara.ChildNodes[2]);
                            }
                            nueva.agregarAtributo(a);
                        }
                    }
                }
                else if (cuerpo.ChildNodes[0].Term.Name.Equals("VISIBILIDAD"))
                {
                    int vis = Simbolo.getVisibilidad(cuerpo.ChildNodes[0].ChildNodes[0].Token.Text);
                    ParseTreeNode declara = cuerpo.ChildNodes[1];
                    int tipo = Simbolo.getTipo(declara.ChildNodes[0].Token.Text);
                    ParseTreeNode ids = declara.ChildNodes[1];
                    if (ids.Term.Name.Equals("NARREGLO"))
                    {
                        Atributo variable = new Atributo(ids.ChildNodes[0].Token.Text, tipo,
                                ids.ChildNodes[0].Token.Location.Line, ids.ChildNodes[0].Token.Location.Column);
                        variable.asignarArreglo(ids.ChildNodes[1]);
                        if (tipo == (int)Simbolo.Tipo.CLASE)
                        {
                            variable.asignarClase(declara.ChildNodes[0].ChildNodes[0].Token.Text);
                        }
                        nueva.agregarAtributo(variable);
                    }
                    else
                    {
                        foreach (ParseTreeNode var in ids.ChildNodes)
                        {
                            Atributo a = new Atributo(var.Token.Text, tipo,
                                var.Token.Location.Line, var.Token.Location.Column);
                            if (declara.ChildNodes.Count == 3)
                            {
                                a.asignarValor(declara.ChildNodes[2]);
                            }
                            nueva.agregarAtributo(a);
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
                if(nodo.ChildNodes[2].ChildNodes.Count == 3)
                {
                    int tipo = Simbolo.getTipo(nodo.ChildNodes[2].ChildNodes[1].Token.Text);
                    Procedimiento proc = new Procedimiento(id, tipo, vis, 
                        parametros, nodo.ChildNodes[5].ChildNodes[0], true,
                        nodo.ChildNodes[3].Token.Location.Line,
                        nodo.ChildNodes[3].Token.Location.Column);
                    if(tipo == (int)Simbolo.Tipo.CLASE)
                    {
                        proc.agregarClase(nodo.ChildNodes[2].ChildNodes[1].Token.Text);
                    }
                    if(nodo.ChildNodes[2].ChildNodes[2].ChildNodes.Count > 0)
                    {
                        proc.agregarArreglo(nodo.ChildNodes[2].ChildNodes[2]);
                    }
                    return proc;
                }
                else
                {
                    return new Procedimiento(id, (int)Simbolo.Tipo.VACIO, vis, parametros, 
                        nodo.ChildNodes[5].ChildNodes[0], true,
                        nodo.ChildNodes[3].Token.Location.Line,
                        nodo.ChildNodes[3].Token.Location.Column);
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
                    if (nodo.ChildNodes[2].ChildNodes.Count == 3)
                    {
                        int tipo = Simbolo.getTipo(nodo.ChildNodes[2].ChildNodes[1].Token.Text);
                        Procedimiento proc = new Procedimiento(id, tipo, vis,
                            parametros, nodo.ChildNodes[4].ChildNodes[0], false,
                            nodo.ChildNodes[2].Token.Location.Line,
                            nodo.ChildNodes[2].Token.Location.Column);
                        if(tipo == (int)Simbolo.Tipo.CLASE)
                        {
                            proc.agregarClase(nodo.ChildNodes[2].ChildNodes[1].Token.Text);
                        }
                        if (nodo.ChildNodes[2].ChildNodes[2].ChildNodes.Count > 0)
                        {
                            proc.agregarArreglo(nodo.ChildNodes[2].ChildNodes[2]);
                        }
                        return proc;
                    }
                    else
                    {
                        return new Procedimiento(id, (int)Simbolo.Tipo.VACIO, vis, parametros,
                            nodo.ChildNodes[4].ChildNodes[0], false,
                            nodo.ChildNodes[2].Token.Location.Line,
                            nodo.ChildNodes[2].Token.Location.Column);
                    }
                }
                else
                {
                    if (nodo.ChildNodes[2].ChildNodes.Count == 3)
                    {
                        int tipo = Simbolo.getTipo(nodo.ChildNodes[2].ChildNodes[1].Token.Text);
                        Procedimiento proc = new Procedimiento(id, tipo,
                            parametros, nodo.ChildNodes[4].ChildNodes[0], true,
                            nodo.ChildNodes[2].Token.Location.Line,
                            nodo.ChildNodes[2].Token.Location.Column);
                        if (tipo == (int)Simbolo.Tipo.CLASE)
                        {
                            proc.agregarClase(nodo.ChildNodes[2].ChildNodes[1].Token.Text);
                        }
                        if (nodo.ChildNodes[2].ChildNodes[2].ChildNodes.Count > 0)
                        {
                            proc.agregarArreglo(nodo.ChildNodes[2].ChildNodes[2]);
                        }
                        return proc;
                    }
                    else
                    {
                        return new Procedimiento(id, (int)Simbolo.Tipo.VACIO, parametros,
                            nodo.ChildNodes[4].ChildNodes[0], true,
                            nodo.ChildNodes[2].Token.Location.Line,
                            nodo.ChildNodes[2].Token.Location.Column);
                    }
                }
            }
            else
            {
                // METODO -> TPROC id LISTA_PARAMETROS BLOQUE
                parametros = guardarParametros(nodo.ChildNodes[2]);
                String id = nodo.ChildNodes[1].Token.Text;
                if (nodo.ChildNodes[0].ChildNodes.Count == 3)
                {
                    int tipo = Simbolo.getTipo(nodo.ChildNodes[0].ChildNodes[1].Token.Text);
                    Procedimiento proc = new Procedimiento(id, tipo,
                        parametros, nodo.ChildNodes[3].ChildNodes[0], false,
                        nodo.ChildNodes[1].Token.Location.Line,
                            nodo.ChildNodes[1].Token.Location.Column);
                    if(tipo == (int)Simbolo.Tipo.CLASE)
                    {
                        proc.agregarClase(nodo.ChildNodes[0].ChildNodes[1].Token.Text);
                    }
                    if (nodo.ChildNodes[0].ChildNodes[2].ChildNodes.Count > 0)
                    {
                        proc.agregarArreglo(nodo.ChildNodes[2].ChildNodes[2]);
                    }
                    return proc;
                }
                else
                {
                    return new Procedimiento(id, (int)Simbolo.Tipo.VACIO, parametros,
                        nodo.ChildNodes[3].ChildNodes[0], false,
                        nodo.ChildNodes[1].Token.Location.Line,
                            nodo.ChildNodes[1].Token.Location.Column);
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
                    parametros, nodo.ChildNodes[2], nodo.ChildNodes[0].Token.Location.Line,
                            nodo.ChildNodes[0].Token.Location.Column);
            }
            else
            {
                // Sin parametros
                return new Procedimiento("constructor", (int)Simbolo.Tipo.CONSTRUCTOR, 
                    new List<Atributo>(), nodo.ChildNodes[1], nodo.ChildNodes[0].Token.Location.Line,
                            nodo.ChildNodes[0].Token.Location.Column);
            }
        }

        public List<Atributo> guardarParametros(ParseTreeNode nodo)
        {
            List<Atributo> parametros = new List<Atributo>();
            foreach(ParseTreeNode pa in nodo.ChildNodes)
            {
                // PARAMETRO -> TIPO NARREGLO -> id INDICES
                int tipo = Simbolo.getTipo(pa.ChildNodes[0].Token.Text);
                Atributo parametro = new Atributo(pa.ChildNodes[1].Token.Text, tipo,
                    pa.ChildNodes[1].Token.Location.Line,
                    pa.ChildNodes[1].Token.Location.Column);
                if(tipo == (int)Simbolo.Tipo.CLASE)
                {
                    parametro.asignarClase(pa.ChildNodes[0].ChildNodes[0].Token.Text);
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
