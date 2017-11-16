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
        bool error = false;

        // Atributos a utilizar!
        int ambito;
        bool esHeap, esStack;
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
                    metodos.Add(instruccion.cadena, i);
                }
            }
        }
        #endregion

        #region "Ejecucion"
        //Llenar estructuras
        public void llenar()
        {
            for(int i = 0; i < 100000; i++)
            {
                Stack[i] = NULL;
                Heap[i] = NULL;
            }
        }

        // Metodo inicial
        public void ejecutar(List<C3D> instrucciones)
        {
            puntero = 0;
            Stack = new double[100000];
            Heap = new double[100000];
            llenar();
            ambitos = new DisplayAmbitos();
            etiquetas = new Dictionary<string, int>();
            metodos = new Dictionary<string, int>();
            this.instrucciones = instrucciones;
            // Guardar las etiquetas y sus punteros
            guardarEtiquetasMetodos();
            // Empezar ejecucion
            esHeap = false;
            esStack = false;
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
                /*case (int)C3D.TipoC3D.EXIT:
                    // EXIT -> "EXIT " + cadena; //VER QUE SE GENERA!!!!!??????
                    int ex = (int)Stack[P + 1];
                    switch (ex)
                    {
                        case 0:
                            puntero = -1;
                            break;
                        case 102:
                            Errores.getInstance.agregar(new Error((int)Error.tipoError.EXCEPCION,
                            "NullPointerException, objeto es null!", puntero, 0));
                            puntero = ambitos.buscarSalida();
                            ambitos.disminuirAmbito();
                            break;
                        case 243:
                            Errores.getInstance.agregar(new Error((int)Error.tipoError.EXCEPCION,
                            "MissingReturnException, falta retorno en " + ambitos.ElementAt(0).nombre + "!", puntero, 0));
                            puntero = ambitos.buscarSalida();
                            ambitos.disminuirAmbito();
                            break;
                        case 396:
                            Errores.getInstance.agregar(new Error((int)Error.tipoError.EXCEPCION,
                            "ArithmeticException, no se puede operar!", puntero, 0));
                            puntero = ambitos.buscarSalida();
                            ambitos.disminuirAmbito();
                            break;
                        case 624:
                            Errores.getInstance.agregar(new Error((int)Error.tipoError.EXCEPCION,
                            "StackOverFlowException, no se puede guardar.. se excedio tamaño del Stack!", puntero, 0));
                            puntero = ambitos.buscarSalida();
                            ambitos.disminuirAmbito();
                            break;
                        case 789:
                            Errores.getInstance.agregar(new Error((int)Error.tipoError.EXCEPCION,
                            "HeapOverFlowException, no se puede guardar.. se excedio tamaño del Heap!", puntero, 0));
                            puntero = ambitos.buscarSalida();
                            ambitos.disminuirAmbito();
                            break;
                        case 801:
                            Errores.getInstance.agregar(new Error((int)Error.tipoError.EXCEPCION,
                            "PoolOverFlowException, no se puede guardar.. se excedio tamaño del Pool!", puntero, 0));
                            puntero = ambitos.buscarSalida();
                            ambitos.disminuirAmbito();
                            break;
                    }
                    break;*/
                case (int)C3D.TipoC3D.NATIVA:
                    if (mostrarMensaje)
                    {
                        // inNum, inStr
                        recibirParametro(instruccion);
                    }
                    else
                    {
                        ejecutarNativa(instruccion);
                    }
                    break;
                default:
                    break;
            }
        }

        public void ejecutarNativa(C3D instruccion)
        {
            /*switch (instruccion.cadena)
            {
                case "$$_inStr":
                    // 0 -> retorno, no se usa
                    // 1 -> referencia a la posicion de la variable en Stack
                    // 2 -> referencia de donde leer la cadena
                    int refVar = (int)Stack[P + 1];
                    int refCadena = (int)Stack[P + 2];
                    cadenaMostrar = devolverCadena(refCadena);
                    puntero = puntero - 1;
                    mostrarMensaje = true;
                    break;
                case "$$_inNum":
                    // 0 -> retorno, si se usa
                    // 1 -> referencia a la cadena a mostrar
                    // 2 -> num por defecto
                    int ptr = (int)Stack[P + 1];
                    cadenaMostrar = devolverCadena(ptr);
                    puntero = puntero - 1;
                    mostrarMensaje = true;
                    break;
                case "$$_getNum":
                    // 0 -> retorno, si se usa
                    // 1 -> referencia a la base
                    // 2 -> referencia a la cadena a convertir
                    // 3 -> num por defecto
                    InterpreteInNum interpretar = new InterpreteInNum();
                    int ptrN = (int)Stack[P + 2];
                    String cadena = devolverCadena(ptrN);
                    double valor = interpretar.analizar(cadena);
                    if(valor == -1)
                    {
                        Stack[P + 0] = Stack[P + 3];
                    }
                    else
                    {
                        Stack[P + 0] = valor;
                    }
                    break;
                case "$$_getBool":
                    // 0 -> retorno, bool
                    // 1 -> referencia a la cadena 
                    int ptrCad = (int)Stack[P + 1];
                    String cad = devolverCadena(ptrCad);
                    if (cad.Equals("true"))
                    {
                        Stack[P + 0] = 1;
                    }
                    else
                    {
                        Stack[P + 0] = 0;
                    }
                    break;
                case "$$_outStr":
                    // 0 -> retorno, no se usa
                    // 1 -> referencia a la cadena a mostrar
                    int ptroCad = (int)Stack[P + 1];
                    cadenaMostrar = devolverCadena(ptroCad);
                    imprimir = true;
                    break;
                case "$$_outNum":
                    // 0 -> retorno, no se usa
                    // 1 -> numero
                    // 2 -> booleano
                    double td = Stack[P + 1];
                    int ptrB = (int)Stack[P + 2];
                    if(ptrB == 1)
                    {
                        int V = (int)td;
                        cadenaMostrar = V.ToString();
                    }
                    else
                    {
                        cadenaMostrar = td.ToString();
                    }
                    imprimir = true;
                    break;
                case "$$_show":
                    // 0 -> retorno, no se usa
                    // 1 -> referencia a la cadena
                    int ptrC = (int)Stack[P + 1];
                    cadenaMostrar = devolverCadena(ptrC);
                    imprimir = true;
                    break;
                case "$$_getRandom":
                    // 0 -> retorno
                    Random rnd = new Random();
                    Stack[P + 0] = rnd.NextDouble();
                    break;
                case "$$_getStrLength":
                    // 0 -> retorno
                    // 1 -> referencia a la cadena
                    int ptrCc = (int)Stack[P + 1];
                    String cdn = devolverCadena(ptrCc);
                    Stack[P + 0] = cdn.Length;
                    break;
            }*/
        }

        public void recibirParametro(C3D instruccion)
        {
            /*switch (instruccion.cadena)
            {
                case "$$_inStr":
                    // 0 -> retorno, no se usa
                    // 1 -> referencia a la posicion de la variable en Stack
                    // 2 -> referencia de donde leer la cadena
                    int rV = (int)Stack[P + 1];
                    Stack[rV] = H;
                    H = H + 1;
                    foreach (char c in Debugger.guardada)
                    {
                        double v = (int)c;
                        Pool[S] = v;
                        S = S + 1;
                    }
                    Pool[S] = 0;
                    S = S + 1;
                    break;
                case "$$_inNum":
                    // 0 -> retorno, si se usa
                    // 1 -> referencia a la cadena a mostrar
                    // 2 -> num por defecto
                    InterpreteInNum interpretarn = new InterpreteInNum();
                    double val = interpretarn.analizar(Debugger.guardada);
                    if(val == -1)
                    {
                        Stack[P + 0] = Stack[P + 2];
                    }
                    else
                    {
                        Stack[P + 0] = val;
                    }
                    break;
            }
            mostrarMensaje = false;*/
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