using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Windows.Forms;

namespace _Compi2_Proyecto2_201314863
{
    public class Ejecutor
    {
        #region "Atributos"
        // Inicio

        // Listas de C3D
        public List<C3D> instrucciones;
        Dictionary<String, int> etiquetas;
        Dictionary<String, int> metodos;
        public DisplayAmbitos ambitos;
        public String cadenaMostrar = "";
        public bool mostrarMensaje = false;
        public bool imprimir = false;
        RichTextBox txtCuerpo;

        // Puntero a lista de instrucciones
        public int puntero;

        // Atributos a utilizar!
        int ambito;
        public int P,H;
        public double NULL = -201314863.102494;
        public double SIGNUM = -241094.22;

        // Estructuras C3D
        public double[] Stack;
        public double[] Heap;
        #endregion

        public Ejecutor(List<C3D> codigo, RichTextBox texto)
        {
            this.txtCuerpo = texto;
            ejecutar(codigo);
        }

        #region "Manejo de etiquetas y metodos"
        // Guardar los punteros a las etiquetas
        public void guardarEtiquetasMetodos()
        {
            for(int i = 0; i < instrucciones.Count; i++)
            {
                C3D instruccion = instrucciones.ElementAt(i);
                if(instruccion.tipo == (int)C3D.TipoC3D.ETIQUETA)
                {
                    etiquetas.Add(instruccion.cadena, i);
                }
                else if(instruccion.tipo == (int)C3D.TipoC3D.INICIO_METODO)
                {
                    metodos.Add(instruccion.cadena, i+1);
                }
            }
        }
        #endregion

        #region "Ejecucion"
        //Llenar estructuras
        public void llenar()
        {
            for(int i = 0; i < 10000; i++)
            {
                Stack[i] = NULL;
                Heap[i] = NULL;
            }
        }

        // Metodo inicial
        public void ejecutar(List<C3D> instrucciones)
        {
            puntero = 0;
            Stack = new double[10000];
            Heap = new double[10000];
            llenar();
            ambitos = new DisplayAmbitos();
            etiquetas = new Dictionary<string, int>();
            metodos = new Dictionary<string, int>();
            this.instrucciones = instrucciones;
            // Guardar las etiquetas y sus punteros
            guardarEtiquetasMetodos();
            // Empezar ejecucion
            ambitos.aumentarAmbito(-1,0,"");
            ejecutarInstrucciones();
        }

        // Metodo que ejecuta las instrucciones dependiendo del puntero
        public void ejecutarInstrucciones()
        {
            if(puntero >= instrucciones.Count)
            {
                return;
            }
            if(puntero == -1)
            {
                return;
            }
            C3D actual = instrucciones.ElementAt(puntero);
            puntero++;
            // Acciones a ejecutar!
            ejecutarInstruccion(actual);
            ejecutarInstrucciones();
        }

        // Metodo que ejecuta la isntruccion
        public void ejecutarInstruccion(C3D instruccion)
        {
            switch (instruccion.tipo)
            {
                case (int)C3D.TipoC3D.ASIGNACION:
                    // ASIGNACION -> cadena + " = " + id1 + " " + operacion + " " + id2;
                    double id1 = getValor(instruccion.id1);
                    double id2 = getValor(instruccion.id2);
                    double resultado = operar(id1,instruccion.operacion,id2);
                    if(instruccion.cadena == "P")
                    {
                        if (instruccion.id1.Equals("P") && instruccion.operacion.Equals("+"))
                        {
                            ambito = (int)id2;
                        }
                        P = (int)resultado;
                    }
                    else if(instruccion.cadena == "H")
                    {
                        if (instruccion.id1.Equals("H") && instruccion.operacion.Equals("+"))
                        {
                            ambito = (int)id2;
                        }
                        H = (int)resultado;
                    }
                    else
                    {
                        ambitos.agregarTemporal(instruccion.cadena, resultado);
                    }
                    break;
                case (int)C3D.TipoC3D.ACCESO:
                    // ACCESO -> id1 + " = " + cadena + "[" + id2 + "]";
                    double indice = getValor(instruccion.id2);
                    double valor = getValor(instruccion.cadena, indice);
                    ambitos.agregarTemporal(instruccion.id1, valor);
                    break;
                case (int)C3D.TipoC3D.VALOR:
                    //VALOR -> cadena + "[" + id1 + "] = " + id2;
                    double indicev = getValor(instruccion.id1);
                    double valorv = getValor(instruccion.id2);
                    guardarValor(instruccion.cadena, indicev, valorv);
                    break;
                case (int)C3D.TipoC3D.INCONDICIONAL:
                    //INCONDICIONAL -> "goto " + cadena;
                    puntero = getPuntero(instruccion.cadena);
                    break;
                case (int)C3D.TipoC3D.CONDICIONAL:
                    //CONDICIONAL -> "if(" + id1 + " " + operacion + " " + id2 + ") goto " + cadena;
                    double valor1 = getValor(instruccion.id1);
                    double valor2 = getValor(instruccion.id2);
                    evaluarCondicional(valor1, instruccion.operacion, valor2, instruccion.cadena);
                    break;
                case (int)C3D.TipoC3D.FIN_METODO:
                    // FIN_METODO ->  "}";
                    puntero = ambitos.buscarSalida();
                    ambitos.disminuirAmbito();
                    break;
                case (int)C3D.TipoC3D.LLAMADA:
                    // LLAMADA -> "call " + cadena;
                    ambitos.aumentarAmbito(puntero, ambito, instruccion.cadena);
                    puntero = getPunteroMetodo(instruccion.cadena);
                    break;
                case (int)C3D.TipoC3D.IMPRIMIR:
                    // IMPRIMIR -> "printf(" + cadena + "," + id1 + ")";
                    double valorI = getValor(instruccion.id1);
                    if (instruccion.cadena.Equals("%c"))
                    {
                        char c = (char)(int)valorI;
                        txtCuerpo.AppendText(c.ToString());
                    }
                    else
                    {
                        if (instruccion.cadena.Equals("%d"))
                        {
                            txtCuerpo.AppendText(Convert.ToString((int)valorI));
                        }
                        else
                        {
                            // %f
                            txtCuerpo.AppendText(Convert.ToString(valorI));
                        }
                    }
                    break;
                case (int)C3D.TipoC3D.EXIT:
                    // EXIT -> "EXIT " + cadena; //VER QUE SE GENERA!!!!!??????
                    puntero = -1;
                    break;
                case (int)C3D.TipoC3D.NATIVA:
                    ejecutarNativa(instruccion);
                    break;
                default:
                    break;
            }
        }

