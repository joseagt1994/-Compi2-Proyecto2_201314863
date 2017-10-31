using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi2_Proyecto2_201314863
{
    public class TablaSimbolos : List<Simbolo>
    {
        private static TablaSimbolos instance;

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
    }
}
