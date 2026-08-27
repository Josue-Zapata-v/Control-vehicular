namespace ControlVehicular.Models;

public class Ingreso
{
    public DateTime FechaHora { get; set; }
    public string TipoDocumento { get; set; } = string.Empty;
    public string NumeroDocumento { get; set; } = string.Empty;
    public string Placa { get; set; } = string.Empty;
    public string Turno { get; set; } = string.Empty;
    public string Conductor { get; set; } = string.Empty;
    public string Cliente { get; set; } = string.Empty;
    public string Producto { get; set; } = string.Empty;
    public decimal Peso { get; set; }
    public string Transporte { get; set; } = string.Empty;
}
