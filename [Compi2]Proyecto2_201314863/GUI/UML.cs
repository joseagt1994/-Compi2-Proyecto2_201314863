using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _Compi2_Proyecto2_201314863
{
    public partial class UML : Form
    {
        #region "Variables"
        private String codigo, graphviz;
        private List<Clase> clases;
        private List<Relacion> relaciones;
        private int contadorImgs = 1;
        public UML()
        {
            InitializeComponent();
            clases = new List<Clase>();
            relaciones = new List<Relacion>();
            timer1.Interval = 1000;
            timer1.Start();
        }
        #endregion

        #region "Acciones"
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            agregarClase();
        }

        private void imgHerencia_Click(object sender, EventArgs e)
        {
            agregarRelacion((int)Relacion.Tipo.HERENCIA);
        }

        private void imgAgregacion_Click(object sender, EventArgs e)
        {
            agregarRelacion((int)Relacion.Tipo.AGREGACION);
        }

        private void imgComposicion_Click(object sender, EventArgs e)
        {
            agregarRelacion((int)Relacion.Tipo.COMPOSICION);
        }

        private void imgAsignacion_Click(object sender, EventArgs e)
        {
            agregarRelacion((int)Relacion.Tipo.ASOCIACION);
        }

        private void imgDependencia_Click(object sender, EventArgs e)
        {
            agregarRelacion((int)Relacion.Tipo.DEPENDENCIA);
        }

        private void btnConvertirImg_Click(object sender, EventArgs e)
        {
            // Convertir imagen a codigo
        }

        private void btnConvertirCodigo_Click(object sender, EventArgs e)
        {
            // Convertir codigo a imagen
        }

        public void agregarClase()
        {
            Clase nueva = new Clase("Clase_" + (clases.Count + 1), 0, 0);
            this.clases.Add(nueva);
            EditorClase editor = new EditorClase();
            editor.setClase(ref nueva);
            editor.Show();
        }

        public void agregarRelacion(int tipo)
        {
            EditorRelacion editor = new EditorRelacion();
            editor.setDatos(clases, tipo, ref relaciones);
            editor.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Repintar
            repintar();
            if (contadorImgs > 1)
            {
                for (int i = 0; i < contadorImgs; i++)
                {
                    try
                    {
                        File.Delete(@"C:\Graphviz\Imagenes\nueva" + i + ".png");
                    }
                    catch (IOException ex)
                    {

                    }
                }
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            ejecutarGraphviz();
            Thread.Sleep(1000);
            File.Delete(@"C:\Graphviz\Imagenes\imagen" + contadorImgs + ".txt");
            this.imgDiagrama.Image = new System.Drawing.Bitmap(@"C:\Graphviz\Imagenes\nueva" + contadorImgs + ".png");
            contadorImgs++;
        }

        private void agregarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            agregarClase();
        }

        #endregion

        #region "Graphviz"
        public void generarGraphviz()
        {
            graphviz = "strict digraph{\nsplines=ortho\nrankdir = LR;\n";
            // Recorrer clases
            generarClases();
            // Recorrer relaciones
            generarRelaciones();
            graphviz += "}";
        }

        public void generarClases()
        {
            graphviz += "\n\n";
            foreach(Clase clase in clases)
            {
                graphviz += clase.nombre + " [shape = none margin = 0 label = ";
                graphviz += "<<table border=\"1\" cellpadding=\"10\" bgcolor=\"khaki\">";
                // Nombre de la clase
                graphviz += "<tr><td>"+clase.nombre+"</td></tr>";
                // Atributos de la clase
                graphviz += "<tr><td align=\"LEFT\">";
                foreach(Atributo a in clase.atributos)
                {
                    // Escribir atributo
                    graphviz += a.getValor() + "<br/>";
                }
                graphviz += "</td></tr>";
                // Procedimientos de la clase
                graphviz += "<tr><td align=\"LEFT\">";
                foreach (Procedimiento p in clase.procedimientos)
                {
                    // Escribir atributo
                    graphviz += p.getValor() + "<br/>";
                }
                graphviz += "</td></tr>";
                graphviz += "</table>>];\n";
            }
            graphviz += "\n\n";
        }

        public void generarRelaciones()
        {
            graphviz += "\n\n";
            foreach (Relacion rel in relaciones)
            {
                switch (rel.relacion)
                {
                    case (int)Relacion.Tipo.AGREGACION:
                        graphviz += rel.clase_1 + "->" + rel.clase_2 + " [arrowhead = odiamond];\n";
                        break;
                    case (int)Relacion.Tipo.ASOCIACION:
                        graphviz += rel.clase_1 + "->" + rel.clase_2 + ";\n";
                        break;
                    case (int)Relacion.Tipo.COMPOSICION:
                        graphviz += rel.clase_1 + "->" + rel.clase_2 + " [arrowhead = diamond];\n";
                        break;
                    case (int)Relacion.Tipo.DEPENDENCIA:
                        graphviz += rel.clase_1 + "->" + rel.clase_2 + " [arrowhead = vee style = dashed];\n";
                        break;
                    default:
                        // HERENCIA
                        graphviz += rel.clase_1 + "->" + rel.clase_2 + " [arrowhead = o];\n";
                        break;
                }
            }
            graphviz += "\n\n";
        }

        public void repintar()
        {
            generarGraphviz();
        }

        public void ejecutarGraphviz()
        {
            // CREAR ARCHIVO graphviz
            File.WriteAllText(@"C:\Graphviz\Imagenes\imagen"+contadorImgs+".txt", graphviz);
            // EJECUTAR EN CONSOLA COMANDO PARA CONVERTIR A PNG
            String comando = @"C:\Graphviz\bin\dot.exe " + @"C:\Graphviz\Imagenes\imagen"+contadorImgs+".txt " + "-o " + @"C:\Graphviz\Imagenes\nueva"+contadorImgs+".png " + "-Tpng";
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
        #endregion
        
        public class Relacion
        {
            public String clase_1, clase_2;
            public int relacion;

            public enum Tipo
            {
                HERENCIA, AGREGACION, COMPOSICION, DEPENDENCIA, ASOCIACION
            }

            public Relacion(String clase_1, String clase_2, int tipo)
            {
                this.clase_1 = clase_1;
                this.clase_2 = clase_2;
                this.relacion = tipo;
            }

            public static String getTipo(int tipo)
            {
                switch (tipo)
                {
                    case (int)Tipo.AGREGACION:
                        return "Agregado a";
                    case (int)Tipo.ASOCIACION:
                        return "Asociado a";
                    case (int)Tipo.COMPOSICION:
                        return "Esta compuesto";
                    case (int)Tipo.DEPENDENCIA:
                        return "Depende de";
                    default:
                        return "Hereda de";
                }
            }

        }
    }
}
