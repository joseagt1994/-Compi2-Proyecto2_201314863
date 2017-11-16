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
            codigo = "";
            resetearClases();
            generarCodigoRelaciones();
            foreach(Clase clase in clases)
            {
                if (clase.padre != null)
                {
                    // Herencia
                    if(tipoArchivo.SelectedIndex == 0)
                    {
                        codigo += "clase " + clase.nombre + " hereda_de " + clase.padre + "{\n\n";
                    }
                    else if(tipoArchivo.SelectedIndex == 1)
                    {
                        codigo += "clase " + clase.nombre + "[" + clase.padre + "]:\n\n";
                    }
                }else
                {
                    // Sin Herencia
                    if (tipoArchivo.SelectedIndex == 0)
                    {
                        codigo += "clase " + clase.nombre + "{\n\n";
                    }
                    else if (tipoArchivo.SelectedIndex == 1)
                    {
                        codigo += "clase " + clase.nombre + "[]:\n\n";
                    }
                }
                codigo += generarCodigoAtributos(clase);
                codigo += clase.textoAtributos;
                // Constructor
                if (tipoArchivo.SelectedIndex == 0)
                {
                    codigo += "\t" + clase.nombre + "(){\n\n\t}\n\n";
                }
                else if (tipoArchivo.SelectedIndex == 1)
                {
                    codigo += "\t" + clase.nombre + "[]:\n\n";
                }
                codigo += clase.textoProcedimientos;
                codigo += generarCodigoProcedimientos(clase);
                // Final
                if (tipoArchivo.SelectedIndex == 0)
                {
                    codigo += "}\n\n";
                }
            }
            txtCodigo.Text = codigo;
        }

        private void btnConvertirCodigo_Click(object sender, EventArgs e)
        {
            // Convertir codigo a imagen
            
            if(tipoArchivo.SelectedIndex == 0)
            {
                // OLC
                InterpreteOLC interprete = new InterpreteOLC();
                clases = interprete.analizarOLC(txtCodigo.Text);
            }
            else if(tipoArchivo.SelectedIndex == 1)
            {
                // Tree
                InterpreteTree interprete = new InterpreteTree();
                clases = interprete.analizarTree(txtCodigo.Text);
            }
            else
            {
                MessageBox.Show("Seleccione un lenguaje!");
                return;
            }
            relaciones = new List<Relacion>();
            // Recorrer clases para buscar relaciones necesarias
            generarRelacionesUML();
            repintar();
            ejecutarGraphviz();
            Thread.Sleep(1000);
            File.Delete(@"C:\Graphviz\Imagenes\imagen" + contadorImgs + ".txt");
            this.imgDiagrama.Image = new System.Drawing.Bitmap(@"C:\Graphviz\Imagenes\nueva" + contadorImgs + ".png");
            contadorImgs++;
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

        #region "Generar codigo del Diagrama UML"

        public void resetearClases()
        {
            foreach(Clase clase in clases)
            {
                clase.textoAtributos = "";
                clase.textoProcedimientos = "";
            }
        }

        public void generarCodigoRelaciones()
        {
            int tipo = tipoArchivo.SelectedIndex;
            foreach(Clase clase in clases)
            {
                // Llenar las relaciones en cada una de las clases
                foreach(Relacion relacion in relaciones)
                {
                    if (relacion.clase_1.Equals(clase.nombre))
                    {
                        // HERENCIA, DEPENDENCIA, COMPOSICION, AGREGACION
                        switch (relacion.relacion)
                        {
                            case (int)Relacion.Tipo.HERENCIA:
                                clase.padre = relacion.clase_2;
                                break;
                            case (int)Relacion.Tipo.DEPENDENCIA:
                                if(tipo == 0)
                                {
                                    // OLC++
                                    clase.textoProcedimientos += "\tpublico void usar(" + relacion.clase_2 + " objeto){\n\n\t}\n\n";
                                }
                                else if(tipo == 1)
                                {
                                    // Tree
                                    clase.textoProcedimientos += "\tpublico metodo usar[" + relacion.clase_2 + " objeto]:\n\n";
                                }
                                break;
                            case (int)Relacion.Tipo.COMPOSICION:
                                if(tipo == 0)
                                {
                                    // OLC++
                                    clase.textoAtributos += "\tpublico " + relacion.clase_2 + " objeto;\n\n";
                                    clase.textoAtributos += "\t" + clase.nombre + "(" + relacion.clase_2 + " objeto){\n";
                                    clase.textoAtributos += "\t\teste.objeto = objeto;\n\n\t}\n\n";
                                }
                                else if(tipo == 1)
                                {
                                    // Tree
                                    clase.textoAtributos += "\tpublico " + relacion.clase_2 + " objeto\n\n";
                                    clase.textoAtributos += "\t" + clase.nombre + "[" + relacion.clase_2 + " objeto]:\n";
                                    clase.textoAtributos += "\t\tself.objeto = objeto\n\n";
                                }
                                break;
                            case (int)Relacion.Tipo.AGREGACION:
                                /*
                                 * publico void agregar(Clase2 objeto):
		                            self.objeto = objeto
                                */
                                if(tipo == 0)
                                {
                                    // OLC++
                                    clase.textoAtributos += "\tpublico " + relacion.clase_2 + " objeto;\n\n";
                                    clase.textoProcedimientos += "\tpublico void agregar(" + relacion.clase_2 + " objeto){\n";
                                    clase.textoProcedimientos += "\t\teste.objeto = objeto;\n\n\t}\n\n";
                                }
                                else if(tipo == 1)
                                {
                                    // Tree
                                    clase.textoAtributos += "\tpublico " + relacion.clase_2 + " objeto\n\n";
                                    clase.textoProcedimientos += "\tpublico metodo agregar[" + relacion.clase_2 + " objeto]:\n";
                                    clase.textoProcedimientos += "\t\tself.objeto = objeto\n\n";
                                }
                                break;
                        }
                    }
                    else if (relacion.clase_2.Equals(clase.nombre))
                    {
                        // ASOCIACION
                        if(relacion.relacion == (int)Relacion.Tipo.ASOCIACION)
                        {
                            if(tipo == 0)
                            {
                                // OLC++
                                clase.textoAtributos += "\tpublico " + relacion.clase_1 + " objeto;\n\n";
                            }
                            else if(tipo == 1)
                            {
                                // Tree
                                clase.textoAtributos += "\tpublico " + relacion.clase_1 + " objeto\n\n";
                            }
                        }
                    }
                }
            }
        }

        public String generarCodigoAtributos(Clase clase)
        {
            String cadena = "";
            foreach(Atributo a in clase.atributos)
            {
                if(tipoArchivo.SelectedIndex == 0)
                {
                    // OLC++
                    cadena += "\t" + getVisibilidad(a.visibilidad) + " " + Simbolo.getValor(a.tipo) + " "
                    + a.nombre + ";\n";
                }
                else if(tipoArchivo.SelectedIndex == 1)
                {
                    // Tree
                    cadena += "\t" + getVisibilidad(a.visibilidad) + " " + Simbolo.getValor(a.tipo) + " "
                    + a.nombre + "\n";
                }
            }
            return cadena;
        }

        public String generarCodigoProcedimientos(Clase clase)
        {
            String cadena = "";
            foreach (Procedimiento a in clase.procedimientos)
            {
                if (tipoArchivo.SelectedIndex == 0)
                {
                    // OLC++
                    cadena += "\t" + getVisibilidad(a.visibilidad) + " " + Simbolo.getValor(a.tipo) + " "
                    + a.nombre + "(){\n\n\t}\n\n";
                }
                else if (tipoArchivo.SelectedIndex == 1)
                {
                    // Tree
                    if(a.tipo == (int)Simbolo.Tipo.VACIO)
                    {
                        cadena += "\t" + getVisibilidad(a.visibilidad) + " metodo " + a.nombre + "():\n\n";
                    }
                    else
                    {
                        cadena += "\t" + getVisibilidad(a.visibilidad) + " funcion " + Simbolo.getValor(a.tipo) +" "+ a.nombre + "():\n\n";
                    }
                }
            }
            return cadena;
        }
        
        public String getVisibilidad(int tipo)
        {
            switch (tipo)
            {
                case (int)Simbolo.Visibilidad.PUBLICO:
                    return "publico";
                case (int)Simbolo.Visibilidad.PRIVADO:
                    return "privado";
                default:
                    return "protegido";
            }
        }

        #endregion

        #region "Generar Diagrama UML del codigo"
        
        public void generarRelacionesUML()
        {
            // Recorrer clases
            foreach(Clase clase in clases)
            {
                if(clase.padre != null)
                {
                    // HERENCIA
                    relaciones.Add(new Relacion(clase.nombre, clase.padre, 
                        (int)Relacion.Tipo.HERENCIA));
                }
                // Verificar si hay una declaracion global de tipo Clase
                List<Atributo> atributos = getAtributoClase(clase);
                if(atributos.Count > 0)
                {
                    foreach (Atributo a in atributos)
                    {
                        // Verificar si hay un contructor con parametro con esa Clase y nombre
                        if (getConstructorClase(clase, a.clase, a.nombre))
                        {
                            relaciones.Add(new Relacion(clase.nombre, a.clase, (int)Relacion.Tipo.COMPOSICION));
                        }
                        // Sino Verificar si hay un metodo con parametro con esa Clase y nombre
                        else if (getProcedimientoClase(clase, a.clase, a.nombre))
                        {
                            relaciones.Add(new Relacion(clase.nombre, a.clase, (int)Relacion.Tipo.AGREGACION));
                        }
                        // Sino solo es Asociacion
                        else
                        {
                            relaciones.Add(new Relacion(a.clase,clase.nombre,(int)Relacion.Tipo.ASOCIACION));
                        }
                    }
                }
                else
                {
                    // Sino Verificar si hay algun metodo con parametro de tipo Clase
                    List<String> clases = getClaseProcedimiento(clase);
                    foreach(String c in clases)
                    {
                        relaciones.Add(new Relacion(clase.nombre, c, (int)Relacion.Tipo.DEPENDENCIA));
                    }
                }
            }
        }

        public List<String> getClaseProcedimiento(Clase clase)
        {
            List<String> clases = new List<String>();
            foreach(Procedimiento p in clase.procedimientos)
            {
                foreach(Atributo a in p.parametros)
                {
                    if(a.tipo == (int)Simbolo.Tipo.CLASE && a.clase != clase.nombre)
                    {
                        clases.Add(a.clase);
                    }
                }
            }
            return clases;
        }

        public bool getProcedimientoClase(Clase clase, String nclase, String id)
        {
            foreach (Procedimiento c in clase.procedimientos)
            {
                foreach (Atributo parametro in c.parametros)
                {
                    if (parametro.nombre == id && parametro.clase == nclase)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool getConstructorClase(Clase clase, String nclase, String id)
        {
            foreach(Procedimiento c in clase.constructores)
            {
                foreach(Atributo parametro in c.parametros)
                {
                    if(parametro.nombre == id && parametro.clase == nclase)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public List<Atributo> getAtributoClase(Clase clase)
        {
            List<Atributo> atributos = new List<Atributo>();
            foreach(Atributo a in clase.atributos)
            {
                if(a.tipo == (int)Simbolo.Tipo.CLASE)
                {
                    atributos.Add(a);
                }
            }
            return atributos;
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
