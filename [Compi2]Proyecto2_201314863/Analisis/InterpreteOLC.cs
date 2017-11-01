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
    public class InterpreteOLC
    {
        #region "Variables"
        //TABLA DE SIMBOLOS!
        public TablaSimbolos tablaSimbolos;
        ParseTreeNode principal; //Sentencias del Main
        #endregion

        #region "Interprete"
        public List<Clase> analizar(String entrada, RichTextBox txtCodigo)
        {
            tablaSimbolos = TablaSimbolos.getInstance;
            AnalizadorOLC gramatica = new AnalizadorOLC();
            Parser parser = new Parser(gramatica);

            ParseTree arbol = parser.Parse(entrada);
            ParseTreeNode raiz = arbol.Root;
            //AST ast = new AST();
            //ast.graficarAST(raiz);
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
                //guardar(raiz);
                //Generar reporte de tabla de Simbolos
                //ReporteHTML rpt = new ReporteHTML();
                //rpt.generarReporteTS(tablaSimbolos);
                //Generar C3D
                //Generador genera = new Generador();
                //return genera.generacionC3D(tablaSimbolos, principal, txtCodigo);
            }
            return null;
        }
        #endregion
    }
}
