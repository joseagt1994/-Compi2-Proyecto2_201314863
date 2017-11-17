using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi2_Proyecto2_201314863
{
    class Convertidor
    {
        // Nativas
        public static Nodo generarC3D(String id, Nodo expresion)
        {
            switch (id.ToUpper())
            {
                case "PARSEINT":
                    if(expresion.tipo != (int)Simbolo.Tipo.CADENA)
                    {
                        // Error semantico!
                        return null;
                    }
                    return generarNativaC3D(expresion, "$$_parseInt", (int)Simbolo.Tipo.NUMERO);
                case "PARSEDOUBLE":
                    if (expresion.tipo != (int)Simbolo.Tipo.CADENA)
                    {
                        // Error semantico!
                        return null;
                    }
                    return generarNativaC3D(expresion, "$$_parseDouble", (int)Simbolo.Tipo.DECIMAL);
                case "INTTOSTR":
                    if (expresion.tipo != (int)Simbolo.Tipo.NUMERO)
                    {
                        // Error semantico!
                        return null;
                    }
                    return generarNativaC3D(expresion, "$$_intToStr", (int)Simbolo.Tipo.CADENA);
                case "DOUBLETOSTR":
                    if (expresion.tipo != (int)Simbolo.Tipo.DECIMAL)
                    {
                        // Error semantico!
                        return null;
                    }
                    return generarNativaC3D(expresion, "$$_doubleToStr", (int)Simbolo.Tipo.CADENA);
                default:
                    // doubleToInt
                    if (expresion.tipo != (int)Simbolo.Tipo.DECIMAL)
                    {
                        // Error semantico!
                        return null;
                    }
                    return generarNativaC3D(expresion, "$$_doubleToInt", (int)Simbolo.Tipo.NUMERO);
            }
        }

        public static Nodo generarNativaC3D(Nodo expresion, String nombre, int tipo)
        {
            // Parametros
            // cadena valor : P + 1
            // retorno entero : P + 0
            String tam = Convert.ToString(C3DSentencias.procedimientoActual.tam);
            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Guardar la cadena"));
            String t1 = GeneradorC3D.getTemporal();
            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, t1, "P", "+", 
                tam));
            String t2 = GeneradorC3D.getTemporal();
            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, t2, t1, "+", "1"));
            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.VALOR, "Stack", t1, expresion.cadena));
            // Aumentar ambito
            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, "P", "P", "+", tam));
            // Llamar a la funcion nativa
            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.NATIVA, nombre));
            // Recuperar el retorno
            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.COMENTARIO, "// Recuperar el retorno"));
            String t3 = GeneradorC3D.getTemporal();
            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, t3, "P", "+", "0"));
            Nodo nodo = new Nodo();
            nodo.cadena = GeneradorC3D.getTemporal();
            nodo.tipo = tipo;
            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ACCESO, "Stack", nodo.cadena, t3));
            // Disminuir ambito
            GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION, "P", "P", "-", tam));
            return nodo;
        }
        
    }
}
