using EncuestaApp.Views;

namespace EncuestaApp.Views;

public partial class BottomNavBar : ContentView
{
    public static readonly BindableProperty ActiveTabProperty =
        BindableProperty.Create(nameof(ActiveTab), typeof(string),
        typeof(BottomNavBar), "home", propertyChanged: OnActiveTabChanged);

    public string ActiveTab
    {
        get => (string)GetValue(ActiveTabProperty);
        set => SetValue(ActiveTabProperty, value);
    }

    public BottomNavBar()
    {
        InitializeComponent();
    }

    private static void OnActiveTabChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var nav = (BottomNavBar)bindable;
        string active = newValue?.ToString() ?? "home";

        // Todos inactivos: blanco semitransparente
        nav.LabelHome.TextColor = Color.FromArgb("#80FFFFFF");
        nav.LabelAgenda.TextColor = Color.FromArgb("#80FFFFFF");
        nav.LabelSync.TextColor = Color.FromArgb("#80FFFFFF");
        nav.LabelPerfil.TextColor = Color.FromArgb("#80FFFFFF");

        // Solo el activo: blanco sólido + negrita
        Label activeLabel = active switch
        {
            "home" => nav.LabelHome,
            "agenda" => nav.LabelAgenda,
            "sync" => nav.LabelSync,
            "perfil" => nav.LabelPerfil,
            _ => nav.LabelHome
        };

        activeLabel.TextColor = Colors.White;
        activeLabel.FontAttributes = FontAttributes.Bold;

        // Reset negrita en los demás
        foreach (var label in new[] { nav.LabelHome, nav.LabelAgenda, nav.LabelSync, nav.LabelPerfil })
        {
            if (label != activeLabel)
                label.FontAttributes = FontAttributes.None;
        }
    }

    private async void OnHomeTapped(object sender, EventArgs e)
    {

    }
        //=> await Navigation.PushAsync(new MainPage());

    private async void OnAgendaTapped(object sender, EventArgs e)
        => await Navigation.PushAsync(new ConfiguracionPage());

    private async void OnSyncTapped(object sender, EventArgs e)
        => await Navigation.PushAsync(new ConfiguracionPage());

    private async void OnPerfilTapped(object sender, EventArgs e)
        => await Navigation.PushAsync(new ConfiguracionPage());
}