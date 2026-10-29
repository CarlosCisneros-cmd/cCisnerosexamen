namespace cCisnerosexamen.Views;

public partial class vResumen : ContentPage
{
    string usuario;

    public vResumen(string usuarioConectado, string nombre, string apellido, int edad,
                    DateTime fechaInicio, string plan, bool casillero,
                    decimal montoInicial, decimal pagoMensual, decimal pagoTotal)
    {
        InitializeComponent();
        usuario = usuarioConectado;
        lblUsuarioConectado.Text = "Usuario conectado: " + usuario;
        lblNombre.Text = nombre;
        lblApellido.Text = apellido;
        lblEdad.Text = edad.ToString();
        lblFechaInicio.Text = fechaInicio.ToString("dd/MM/yyyy");
        lblPlan.Text = plan;
        lblCasillero.Text = casillero ? "Sí" : "No";
        lblMontoInicial.Text = "$" + montoInicial.ToString("0.00");
        lblPagoMensual.Text = "$" + pagoMensual.ToString("0.00");
        lblPagoTotal.Text = "$" + pagoTotal.ToString("0.00");
    }

    private async void btnExportar_Clicked(object sender, EventArgs e)
    {
        try
        {
            string carpeta = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            Directory.CreateDirectory(carpeta);

            string nombreArchivo = "Resumen_" + lblNombre.Text + "_" + lblApellido.Text + "_"
                                   + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt";
            string ruta = Path.Combine(carpeta, nombreArchivo);

            string contenido =
                "RESUMEN DE SOCIO" + Environment.NewLine +
                "Usuario conectado: " + usuario + Environment.NewLine +
                "Nombre: " + lblNombre.Text + Environment.NewLine +
                "Apellido: " + lblApellido.Text + Environment.NewLine +
                "Edad: " + lblEdad.Text + Environment.NewLine +
                "Fecha de inicio: " + lblFechaInicio.Text + Environment.NewLine +
                "Plan: " + lblPlan.Text + Environment.NewLine +
                "Casillero: " + lblCasillero.Text + Environment.NewLine +
                "Monto Inicial: " + lblMontoInicial.Text + Environment.NewLine +
                "Pago mensual: " + lblPagoMensual.Text + Environment.NewLine +
                "Pago Total: " + lblPagoTotal.Text + Environment.NewLine;

            await File.WriteAllTextAsync(ruta, contenido);

            await DisplayAlertAsync("Archivo exportado",
                "Nombre: " + nombreArchivo + Environment.NewLine + "Ruta: " + carpeta, "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "Cerrar");
        }
    }

    private void btnCerrarSesion_Clicked(object sender, EventArgs e)
    {
        Application.Current!.Windows[0].Page = new NavigationPage(new vLogin());
    }
}
