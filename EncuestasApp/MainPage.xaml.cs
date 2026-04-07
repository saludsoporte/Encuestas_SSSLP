using EncuestaApp.Views;

namespace EncuestasApp;

public partial class MainPage : ContentPage
{
    int count = 0;

    public MainPage()
    {
        InitializeComponent();
    }

    // Tu método original
    private void OnCounterClicked(object sender, EventArgs e)
    {
        count++;
        CounterBtn.Text = count == 1 ? "Clicked 1 time" : $"Clicked {count} times";
        SemanticScreenReader.Announce(CounterBtn.Text);
    }

    // ===== Navegación del menú =====
    private async void OnHomeTapped(object sender, EventArgs e)
    {
        // Ya estás en Home, puedes ignorar o hacer scroll al top
    }

    private async void OnAgendaTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ConfiguracionPage());
    }

    private async void OnSyncTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ConfiguracionPage());
    }

    private async void OnPerfilTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ConfiguracionPage());
    }

    private async void OnAddTapped(object sender, EventArgs e)
    {
        // Navega a la página de nueva encuesta
        await DisplayAlert("Nuevo", "Abrir formulario de nueva encuesta", "OK");
        //await Navigation.PushAsync(new EncuestaPage());
    }
}