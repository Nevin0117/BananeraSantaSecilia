namespace SantaSecilia.ViewModels
{
    public class TrabajadorFormViewModel
    {
        public required string Nombre { get; set; }
        public required string Cedula { get; set; }
        public bool Activo { get; set; }

        public List<string> Estados => new() { "Activo", "Inactivo" };
    }
}
