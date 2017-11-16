using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace _Compi2_Proyecto2_201314863
{
    public class Auxiliar
    {
        public List<Atributo> declaraciones;
        private List<Atributo> globales;
        private LinkedList<List<string>> variables;
        public void llenarDeclaraciones(ParseTreeNode sentencias, List<Atributo> globales)
        {
            declaraciones = new List<Atributo>();
            variables = new LinkedList<List<string>>();
            // Guardar globales
            this.globales = globales;
            // Recorrer sentencias
            if(sentencias != null)
            {
                aumentarAmbito();
                foreach(ParseTreeNode sen in sentencias.ChildNodes)
                {
                    guardarDeclaraciones(sen);
                }
                decrementarAmbito();
            }
        }

        public void aumentarAmbito()
        {
            variables.AddLast(new List<string>());
        }

        public void decrementarAmbito()
        {
            variables.RemoveLast();
        }

        public void agregarVariable(Atributo var)
        {
            // Buscar en el listado de ambitos que no exista!
            foreach(List<string> lista in variables)
            {
                if (lista.Contains(var.nombre))
                {
                    Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
                        "La variable " + var.nombre + "ya existe en el metodo!", var.linea, 
                        var.columna));
                    return;
                }     
            }
            //if(globales != null)
            //{
            //    foreach(Atributo g in globales)
            //    {
            //        if(g.nombre == var.nombre)
            //        {
            //            Errores.getInstance.agregar(new Error((int)Error.tipoError.SEMANTICO,
            //            "La variable " + var.nombre + "ya existe y es global!", var.linea,
            //            var.columna));
            //            return;
            //        }
            //    }
            //}
            declaraciones.Add(var);
            variables.Last.Value.Add(var.nombre);
        }

        public void guardarDeclaraciones(ParseTreeNode sen)
        {
            switch (sen.Term.Name)
            {
                case "SENTENCIAS":
                    foreach(ParseTreeNode s in sen.ChildNodes)
                    {
                        guardarDeclaraciones(s);
                    }
                    break;
                case "DECLARACION":
                    evaluarDeclaracion(sen);
                    break;
                case "IF1":
                    // IF1 -> si + EXP + dosp + Eos + BLOQUE
                    aumentarAmbito();
                    guardarDeclaraciones(sen.ChildNodes[1]);
                    decrementarAmbito();
                    break;
                case "IF2":
                    // IF2 -> si + EXP + dosp + Eos + BLOQUE + sino + dosp + Eos + BLOQUE;
                    aumentarAmbito();
                    guardarDeclaraciones(sen.ChildNodes[1]);
                    decrementarAmbito();
                    aumentarAmbito();
                    guardarDeclaraciones(sen.ChildNodes[2]);
                    decrementarAmbito();
                    break;
                case "IF3":
                    // IF3 -> si + EXP + dosp + Eos + BLOQUE + IF4;
                    aumentarAmbito();
                    guardarDeclaraciones(sen.ChildNodes[1]);
                    decrementarAmbito();
                    guardarDeclaraciones(sen.ChildNodes[2]);
                    break;
                case "IF4":
                    // IF4 -> IF4 + sinosi + EXP + dosp + Eos + BLOQUE
                    //      | sinosi + EXP + dosp + Eos + BLOQUE;
                    if(sen.ChildNodes.Count == 3)
                    {
                        guardarDeclaraciones(sen.ChildNodes[0]);
                        aumentarAmbito();
                        guardarDeclaraciones(sen.ChildNodes[2]);
                        decrementarAmbito();
                    }
                    else
                    {
                        aumentarAmbito();
                        guardarDeclaraciones(sen.ChildNodes[1]);
                        decrementarAmbito();
                    }
                    break;
                case "IF5":
                    // IF5 -> si + EXP + dosp + Eos + BLOQUE + IF4 + sino + dosp + Eos + BLOQUE;
                    aumentarAmbito();
                    guardarDeclaraciones(sen.ChildNodes[1]);
                    decrementarAmbito();
                    // Evaluar If anidados
                    guardarDeclaraciones(sen.ChildNodes[2]);
                    aumentarAmbito();
                    guardarDeclaraciones(sen.ChildNodes[3]);
                    decrementarAmbito();
                    break;
                case "FOR":
                    // FOR -> para + acor + Fasignar + dosp + EXP + dosp + EXP + ccor + dosp + Eos + BLOQUE;
                    aumentarAmbito();
                    // Ver si es declaracion
                    guardarDeclaraciones(sen.ChildNodes[0]);
                    guardarDeclaraciones(sen.ChildNodes[3]);
                    decrementarAmbito();
                    break;
                case "X":
                    // X -> x + apar + EXP + coma + EXP + cpar + alla + LISTA_SENTENCIAS + clla;
                    aumentarAmbito();
                    guardarDeclaraciones(sen.ChildNodes[2]);
                    decrementarAmbito();
                    break;
                case "WHILE":
                    // WHILE -> mientras + EXP + dosp + Eos + BLOQUE;
                    aumentarAmbito();
                    guardarDeclaraciones(sen.ChildNodes[1]);
                    decrementarAmbito();
                    break;
                case "DO_WHILE":
                    // DO_WHILE -> hacer + dosp + Eos + BLOQUE + mientras + EXP + Eos;
                    aumentarAmbito();
                    guardarDeclaraciones(sen.ChildNodes[0]);
                    decrementarAmbito();
                    break;
                case "REPEAT":
                    // REPEAT -> repetir + dosp + Eos + BLOQUE + hasta + EXP + Eos;
                    aumentarAmbito();
                    guardarDeclaraciones(sen.ChildNodes[0]);
                    decrementarAmbito();
                    break;
            }
        }

        public void evaluarDeclaracion(ParseTreeNode declara)
        {
            /* OLC++
             * DECLARACION.Rule = TIPO + LISTA_IDS + asignarR
                             | TIPO + LISTA_IDS + DARREGLO -> INDICES (ARREGLO)?
                             | TIPO + LISTA_IDS;
             * Tree
             * DECLARACION.Rule = TIPO + LISTA_IDS + asignarR
                             | TIPO + LISTA_IDS
                             | TIPO + NARREGLO -> id + INDICES;
            */
            int tipo = Simbolo.getTipo(declara.ChildNodes[0].Token.Text);
            ParseTreeNode ids = declara.ChildNodes[1];
            if (ids.Term.Name.Equals("NARREGLO"))
            {
                Atributo variable = new Atributo(ids.ChildNodes[0].Token.Text, tipo,
                        ids.ChildNodes[0].Token.Location.Line, 
                        ids.ChildNodes[0].Token.Location.Column);
                variable.asignarArreglo(ids.ChildNodes[1]);
                if (tipo == (int)Simbolo.Tipo.CLASE)
                {
                    variable.asignarClase(declara.ChildNodes[0].Token.Text);
                }
                agregarVariable(variable);
            }
            else
            {
                foreach (ParseTreeNode var in ids.ChildNodes)
                {
                    Atributo a = new Atributo(var.Token.Text, tipo,
                        var.Token.Location.Line, var.Token.Location.Column);
                    if (tipo == (int)Simbolo.Tipo.CLASE)
                    {
                        a.asignarClase(declara.ChildNodes[0].Token.Text);
                    }
                    if (declara.ChildNodes.Count == 3)
                    {
                        if (declara.ChildNodes[2].Term.Name.Equals("DARREGLO"))
                        {
                            a.asignarArreglo(declara.ChildNodes[2].ChildNodes[0]);
                            if(declara.ChildNodes[2].ChildNodes.Count == 2)
                            {
                                a.asignarValor(declara.ChildNodes[2].ChildNodes[1]);
                            }
                        }
                        else
                        {
                            a.asignarValor(declara.ChildNodes[2]);
                        }
                    }
                    agregarVariable(a);
                }
            }
        }
    }
}
