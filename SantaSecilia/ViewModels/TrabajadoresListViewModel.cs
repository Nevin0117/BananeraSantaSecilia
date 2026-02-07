using System.Collections.ObjectModel;

namespace SantaSecilia.ViewModels
{
    public class TrabajadorItem
    {
        public int Codigo { get; set; }
        public required string Nombre { get; set; }
        public required string Cedula { get; set; }
        public string Estado => Activo ? "Activo" : "Inactivo";
        public bool Activo { get; set; }
    }

    public class TrabajadoresListViewModel
    {
        public ObservableCollection<TrabajadorItem> Trabajadores { get; set; }

        public TrabajadoresListViewModel()
        {
            Trabajadores = new ObservableCollection<TrabajadorItem>
            {
                new(){Codigo=1,Nombre="Carlos Mendoza",Cedula="4-834-2892",Activo=true},
                new(){Codigo=2,Nombre="Lucas Modric",Cedula="5-327-2513",Activo=false},
                new(){Codigo=3,Nombre="Federico Estevez",Cedula="4-221-5561",Activo=true},
                new(){Codigo=4,Nombre="David Villa",Cedula="9-450-9012",Activo=true},
                new(){Codigo=5,Nombre="Juan Guerra",Cedula="3-8592-6709",Activo=false},
            };
        }
    }
}
