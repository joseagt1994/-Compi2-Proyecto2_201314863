using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Parsing;

namespace _Compi2_Proyecto2_201314863
{
    public class AnalizadorTree : Grammar
    {
        public AnalizadorTree()
            : base(caseSensitive:false)
        {
            //Comentarios
            CommentTerminal COMENTARIO_SIMPLE = new CommentTerminal("comentario_simple", "##", "\n", "\r\n");
            CommentTerminal COMENTARIO_MULT = new CommentTerminal("comentario_mult", "{--", "--}");
            NonGrammarTerminals.Add(COMENTARIO_SIMPLE);
            NonGrammarTerminals.Add(COMENTARIO_MULT);

            //Palabras Reservadas
            MarkReservedWords("importar");
            MarkReservedWords("__constructor");
            MarkReservedWords("super");
            MarkReservedWords("self");
            MarkReservedWords("publico");
            MarkReservedWords("privado");
            MarkReservedWords("protegido");
            MarkReservedWords("/**Sobreescribir**/");
            MarkReservedWords("metodo");
            MarkReservedWords("funcion");
            MarkReservedWords("clase");
            MarkReservedWords("si");
            MarkReservedWords("si_no_si");
            MarkReservedWords("si_no");
            MarkReservedWords("elegir");
            MarkReservedWords("caso");
            MarkReservedWords("defecto");
            MarkReservedWords("para");
            MarkReservedWords("hacer");
            MarkReservedWords("mientras");
            MarkReservedWords("repetir");
            MarkReservedWords("hasta");
            MarkReservedWords("loop");
            MarkReservedWords("nuevo");
            MarkReservedWords("return");
            MarkReservedWords("salir");
            MarkReservedWords("continuar");
            MarkReservedWords("true");
            MarkReservedWords("false");
            MarkReservedWords("void");
            MarkReservedWords("entero");
            MarkReservedWords("decimal");
            MarkReservedWords("caracter");
            MarkReservedWords("cadena");
            MarkReservedWords("booleano");
            MarkReservedWords("out_String");
            MarkReservedWords("parseInt");
            MarkReservedWords("parseDouble");
            MarkReservedWords("intToStr");
            MarkReservedWords("doubleToStr");
            MarkReservedWords("doubleToInt");
            MarkReservedWords("imprimir");

            //Palabras reservadas
            var importar = ToTerm("importar");
            var constructor = ToTerm("__constructor");
            var super = ToTerm("super");
            var self = ToTerm("self");
            var publico = ToTerm("publico");
            var privado = ToTerm("privado");
            var protegido = ToTerm("protegido");
            var sobreescribir = ToTerm("/**Sobreescribir**/");
            var metodo = ToTerm("metodo");
            var funcion = ToTerm("funcion");
            var clase = ToTerm("clase");
            var si = ToTerm("si");
            var sinosi = ToTerm("si_no_si");
            var sino = ToTerm("si_no");
            var selector = ToTerm("elegir");
            var caso = ToTerm("caso");
            var defecto = ToTerm("defecto");
            var para = ToTerm("para");
            var hacer = ToTerm("hacer");
            var mientras = ToTerm("mientras");
            var repetir = ToTerm("repetir");
            var hasta = ToTerm("hasta");
            var loop = ToTerm("loop");
            var nuevo = ToTerm("nuevo");
            var retornar = ToTerm("return");
            var quiebre = ToTerm("salir");
            var continuar = ToTerm("continuar");
            var verdadero = ToTerm("true");
            var falso = ToTerm("false");
            var vacio = ToTerm("void");
            var num = ToTerm("entero");
            var dec = ToTerm("decimal");
            var caracter = ToTerm("caracter");
            var str = ToTerm("cadena");
            var boolean = ToTerm("booleano");
            var getNum = ToTerm("out_String");
            var getBool = ToTerm("parseInt");
            var outNum = ToTerm("parseDouble");
            var outStr = ToTerm("intToStr");
            var inStr = ToTerm("doubleToStr");
            var inNum = ToTerm("doubleToInt");
            var mostrar = ToTerm("imprimir");

            //Signos
            var asignar = ToTerm("=>");
            var apar = ToTerm("(");
            var cpar = ToTerm(")");
            var acor = ToTerm("[");
            var ccor = ToTerm("]");
            var coma = ToTerm(",");
            var punto = ToTerm(".");
            var dosp = ToTerm(":");

            //No Terminales
            var INICIO = new NonTerminal("INICIO");
            var IMPORTACIONES = new NonTerminal("IMPORTACIONES");
            var IMPORTAR = new NonTerminal("IMPORTAR");
            var LISTA_ARCHIVOS = new NonTerminal("LISTA_ARCHIVOS");
            var ARCHIVO = new NonTerminal("ARCHIVO");
            var CLASES = new NonTerminal("CLASES");
            var CLASE = new NonTerminal("CLASE");
            var BCUERPO = new NonTerminal("BCUERPO");
            var CUERPO = new NonTerminal("CUERPO");
            var METODO = new NonTerminal("METODO");
            var VISIBILIDAD = new NonTerminal("VISIBILIDAD");
            var DECLARACIONES = new NonTerminal("DECLARACIONES");
            var LISTA_IDS = new NonTerminal("IDS");
            var BLOQUE = new NonTerminal("BLOQUE");
            var LISTA_SENTENCIAS = new NonTerminal("SENTENCIAS");
            var SENTENCIA = new NonTerminal("SENTENCIA");
            var DECLARAR = new NonTerminal("DECLARAR");
            var asignarR = new NonTerminal("asignarR");
            var CONTROL = new NonTerminal("CONTROL");
            var IF1 = new NonTerminal("IF1");
            var IF2 = new NonTerminal("IF2");
            var IF3 = new NonTerminal("IF3");
            var IF4 = new NonTerminal("IF4");
            var IF5 = new NonTerminal("IF5");
            var SWITCH = new NonTerminal("SWITCH");
            var CASOS = new NonTerminal("CASOS");
            var BCASO = new NonTerminal("BCASO");
            var CASO = new NonTerminal("CASO");
            var ECASO = new NonTerminal("ECASO");
            var DEFECTO = new NonTerminal("DEFECTO");
            var WHILE = new NonTerminal("WHILE");
            var DO_WHILE = new NonTerminal("DO_WHILE");
            var FOR = new NonTerminal("FOR");
            var LOOP = new NonTerminal("LOOP");
            var REPEAT = new NonTerminal("REPEAT");
            var BANDERA = new NonTerminal("BANDERA");
            var EXP = new NonTerminal("EXP");
            var EXPS = new NonTerminal("EXPS");
            var ACCESO = new NonTerminal("ACCESO");
            var TACCESO = new NonTerminal("TACCESO");
            var TIPO = new NonTerminal("TIPO");
            var LLAMADA = new NonTerminal("LLAMADA");
            var TPROC = new NonTerminal("TPROC");
            var CUERPOS = new NonTerminal("CUERPOS");
            var VARIABLES = new NonTerminal("VARIABLES");
            var RETORNO = new NonTerminal("RETORNO");
            var INTERRUMPIR = new NonTerminal("INTERRUMPIR");
            var CONTINUAR = new NonTerminal("CONTINUAR");
            var LISTA_PARAMETROS = new NonTerminal("LISTA_PARAMETROS");
            var PARAMETRO = new NonTerminal("PARAMETRO");
            var PARAM = new NonTerminal("PARAM");
            var DECLARACION = new NonTerminal("DECLARACION");
            var ASIGNACION = new NonTerminal("ASIGNACION");
            var DIMENSION = new NonTerminal("DIMENSION");
            var NATIVAS = new NonTerminal("NATIVAS");
            var CADENA = new NonTerminal("CADENA");
            var IMPRIMIR = new NonTerminal("IMPRIMIR");
            var EXCEPTION = new NonTerminal("EXCEPTION");
            var Fasignar = new NonTerminal("Fasignar");
            var DIM = new NonTerminal("DIM");
            var DIM2 = new NonTerminal("DIM");
            var INDICES = new NonTerminal("INDICES");
            var AID = new NonTerminal("AID");
            var Tasignar = new NonTerminal("EXP");
            var CRECE = new NonTerminal("CRECE");
            var ARR = new NonTerminal("ARR");
            var A = new NonTerminal("A");
            var NARREGLO = new NonTerminal("NARREGLO");
            var DARREGLO = new NonTerminal("NARREGLO");
            var NUMEROS = new NonTerminal("INDICES");
            var NUEVO = new NonTerminal("NUEVO");
            var SUPER = new NonTerminal("SUPER");
            var ESTE = new NonTerminal("ESTE");

            //Terminales Expresiones Regulares
            RegexBasedTerminal archivo = new RegexBasedTerminal("archivo", "[a-zA-Z][0-9a-zA-Z]*.(tree|olc)");
            RegexBasedTerminal ruta = new RegexBasedTerminal("ruta", "C://([a-zA-Z][0-9a-zA-Z]*/)*[a-zA-Z][0-9a-zA-Z]*.(tree|olc)");
            RegexBasedTerminal url = new RegexBasedTerminal("url", "http://([a-zA-Z][0-9a-zA-Z]*/)*[a-zA-Z][0-9a-zA-Z]*.(tree|olc)");
            NumberLiteral numero = TerminalFactory.CreateCSharpNumber("numero");
            IdentifierTerminal id = TerminalFactory.CreateCSharpIdentifier("id");
            var tstring = new StringLiteral("cadena", "\"", StringOptions.AllowsDoubledQuote);
            var tchar = new StringLiteral("caracter", "'", StringOptions.AllowsDoubledQuote);

            //No Terminal Inicial
            this.Root = INICIO;

            //Producciones
            INICIO.Rule = IMPORTACIONES + CLASES
                | CLASES;

            IMPORTACIONES.Rule = MakeStarRule(IMPORTACIONES, IMPORTAR);

            IMPORTAR.Rule = importar + LISTA_ARCHIVOS + Eos;

            LISTA_ARCHIVOS.Rule = MakeStarRule(LISTA_ARCHIVOS, coma, ARCHIVO);

            ARCHIVO.Rule = ruta
                | archivo
                | url;

            CLASES.Rule = MakeStarRule(CLASES, CLASE);

            CLASE.Rule = clase + id + acor + id + ccor + dosp + Eos + BCUERPO
                       | clase + id + acor + ccor + dosp + Eos + BCUERPO;

            BCUERPO.Rule = Indent + CUERPOS + Dedent;

            CUERPOS.Rule = MakeStarRule(CUERPOS, CUERPO);

            CUERPO.Rule = VISIBILIDAD + DECLARACION + Eos
                        | METODO 
                        | constructor + acor + LISTA_PARAMETROS + ccor + dosp + Eos + BLOQUE
                        | DECLARACION + Eos
                        | ASIGNACION + Eos;

            METODO.Rule = sobreescribir + Eos + VISIBILIDAD + TPROC + id + acor + LISTA_PARAMETROS + ccor + dosp + Eos + BLOQUE
                        | VISIBILIDAD + TPROC + id + acor + LISTA_PARAMETROS + ccor + dosp + Eos + BLOQUE
                        | sobreescribir + Eos + TPROC + id + acor + LISTA_PARAMETROS + ccor + dosp + Eos + BLOQUE
                        | TPROC + id + acor + LISTA_PARAMETROS + ccor + dosp + Eos + BLOQUE;

            VISIBILIDAD.Rule = publico
                             | privado
                             | protegido;

            LISTA_PARAMETROS.Rule = MakeStarRule(LISTA_PARAMETROS, coma, PARAMETRO);

            PARAMETRO.Rule = TIPO + id + ARR;

            ARR.Rule = MakeStarRule(ARR, A);

            A.Rule = acor + ccor;

            DECLARACION.Rule = TIPO + LISTA_IDS + asignarR
                             | TIPO + LISTA_IDS
                             | TIPO + DARREGLO;

            DARREGLO.Rule = id + NUMEROS;

            NUMEROS.Rule = MakeStarRule(NUMEROS, DIM2);

            DIM2.Rule = acor + numero + ccor;

            asignarR.Rule = asignar + EXP;

            ASIGNACION.Rule = ACCESO + asignar + EXP;

            INDICES.Rule = MakeStarRule(INDICES, DIM);

            LISTA_IDS.Rule = LISTA_IDS + coma + id
                           | id;

            BLOQUE.Rule = Indent + LISTA_SENTENCIAS + Dedent;

            LISTA_SENTENCIAS.Rule = MakeStarRule(LISTA_SENTENCIAS, SENTENCIA);

            SENTENCIA.Rule = DECLARACION + Eos
                | self + punto + ASIGNACION + Eos
                | ASIGNACION + Eos
                | CONTROL
                | ACCESO + Eos
                | INTERRUMPIR + Eos
                | CONTINUAR + Eos
                | RETORNO + Eos
                | NATIVAS + Eos
                | IMPRIMIR + Eos
                | SUPER + Eos;
            
            SUPER.Rule = super + punto + ASIGNACION
                       | super + punto + ACCESO
                       | super + acor + EXPS + ccor;

            IMPRIMIR.Rule = mostrar + acor + EXP + ccor
                          | outStr + acor + EXP + ccor
                          | inStr + acor + EXP + coma + EXP + ccor
                          | outNum + acor + EXP + coma + EXP + ccor;

            NATIVAS.Rule = getNum + acor + EXP + ccor
                         | getBool + acor + EXP + coma + EXP + coma + EXP + ccor
                         | inNum + acor + EXP + coma + EXP + ccor;

            INTERRUMPIR.Rule = quiebre
                             | quiebre + id;

            CONTINUAR.Rule = continuar;

            RETORNO.Rule = retornar + EXP;

            LLAMADA.Rule = id + acor + EXPS + ccor;

            CONTROL.Rule = IF1
                         | IF2
                         | IF3
                         | IF5
                         | SWITCH
                         | WHILE
                         | DO_WHILE
                         | FOR
                         | LOOP
                         | REPEAT;

            IF1.Rule = si + EXP + dosp + Eos + BLOQUE;

            IF2.Rule = si + EXP + dosp + Eos + BLOQUE + sino + dosp + Eos + BLOQUE;

            IF3.Rule = si + EXP + dosp + Eos + BLOQUE + IF4;

            IF5.Rule = si + EXP + dosp + Eos + BLOQUE + IF4 + sino + dosp + Eos + BLOQUE;

            IF4.Rule = IF4 + sinosi + EXP + dosp + Eos + BLOQUE
                | sinosi + EXP + dosp + Eos + BLOQUE;

            SWITCH.Rule = selector + caso + EXP + dosp + Eos + BCASO;

            BCASO.Rule = Indent + CASOS + DEFECTO + Dedent
                | Indent + CASOS + Dedent;

            CASOS.Rule = MakeStarRule(CASOS, CASO);

            CASO.Rule = EXP + dosp + Eos + BLOQUE;
            
            DEFECTO.Rule = defecto + dosp + Eos + BLOQUE;

            WHILE.Rule = mientras + EXP + dosp + Eos + BLOQUE;

            DO_WHILE.Rule = hacer + dosp + Eos + BLOQUE + mientras + EXP + Eos;

            REPEAT.Rule = repetir + dosp + Eos + BLOQUE + hasta + EXP + Eos;

            Fasignar.Rule = DECLARACION
                         | ASIGNACION;

            FOR.Rule = para + acor + Fasignar + dosp + EXP + dosp + EXP + ccor + dosp + Eos + BLOQUE;

            LOOP.Rule = loop + dosp + Eos + BLOQUE;

            TPROC.Rule = funcion + TIPO + ARR
                       | metodo;

            TIPO.Rule = vacio
                      | num
                      | dec
                      | caracter
                      | str
                      | boolean
                      | id;

            NARREGLO.Rule = id + INDICES;

            EXP.Rule = EXP + EXP + ToTerm("or")
                | EXP + EXP + ToTerm("and")
                | EXP + EXP + ToTerm("xor")
                | EXP + EXP + ToTerm("==")
                | EXP + EXP + ToTerm("!=")
                | EXP + EXP + ToTerm(">")
                | EXP + EXP + ToTerm("<")
                | EXP + EXP + ToTerm(">=")
                | EXP + EXP + ToTerm("<=")
                | EXP + EXP + ToTerm("+")
                | EXP + EXP + ToTerm("-")
                | EXP + EXP + ToTerm("*")
                | EXP + EXP + ToTerm("/")
                | EXP + EXP + ToTerm("pow")
                | apar + EXP + cpar
                | acor + EXP + ccor
                | EXP + ToTerm("-")
                | EXP + ToTerm("not")
                | CRECE
                | NUEVO
                | LLAMADA
                | NATIVAS
                | super + punto + ACCESO
                | self + punto + ACCESO
                | ACCESO
                | numero
                | tstring
                | tchar
                | BANDERA;

            BANDERA.Rule = falso
                         | verdadero;

            NUEVO.Rule = nuevo + id + acor + EXPS + ccor;

            ACCESO.Rule = MakeStarRule(ACCESO, punto, TACCESO);

            TACCESO.Rule = LLAMADA
                         | id
                         | NARREGLO;

            DIM.Rule = acor + EXP + ccor;

            EXPS.Rule = MakeStarRule(EXPS, coma, EXP);

            CRECE.Rule = EXP + ToTerm("++")
                | EXP + ToTerm("--");

            //Definir Asociatividad
            RegisterOperators(1, Associativity.Left, "or");                       //OR,NOR
            RegisterOperators(2, Associativity.Left, "and");                        //AND,NAND
            RegisterOperators(3, Associativity.Left, "xor");                             //XOR
            RegisterOperators(4, Associativity.Right, "not");                             //NOT
            RegisterOperators(5, Associativity.Left, "==", "!=", ">", "<", ">=", "<="); //MAYORQUES,MENORQUES,IGUAL,DIFERENTE
            RegisterOperators(6, Associativity.Left, "+", "-");                         //MAS,MENOS
            RegisterOperators(7, Associativity.Left, "*", "/");                    //POR,DIVIDIR,MOD
            RegisterOperators(8, Associativity.Right, "-");                             //UNARIO
            RegisterOperators(9, Associativity.Left, "pow");                              //POTENCIA
            RegisterOperators(10, Associativity.Left, "++", "--");

            //Manejo de Errores Lexicos y Sintacticos
            SENTENCIA.ErrorRule = SyntaxError + SENTENCIA;
            CUERPO.ErrorRule = SyntaxError + CUERPO;
            CASOS.ErrorRule = SyntaxError + CASO;

            //Eliminacion de caracters, no terminales que son estorbos
            this.MarkPunctuation("(", ")", ":", "=>", ",", ".", "[", "]", "://", "/");
            this.MarkPunctuation("clase", "si", "si_no_si", "si_no", "elegir", "caso", "defecto", "hacer", "mientras", "loop", "repetir", "para", "return", "continuar", "salir");
            this.MarkTransient(AID, asignarR, IMPORTACIONES, IMPORTAR, ARCHIVO, TACCESO, SENTENCIA, DIM, DIM2, CONTROL, TIPO, DEFECTO);

        }

        public override void CreateTokenFilters(LanguageData language, TokenFilterList filters)
        {
            var outlineFilter = new CodeOutlineFilter(language.GrammarData,
                OutlineOptions.ProduceIndents | OutlineOptions.CheckBraces,
                ToTerm(@"\"));
            filters.Add(outlineFilter);
        }

    }
}
