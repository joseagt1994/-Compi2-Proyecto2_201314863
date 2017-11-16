using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi2_Proyecto2_201314863
{
    public class Imprimir
    {
        public static void imprimirC3D(Nodo texp)
        {
            if(texp.tipo == (int)Simbolo.Tipo.NUMERO)
            {
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.IMPRIMIR,
                    "%d", texp.cadena));
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.IMPRIMIR,
                    "%c", "13"));
            }
            else if(texp.tipo == (int)Simbolo.Tipo.DECIMAL)
            {
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.IMPRIMIR,
                    "%f", texp.cadena));
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.IMPRIMIR,
                    "%c", "13"));
            }
            else if(texp.tipo == (int)Simbolo.Tipo.CARACTER)
            {
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.IMPRIMIR,
                    "%c", texp.cadena));
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.IMPRIMIR,
                    "%c", "13"));
            }
            else if(texp.tipo == (int)Simbolo.Tipo.CADENA)
            {
                // Ciclo para recorrer la cadena
                String eInicio = GeneradorC3D.getEtiqueta();
                String eAux = GeneradorC3D.getEtiqueta();
                String eFin = GeneradorC3D.getEtiqueta();
                String t2 = GeneradorC3D.getTemporal();
                GeneradorC3D.generarEtiquetas(eInicio);
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ACCESO,
                    "Heap", t2, texp.cadena));
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.CONDICIONAL,
                    eFin, t2, "==", "0"));
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.CONDICIONAL,
                    eAux, t2, "!=", "-241094.22"));
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION,
                    texp.cadena, texp.cadena, "+", "1"));
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ACCESO,
                    "Heap", t2, texp.cadena));
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.IMPRIMIR,
                    "%f", t2));
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION,
                    texp.cadena, texp.cadena, "+", "1"));
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL,
                    eInicio));
                GeneradorC3D.generarEtiquetas(eAux);
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.IMPRIMIR,
                    "%c", t2));
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.ASIGNACION,
                    texp.cadena, texp.cadena, "+", "1"));
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.INCONDICIONAL,
                    eInicio));
                GeneradorC3D.generarEtiquetas(eFin);
                GeneradorC3D.instrucciones.Add(new C3D((int)C3D.TipoC3D.IMPRIMIR,
                    "%c", "13"));
            }
            else
            {
                // Error semantico!
            }
        }
    }
}
