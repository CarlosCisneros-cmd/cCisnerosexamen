using System.Globalization;

namespace cCisnerosexamen.Views;

public partial class vRegistro : ContentPage
{
    string usuario;
    const decimal costoMembresia = 1200;
    const int cuotas = 5;
    const decimal recargoCuota = 0.02m;
    const decimal costoCasillero = 10;

    public vRegistro(string usuarioConectado)
    {
        InitializeComponent();
        usuario = usuarioConectado;
        lblUsuarioConectado.Text = "Usuario conectado: " + usuario;
        dpFechaInicio.MinimumDate = DateTime.Today;
        dpFechaInicio.Date = DateTime.Today;
    }

    private bool SoloLetras(string texto)
    {
        foreach (char c in texto)
        {
            if (!char.IsLetter(c) && c != ' ')
                return false;
        }
        return true;
    }

    private bool SoloNumeros(string texto)
    {
        foreach (char c in texto)
        {
            if (!char.IsDigit(c))
                return false;
        }
        return true;
    }

    private bool LeerMonto(out decimal monto)
    {
        string texto = (txtMontoInicial.Text ?? "").Trim().Replace(',', '.');
        return decimal.TryParse(texto, NumberStyles.Number, CultureInfo.InvariantCulture, out monto);
    }

    private async Task<bool> ValidarCalculo()
    {
        if (pkPlan.SelectedIndex == -1)
        {
            await DisplayAlertAsync("Alerta", "Seleccione un plan", "OK");
            return false;
        }

        if (!LeerMonto(out decimal monto))
        {
            await DisplayAlertAsync("Alerta", "El monto inicial debe ser un número", "OK");
            return false;
        }

        if (monto < 120 || monto > 600)
        {
            await DisplayAlertAsync("Alerta", "El monto inicial debe estar entre 120 y 600", "OK");
            return false;
        }

        return true;
    }

    private async Task<bool> ValidarDatosPersonales()
    {
        string nombre = (txtNombre.Text ?? "").Trim();
        string apellido = (txtApellido.Text ?? "").Trim();
        string edadTexto = (txtEdad.Text ?? "").Trim();

        if (nombre == "" || !SoloLetras(nombre))
        {
            await DisplayAlertAsync("Alerta", "Ingrese un nombre válido (solo letras)", "OK");
            return false;
        }

        if (apellido == "" || !SoloLetras(apellido))
        {
            await DisplayAlertAsync("Alerta", "Ingrese un apellido válido (solo letras)", "OK");
            return false;
        }

        if (edadTexto == "" || !SoloNumeros(edadTexto))
        {
            await DisplayAlertAsync("Alerta", "La edad debe contener solo números", "OK");
            return false;
        }

        if (int.Parse(edadTexto) < 16)
        {
            await DisplayAlertAsync("Alerta", "La edad debe ser mayor o igual a 16 años", "OK");
            return false;
        }

        if ((DateTime)dpFechaInicio.Date! < DateTime.Today)
        {
            await DisplayAlertAsync("Alerta", "La fecha de inicio no puede ser una fecha pasada", "OK");
            return false;
        }

        return true;
    }

    private decimal CalcularPagoMensual()
    {
        LeerMonto(out decimal montoInicial);
        decimal cuota = (costoMembresia - montoInicial) / cuotas + costoMembresia * recargoCuota;
        if (swCasillero.IsToggled)
            cuota += costoCasillero;

        return Math.Round(cuota, 2);
    }

    private async void btnCalcular_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (!await ValidarCalculo())
                return;

            txtPagoMensual.Text = CalcularPagoMensual().ToString("0.00");
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "Cerrar");
        }
    }

    private async void btnVerResumen_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (!await ValidarDatosPersonales())
                return;

            if (!await ValidarCalculo())
                return;

            if (string.IsNullOrEmpty(txtPagoMensual.Text))
            {
                await DisplayAlertAsync("Alerta", "Primero calcule el pago mensual", "OK");
                return;
            }

            decimal pagoMensual = CalcularPagoMensual();
            txtPagoMensual.Text = pagoMensual.ToString("0.00");
            LeerMonto(out decimal montoInicial);
            decimal pagoTotal = montoInicial + pagoMensual * cuotas;

            await Navigation.PushAsync(new vResumen(
                usuario,
                txtNombre.Text!.Trim(),
                txtApellido.Text!.Trim(),
                int.Parse(txtEdad.Text!.Trim()),
                (DateTime)dpFechaInicio.Date!,
                pkPlan.SelectedItem.ToString()!,
                swCasillero.IsToggled,
                montoInicial,
                pagoMensual,
                pagoTotal));
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "Cerrar");
        }
    }
}
