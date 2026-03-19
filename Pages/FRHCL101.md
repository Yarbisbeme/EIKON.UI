# Solicitud

Favor tomar el último proyecto que te envíe (EIKON.UI) y busques los formularios:  FRHCL101 - FRHCL111, la tarea es agregarle las validaciones para que no permitan enviar el formulario con los campos de texto en blanco, o fechas vacías, o las listas sin seleccionar un elemento...

-----------------
## Cambios realizados 🧠

### Metodos ⚙️

#### CargarRegistro()
- Se añadio un replace a los SelectedItem para eliminar los espacios. SelectedItem.xxxnume.Replace(oldValue: " ", newValue: "")

### Componentes 🧱

#### HeaderForm
- Se ha creado un componente para los header, ya que el codigo puede llegar a ser repetitivo para muchos Forms, optimizando el archivo y el proyecto.

#### DxTextBox
- Se agrego metodo para cargar registros al hacer Enter.

### Estilos

#### Animaciones
- Se agrego un Slide para cada tab.
- Se añadieron estilos personalizables para los inputs: Eikoninput, EikonReadOnly, EikonError.

#### Reemplazo
- Se Reemplazo Width por MinWidth en las tablas ya que dependiendo de la pantalla quedaban espacios vacios.
![alt text](image-2.png)

---------
## Bugs 🐞

### Creacion de Cargo

![alt text](image.png)
![alt text](image-1.png)

- Al crear un cargo a traves de  una requisicion, se le otorga un ID 000000.
- Al crear el cargo se asigna a la empresa ID " ", la cual no corresponde.

**prioridad:** Es alta ya que no permite crear correctamente los cargos.
**Riesgo:**  Alto ya que no tiene un ID Correcto, Y la empresa a la que pertenece es fantasma.

# Before

![alt text](image-4.png)
![alt text](image-5.png)