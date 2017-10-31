using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi2_Proyecto2_201314863
{
    public class Errores
    {
        public List<Error> errores;
        private static Errores instance;

        private Errores()
        {
            errores = new List<Error>();
        }

        public static Errores getInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Errores();
                }
                return instance;
            }
        }

        public void agregar(Error error)
        {
            errores.Add(error);
        }
    }
}
