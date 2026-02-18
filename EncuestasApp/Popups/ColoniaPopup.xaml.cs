using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;

namespace EncuestaApp.Popups;

public partial class ColoniaPopup : Popup
{
    public string? ColoniaSeleccionada { get; private set; }

    private List<string> _todas;

    public ColoniaPopup(List<string> colonias)
    {
        InitializeComponent();
        _todas = colonias;
        ListaColonias.ItemsSource = _todas;
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        var texto = e.NewTextValue?.ToLower() ?? string.Empty;

        ListaColonias.ItemsSource = _todas
            .Where(x => x.ToLower().Contains(texto))
            .ToList();
    }

    private async void OnSeleccionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is string seleccion)
        {
            ColoniaSeleccionada = seleccion;
            await CloseAsync();
        }
    }
}
