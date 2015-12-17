using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CockTailGuide
{
    public class recipe
    {
        public string title { get; set; }
        public string type { get; set; }
        public List<ingredient> ingredients { get; set; }
        public string step { get; set; }
        public string glass { get; set; }
        public string garnish { get; set; }
        public string preparation { get; set; }
        public string strength { get; set; }

    }
}
