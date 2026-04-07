using EncuestaApp.Views;
using EncuestasApp;

namespace EncuestaApp
{
    public partial class App : Application
    {
        public static int UsuarioLogueadoId { get; set; } = 0;

        public App(EncuestaApp.Services.DatabaseService db)
        {
            try
            {
                InitializeComponent();
            
                Task.Run(async () =>
                {
                    await db.InicializarCodigosPostalesAsync();
                    await db.CargarLocalidadesDesdeCsvAsync();
                });
                MainPage = new NavigationPage(new LoginPage(db));
      
            }
            catch (Exception ex)
            {
                Console.WriteLine($": {ex.Message}");
            }
        }
    }
}