        public void ejecutarNativa(C3D instruccion)
        {
            switch (instruccion.cadena)
            {
                case "$$_parseInt":
                    // 0 -> retorno
                    // 1 -> referencia a la posicion de la cadena en Stack
                    int refCadena = (int)Stack[P + 1];
                    String cadena = devolverCadena(refCadena);
                    try
                    {
                        Stack[P + 0] = Convert.ToInt32(cadena);
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("No se puede convertir "+cadena+" a entero!");
                    }
                    break;
                case "$$_parseDouble":
                    // 0 -> retorno
                    // 1 -> referencia a la posicion de la cadena en Stack
                    int refCadenaD = (int)Stack[P + 1];
                    String cadenaD = devolverCadena(refCadenaD);
                    try
                    {
                        Stack[P + 0] = Convert.ToDouble(cadenaD);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("No se puede convertir " + cadenaD + " a decimal!");
                    }
                    break;
                case "$$_intToStr":
                    // 0 -> retorno, puntero de la cadena
                    // 1 -> referencia al numero a convertir a cadena
                    int refEntero = (int)Stack[P + 1];
                    String entero = refEntero.ToString();
                    int posCadI = H;
                    foreach(char cInt in entero)
                    {
                        Heap[H] = (int)cInt;
                        H = H + 1;
                    }
                    Stack[P + 0] = posCadI;
                    break;
                case "$$_doubleToStr":
                    // 0 -> retorno, puntero de la cadena
                    // 1 -> referencia al decimal a convertir a cadena 
                    double refDec = Stack[P + 1];
                    String cdecimal = refDec.ToString();
                    int posCad = H;
                    foreach (char c in cdecimal)
                    {
                        Heap[H] = (int)c;
                        H = H + 1;
                    }
                    Stack[P + 0] = posCad;
                    break;
                case "$$_doubleToInt":
                    // 0 -> retorno
                    // 1 -> referencia al decimal a convertir a entero
                    double refDecimal = Stack[P + 1];
                    Stack[P + 0] = (int)refDecimal;
                    break;
            }
        }

        public String devolverCadena(int pHeap)
        {
            String cadena = "";
            bool bandera = false;
            int ptrPool = (int)Heap[pHeap];
            for(int i = ptrPool; i < 100000; i++)
            {
                double actual = Heap[i];
                if(actual == NULL)
                {
                    break;
                }
                else if(actual == SIGNUM)
                {
                    bandera = true;
                }
                else if(actual == 0)
                {
                    break;
                }
                else
                {
                    if (bandera)
                    {
                        cadena += actual;
                        bandera = false;
                    }
                    else
                    {
                        int valor = (int)actual;
                        char caracter = (char)valor;
                        cadena += caracter;
                    }
                }
            }
            return cadena;
        }

        #endregion

        #region "Manejo de valores y operaciones"
        // Obtener el valor de temporal, valor o puntero
        public double getValor(String id)
        {
            if (id.Equals("P"))
            {
                return P;
            }
            else if (id.Equals("H"))
            {
                return H;
            }
            else
            {
                // Numero o terminal !
                if(id.Equals(""))
                {
                    // Nada
                    return 0;
                }
                else if (id.ElementAt(0) == 't')
                {
                    // Terminal
                    return ambitos.buscarTemporal(id);
                }
                else
                {
                    // Numero
                    return Convert.ToDouble(id);
                }
            }
        }

