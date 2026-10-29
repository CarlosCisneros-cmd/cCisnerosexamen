namespace cCisnerosexamen.Views;

public partial class vLogin : ContentPage
{
    string[,] usuarios =
    {
        { "recepcion", "gym2026" },
        { "entrenador", "fit_01" },
        { "uisrael", "2026" },
        { "gerente", "admin123" }
    };

    public vLogin()
    {
        InitializeComponent();
    }

    private async void btnIniciarSesion_Clicked(object sender, EventArgs e)
    {
        try
        {
            string usuarioIngreso = txtUsuario.Text?.Trim() ?? "";
            string contrasenaIngreso = txtContrasena.Text ?? "";

            if (usuarioIngreso == "" || contrasenaIngreso == "")
            {
                await DisplayAlertAsync("Alerta", "Ingrese el usuario y la contraseña", "OK");
                return;
            }

            bool accesoCorrecto = false;
            for (int i = 0; i < usuarios.GetLength(0); i++)
            {
                if (usuarios[i, 0] == usuarioIngreso && usuarios[i, 1] == contrasenaIngreso)
                {
                    accesoCorrecto = true;
                    break;
                }
            }

            if (accesoCorrecto)
            {
                txtContrasena.Text = "";
                await Navigation.PushAsync(new vRegistro(usuarioIngreso));
            }
            else
            {
                await DisplayAlertAsync("Dato incorrecto", "Usuario o contraseña incorrectos", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "Cerrar");
        }
    }

    private async void btnAcercaDe_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new vAcercaDe());
    }
}
