
using System.Collections.Generic;
using System.Linq;

// Asume que tus DTOs están en este namespace
namespace EIKON.UI.Interfaces
{
    public static class ModuloMapper
    {
        /// <summary>
        /// Convierte una lista de MenuItemDto (origen) a una lista de ModuloItemDto (destino).
        /// </summary>
        /// <param name="menuItems">La lista de objetos MenuItemDto de origen.</param>
        /// <returns>La lista convertida de ModuloItemDto.</returns>
        public static List<ModuloItemDto> MapList(List<MenuItemDto> menuItems)
        {
            if (menuItems == null)
            {
                return new List<ModuloItemDto>();
            }
           
            return menuItems
                .Select(item => MapSingle(item))
                .ToList();

        }

        public static List<ModuloItemDto> SeleccionarNodosPorCodigo(List<ModuloItemDto> menuItems, List<MenuItemDto> menuRol)
        {
            bool requiereActualizacion = false;

            // Recorre la estructura del árbol para encontrar los nodos
            foreach (var item in menuItems)
            {
                item.Checked=false;
                // Busca en el nivel principal (ejemplo: si el título coincide)
                //if (menuRol.Where(m=>m.Url.Contains(item.Title.Trim())).Count()>0)
                //{
                //    if (item.Checked == false)
                //    {
                //        item.Checked = true;
                //        requiereActualizacion = true;
                //    }
                //}

                // Busca en los nodos hijos
                if (item.Children != null)
                {
                    var ops = menuRol.FirstOrDefault().Children;
                    foreach (var child in item.Children)
                    {
                        child.Checked = false;

                        if (ops.Where(m => m.Url.ToLower().Trim() == (child.Url.Trim().ToLower())).Count() > 0)
                        {
                            if (child.Checked == false)
                            {   
                                child.Checked = true;
                                requiereActualizacion = true;
                            }
                        }
                    }
                    if (requiereActualizacion) 
                        item.Title = item.Title.Trim() + " *";
                    requiereActualizacion= false;   
                }
            }

            // Si se realizaron cambios, fuerza la actualización del componente
            if (requiereActualizacion)
            {
                // Opcional: Para asegurar que la UI se refresque inmediatamente
                //StateHasChanged();
            }
            return menuItems;
        }

        /// <summary>
        /// Convierte un solo MenuItemDto (origen) a un ModuloItemDto (destino).
        /// </summary>
        /// <param name="menuItem">El objeto MenuItemDto de origen.</param>
        /// <returns>El objeto ModuloItemDto convertido.</returns>
        private static ModuloItemDto MapSingle(MenuItemDto menuItem)
        {
            if (menuItem == null)
            {
                return null;
            }

            var moduloItem = new ModuloItemDto
            {
                Title = menuItem.Title,
                Icon = menuItem.Icon,
                Role = menuItem.Role,
                AllowCheck = menuItem.Url.Trim() == "/" ? false : true,
                Checked = false,
                Url = menuItem.Url,
                
            };

            // Recursión: Convierte la lista de hijos
            if (menuItem.Children != null && menuItem.Children.Any())
            {
                moduloItem.Children = menuItem.Children
                    .Select(MapSingle) // La clave está en llamar a la misma función (MapSingle)
                    .ToList();
            }
            else
            {
                // Asegura que la lista de hijos no sea nula si se necesita en el Blazor component
                moduloItem.Children = new List<ModuloItemDto>();
            }

            return moduloItem;
        }
    }
}
