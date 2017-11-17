using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi2_Proyecto2_201314863
{
    public class TablaSimbolos : List<Simbolo>
    {
        #region "TS"
        private static TablaSimbolos instance;

        public static void reiniciar()
        {
            instance = null;
        }

        private TablaSimbolos()
        {

        }

        public static TablaSimbolos getInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TablaSimbolos();
                }
                return instance;
            }
        }
        #endregion

        #region "Insertar"
        public void insertarLista(LinkedList<Simbolo> simbolos)
        {
            foreach(Simbolo simbolo in simbolos)
            {
                this.Add(simbolo);
            }
        }
        #endregion

        #region "Obtener simbolos"
        public Simbolo getClase(String nombre)
        {
            foreach(Simbolo simbolo in this)
            {
                if(simbolo.rol == (int)Simbolo.Tipo.CLASE && simbolo.nombre.Equals(nombre))
                {
                    return simbolo;
                }
            }
            return null;
        }

        public Simbolo getConstructor(String clase, String nombre)
        {
            foreach(Simbolo simbolo in this)
            {
                if(simbolo.rol == (int)Simbolo.Tipo.CONSTRUCTOR && simbolo.padre.Equals(clase)
                    || simbolo.nombre.Equals(nombre))
                {
                    return simbolo;
                }
            }
            return null;
        }

        public Simbolo getProcedimiento(String clase, String nombre)
        {
            foreach (Simbolo simbolo in this)
            {
                if ((simbolo.rol == (int)Simbolo.Tipo.METODO || simbolo.rol == (int)Simbolo.Tipo.FUNCION) 
                    && (simbolo.padre.Equals(clase) && simbolo.nombre.Equals(nombre)))
                {
                    return simbolo;
                }
            }
            return null;
        }

        public List<Simbolo> getParametros(String clase, Simbolo procedimiento)
        {
            List<Simbolo> parametros = new List<Simbolo>();
            int pos = getPosicionPadre(clase, procedimiento.nombre);
            bool salir = false;
            for (int i = pos + 1; i < this.Count; i++)
            {
                if (this[i].rol == (int)Simbolo.Tipo.PARAMETRO)
                {
                    salir = true;
                    if (this[i].padre.Equals(procedimiento.nombre))
                    {
                        parametros.Add(this[i]);
                    }
                }
                else
                {
                    if (salir)
                    {
                        break;
                    }
                }
            }
            return parametros;
        }

        public Simbolo buscarVariable(String nombre, String clase, String procedimiento, Acceso.Tipo tipo)
        {
            if(tipo == Acceso.Tipo.NINGUNO)
            {
                return getVariable(nombre, clase, procedimiento);
            }
            else if(tipo == Acceso.Tipo.ESTE)
            {
                return getVariableGlobal(nombre, clase);
            }
            else
            {
                return getVariableHeredada(nombre, clase);
            }
        }

        public Simbolo getVariable(String nombre, String clase, String procedimiento)
        {
            if(procedimiento != null)
            {
                int pos = getPosicionPadre(clase, procedimiento);
                // Ver si es Local (Variable,Parametro)
                for(int i = pos+1; i < this.Count; i++)
                {
                    if(this[i].rol == (int)Simbolo.Tipo.VARIABLE ||
                       this[i].rol == (int)Simbolo.Tipo.PARAMETRO || this[i].rol == (int)Simbolo.Tipo.RETORNO)
                    {
                        if (this[i].nombre.Equals(nombre))
                        {
                            return this[i];
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            // Global
            Simbolo simbolo = getVariableGlobal(nombre, clase);
            // Heredada
            if(simbolo != null)
            {
                return simbolo;
            }
            return getVariableHeredada(nombre, clase);
        }

        public Simbolo getVariableGlobal(String nombre, String clase)
        {
            foreach (Simbolo simbolo in this)
            {
                if (simbolo.rol == (int)Simbolo.Tipo.VARIABLE && simbolo.padre.Equals(clase)
                    && simbolo.ambito == (int)Simbolo.Tipo.GLOBAL && simbolo.nombre.Equals(nombre))
                {
                    return simbolo;
                }
            }
            return getVariableHeredada(nombre, clase);
        }

        public Simbolo getVariableHeredada(String nombre, String clase)
        {
            Simbolo padre = getClase(clase);
            if (padre.padre != null)
            {
                foreach (Simbolo simbolo in this)
                {
                    if (simbolo.rol == (int)Simbolo.Tipo.VARIABLE && simbolo.padre == padre.padre
                        && simbolo.ambito == (int)Simbolo.Tipo.GLOBAL && simbolo.nombre.Equals(nombre))
                    {
                        if(simbolo.visibilidad == (int)Simbolo.Visibilidad.PUBLICO ||
                            simbolo.visibilidad == (int)Simbolo.Visibilidad.PROTEGIDO)
                        {
                            return simbolo;
                        }
                        else
                        {
                            // Error semantico!
                        }
                        return null;
                    }
                }
            }
            return null;
        }

        public int getPosicionPadre(String clase, String procedimiento)
        {
            int contar = 0;
            foreach (Simbolo simbolo in this)
            {
                if (simbolo.rol == (int)Simbolo.Tipo.FUNCION || simbolo.rol == (int)Simbolo.Tipo.METODO ||
                    simbolo.rol == (int)Simbolo.Tipo.CONSTRUCTOR)
                {
                    if(simbolo.padre != null && simbolo.padre.Equals(clase) && simbolo.nombre.Equals(procedimiento))
                    {
                        return contar;
                    }
                }
                contar++;
            }
            return -1;
        }
        #endregion
    }
}
