namespace ControlVehicular.ViewModels;

public class LoginViewModel : ViewModelBase
{
    private string _usuario = string.Empty;
    private string _mensaje = string.Empty;

    public string Usuario
    {
        get => _usuario;
        set => SetProperty(ref _usuario, value);
    }

    public string Mensaje
    {
        get => _mensaje;
        private set => SetProperty(ref _mensaje, value);
    }

    public bool Validar(string password)
    {
        if (Usuario.Trim().Equals("admin", StringComparison.OrdinalIgnoreCase) && password == "123456")
        {
            Mensaje = string.Empty;
            return true;
        }

        Mensaje = "Usuario o contraseña incorrectos.";
        return false;
    }
}
