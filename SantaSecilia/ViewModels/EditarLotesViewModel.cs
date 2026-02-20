using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SantaSecilia.Domain.Entities;
using SantaSecilia.Infrastructure.Repositories;
using Microsoft.Maui.Controls;

namespace SantaSecilia.ViewModels;

[QueryProperty(nameof(Id), "id")]
public class EditarLotesViewModel : INotifyPropertyChanged
{
    private readonly LotRepository _repository;
    private Lot? _lote;

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string name = "")
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    public int Id { get; set; }

    private string _codigo;
    public string Codigo
    {
        get => _codigo;
        set
        {
            _codigo = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<string> Estados { get; } =
        new ObservableCollection<string> { "Activo", "Inactivo" };

    private string _estadoSeleccionado;
    public string EstadoSeleccionado
    {
        get => _estadoSeleccionado;
        set
        {
            _estadoSeleccionado = value;
            OnPropertyChanged();
        }
    }

    public ICommand GuardarCommand { get; }
    public ICommand CancelarCommand { get; }

    public EditarLotesViewModel(LotRepository repository)
    {
        _repository = repository;

        GuardarCommand = new Command(async () => await GuardarAsync());
        CancelarCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
    }

    public async void OnAppearing()
    {
        _lote = await _repository.GetByIdAsync(Id);

        if (_lote == null)
            return;

        Codigo = _lote.Code.ToString();
        EstadoSeleccionado = _lote.IsActive ? "Activo" : "Inactivo";
    }

    private async Task GuardarAsync()
    {
        if (_lote == null)
            return;

        int nuevoCodigo = int.Parse(Codigo);

        bool existe = await _repository.ExistsByCodeAsync(nuevoCodigo, _lote.Id);

        if (existe)
        {
            await Shell.Current.DisplayAlertAsync("Error",
                "Ya existe un lote con ese código.",
                "OK");
            return;
        }

        _lote.Code = nuevoCodigo;
        _lote.IsActive = EstadoSeleccionado == "Activo";
        _lote.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(_lote);

        await Shell.Current.GoToAsync("..");
    }
}