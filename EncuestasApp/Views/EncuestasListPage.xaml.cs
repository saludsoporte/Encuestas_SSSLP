//using Android.Content;
using EncuestaApp.Models;
using EncuestaApp.Services;
using EncuestaApp.Views;
using EncuestasApp.Models;
using System.Net.Http.Json;
using static EncuestaApp.Services.DatabaseService;

namespace EncuestaApp.Views;

public partial class EncuestasListPage : ContentPage
{
    private readonly DatabaseService _db;

    public EncuestasListPage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadEncuestas();

        var userId = App.UsuarioLogueadoId;

        ToolbarItems.Clear(); // Limpia cualquier ícono previo

        // Solo mostrar ícono si es Admin
        if (userId == 1)
        {
            var configItem = new ToolbarItem
            {
                Text = "Configuración",
                IconImageSource = "settings.png",
            };
            configItem.Clicked += async (s, e) =>
            {
                await Navigation.PushAsync(new ConfiguracionPage());
            };

            ToolbarItems.Add(configItem);
        }
    }

    private async void OnConfigClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ConfiguracionPage());
    }

    private async Task LoadEncuestas()
    {
        var encuestas = await _db.GetEncuestasAsync();
        EncuestasCollection.ItemsSource = encuestas;
    }

    private async void OnNuevaEncuestaClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new EncuestaPage(_db));
    }

    private async void OnSyncClicked(object sender, EventArgs e)
    {
        if (SyncOverlay.IsVisible)
            return;
        try
        {
            SyncButton.IsEnabled = false; //Desactivar botón
            MostrarSyncOverlay(true, "Preparando sincronización...");
            SyncButton.Text = "Sincronizando...";
            

            var encuestas = await _db.GetEncuestasAsync();

            if (encuestas == null || !encuestas.Any())
            {
                await DisplayAlert("Sincronización", "No hay encuestas para enviar", "OK");
                // 🔓 Se vuelve a activar el botón
                ResetUI();
                return;
            }

            int tamañoLote = BatchSettingsService.GetBatchSize(); // <<< configurable
            var lotes = PartirEnLotes(encuestas, tamañoLote);

            using var httpClient = new HttpClient();

            //string apiUrl = "https://localhost:7281/api"; // Pruebas
            //string apiUrl = "https://encuestas-salud.fortiddns.com/EncuestaApi/api/Encuestas/bulk"; // Producción
            string apiBaseUrl = Preferences.Get("ServerUrl", "https://visitandocorazones.slpsalud.gob.mx/EncuestasProxyApi/api");
            string apiUrl = $"{apiBaseUrl}/Proxy/bulk";

            int totalEliminadas = 0;
            int totalFallidas = 0;
            int numLote = 1;

            httpClient.Timeout = TimeSpan.FromMinutes(15);

            foreach (var lote in lotes)
            {
                MostrarSyncOverlay(true,
                $"Sincronizando lote {numLote}/{lotes.Count}...");
                // --- Enviar lote ---
                var response = await httpClient.PostAsJsonAsync(apiUrl, lote);

                if (!response.IsSuccessStatusCode)
                {
                    totalFallidas += lote.Count;
                    continue;
                }

                var resultados = await response.Content.ReadFromJsonAsync<List<SyncResult>>();

                if (resultados != null)
                {
                    foreach (var r in resultados)
                    {
                        if (r.Success)
                        {
                            await _db.DeleteEncuestaAsync(r.LocalId);
                            totalEliminadas++;
                        }
                        else
                        {
                            totalFallidas++;
                        }
                    }
                }

                // Mostrar progreso opcional
                SyncButton.Text = $"Sincronizando lote {numLote}/{lotes.Count}...";
                numLote++;
            }

            await DisplayAlert("Resultado", $"✅ Enviadas: {totalEliminadas}\n❌ Fallidas: {totalFallidas}", "OK");

            ResetUI();
            await LoadEncuestas();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Ocurrió un problema: {ex.Message}", "OK");

            //  Se vuelve a activar el botón
            ResetUI();
        }
        finally
        {
            MostrarSyncOverlay(false);
            SyncButton.IsEnabled = true;
            ResetUI();
        }
    }

    public static List<List<T>> PartirEnLotes<T>(List<T> source, int lote)
    {
        return source
            .Select((item, index) => new { item, index })
            .GroupBy(x => x.index / lote)
            .Select(g => g.Select(x => x.item).ToList())
            .ToList();
    }

    private void ResetUI()
    {
        SyncButton.IsEnabled = true;
        //SyncSpinner.IsVisible = false;
        //SyncSpinner.IsRunning = false;
        SyncButton.Text = "🔄 Sincronizar";
    }
    private void MostrarSyncOverlay(bool mostrar, string mensaje = "Sincronizando...")
    {
        SyncOverlay.IsVisible = mostrar;
        SyncStatusLabel.Text = mensaje;
    }

    private async void OnAddTapped(object sender, EventArgs e)
    {
       // await Navigation.PushAsync(new EncuestaPage());
    }
}