        // Operar segun sea el caso
        public double operar(double id1,String ope,double id2)
        {
            switch(ope)
            {
                case "+":
                    return id1 + id2;
                case "-":
                    return id1 - id2;
                case "*":
                    return id1 * id2;
                case "/":
                    if(id2 == 0)
                    {
                        //Aritmethic Exception!
                        /*Errores.getInstance.agregar(new Error((int)Error.tipoError.EXCEPCION,
                            "ArithmeticException, division entre 0", puntero, 0));
                        puntero = ambitos.buscarSalida();*/
                        ambitos.disminuirAmbito();
                        return 0;
                    }
                    return id1 / id2;
                case "%":
                    if(id2 == 0)
                    {
                        //Aritmethic Exception!
                        /*Errores.getInstance.agregar(new Error((int)Error.tipoError.EXCEPCION,
                            "ArithmeticException, operacion modular entre 0", puntero, 0));
                        puntero = ambitos.buscarSalida();*/
                        ambitos.disminuirAmbito();
                        return 0;
                    }
                    return id1 % id2;
                case "^":
                    return Math.Pow(id1, id2);
                default:
                    return id1;
            }
        }

        // Realizar un acceso
        public double getValor(String estructura, double indice)
        {
            switch (estructura)
            {
                case "Stack":
                    if(indice >= Stack.Count())
                    {
                        // StackOverFlowException!
                       /*Errores.getInstance.agregar(new Error((int)Error.tipoError.EXCEPCION,
                            "StackOverFlowException, se ha pasado el limite de Stack", puntero, 0));
                        puntero = ambitos.buscarSalida();*/
                        ambitos.disminuirAmbito();
                        break;
                    }
                    if(indice == NULL)
                    {
                        MessageBox.Show("NullPointerException en el Stack\nPuntero: "+puntero);
                        return 0;
                    }
                    return Stack[(int)indice];
                case "Heap":
                    if (indice >= Heap.Count())
                    {
                        // HeapOverFlowException!
                        /*Errores.getInstance.agregar(new Error((int)Error.tipoError.EXCEPCION,
                            "HeapOverFlowException, se ha pasado el limite de Heap", puntero, 0));
                        puntero = ambitos.buscarSalida();*/
                        ambitos.disminuirAmbito();
                        break;
                    }
                    if (indice == NULL)
                    {
                        MessageBox.Show("NullPointerException en el Heap\nPuntero: " + puntero);
                        return 0;
                    }
                    return Heap[(int)indice];
            }
            return 0;
        }

        // Guardar un valor en Stack, Heap o Pool
        public void guardarValor(String estructura, double indice, double valor)
        {
            switch (estructura)
            {
                case "Stack":
                    if (indice >= Stack.Count())
                    {
                        // StackOverFlowException!
                        /*Errores.getInstance.agregar(new Error((int)Error.tipoError.EXCEPCION,
                            "StackOverFlowException, no se puede guardar.. se excedio tamaño del Stack!", puntero, 0));*/
                        indice = ambitos.buscarSalida();
                        ambitos.disminuirAmbito();
                        break;
                    }
                    if (indice == NULL)
                    {
                        MessageBox.Show("NullPointerException en el Stack\nPuntero: " + puntero);
                        return;
                    }
                    Stack[(int)indice] = valor;
                    break;
                case "Heap":
                    if (indice >= Heap.Count())
                    {
                        // StackOverFlowException!
                        /*Errores.getInstance.agregar(new Error((int)Error.tipoError.EXCEPCION,
                            "HeapOverFlowException, no se puede guardar.. se excedio tamaño del Heap!", puntero, 0));*/
                        indice = ambitos.buscarSalida();
                        ambitos.disminuirAmbito();
                        break;
                    }
                    if (indice == NULL)
                    {
                        MessageBox.Show("NullPointerException en el Heap\nPuntero: " + puntero);
                        return;
                    }
                    Heap[(int)indice] = valor;
                    break;
            }
        }

        // Get el puntero de una etiqueta
        public int getPuntero(String etiqueta)
        {
            int ptr;
            if(etiquetas.TryGetValue(etiqueta, out ptr))
            {
                return ptr;
            }
            return -1;
        }

        // Get el puntero de un metodo
        public int getPunteroMetodo(String metodo)
        {
            int ptr;
            if(metodos.TryGetValue(metodo, out ptr))
            {
                return ptr;
            }
            return -1;
        }

        // Evaluar condicion
        public void evaluarCondicional(double val1,String ope,double val2,String etiqueta)
        {
            bool condicion = false;
            switch (ope)
            {
                case "==":
                    condicion = (val1 == val2);
                    break;
                case "!=":
                    condicion = (val1 != val2);
                    break;
                case ">":
                    condicion = (val1 > val2);
                    break;
                case ">=":
                    condicion = (val1 >= val2);
                    break;
                case "<":
                    condicion = (val1 < val2);
                    break;
                default:
                    // <=
                    condicion = (val1 <= val2);
                    break; 
            }
            if (condicion)
            {
                puntero = getPuntero(etiqueta);
            }
        }
        #endregion
    }
}