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
            : base(caseSensitive: false)
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
            var asignar = ToTerm("=");
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
            var CLASES = new NonTerminal("CLASES");
            var CLASE = new NonTerminal("CLASE");
            var CUERPO = new NonTerminal("CUERPO");
            var TCUERPO = new NonTerminal("TCUERPO");
            var CMETODO = new NonTerminal("CMETODO");
            var PRINCIPAL = new NonTerminal("PRINCIPAL");
            var CONSTRUCTOR = new NonTerminal("CONSTRUCTOR");
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
            var ARREGLO = new NonTerminal("ARREGLO");
            var TARREGLO = new NonTerminal("TARREGLO");
            var NARREGLO = new NonTerminal("NARREGLO");
            var DARREGLO = new NonTerminal("DARREGLO");
            var X = new NonTerminal("X");
            var NUEVO = new NonTerminal("NUEVO");
            var NUMEROS = new NonTerminal("INDICES");

            //Terminales Expresiones Regulares
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

            LLAMAR.Rule = importar + apar + tstring + cpar + fin
                | llamar + apar + tstring + cpar + fin;

            CLASES.Rule = MakeStarRule(CLASES, CLASE);

            CLASE.Rule = clase + id + hereda + id + alla + CUERPOS + clla
                       | clase + id + alla + CUERPOS + clla;

            CUERPOS.Rule = MakeStarRule(CUERPOS, CUERPO);

            CUERPO.Rule = VISIBILIDAD + TPROC + id + TCUERPO
                        | TPROC + id + TCUERPO
                        | sobreescribir + METODO
                        | PRINCIPAL
                        | VISIBILIDAD + CONSTRUCTOR
                        | CONSTRUCTOR
                        | ASIGNACION + fin;

            CONSTRUCTOR.Rule = id + CMETODO;

            PRINCIPAL.Rule = main + apar + cpar + alla + LISTA_SENTENCIAS + clla;

            TCUERPO.Rule = asignarR + fin
                         | DARREGLO + fin
                         | fin
                         | CMETODO;

            METODO.Rule = VISIBILIDAD + TPROC + id + CMETODO
                        | TPROC + id + CMETODO;

            CMETODO.Rule = apar + LISTA_PARAMETROS + cpar + alla + LISTA_SENTENCIAS + clla;

            VISIBILIDAD.Rule = publico
                             | privado
                             | protegido;

            LISTA_PARAMETROS.Rule = MakeStarRule(LISTA_PARAMETROS, coma, PARAMETRO);

            PARAMETRO.Rule = TIPO + id + ARR;

            ARR.Rule = MakeStarRule(ARR, A);

            A.Rule = acor + ccor;

            DECLARACION.Rule = TIPO + LISTA_IDS + asignarR
                             | TIPO + LISTA_IDS + DARREGLO
                             | TIPO + LISTA_IDS;

            DARREGLO.Rule = NUMEROS + asignar + alla + ARREGLO + clla
                   | NUMEROS;

            NUMEROS.Rule = MakeStarRule(NUMEROS, DIM2);

            DIM2.Rule = acor + numero + ccor;

            LISTA_IDS.Rule = LISTA_IDS + coma + id
                | id;

            asignarR.Rule = asignar + EXP;

            ASIGNACION.Rule = self + punto + ACCESO + asignar + EXP
                | ACCESO + asignar + EXP;

            INDICES.Rule = MakeStarRule(INDICES, DIM);

            LISTA_SENTENCIAS.Rule = MakeStarRule(LISTA_SENTENCIAS, SENTENCIA);

            SENTENCIA.Rule = DECLARACION + fin
                | ASIGNACION + fin
                | CONTROL
                | LLAMADA + fin
                | RETORNO + fin
                | IMPRIMIR + fin;

            IMPRIMIR.Rule = mostrar + apar + EXP + cpar;

            RETORNO.Rule = retornar + EXP;

            LLAMADA.Rule = self + punto + id + apar + EXPS + cpar
                | id + apar + EXPS + cpar;

            CONTROL.Rule = IF1
                         | IF2
                         | IF3
                         | IF5
                         | WHILE
                         | DO_WHILE
                         | FOR
                         | REPEAT
                         | X;

            X.Rule = x + apar + EXP + coma + EXP + cpar + alla + LISTA_SENTENCIAS + clla;

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

            TPROC.Rule = TIPO + ARR
                | TIPO;

            TIPO.Rule = vacio
                      | num
                      | dec
                      | caracter
                      | str
                      | boolean
                      | id;

            NARREGLO.Rule = id + INDICES;

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
                | NUEVO
                | self + punto + ACCESO
                | ACCESO
                | numero
                | tstring
                | tchar
                | BANDERA;

            NUEVO.Rule = nuevo + id + apar + EXPS + cpar;

            ARREGLO.Rule = MakeStarRule(ARREGLO, coma, TARREGLO);

            TARREGLO.Rule = alla + ARREGLO + clla
                | EXP;

            BANDERA.Rule = falso
                         | verdadero;

            ACCESO.Rule = MakeStarRule(ACCESO, punto, TACCESO);

            TACCESO.Rule = LLAMADA
                         | id
                         | NARREGLO;

            DIM.Rule = acor + EXP + ccor;

            EXPS.Rule = MakeStarRule(EXPS, coma, EXP);

            CRECE.Rule = EXP + ToTerm("++")
                | EXP + ToTerm("--");

            //Definir Asociatividad
            RegisterOperators(1, Associativity.Left, "||");                       //OR,NOR
            RegisterOperators(2, Associativity.Left, "&&");                        //AND,NAND
            RegisterOperators(3, Associativity.Left, "??");                             //XOR
            RegisterOperators(4, Associativity.Right, "!");                             //NOT
            RegisterOperators(5, Associativity.Left, "==", "!=", ">", "<", ">=", "<="); //MAYORQUES,MENORQUES,IGUAL,DIFERENTE
            RegisterOperators(6, Associativity.Left, "+", "-");                         //MAS,MENOS
            RegisterOperators(7, Associativity.Left, "*", "/");                    //POR,DIVIDIR,MOD
            RegisterOperators(8, Associativity.Right, "-");                             //UNARIO
            RegisterOperators(9, Associativity.Left, "^");                              //POTENCIA
            RegisterOperators(10, Associativity.Left, "++", "--");

            //Manejo de Errores Lexicos y Sintacticos
            SENTENCIA.ErrorRule = SyntaxError + SENTENCIA;
            CUERPO.ErrorRule = SyntaxError + CUERPO;
            CASOS.ErrorRule = SyntaxError + CASO;

            //Eliminacion de caracters, no terminales que son estorbos
            this.MarkPunctuation("(", ")", ":", "=", ",", ".", "[", "]", "{", "}", ";");
            this.MarkPunctuation("clase", "hereda_de", "principal", "si", "sino", "hacer", "mientras", "repetir", "para", "retornar");
            this.MarkTransient(AID, TACCESO, IMPORTACIONES, SENTENCIA, asignarR, DIM, DIM2, CONTROL, TIPO, TCUERPO, TARREGLO);

        }
    }
}
