using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;
using ControlVehicular.Models;

namespace ControlVehicular.ViewModels;

public class MainViewModel : ViewModelBase
{
    public ObservableCollection<Conductor> Conductores { get; } = new();
    public ObservableCollection<Ingreso> Ingresos { get; } = new();
    public ICollectionView VistaIngresos { get; }

    public string[] TiposDocumento { get; } = ["Guía de remisión", "Factura", "Boleta"];
    public string[] Turnos { get; } = ["Mañana", "Tarde", "Noche"];

    private string _seccionActual = "Ingresos";
    public string SeccionActual { get => _seccionActual; set => SetProperty(ref _seccionActual, value); }

    private string _tipoDocumento = "Guía de remisión";
    public string TipoDocumento { get => _tipoDocumento; set => SetProperty(ref _tipoDocumento, value); }
    private string _numeroDocumento = string.Empty;
    public string NumeroDocumento { get => _numeroDocumento; set => SetProperty(ref _numeroDocumento, value); }
    private string _placa = string.Empty;
    public string Placa { get => _placa; set => SetProperty(ref _placa, value); }
    private string _turno = "Mañana";
    public string Turno { get => _turno; set => SetProperty(ref _turno, value); }
    private string _nombreConductor = string.Empty;
    public string NombreConductor { get => _nombreConductor; set => SetProperty(ref _nombreConductor, value); }
    private string _nombreCliente = string.Empty;
    public string NombreCliente { get => _nombreCliente; set => SetProperty(ref _nombreCliente, value); }
    private string _producto = string.Empty;
    public string Producto { get => _producto; set => SetProperty(ref _producto, value); }
    private string _pesoIngreso = string.Empty;
    public string PesoIngreso { get => _pesoIngreso; set => SetProperty(ref _pesoIngreso, value); }
    public DateTime FechaHora { get; } = DateTime.Now;

    private string _nuevoConductor = string.Empty;
    public string NuevoConductor { get => _nuevoConductor; set => SetProperty(ref _nuevoConductor, value); }
    private string _licencia = string.Empty;
    public string Licencia { get => _licencia; set => SetProperty(ref _licencia, value); }
    private string _transporte = string.Empty;
    public string Transporte { get => _transporte; set => SetProperty(ref _transporte, value); }

    private DateTime? _fechaInicio;
    public DateTime? FechaInicio { get => _fechaInicio; set => SetProperty(ref _fechaInicio, value); }
    private DateTime? _fechaFin;
    public DateTime? FechaFin { get => _fechaFin; set => SetProperty(ref _fechaFin, value); }
    private string _filtroPlaca = string.Empty;
    public string FiltroPlaca { get => _filtroPlaca; set => SetProperty(ref _filtroPlaca, value); }
    private string _filtroConductor = string.Empty;
    public string FiltroConductor { get => _filtroConductor; set => SetProperty(ref _filtroConductor, value); }
    private string _filtroProducto = string.Empty;
    public string FiltroProducto { get => _filtroProducto; set => SetProperty(ref _filtroProducto, value); }

    private string _mensaje = "Listo para registrar operaciones.";
    public string Mensaje { get => _mensaje; private set => SetProperty(ref _mensaje, value); }

    public ICommand CambiarSeccionCommand { get; }
    public ICommand RegistrarIngresoCommand { get; }
    public ICommand RegistrarConductorCommand { get; }
    public ICommand BuscarIngresosCommand { get; }
    public ICommand LimpiarFiltrosCommand { get; }

    public MainViewModel()
    {
        VistaIngresos = CollectionViewSource.GetDefaultView(Ingresos);
        VistaIngresos.Filter = CoincideFiltro;
        CambiarSeccionCommand = new RelayCommand(p => SeccionActual = p?.ToString() ?? "Ingresos");
        RegistrarIngresoCommand = new RelayCommand(_ => RegistrarIngreso());
        RegistrarConductorCommand = new RelayCommand(_ => RegistrarConductor());
        BuscarIngresosCommand = new RelayCommand(_ => VistaIngresos.Refresh());
        LimpiarFiltrosCommand = new RelayCommand(_ => LimpiarFiltros());

        Conductores.Add(new Conductor { Nombre = "Carlos Mendoza", Licencia = "Q12345678", Transporte = "Transportes Lima SAC" });
        Ingresos.Add(new Ingreso { FechaHora = DateTime.Today.AddHours(8), TipoDocumento = "Guía de remisión", NumeroDocumento = "GR-001", Placa = "ABC-123", Turno = "Mañana", Conductor = "Carlos Mendoza", Cliente = "Comercial Andina", Producto = "Cemento", Peso = 12500, Transporte = "Transportes Lima SAC" });
    }

    private void RegistrarIngreso()
    {
        if (string.IsNullOrWhiteSpace(NumeroDocumento) || string.IsNullOrWhiteSpace(Placa) || string.IsNullOrWhiteSpace(NombreConductor) || string.IsNullOrWhiteSpace(NombreCliente) || !decimal.TryParse(PesoIngreso, NumberStyles.Number, CultureInfo.CurrentCulture, out var peso) || peso <= 0)
        {
            Mensaje = "Complete los campos obligatorios y registre un peso válido.";
            return;
        }

        var conductor = Conductores.FirstOrDefault(c => c.Nombre.Equals(NombreConductor.Trim(), StringComparison.OrdinalIgnoreCase));
        Ingresos.Add(new Ingreso { FechaHora = FechaHora, TipoDocumento = TipoDocumento, NumeroDocumento = NumeroDocumento.Trim(), Placa = Placa.Trim().ToUpperInvariant(), Turno = Turno, Conductor = NombreConductor.Trim(), Cliente = NombreCliente.Trim(), Producto = Producto.Trim(), Peso = peso, Transporte = conductor?.Transporte ?? "No registrado" });
        Mensaje = $"Ingreso registrado para la placa {Placa.Trim().ToUpperInvariant()}.";
        NumeroDocumento = Placa = NombreConductor = NombreCliente = Producto = PesoIngreso = string.Empty;
    }

    private void RegistrarConductor()
    {
        if (string.IsNullOrWhiteSpace(NuevoConductor) || string.IsNullOrWhiteSpace(Licencia) || string.IsNullOrWhiteSpace(Transporte))
        {
            Mensaje = "Complete nombre, licencia y transporte del conductor.";
            return;
        }
        Conductores.Add(new Conductor { Nombre = NuevoConductor.Trim(), Licencia = Licencia.Trim().ToUpperInvariant(), Transporte = Transporte.Trim() });
        Mensaje = $"Conductor {NuevoConductor.Trim()} registrado.";
        NuevoConductor = Licencia = Transporte = string.Empty;
    }

    private bool CoincideFiltro(object obj)
    {
        if (obj is not Ingreso ingreso) return false;
        return (!FechaInicio.HasValue || ingreso.FechaHora.Date >= FechaInicio.Value.Date)
            && (!FechaFin.HasValue || ingreso.FechaHora.Date <= FechaFin.Value.Date)
            && Contiene(ingreso.Placa, FiltroPlaca)
            && Contiene(ingreso.Conductor, FiltroConductor)
            && Contiene(ingreso.Producto, FiltroProducto);
    }

    private static bool Contiene(string valor, string filtro) => string.IsNullOrWhiteSpace(filtro) || valor.Contains(filtro.Trim(), StringComparison.OrdinalIgnoreCase);
    private void LimpiarFiltros() { FechaInicio = FechaFin = null; FiltroPlaca = FiltroConductor = FiltroProducto = string.Empty; VistaIngresos.Refresh(); }
}
