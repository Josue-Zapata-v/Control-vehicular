using System.Windows;
using ControlVehicular.ViewModels;

namespace ControlVehicular;

public partial class LoginWindow : Window
{
    public LoginWindow() => InitializeComponent();

    private void IniciarSesion_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is not LoginViewModel viewModel || !viewModel.Validar(PasswordInput.Password)) return;
        new MainWindow().Show();
        Close();
    }
}
