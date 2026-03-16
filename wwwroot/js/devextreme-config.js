// wwwroot/js/devextreme-config.js

window.addEventListener('load', function() {
    // Intentar configurar DevExpress después de que todos los recursos hayan cargado
    if (typeof DevExpress !== 'undefined') {
        DevExpress.config({
            themeLoadTimeout: 5000 
        });
    } else {
        console.error("DevExpress no está definido incluso después de la carga de la ventana.");
    }
});
//<script src="_framework/blazor.server.js"></script>
//<script src="js/cookieInterop.js"></script>
//@await RenderSectionAsync("Scripts", required: false)
