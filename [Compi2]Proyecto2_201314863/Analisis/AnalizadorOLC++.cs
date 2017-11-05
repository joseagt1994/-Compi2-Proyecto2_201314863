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
    public class AnalizadorOLC : Grammar
    {
        public AnalizadorOLC()
            : base(caseSensitive:false)
        {
            //Comentarios
            CommentTerminal COMENTARIO_SIMPLE = new CommentTerminal("comentario_simple", "//", "\n", "\r\n");
            CommentTerminal COMENTARIO_MULT = new CommentTerminal("comentario_mult", "/-", "-/");
            NonGrammarTerminals.Add(COMENTARIO_SIMPLE);
            NonGrammarTerminals.Add(COMENTARIO_MULT);

            //Palabras Reservadas
            MarkReservedWords("importar");
            MarkReservedWords("llamar");
            MarkReservedWords("hereda_de");
            MarkReservedWords("este");
            MarkReservedWords("principal");
            MarkReservedWords("publico");
            MarkReservedWords("privado");
            MarkReservedWords("protegido");
            MarkReservedWords("@Sobreescribir");
            MarkReservedWords("clase");
            MarkReservedWords("si");
            MarkReservedWords("sino");
            MarkReservedWords("para");
            MarkReservedWords("hacer");
            MarkReservedWords("mientras");
            MarkReservedWords("x");
            MarkReservedWords("repetir");
            MarkReservedWords("until");
            MarkReservedWords("new");
            MarkReservedWords("retornar");
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
            var llamar = ToTerm("llamar");
            var hereda = ToTerm("hereda_de");
            var self = ToTerm("este");
            var main = ToTerm("principal");
            var publico = ToTerm("publico");
            var privado = ToTerm("privado");
            var protegido = ToTerm("protegido");
            var sobreescribir = ToTerm("@Sobreescribir");
            var clase = ToTerm("clase");
            var si = ToTerm("si");
            var sino = ToTerm("sino");
            var para = ToTerm("para");
            var hacer = ToTerm("hacer");
            var mientras = ToTerm("mientras");
            var x = ToTerm("x");
            var repetir = ToTerm("repetir");
            var hasta = ToTerm("until");
            var nuevo = ToTerm("new");
            var retornar = ToTerm("retornar");
            var verdadero = ToTerm("true");
            var falso = ToTerm("false");
            var vacio = ToTerm("void");
            var num = ToTerm("entero");
            var dec = ToTerm("decimal");
            var caracter = ToTerm("caracter");
            var str = ToTerm("cadena");
            var boolean = ToTerm("booleano");
            var mostrar = ToTerm("imprimir");

            //Signos
            var asignar = ToTerm("=>");
            var alla = ToTerm("{");
            var clla = ToTerm("}");
            var apar = ToTerm("(");
            var cpar = ToTerm(")");
            var acor = ToTerm("[");
            var ccor = ToTerm("]");
            var coma = ToTerm(",");
            var punto = ToTerm(".");
            var fin = ToTerm(";");

            //No Terminales
            var INICIO = new NonTerminal("INICIO");
            var IMPORTACIONES = new NonTerminal("IMPORTACIONES");
            var LLAMAR = new NonTerminal("LLAMAR");
            var LISTA_ARCHIVOS = new NonTerminal("LISTA_ARCHIVOS");
            var ARCHIVO = new NonTerminal("ARCHIVO");
            var CLASES = new NonTerminal("CLASES");
            var CLASE = new NonTerminal("CLASE");
            var CUERPO = new NonTerminal("CUERPO");
            var METODO = new NonTerminal("METODO");
            var VISIBILIDAD = new NonTerminal("VISIBILIDAD");
            var DECLARACIONES = new NonTerminal("DECLARACIONES");
            var LISTA_IDS = new NonTerminal("IDS");
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
            var LEXPS = new NonTerminal("LEXPS");
            var ACCESO = new NonTerminal("ACCESO");
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
            var INDICES = new NonTerminal("INDICES");
            var AID = new NonTerminal("AID");
            var Tasignar = new NonTerminal("EXP");
            var CRECE = new NonTerminal("EXP");
            var ARR = new NonTerminal("ARR");
            var A = new NonTerminal("A");

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

            IMPORTACIONES.Rule = MakeStarRule(IMPORTACIONES, LLAMAR);

            LLAMAR.Rule = importar + apar + str + cpar + fin
                | llamar + apar + str + cpar + fin;

            CLASES.Rule = MakeStarRule(CLASES, CLASE);

            CLASE.Rule = clase + id + hereda + id + alla + CUERPOS + clla
                       | clase + id + alla + CUERPOS + clla;

            CUERPOS.Rule = MakeStarRule(CUERPOS, CUERPO);

            CUERPO.Rule = VISIBILIDAD + DECLARACION + fin
                        | METODO
                        | id + apar + LISTA_PARAMETROS + cpar + alla + LISTA_SENTENCIAS + clla
                        | DECLARACION + fin
                        | ASIGNACION + fin;

            METODO.Rule = sobreescribir + VISIBILIDAD + TPROC + id + apar + LISTA_PARAMETROS + cpar + alla + LISTA_SENTENCIAS + clla
                        | VISIBILIDAD + TPROC + id + apar + LISTA_PARAMETROS + cpar + alla + LISTA_SENTENCIAS + clla
                        | sobreescribir + TPROC + id + apar + LISTA_PARAMETROS + cpar + alla + LISTA_SENTENCIAS + clla
                        | TPROC + id + apar + LISTA_PARAMETROS + cpar + alla + LISTA_SENTENCIAS + clla;

            VISIBILIDAD.Rule = publico
                             | privado
                             | protegido;

            LISTA_PARAMETROS.Rule = MakeStarRule(LISTA_PARAMETROS, coma, PARAMETRO);

            PARAMETRO.Rule = TIPO + id + ARR;

            ARR.Rule = MakeStarRule(ARR, A);

            A.Rule = acor + ccor;

            DECLARACION.Rule = TIPO + LISTA_IDS + asignarR
                             | TIPO + LISTA_IDS
                             | TIPO + id + INDICES;

            asignarR.Rule = asignar + EXP;

            ASIGNACION.Rule = Tasignar + asignar + EXP;

            Tasignar.Rule = ACCESO
                         | id + INDICES;

            INDICES.Rule = MakeStarRule(INDICES, DIM);

            LISTA_IDS.Rule = MakeStarRule(LISTA_IDS, coma, id);

            LISTA_SENTENCIAS.Rule = MakeStarRule(LISTA_SENTENCIAS, SENTENCIA);

            SENTENCIA.Rule = DECLARACION + fin
                | ASIGNACION + fin
                | CONTROL
                | LLAMADA + fin
                | RETORNO + fin
                | IMPRIMIR + fin;

            IMPRIMIR.Rule = mostrar + apar + EXP + cpar;

            RETORNO.Rule = retornar + EXP;

            LLAMADA.Rule = self + punto + id + acor + LEXPS
                | id + acor + LEXPS;

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

            IF1.Rule = si + EXP + alla + LISTA_SENTENCIAS + clla;

            IF2.Rule = si + EXP + alla + LISTA_SENTENCIAS + clla + sino + alla + LISTA_SENTENCIAS + clla;

            IF3.Rule = si + EXP + alla + LISTA_SENTENCIAS + clla + IF4;

            IF5.Rule = si + EXP + alla + LISTA_SENTENCIAS + clla + IF4 + sino + alla + LISTA_SENTENCIAS + clla;

            IF4.Rule = IF4 + sino + si + EXP + alla + LISTA_SENTENCIAS + clla
                | sino + si + EXP + alla + LISTA_SENTENCIAS + clla;

            WHILE.Rule = mientras + EXP + alla + LISTA_SENTENCIAS + clla;

            DO_WHILE.Rule = hacer + alla + LISTA_SENTENCIAS + clla + mientras + EXP + fin;

            REPEAT.Rule = repetir + alla + LISTA_SENTENCIAS + clla + hasta + EXP + fin;

            Fasignar.Rule = DECLARACION
                         | ASIGNACION;

            FOR.Rule = para + apar + Fasignar + fin + EXP + fin + EXP + cpar + alla + LISTA_SENTENCIAS + clla;

            TPROC.Rule = TIPO + ARR;

            TIPO.Rule = vacio
                      | num
                      | dec
                      | caracter
                      | str
                      | boolean
                      | id;

            EXP.Rule = EXP + ToTerm("||") + EXP
                | EXP + ToTerm("&&") + EXP
                | EXP + ToTerm("??") + EXP
                | EXP + ToTerm("==") + EXP
                | EXP + ToTerm("!=") + EXP
                | EXP + ToTerm(">") + EXP
                | EXP + ToTerm("<") + EXP
                | EXP + ToTerm(">=") + EXP
                | EXP + ToTerm("<=") + EXP
                | EXP + ToTerm("+") + EXP
                | EXP + ToTerm("-") + EXP
                | EXP + ToTerm("*") + EXP
                | EXP + ToTerm("/") + EXP
                | EXP + ToTerm("^") + EXP
                | apar + EXP + cpar
                | ToTerm("-") + EXP
                | ToTerm("!") + EXP
                | CRECE
                | nuevo + acor + LEXPS
                | LLAMADA
                | NATIVAS
                | self + punto + ACCESO
                | self + punto + id + INDICES
                | ACCESO
                | id + INDICES
                | numero
                | tstring
                | tchar
                | BANDERA;

            BANDERA.Rule = falso
                         | verdadero;

            ACCESO.Rule = MakeStarRule(ACCESO, punto, id);

            DIM.Rule = acor + numero + ccor;

            LEXPS.Rule = ccor
                       | EXPS + ccor;

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
            this.MarkPunctuation("clase", "si", "sino", "hacer", "mientras", "repetir", "para", "retornar");
            this.MarkTransient(AID, IMPORTACIONES, LLAMAR, ARCHIVO, SENTENCIA, DIM, CONTROL, Fasignar, TIPO);

        }
    }
}
