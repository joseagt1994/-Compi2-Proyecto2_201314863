using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using System.IO;
using System.Diagnostics;

namespace _Compi2_Proyecto2_201314863
{
    public class AST
    {
        List<string> nodos;
        List<string> relaciones;
        int id = 1;
        String color = "gray";
        String forma = "oval";

        public AST()
        {
            nodos = new List<string>();
            relaciones = new List<string>();
            id = 1;
        }

        public void graficarAST(ParseTreeNode raiz)
        {
            if (raiz != null)
            {
                //Graficar el arbol
                generarNodos(raiz);
                //Escribir en un archivo
                String graphviz = "digraph grafo{\n";
                foreach (String nodo in nodos)
                {
                    graphviz += nodo + "\n";
                }
                foreach (String relacion in relaciones)
                {
                    graphviz += relacion + "\n";
                }
                graphviz += "}";
                ejecutarGrafo(graphviz);
                Process.Start(@"C:\Graphviz\Archivos\arbolAST.png");
            }
        }

        public void generarNodos(ParseTreeNode nodo)
        {
            int padre = id;
            id++;
            if (nodo.Token != null)
            {
                nodos.Add("nodo" + padre + " [label=\"" + nodo.Term.Name + "," + nodo.Token.Value.ToString().Replace("\"", "") + "\", fillcolor=\"" + color + "\", style=\"filled\", shape=\"" + forma + "\"];");
            }
            else
            {
                nodos.Add("nodo" + padre + " [label=\"" + nodo.Term.Name + "\", fillcolor=\"" + color + "\", style=\"filled\", shape=\"" + forma + "\"];");
            }
            foreach (ParseTreeNode hijo in nodo.ChildNodes)
            {
                relaciones.Add("nodo" + padre + "->" + "nodo" + id + ";");
                generarNodos(hijo);
                id++;
            }
        }

        public void ejecutarGrafo(String graphviz)
        {
            // CREAR ARCHIVO graphviz
            File.WriteAllText(@"C:\Graphviz\Archivos\arbolAST.txt", graphviz);
            // EJECUTAR EN CONSOLA COMANDO PARA CONVERTIR A PNG
            String comando = @"C:\Graphviz\bin\dot.exe " + @"C:\Graphviz\Archivos\arbolAST.txt " + "-o " + @"C:\Graphviz\Archivos\arbolAST.png " + "-Tpng";
            try
            {
                ProcessStartInfo e = new ProcessStartInfo("cmd", "/c " + comando);
                e.RedirectStandardOutput = true;
                e.UseShellExecute = false;
                e.CreateNoWindow = false;
                Process proceso = new Process();
                proceso.StartInfo = e;
                proceso.Start();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ERROR!" + ex.Message.ToString());
            }
        }
    }
}
