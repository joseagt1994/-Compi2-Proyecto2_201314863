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
        List<Clase> clases;
        TablaSimbolos tablaSimbolos;

        public Compilador()
        {
            clases = new List<Clase>();
            tablaSimbolos = TablaSimbolos.getInstance;
        }

        public List<C3D> compilar(String texto, RichTextBox consola, bool lenguaje)
        {
            // ****************** FASE 1 Analisis Alto Nivel ********************//
            if (lenguaje)
            {
                // OLC++
            }
            else
            {
                // Tree
                InterpreteTree itree = new InterpreteTree();
                clases = itree.analizar(texto);
            }
            // **************** FASE 2 Llenar Tabla de Simbolos *****************//

            return null;
        }
    }
}
