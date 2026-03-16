using EIKON.UI.Interfaces;
namespace EIKON.UI.Data
{
    public class OpcionesMenu
    {
        public string Name { get; }
        public List<Trhis000Dto> Opciones { get; }
        public bool AllowCheck => false;
        public OpcionesMenu(string name, IList<OpcionesMenu> opciones)
        {
            Name = name;
            Opciones = new List<Trhis000Dto>(Opciones);
        }
    }
}

