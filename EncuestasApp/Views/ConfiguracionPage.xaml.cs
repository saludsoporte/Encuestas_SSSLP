using Microsoft.Maui.Storage;
using static EncuestaApp.Services.DatabaseService;

namespace EncuestaApp.Views;

public partial class ConfiguracionPage : ContentPage
{
    private const string ServerUrlKey = "ServerUrl";

    public ConfiguracionPage()
    {
        InitializeComponent();

        // Cargar URL almacenada (si existe)
        ServidorEntry.Text = Preferences.Get(ServerUrlKey, "https://visitandocorazones.slpsalud.gob.mx/EncuestasProxyApi/api");

        string deviceId = DeviceService.GetDeviceId();
        DeviceIdEntry.Text = deviceId;

        // Mostrar el valor actual
        int batchSize = BatchSettingsService.GetBatchSize();
        BatchEntry.Text = batchSize.ToString();
    }

    private async void OnGuardarClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(ServidorEntry.Text))
        {
            await DisplayAlert("Error", "Debes ingresar una URL válida.", "OK");
            return;
        }

        Preferences.Set(ServerUrlKey, ServidorEntry.Text.Trim());

        if (int.TryParse(BatchEntry.Text, out int batchSize) && batchSize > 0)
        {
            BatchSettingsService.SetBatchSize(batchSize);
        }
        else
        {
            await DisplayAlert("Error", "Ingrese un número válido mayor a 0.", "OK");
            return;
        }

        await DisplayAlert("Configuración", "La configuración se ha guardado correctamente ✅", "OK");

        await Navigation.PopAsync(); // Volver a la pantalla anterior
    }
}
