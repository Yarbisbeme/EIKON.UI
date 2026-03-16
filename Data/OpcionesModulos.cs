using System.Collections.Generic;
using System.Linq;

namespace EIKON.UI.Interfaces
{
 

    public class ModuloItemDto
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public bool AllowCheck { get; set; }
        public bool Checked { get; set; }
        public List<ModuloItemDto> Children { get; set; }
        public string Role { get; set; } // Rol al que pertenece el menú
    }


}

