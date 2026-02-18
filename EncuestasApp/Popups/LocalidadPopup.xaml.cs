using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;

namespace EncuestaApp.Popups;

public partial class LocalidadPopup : Popup
{
    public string? LocalidadSeleccionada { get; private set; }

    private List<string> _todas;

    public LocalidadPopup(List<string> localidades)
    {
        InitializeComponent();
        _todas = localidades;
        ListaLocalidades.ItemsSource = _todas;
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        var texto = e.NewTextValue?.ToLower() ?? string.Empty;

        ListaLocalidades.ItemsSource = _todas
            .Where(x => x.ToLower().Contains(texto))
            .ToList();
    }

    private async void OnSeleccionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is string seleccion)
        {
            LocalidadSeleccionada = seleccion;
            await CloseAsync();
        }
    }
}