using CommunityToolkit.Maui.Extensions;
using EncuestaApp.Models;
using EncuestaApp.Popups;
using EncuestaApp.Services;
using EncuestasApp.Models;
using Microsoft.Maui.Devices.Sensors;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using static EncuestaApp.Services.DatabaseService;
using EncuestasApp.utilerias;

namespace EncuestaApp.Views;

public partial class EncuestaPage : ContentPage
{
    private readonly DatabaseService _db;
    private List<CodigoPostalCat> _coloniasPorCp = new();
    private List<CodigoPostalCat> _registrosPorCP = new();

    //para localidad
    private String _cveMunSeleccionado;
    private List<Localidad> _localidadesActuales = new();
    private List<CodigoPostalCat> _municipiosPorCP = new();
    private String ? _cveLocSeleccionada;
    private string? _ambitoLocalidad = null;
    
   

    public EncuestaPage(DatabaseService db)
    {

        InitializeComponent();
        ColoniaManualContainer.IsVisible = false;
        SeccionManualEntry.IsVisible = false;
        RespiratorioPicker.SelectedIndex = 0;
        PsicologicoPicker.SelectedIndex = 0;
        GenitourinarioPicker.SelectedIndex = 0;
        NeurologicoPicker.SelectedIndex = 0;
        MusculoesqueleticoPicker.SelectedIndex = 0;
        CardiovascularPicker.SelectedIndex = 0;
        ReferenciasNecesariasPicker.SelectedIndex = 0;
        DigestivoPicker.SelectedIndex = 0;
        EstadoGeneralPicker.SelectedIndex = 0;

        TamizajeHipertensionPicker.SelectedIndex = 0;
        TamizajeDiabetesPicker.SelectedIndex = 0;
        DeteccionesExploracionMamariaPicker.SelectedIndex = 0;
        ExploracionClinicaMamariaPicker.SelectedIndex = 0;
        DeteccionesEvaluacionEstadoNutricionalPicker.SelectedIndex = 0;
        DeteccionesSaludMentalPicker.SelectedIndex = 0;
        DeteccionesRevisionBucalPicker.SelectedIndex = 0;

        _db = db;
        ConfigurarCalculoIMC();
    }

    private List<ValidationResult> ValidarModelo(object modelo)
    {
        var contexto = new ValidationContext(modelo, null, null);
        var resultados = new List<ValidationResult>();

        Validator.TryValidateObject(modelo, contexto, resultados, true);

        return resultados;
    }

    //private bool ValidarCurp(string curp)
    //{
    //    string patron = @"^[A-Z]{4}\d{6}[HM]{1}[A-Z]{5}[0-9A-Z]{2}$";
    //    return Regex.IsMatch(curp, patron);
    //}
   
    private async void OnGuardarClicked(object sender, EventArgs e)
    {
        string coloniaSeleccionada = ColoniaSelectorEntry.Text?.Trim();
        string coloniaManual = ColoniaManualEntry.Text?.Trim();
        string seccionSeleccionada = SeccionPicker.SelectedItem?.ToString();
        string seccionManual = SeccionManualEntry.Text?.Trim();
        bool confirmar = await DisplayAlert(
        "Confirmar",
        "¿Estás seguro de terminar la encuesta? una vez concluída no se podrá modificar.",
        "Sí", "No");

        if (!confirmar)
            return; // si presiona "No", se cancela la acción


        // Intentar obtener ubicación
        Location location = null;

        try
        {
            MostrarLoading(true);
            location = await Geolocation.GetLastKnownLocationAsync();
            if (location == null)
            {
                location = await Geolocation.GetLocationAsync(
                    new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10)));
            }
        

        // Validar CURP
        string curp = CurpEntry.Text?.Trim().ToUpper() ?? "";

        // Validar CURP existente en la base de datos local
        var existeCurp = await _db.GetCurpExistenteAsync(curp);
        if (existeCurp is not null)
        {
            await DisplayAlert("Error", "La CURP ya existe en la base de datos local.", "OK");
            return;
        }
        bool coincide = GeneraCurp.CoincideConDatos(CurpEntry.Text,NombreEntry.Text,pApellidoEntry.Text,sApellidoEntry.Text,FechaNacimientoEntry.Date,GeneroPicker.SelectedItem?.ToString(),ObtenerCodigoEntidad(EntidadPicker.SelectedItem?.ToString()));

        if (!coincide)
        {
            bool aceptarCurp = await DisplayAlert(
       "Confirmar",
       "Se detecto que los datos de identidad (nombre/género/fecha/estado) no coincide con la curp ¿Quieres continuar de todos modos?",
       "Sí", "Corregir");

            if (!aceptarCurp)
                return; //si presiona no se cancela la accion
        }

        bool coloniaValida =
    (!ColoniaNoEncontradaCheck.IsChecked && !string.IsNullOrWhiteSpace(coloniaSeleccionada))
    || (ColoniaNoEncontradaCheck.IsChecked && !string.IsNullOrWhiteSpace(coloniaManual));

        if (!coloniaValida)
        {
            await DisplayAlert(
                "Error",
                "Debe seleccionar una colonia o capturarla manualmente.",
                "OK"
            );
            return;
        }

        bool seccionValida =
    (!SeccionNoEncontradaCheck.IsChecked && !string.IsNullOrWhiteSpace(seccionSeleccionada))
    || (SeccionNoEncontradaCheck.IsChecked && !string.IsNullOrWhiteSpace(seccionManual));

        if (!seccionValida)
        {
            await DisplayAlert(
                "Error",
                "Debe seleccionar una sección electoral o capturarla manualmente.",
                "OK"
            );
            return;
        }
        if (!ValidarContacto())
        {
            await DisplayAlert(
                "Error",
                "Debe capturar un teléfono o celular válido o seleccionar 'No tiene'.",
                "OK"
            );
            return;
        }

        if (!NoTelAcompCheck.IsChecked && (TelefonoAcompananteEntry.Text ?? "").Length != 10)
        {
            await DisplayAlert(
                "Error",
                "Debe capturar un teléfono para acompañante válido o seleccionar 'No tiene'.",
                "OK"
            );
            return;
        }

        if (RelacionAcompanantePicker.SelectedIndex == -1)
        {
            await DisplayAlert(
                "Error",
                "Debe seleccionar la relación del acompañante.",
                "OK");
            return;
        }
        if (PuebloIndigenaPicker.SelectedItem is not null)
            if (PuebloIndigenaPicker.SelectedItem.ToString() == "Sí")
            {
                if (CualPuebloIndigenaEntry.Text == "" || CualPuebloIndigenaEntry.Text is null) {
                    await DisplayAlert("Error", "Cual pueblo indigena, no puede estar vacio", "OK");
                    return;
                }     
            }

        if (IdiomaIndigenaPicker.SelectedItem is not null)
            if (IdiomaIndigenaPicker.SelectedItem.ToString() == "Sí")
            {
                if (CualIdiomaIndigenaEntry.Text == "" || CualIdiomaIndigenaEntry.Text is null)
                {
                    await DisplayAlert("Error", "Cual idioma indigena, no puede estar vacio", "OK");
                    return;
                }
            }

        if (CancerCheckBox.IsChecked)
        {
            if (EspecificaCancerEntry.Text == "" || EspecificaCancerEntry.Text is null)
            {
                await DisplayAlert("Error", "Especificar cancer, no puede estar vacio", "OK");
                return;
            }
        }

        if (SaludMentalCheckBox.IsChecked)
        {
            if (EspecificaSaludMentalEntry.Text == "" || EspecificaSaludMentalEntry.Text is null)
            {
                await DisplayAlert("Error", "Especificar salud mental, no puede estar vacio", "OK");
                return;
            }
        }

        if (AlergiasCheckBox.IsChecked)
        {
            if (EspecificaAlergiasEntry.Text == "" || EspecificaAlergiasEntry.Text is null)
            {
                await DisplayAlert("Error", "Especificar alergias, no puede estar vacio", "OK");
                return;
            }
        }

        if (CirugiasPreviasCheckBox.IsChecked)
        {
            if (EspecificaCirugiasPreviasEntry.Text == "" || EspecificaCirugiasPreviasEntry.Text is null)
            {
                await DisplayAlert("Error", "Especificar cirugias previas, no puede estar vacio", "OK");
                return;
            }
        }

        if (InfeccionesTransmisionSexualCheckBox.IsChecked)
        {
            if ((EspecificaInfeccionesTransmisionSexualEntry.Text == "" || MedicamentosActualesEntry.Text == "") || (EspecificaInfeccionesTransmisionSexualEntry.Text is null || MedicamentosActualesEntry.Text is null))
            {
                await DisplayAlert("Error", "Especificar infecciones de trasnmisión sexual y Medicamentos actuales, no pueden estar vacios", "OK");
                return;
            }
        }

        if (OtrosAntecedentesCheckBox.IsChecked)
        {
            if (EspecificaOtrosAntecedentesEntry.Text == "" || EspecificaOtrosAntecedentesEntry.Text is null)
            {
                await DisplayAlert("Error", "Especificar otros antecedentes, no pueden estar vacios", "OK");
                return;
            }
        }

        if (AntecedentesHipertensionCheckBox.IsChecked)
        {
            if (ParentescoHipertensionPicker.SelectedItem is null  )
            {
                await DisplayAlert("Error", "Especificar parentesco antecedentes Hipertensión Arterial, no pueden estar vacios", "OK");
                return;
            }
        }

        if (AntecedentesDiabetesCheckBox.IsChecked)
        {
            if (ParentescoDiabetesPicker.SelectedItem is null)
            {
                await DisplayAlert("Error", "Especificar parentesco antecedentes Diabetes Mellitus, no pueden estar vacios", "OK");
                return;
            }
        }

        if (AntecedentesObesidadCheckBox.IsChecked)
        {
            if (ParentescoObesidadPicker.SelectedItem is null)
            {
                await DisplayAlert("Error", "Especificar parentesco antecedentes Obesidad, no pueden estar vacios", "OK");
                return;
            }
        }

        if (AntecedentesEnfermedadCerdiovascularCheckBox.IsChecked)
        {
            if (ParentescoEnfermedadCardiovascularPicker.SelectedItem is null)
            {
                await DisplayAlert("Error", "Especificar parentesco antecedentes Enfermedad cardiovascular, no pueden estar vacios", "OK");
                return;
            }
        }

        if (AntecedentesDislipidemiaCheckBox.IsChecked)
        {
            if (ParentescoDislipidemiaPicker.SelectedItem is null)
            {
                await DisplayAlert("Error", "Especificar parentesco antecedentes Dislipidemia (Colesterol/Trigliceridos), no pueden estar vacios", "OK");
                return;
            }
        }

        if (AntecedentesAsmaCheckBox.IsChecked)
        {
            if (ParentescoAsmaPicker.SelectedItem is null)
            {
                await DisplayAlert("Error", "Especificar parentesco antecedentes Asma/EPOC, no pueden estar vacios", "OK");
                return;
            }
        }

        if (AntecedentesEpilepsiaCheckBox.IsChecked)
        {
            if (ParentescoEpilepsiaPicker.SelectedItem is null)
            {
                await DisplayAlert("Error", "Especificar parentesco antecedentes Epilepsia, no pueden estar vacios", "OK");
                return;
            }
        }

        if (AntecedentesCancerCheckBox.IsChecked)
        {
            if (ParentescoCancerPicker.SelectedItem is null)
            {
                await DisplayAlert("Error", "Especificar parentesco antecedentes Cáncer, no pueden estar vacios", "OK");
                return;
            }
        }
        if (AntecedentesSaludMentalCheckBox.IsChecked)
        {
            if (ParentescoSaludMentalPicker.SelectedItem is null)
            {
                await DisplayAlert("Error", "Especificar parentesco antecedentes Trastorno de salud mental, no pueden estar vacios", "OK");
                return;
            }
        }

        if (TabaquismoPicker.SelectedItem is not null)
            if (TabaquismoPicker.SelectedItem.ToString() == "Sí")
            {
                if (TabaquismoNumEntry.Text == "" || TabaquismoNumEntry.Text is null)
                {
                    await DisplayAlert("Error", "Cuantos cigarros, no puede estar vacio", "OK");
                    return;
                }
            }

        if (ConsumoAlcoholPicker.SelectedItem is not null)
            if (ConsumoAlcoholPicker.SelectedItem.ToString() == "Sí")
            {
                if (ConsumoAlcoholFrecuenciaEntry.Text == "" || ConsumoAlcoholFrecuenciaEntry.Text is null)
                {
                    await DisplayAlert("Error", "Frecuencia de consumo de alcohol, no puede estar vacio", "OK");
                    return;
                }
            }

        if (ConsumoDrogasPicker.SelectedItem is not null)
            if (ConsumoDrogasPicker.SelectedItem.ToString() == "Sí")
            {
                if (ConsumoDrogasFrecuenciaEntry.Text == "" || ConsumoDrogasFrecuenciaEntry.Text is null)
                {
                    await DisplayAlert("Error", "Frecuencia de consumo de drogas, no puede estar vacio", "OK");
                    return;
                }
            }

        if (TieneMascotasPicker.SelectedItem is not null)
            if (TieneMascotasPicker.SelectedItem.ToString() == "Sí")
            {
                if (EstanVacunadosEntry.Text == "" || EstanVacunadosEntry.Text is null)
                {
                    await DisplayAlert("Error", "Están vacunados, no puede estar vacio", "OK");
                    return;
                }
            }

        if (RespiratorioPicker.SelectedItem is not null)
            if (RespiratorioPicker.SelectedItem.ToString() == "Otro")
            {
                if (EspecificaOtroRespiratorioEntry.Text == "" || EspecificaOtroRespiratorioEntry.Text is null)
                {
                    await DisplayAlert("Error", "Especificar otro respiratorio, no puede estar vacio", "OK");
                    return;
                }
            }

        if (CardiovascularPicker.SelectedItem is not null)
            if (CardiovascularPicker.SelectedItem.ToString() == "Otro")
            {
                if (EspecificaOtroCardiovascularEntry.Text == "" || EspecificaOtroCardiovascularEntry.Text is null)
                {
                    await DisplayAlert("Error", "Especificar otro cardiovascular, no puede estar vacio", "OK");
                    return;
                }
            }

        if (DigestivoPicker.SelectedItem is not null)
            if (DigestivoPicker.SelectedItem.ToString() == "Otro")
            {
                if (EspecificaOtroDigestivoEntry.Text == "" || EspecificaOtroDigestivoEntry.Text is null)
                {
                    await DisplayAlert("Error", "Especificar otro digestivo, no puede estar vacio", "OK");
                    return;
                }
            }

        if (MusculoesqueleticoPicker.SelectedItem is not null)
            if (MusculoesqueleticoPicker.SelectedItem.ToString() == "Otro")
            {
                if (EspecificaOtroMusculoesqueleticoEntry.Text == "" || EspecificaOtroMusculoesqueleticoEntry.Text is null)
                {
                    await DisplayAlert("Error", "Especificar otro musculo esqueletico, no puede estar vacio", "OK");
                    return;
                }
            }

        if (NeurologicoPicker.SelectedItem is not null)
            if (NeurologicoPicker.SelectedItem.ToString() == "Otro")
            {
                if (EspecificaOtroNeurologicoEntry.Text == "" || EspecificaOtroNeurologicoEntry.Text is null)
                {
                    await DisplayAlert("Error", "Especificar otro neurologico, no puede estar vacio", "OK");
                    return;
                }
            }

        if (GenitourinarioPicker.SelectedItem is not null)
            if (GenitourinarioPicker.SelectedItem.ToString() == "Otro")
            {
                if (EspecificaOtroGenitourinarioEntry.Text == "" || EspecificaOtroGenitourinarioEntry.Text is null)
                {
                    await DisplayAlert("Error", "Especificar otro genitourinario, no puede estar vacio", "OK");
                    return;
                }
            }

        if (PsicologicoPicker.SelectedItem is not null)
            if (PsicologicoPicker.SelectedItem.ToString() == "Otro")
            {
                if (EspecificaOtroPsicologicoEntry.Text == "" || EspecificaOtroPsicologicoEntry.Text is null)
                {
                    await DisplayAlert("Error", "Especificar otro psicológico, no puede estar vacio", "OK");
                    return;
                }
        }

        if (FrecuenciaCardiacaEntry.Text != "")
        {
            try {
                int frecuencia = 0;
                frecuencia = Convert.ToInt32(FrecuenciaCardiacaEntry.Text);
                if (frecuencia != 0)
                {
                    if (frecuencia < 40 && frecuencia > 220)
                    {
                        await DisplayAlert("Error", "La Frecuencia Cardiaca no es valida,el valor del rango debe ser mayor o igual a “40” y menor o igual “220”", "OK");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
               
            }         
        }

        if (TemperaturaEntry.Text != "")
        {
            try
            {
                int temp2 = 0;
                temp2 = Convert.ToInt32(TemperaturaEntry.Text);
                if (temp2 != 0)
                {
                    if (temp2 < 35 && temp2 > 40)
                    {
                        await DisplayAlert("Error", "La Temperatura no es valida,el valor del rango debe ser mayor o igual a “35” y menor o igual “40”", "OK");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        if (FrecuenciaRespiratoriaEntry.Text != "")
        {
            try
            {
                int frecuencia = 0;
                frecuencia = Convert.ToInt32(FrecuenciaRespiratoriaEntry.Text);
                if (frecuencia != 0)
                {
                    if (frecuencia < 40 && frecuencia > 220)
                    {
                        await DisplayAlert("Error", "La Frecuencia Respiratoria no es valida,el valor del rango debe ser mayor o igual a “40” y menor o igual “220”", "OK");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        if (SaturacionO2Entry.Text != "")
        {
            try
            {
                int saturacion = 0;
                saturacion = Convert.ToInt32(SaturacionO2Entry.Text);
                if (saturacion != 0)
                {
                    if (saturacion < 0 && saturacion > 100)
                    {
                        await DisplayAlert("Error", "La Saturación de Oxigeno no es valida,el valor del rango debe ser mayor o igual a “35” y menor o igual “40”", "OK");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        if (PesoEntry.Text != "")
        {
            try
            {
                double peso1 = 0;
                peso1 = Convert.ToDouble(PesoEntry.Text);
                if (peso1 != 999.000)
                {
                    if (peso1 < 1 && peso1 > 400)
                    {
                        await DisplayAlert("Error", "El peso no es valido,el valor del rango debe ser mayor o igual a “1” y menor o igual “400”", "OK");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        if (TallaEntry.Text != "")
        {
            try
            {
                double talla = 0;
                talla = Convert.ToDouble(TallaEntry.Text);
                if (talla != 999.00)
                {
                    if (talla < .30 && talla > 2.20)
                    {
                        await DisplayAlert("Error", "El peso no es valido,el valor del rango debe ser mayor o igual a “.30” y menor o igual “2.20”", "OK");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        if (string.IsNullOrWhiteSpace(ImcEntry.Text) || ImcPicker.SelectedItem == null)
        {
            await DisplayAlert("Error", "Por favor, ingrese peso y talla para calcular el IMC", "OK");
            return;
        }

        string mensaje="";

        if (ImpresionDiagnostica1Entry.Text == "" || ImpresionDiagnostica1Entry.Text is null)
        {
            ImpresionDiagnostica1Entry.Text = "";
        }

        if (ImpresionDiagnostica2Entry.Text == "" || ImpresionDiagnostica2Entry.Text is null)
        {
            ImpresionDiagnostica2Entry.Text = "";
        }

        if (ImpresionDiagnostica3Entry.Text == "" || ImpresionDiagnostica3Entry.Text is null)
        {
            ImpresionDiagnostica3Entry.Text = "";
        }

        if (EspecificaOtroDigestivoEntry.Text == "" || EspecificaOtroDigestivoEntry.Text is null)
        {
            EspecificaOtroDigestivoEntry.Text = "";
        }

        if (EspecificaOtroNeurologicoEntry.Text == "" || EspecificaOtroNeurologicoEntry.Text is null)
        {
            EspecificaOtroNeurologicoEntry.Text = "";
        }

        if (EspecificaOtroMusculoesqueleticoEntry.Text == "" || EspecificaOtroMusculoesqueleticoEntry.Text is null)
        {
            EspecificaOtroMusculoesqueleticoEntry.Text = "";
        }

        if (EspecificaOtroGenitourinarioEntry.Text == "" || EspecificaOtroGenitourinarioEntry.Text is null)
        {
            EspecificaOtroGenitourinarioEntry.Text = "";
        }

        if (EspecificaOtroPsicologicoEntry.Text == "" || EspecificaOtroPsicologicoEntry.Text is null)
        {
            EspecificaOtroPsicologicoEntry.Text = "";
        }

        if (EspecificaOtroRespiratorioEntry.Text == "" || EspecificaOtroRespiratorioEntry.Text is null)
        {
            EspecificaOtroRespiratorioEntry.Text = "";
        }

        if (EspecificaOtroCardiovascularEntry.Text == "" || EspecificaOtroCardiovascularEntry.Text is null)
        {
            EspecificaOtroCardiovascularEntry.Text = "";
        }

        if (TamizajeHipertensionEntry.Text == "" || TamizajeHipertensionEntry.Text is null)
        {
            TamizajeHipertensionEntry.Text = "";
        }

        if (TamizajeDiabetesEntry.Text == "" || TamizajeDiabetesEntry.Text is null)
        {
            TamizajeDiabetesEntry.Text = "";
        }

        if (DeteccionesExploracionMamariaEntry.Text == "" || DeteccionesExploracionMamariaEntry.Text is null)
        {
            DeteccionesExploracionMamariaEntry.Text = "";
        }

        if (ExploracionClinicaMamariaEntry.Text == "" || ExploracionClinicaMamariaEntry.Text is null)
        {
            ExploracionClinicaMamariaEntry.Text = "";
        }

        if (DeteccionesEvaluacionEstadoNutricionalEntry.Text == "" || DeteccionesEvaluacionEstadoNutricionalEntry.Text is null)
        {
            DeteccionesEvaluacionEstadoNutricionalEntry.Text = "";
        }

        if (DeteccionesSaludMentalEntry.Text == "" || DeteccionesSaludMentalEntry.Text is null)
        {
            DeteccionesSaludMentalEntry.Text = "";
        }

        if (DeteccionesRevisionBucalEntry.Text == "" || DeteccionesRevisionBucalEntry.Text is null)
        {
            DeteccionesRevisionBucalEntry.Text = "";
        }

        string respitatorio = "";
        string psicologico = "";
        string genitourinario = "";
        string neurologico = "";
        string musculoesqueletico = "";
        string cardiovascular = "";
        string digestivo = "";
        string referenciasNecesarias = "";
        string estadoGeneral = "";
        string tamizajeHipertension = "";
        string tamizajeDiabetes = ""; 
        string deteccionesExploracionMamaria = ""; 
        string exploracionClinicaMamaria = ""; 
        string deteccionesEvaluacionEstadoNutricional = "";
        string deteccionesSaludMental = ""; 
        string deteccionesRevisionBucal = ""; 

        if (DeteccionesRevisionBucalPicker.SelectedItem.ToString() == "Aplicado")
        {
            deteccionesRevisionBucal = "";
        }
        else
        {
            deteccionesRevisionBucal = DeteccionesRevisionBucalPicker.SelectedItem.ToString();
        }

        if (DeteccionesSaludMentalPicker.SelectedItem.ToString() == "Aplicado")
        {
            deteccionesSaludMental = "";
        }
        else
        {
            deteccionesSaludMental = DeteccionesSaludMentalPicker.SelectedItem.ToString();
        }

        if (DeteccionesEvaluacionEstadoNutricionalPicker.SelectedItem.ToString() == "Aplicado")
        {
            deteccionesEvaluacionEstadoNutricional = "";
        }
        else
        {
            deteccionesEvaluacionEstadoNutricional = DeteccionesEvaluacionEstadoNutricionalPicker.SelectedItem.ToString();
        }

        if (ExploracionClinicaMamariaPicker.SelectedItem.ToString() == "Aplicado")
        {
            exploracionClinicaMamaria = "";
        }
        else
        {
            exploracionClinicaMamaria = ExploracionClinicaMamariaPicker.SelectedItem.ToString();
        }

        if (DeteccionesExploracionMamariaPicker.SelectedItem.ToString() == "Aplicado")
        {
            deteccionesExploracionMamaria = "";
        }
        else
        {
            deteccionesExploracionMamaria = DeteccionesExploracionMamariaPicker.SelectedItem.ToString();
        }

        if (TamizajeDiabetesPicker.SelectedItem.ToString() == "Aplicado")
        {
            tamizajeDiabetes = "";
        }
        else
        {
            tamizajeDiabetes = TamizajeDiabetesPicker.SelectedItem.ToString();
        }

        if (TamizajeHipertensionPicker.SelectedItem.ToString() == "Aplicado")
        {
            tamizajeHipertension = "";
        }
        else
        {
            tamizajeHipertension = TamizajeHipertensionPicker.SelectedItem.ToString();
        }

        if (EstadoGeneralPicker.SelectedItem.ToString() == "Selecciona una opción")
        {
            estadoGeneral = "";
        }
        else
        {
            estadoGeneral = EstadoGeneralPicker.SelectedItem.ToString();
        }

        if (ReferenciasNecesariasPicker.SelectedItem.ToString() == "Selecciona una opción")
        {
            referenciasNecesarias = "";
        }
        else
        {
            referenciasNecesarias = ReferenciasNecesariasPicker.SelectedItem.ToString();
        }

        if (RespiratorioPicker.SelectedItem.ToString() == "Selecciona una opción")
        {
            respitatorio = "";
        }
        else
        {
            respitatorio = RespiratorioPicker.SelectedItem.ToString();
        }

        if (PsicologicoPicker.SelectedItem.ToString() == "Selecciona una opción")
        {
            psicologico = "";
        }
        else
        {
            psicologico = PsicologicoPicker.SelectedItem.ToString();
        }

        if (GenitourinarioPicker.SelectedItem.ToString() == "Selecciona una opción")
        {
            genitourinario = "";
        }
        else
        {
            genitourinario = GenitourinarioPicker.SelectedItem.ToString();
        }

        if (NeurologicoPicker.SelectedItem.ToString() == "Selecciona una opción")
        {
            neurologico = "";
        }
        else
        {
            neurologico = NeurologicoPicker.SelectedItem.ToString();
        }

        if (MusculoesqueleticoPicker.SelectedItem.ToString() == "Selecciona una opción")
        {
            musculoesqueletico = "";
        }
        else
        {
            musculoesqueletico = MusculoesqueleticoPicker.SelectedItem.ToString();
        }

        if (CardiovascularPicker.SelectedItem.ToString() == "Selecciona una opción")
        {
            cardiovascular = "";
        }
        else
        {
            cardiovascular = CardiovascularPicker.SelectedItem.ToString();
        }

        if (DigestivoPicker.SelectedItem.ToString() == "Selecciona una opción")
        {
            digestivo = "";
        }
        else
        {
            digestivo = DigestivoPicker.SelectedItem.ToString();
        }
        
        //.SelectedIndex = 0;

        var encuesta = new Encuesta
        {
            // Id del dispositivo
            DeviceId = DeviceService.GetDeviceId(),

            // 1. Datos Generales del paciente
            NombreCompleto =$"{NombreEntry.Text?.Trim()} {pApellidoEntry.Text?.Trim()} {sApellidoEntry.Text?.Trim()}".Trim(),
            Edad = int.TryParse(EdadEntry.Text, out int edad) ? edad : 0,
            Genero = GeneroPicker.SelectedItem?.ToString() ?? "",
            FechaNacimiento = FechaNacimientoEntry.Date,
            Curp = curp,
            SeccionElectoral = SeccionNoEncontradaCheck.IsChecked? SeccionManualEntry.Text?.Trim(): SeccionPicker.SelectedItem?.ToString(),
            Calle = CallenEntry.Text,
            NumExterior = NumExteriorEntry.Text,
            NumInterior = NumInteriorEntry.Text,
            

            Localidad = LocalidadSelectorEntry.Text.Trim(),
            //Municipio = MunicipioEntry.Text,
     
            CodigoPostal = CodigoPostalEntry.Text,
            Colonia = ColoniaNoEncontradaCheck.IsChecked?ColoniaManualEntry.Text?.Trim(): ColoniaSelectorEntry.Text?.Trim(),
            Municipio = MunicipioPicker.SelectedItem is CodigoPostalCat mun ? mun.Municipio: "",
            Telefono = TelefonoEntry.Text ?? "",
            Celular = CelularEntry.Text ?? "",
            EstadoCivil = EstadoCivilPicker.SelectedItem?.ToString() ?? "",
            Ocupacion = OcupacionEntry.Text,
            PuebloIndigena = PuebloIndigenaPicker.SelectedItem?.ToString() ?? "",
            CualPuebloIndigena = CualPuebloIndigenaEntry.Text,
            IdiomaIndigena = IdiomaIndigenaPicker.SelectedItem?.ToString() ?? "",
            CualIdiomaIndigena = CualIdiomaIndigenaEntry.Text,
            Acompanante = AcompananteEntry.Text,
            RelacionAcompanante = RelacionAcompanantePicker.SelectedItem?.ToString(),
            TelefonoAcompanante = TelefonoAcompananteEntry.Text ?? "",

            // 2. Antecedentes Personales
            HipertensionArterial = HipertensionCheckBox.IsChecked,
            Diabetes = DiabetesCheckBox.IsChecked,
            Obesidad = ObesidadCheckBox.IsChecked,
            EnfermedadCerdiovascular = EnfermedadCerdiovascularCheckBox.IsChecked,
            Dislipidemia = DislipidemiaCheckBox.IsChecked,
            Asma = AsmaCheckBox.IsChecked,
            Epilepsia = EpilepsiaCheckBox.IsChecked,
            Tuberculosis = TuberculosisCheckBox.IsChecked,
            Cancer = CancerCheckBox.IsChecked,
            EspecificaCancer = EspecificaCancerEntry.Text,
            SaludMental = SaludMentalCheckBox.IsChecked,
            EspecificaSaludMental = EspecificaSaludMentalEntry.Text,
            Caries = CariesCheckBox.IsChecked,
            Alergias = AlergiasCheckBox.IsChecked,
            EspecificaAlergias = EspecificaAlergiasEntry.Text,
            CirugiasPrevias = CirugiasPreviasCheckBox.IsChecked,
            EspecificaCirugiasPrevias = EspecificaCirugiasPreviasEntry.Text,
            InfeccionesTransmisionSexual = InfeccionesTransmisionSexualCheckBox.IsChecked,
            EspecificaInfeccionesTransmisionSexual = EspecificaInfeccionesTransmisionSexualEntry.Text,
            MedicamentosActuales = MedicamentosActualesEntry.Text,
            OtrosAntecedentes = OtrosAntecedentesCheckBox.IsChecked,
            EspecificaOtrosAntecedentes = EspecificaOtrosAntecedentesEntry.Text,
            Ginecologicos = GinecologicosCheckBox.IsChecked,
            Menarca = MenarcaEntry.Text,
            InicioVidaSexual = InicioVidaSexualEntry.Text,
            ParejasSexuales = ParejasSexualesEntry.Text,
            Gestas = GestasEntry.Text,
            Partos = PartosEntry.Text,
            Cesareas = CesareasEntry.Text,
            Abortos = AbortosEntry.Text,
            CitologiaCervical = CitologiaCervicalEntry.Text,
            ExploracionMamariaObservaciones = ExploracionMamariaEntry.Text,
            Mastografia = MastografiaEntry.Text,
            PlanificacionFamiliar = PlanificacionFamiliarEntry.Text,

            // 3. Antecedentes Familiares
            AntecedentesHipertension = AntecedentesHipertensionCheckBox.IsChecked,
            ParentescoHipertension = ParentescoHipertensionPicker.SelectedItem?.ToString() ?? "",
            AntecedentesDiabetes = AntecedentesDiabetesCheckBox.IsChecked,
            ParentescoDiabetes = ParentescoDiabetesPicker.SelectedItem?.ToString() ?? "",
            AntecedentesObesidadCheckBox = AntecedentesObesidadCheckBox.IsChecked,
            ParentescoObesidad = ParentescoObesidadPicker.SelectedItem?.ToString() ?? "",
            AntecedentesEnfermedadCardiovascular = AntecedentesEnfermedadCerdiovascularCheckBox.IsChecked,
            ParentescoEnfermedadCardiovascular = ParentescoEnfermedadCardiovascularPicker.SelectedItem?.ToString() ?? "",
            AntecedentesDislipidemia = AntecedentesDislipidemiaCheckBox.IsChecked,
            ParentescoDislipidemian = ParentescoDislipidemiaPicker.SelectedItem?.ToString() ?? "",
            AntecedentesAsma = AntecedentesAsmaCheckBox.IsChecked,
            ParentescoAsma = ParentescoAsmaPicker.SelectedItem?.ToString() ?? "",
            AntecedentesEpilepsia = AntecedentesEpilepsiaCheckBox.IsChecked,
            ParentescoEpilepsia = ParentescoEpilepsiaPicker.SelectedItem?.ToString() ?? "",
            AntecedentesCancer = AntecedentesCancerCheckBox.IsChecked,
            ParentesCancer = ParentescoCancerPicker.SelectedItem?.ToString() ?? "",
            AntecedentesSaludMental = AntecedentesSaludMentalCheckBox.IsChecked,
            ParentescoSaludMental = ParentescoSaludMentalPicker.SelectedItem?.ToString() ?? "",

            // 4. Estilo de vida y factores de riesgo
            Tabaquismo = TabaquismoPicker.SelectedItem?.ToString() ?? "",
            TabaquismoNum = TabaquismoNumEntry.Text,
            ConsumoAlcohol = ConsumoAlcoholPicker.SelectedItem?.ToString() ?? "",
            ConsumoAlcoholFrecuencia = ConsumoAlcoholFrecuenciaEntry.Text,
            ConsumoDrogas = ConsumoDrogasPicker.SelectedItem?.ToString() ?? "",
            ConsumoDrogasFrecuencia = ConsumoDrogasFrecuenciaEntry.Text,
            AcividadFisica = AcividadFisicaPicker.SelectedItem?.ToString() ?? "",
            Alimentacion = AlimentacionPicker.SelectedItem?.ToString() ?? "",
            NivelEstres = NivelEstresPicker.SelectedItem?.ToString() ?? "",
            ApoyoFamiliar = ApoyoFamiliarPicker.SelectedItem?.ToString() ?? "",
            TieneMascotas = TieneMascotasPicker.SelectedItem?.ToString() ?? "",
            EstanVacunados = EstanVacunadosEntry.Text,

            // 5. Evaluación social y condiciones del entorno
            TipoVivienda = TipoViviendaPicker.SelectedItem?.ToString() ?? "",
            CondicionesHogar = CondicionesHogarPicker.SelectedItem?.ToString() ?? "",
            ServicioAgua = ServicioAguaCheckBox.IsChecked,
            ServicioLuz = ServicioLuzCheckBox.IsChecked,
            ServicioGas = ServicioGasCheckBox.IsChecked,
            ServicioDrenaje = ServicioDrenajeCheckBox.IsChecked,
            PersonasMayores = PersonasMayoresCheckBox.IsChecked,
            PersonasDiscapacidad = PersonasDiscapacidadCheckBox.IsChecked,
            Menores5anios = Menores5aniosCheckBox.IsChecked,
            ViolenciaFamiliar = ViolenciaFamiliarCheckBox.IsChecked,
            Abandono = AbandonoCheckBox.IsChecked,
            InseguridadAlimentaria = InseguridadAlimentariaCheckBox.IsChecked,
            Desempleo = DesempleoCheckBox.IsChecked,
            RiegosSocialesObservaciones = RiegosSocialesObservacionesEntry.Text,

            // 6. Motivo de consulta y padecimiento actual 
            MotivoConsulta = MotivoConsultaEditor.Text,

            // 7. Interrogatorio por Aparatos y Sistemas (breve)
            Respiratorio = respitatorio ?? "",//RespiratorioPicker.SelectedItem?.ToString() ?? "",
            EspecificaOtroRespiratorio = EspecificaOtroRespiratorioEntry.Text,
            Cardiovascular = cardiovascular ?? "",//CardiovascularPicker.SelectedItem?.ToString() ?? "",
            EspecificaOtroCardiovascular = EspecificaOtroCardiovascularEntry.Text,
            Digestivo = digestivo ?? "",//DigestivoPicker.SelectedItem?.ToString() ?? "",
            EspecificaOtroDigestivo = EspecificaOtroDigestivoEntry.Text,
            Musculoesqueletico = musculoesqueletico ?? "", //MusculoesqueleticoPicker.SelectedItem?.ToString() ?? "",
            EspecificaOtroMusculoesqueletico = EspecificaOtroMusculoesqueleticoEntry.Text,
            Neurologico = neurologico ?? "",// NeurologicoPicker.SelectedItem?.ToString() ?? "",
            EspecificaOtroNeurologico = EspecificaOtroNeurologicoEntry.Text,
            Genitourinario = genitourinario ?? "",// GenitourinarioPicker.SelectedItem?.ToString() ?? "",
            EspecificaOtroGenitourinario = EspecificaOtroGenitourinarioEntry.Text,
            Psicologico = psicologico ?? "",// PsicologicoPicker.SelectedItem?.ToString() ?? "",
            EspecificaOtroPsicologico = EspecificaOtroPsicologicoEntry.Text,

            // 8. Examen físico
            TensionArterial = TensionArterialEntry.Text,
            FrecuenciaCardiaca = FrecuenciaCardiacaEntry.Text,//int.TryParse(FrecuenciaCardiacaEntry.Text, out int fcardiaca) ? fcardiaca : 0,// FrecuenciaCardiacaEntry.Text,
            Temperatura = TemperaturaEntry.Text,
            //Temperatura = decimal.TryParse(TemperaturaEntry.Text, out decimal temp) ? temp : 0,//TemperaturaEntry.Text,
            FrecuenciaRespiratoria = FrecuenciaRespiratoriaEntry.Text,//int.TryParse(FrecuenciaRespiratoriaEntry.Text, out int fresp) ? fresp : 0,//FrecuenciaRespiratoriaEntry.Text,
            SaturacionO2 = SaturacionO2Entry.Text,//  int.TryParse(SaturacionO2Entry.Text, out int sat) ? sat : 0,//SaturacionO2Entry.Text,
                                                  //Peso = decimal.TryParse(PesoEntry.Text, out decimal p) ? p : 0, //PesoEntry.Text,
                                                  //Talla = decimal.TryParse(TallaEntry.Text, out decimal t) ? t : 0, //TallaEntry.Text,
                                                  //Imc = decimal.TryParse(ImcEntry.Text, out decimal i) ? i : 0, //ImcEntry.Text,
            Peso = PesoEntry.Text,
            Talla = TallaEntry.Text,
            Imc = ImcEntry.Text,
            ImcPicker = ImcPicker.SelectedItem?.ToString() ?? "",
            EstadoGeneral = estadoGeneral ?? "",//EstadoGeneralPicker.SelectedItem?.ToString() ?? "",
            ExploracionFisica = ExploracionFisicaEditor.Text,

            // 9. Detecciones (aplicar según edad y género)
            TamizajeHipertension = tamizajeHipertension ?? "",//TamizajeHipertensionPicker.SelectedItem?.ToString() ?? "",
            TamizajeHipertensionObservaciones = TamizajeHipertensionEntry.Text,
            TamizajeDiabetes = tamizajeDiabetes ?? "",//TamizajeDiabetesPicker.SelectedItem?.ToString() ?? "",
            TamizajeDiabetesObservaciones = TamizajeDiabetesEntry.Text,
            DeteccionesExploracionMamaria = deteccionesExploracionMamaria ?? "",//DeteccionesExploracionMamariaPicker.SelectedItem?.ToString() ?? "",
            DeteccionesExploracionMamariaObservaciones = DeteccionesExploracionMamariaEntry.Text,
            DeteccionesExploracionClinicaMamaria = exploracionClinicaMamaria ?? "", //ExploracionClinicaMamariaPicker.SelectedItem?.ToString() ?? "",
            DeteccionesExploracionClinicaMamariaObservaciones = ExploracionClinicaMamariaEntry.Text,
            DeteccionesEvaluacionEstadoNutricional = deteccionesEvaluacionEstadoNutricional ?? "",//DeteccionesEvaluacionEstadoNutricionalPicker.SelectedItem?.ToString() ?? "",
            DeteccionesEvaluacionEstadoNutricionalObservaciones = DeteccionesEvaluacionEstadoNutricionalEntry.Text,
            DeteccionesSaludMental = deteccionesSaludMental ?? "", //DeteccionesSaludMentalPicker.SelectedItem?.ToString() ?? "",
            DeteccionesSaludMentalObservaciones = DeteccionesSaludMentalEntry.Text,
            DeteccionesRevisionBucal = deteccionesRevisionBucal ?? "",//DeteccionesRevisionBucalPicker.SelectedItem?.ToString() ?? "",
            DeteccionesRevisionBucalObservaciones = DeteccionesRevisionBucalEntry.Text,

            // 10. Impresión Diagnóstica / Problemas Identificados
            ImpresionDiagnostica1 = ImpresionDiagnostica1Entry.Text,
            ImpresionDiagnostica2 = ImpresionDiagnostica2Entry.Text,
            ImpresionDiagnostica3 = ImpresionDiagnostica3Entry.Text,


            // 11. Plan de manejo
            TratamientoMedicamentosEntregados = TratamientoMedicamentosEntregadosEditor.Text,
            EntregaSuplementosInsumos = EntregaSuplementosInsumosEditor.Text,
            EducacionSalud = EducacionSaludEditor.Text,
            ReferenciasNecesarias = referenciasNecesarias ?? "",//ReferenciasNecesariasPicker.SelectedItem?.ToString() ?? "",
            Lugar = LugarEntry.Text,
            VisitaSeguimiento = VisitaSeguimientoPicker.SelectedItem?.ToString() ?? "",
            FechaSugerida = FechaSugeridaEntry.Date,

            // 12. Profesional que Realiza la Atención
            NombreEncuestador = NombreEncuestadorEntry.Text,
            CargoEncuestador = CargoEncuestadorPicker.SelectedItem?.ToString() ?? "",
            DisposicionEncuestado = DisposicionEncuestadoPicker.SelectedItem?.ToString() ?? "",
            FechaEncuesta = DateTime.Now,

            // Id del usuario logueado
            UsuarioId = App.UsuarioLogueadoId,

            // Geolocalización
            Latitud = location?.Latitude,
            Longitud = location?.Longitude
        };

        // Validar el modelo
        var errores = ValidarModelo(encuesta);
        if (errores.Any())
        {
            // revisa ocultar campos
            if (PuebloIndigenaPicker.SelectedItem is not null)
                if (PuebloIndigenaPicker.SelectedItem.ToString() == "Sí")
                    CualPuebloIndigenaEntry.IsVisible = true;
                else
                    CualPuebloIndigenaEntry.IsVisible = false;

            if (IdiomaIndigenaPicker.SelectedItem is not null)
                if (IdiomaIndigenaPicker.SelectedItem.ToString() == "Sí")
                    CualIdiomaIndigenaEntry.IsVisible = true;
                else
                    CualIdiomaIndigenaEntry.IsVisible = false;

            if (CancerCheckBox.IsChecked)
                EspecificaCancerEntry.IsVisible = true;
            else
                EspecificaCancerEntry.IsVisible = false;

            if (SaludMentalCheckBox.IsChecked)
                EspecificaSaludMentalEntry.IsVisible = true;
            else
                EspecificaSaludMentalEntry.IsVisible = false;

            if (AlergiasCheckBox.IsChecked)
                EspecificaAlergiasEntry.IsVisible = true;
            else
                EspecificaAlergiasEntry.IsVisible = false;

            if (CirugiasPreviasCheckBox.IsChecked)
                EspecificaCirugiasPreviasEntry.IsVisible = true;
            else
                EspecificaCirugiasPreviasEntry.IsVisible = false;

            if (InfeccionesTransmisionSexualCheckBox.IsChecked)
            {
                EspecificaInfeccionesTransmisionSexualEntry.IsVisible = true;
                MedicamentosActualesEntry.IsVisible = true;
                textMedicamentosActualesEntry.IsVisible = true;
            }
            else
            {
                EspecificaInfeccionesTransmisionSexualEntry.IsVisible = false;
                MedicamentosActualesEntry.IsVisible = false;
                textMedicamentosActualesEntry.IsVisible = false;
            }

            if (OtrosAntecedentesCheckBox.IsChecked)
                EspecificaOtrosAntecedentesEntry.IsVisible = true;
            else
                EspecificaOtrosAntecedentesEntry.IsVisible = false;

            if (AntecedentesHipertensionCheckBox.IsChecked)
                ParentescoHipertensionPicker.IsVisible = true;
            else
                ParentescoHipertensionPicker.IsVisible = false;

            if (AntecedentesDiabetesCheckBox.IsChecked)
                ParentescoDiabetesPicker.IsVisible = true;
            else
                ParentescoDiabetesPicker.IsVisible = false;

            if (AntecedentesObesidadCheckBox.IsChecked)
                ParentescoObesidadPicker.IsVisible = true;
            else
                ParentescoObesidadPicker.IsVisible = false;

            if (AntecedentesEnfermedadCerdiovascularCheckBox.IsChecked)
                ParentescoEnfermedadCardiovascularPicker.IsVisible = true;
            else
                ParentescoEnfermedadCardiovascularPicker.IsVisible = false;

            if (AntecedentesDislipidemiaCheckBox.IsChecked)
                ParentescoDislipidemiaPicker.IsVisible = true;
            else
                ParentescoDislipidemiaPicker.IsVisible = false;

            if (AntecedentesAsmaCheckBox.IsChecked)
                ParentescoAsmaPicker.IsVisible = true;
            else
                ParentescoAsmaPicker.IsVisible = false;

            if (AntecedentesEpilepsiaCheckBox.IsChecked)
                ParentescoEpilepsiaPicker.IsVisible = true;
            else
                ParentescoEpilepsiaPicker.IsVisible = false;

            if (AntecedentesCancerCheckBox.IsChecked)
                ParentescoCancerPicker.IsVisible = true;
            else
                ParentescoCancerPicker.IsVisible = false;

            if (AntecedentesSaludMentalCheckBox.IsChecked)
                ParentescoSaludMentalPicker.IsVisible = true;
            else
                ParentescoSaludMentalPicker.IsVisible = false;

            if (TabaquismoPicker.SelectedItem is not null)
                if (TabaquismoPicker.SelectedItem.ToString() == "Sí")
                    TabaquismoPicker.IsVisible = true;
                else
                    TabaquismoPicker.IsVisible = false;

            if (ConsumoAlcoholPicker.SelectedItem is not null)
                if (ConsumoAlcoholPicker.SelectedItem.ToString() == "Sí")
                    ConsumoAlcoholFrecuenciaEntry.IsVisible = true;
                else
                    ConsumoAlcoholFrecuenciaEntry.IsVisible = false;

            if (ConsumoDrogasPicker.SelectedItem is not null)
                if (ConsumoDrogasPicker.SelectedItem.ToString() == "Sí")
                    ConsumoDrogasFrecuenciaEntry.IsVisible = true;
                else
                    ConsumoDrogasFrecuenciaEntry.IsVisible = false;

            if (TieneMascotasPicker.SelectedItem is not null)
                if (TieneMascotasPicker.SelectedItem.ToString() == "Sí")
                {
                    EstanVacunadosContainer.IsVisible = true;
                    textEstanVacunadosEntry.IsVisible = true;
                }
                else
                {
                    EstanVacunadosContainer.IsVisible = false;
                    textEstanVacunadosEntry.IsVisible = false;
                }
            

            if (RespiratorioPicker.SelectedItem is not null)
                if (RespiratorioPicker.SelectedItem.ToString() == "Otro")
                EspecificaOtroRespiratorioEntry.IsVisible = true;
                else
                    EspecificaOtroRespiratorioEntry.IsVisible = false;

            if (CardiovascularPicker.SelectedItem is not null)
                if (CardiovascularPicker.SelectedItem.ToString() == "Otro")
                   EspecificaOtroCardiovascularEntry.IsVisible = true;
                else
                    EspecificaOtroCardiovascularEntry.IsVisible = false;


            if (DigestivoPicker.SelectedItem is not null)
                if (DigestivoPicker.SelectedItem.ToString() == "Otro")
                   EspecificaOtroDigestivoEntry.IsVisible = true;
                else
                    EspecificaOtroDigestivoEntry.IsVisible = false;

            if (MusculoesqueleticoPicker.SelectedItem is not null)
                if (MusculoesqueleticoPicker.SelectedItem.ToString() == "Otro")
                  EspecificaOtroMusculoesqueleticoEntry.IsVisible = true;
                else
                    EspecificaOtroMusculoesqueleticoEntry.IsVisible = false;

            if (NeurologicoPicker.SelectedItem is not null)
                if (NeurologicoPicker.SelectedItem.ToString() == "Otro")
                    EspecificaOtroNeurologicoEntry.IsVisible = true;
                else
                    EspecificaOtroNeurologicoEntry.IsVisible = false;


            if (GenitourinarioPicker.SelectedItem is not null)
                if (GenitourinarioPicker.SelectedItem.ToString() == "Otro")
                   EspecificaOtroGenitourinarioEntry.IsVisible = true;
                else
                    EspecificaOtroGenitourinarioEntry.IsVisible = false;


            if (PsicologicoPicker.SelectedItem is not null)
                if (PsicologicoPicker.SelectedItem.ToString() == "Otro")
                    EspecificaOtroPsicologicoEntry.IsVisible = true;
                else
                    EspecificaOtroPsicologicoEntry.IsVisible = false;

            mensaje = string.Join("\n", errores.Select(e => "• " + e.ErrorMessage));
            await DisplayAlert("Errores de validación", mensaje, "OK");
            return; // no guarda si no pasa validación
        }
        
        await _db.SaveEncuestaAsync(encuesta);

        await DisplayAlert("Éxito", "Encuesta guardada localmente", "OK");
        await Navigation.PopAsync(); // <- regresa a la lista
        }
        catch (Exception ex)
        {
            await DisplayAlert("Aviso", "Hubo un error al guardar la encuesta: " + ex.Message, "OK");
        }
        finally
        {
            MostrarLoading(false);
        }
    }

    void OnCheckBoxCheckedChangedCancerCheckBox(object sender, CheckedChangedEventArgs e)
    {
        var checkbox = (CheckBox)sender;

        bool valor = e.Value;
        if (valor)

        {
            EspecificaCancerEntry.IsVisible = true;
        }
        else
        {
            EspecificaCancerEntry.IsVisible = false;
        }
    }

    void OnCheckBoxCheckedChangedSaludMental(object sender, CheckedChangedEventArgs e)
    {
        var checkbox = (CheckBox)sender;

        bool valor = e.Value;
        if (valor)

        {
            EspecificaSaludMentalEntry.IsVisible = true;
        }
        else
        {
            EspecificaSaludMentalEntry.IsVisible = false;
        }
    }

    void OnCheckBoxCheckedChangedInfeccionesTransmisionSexual(object sender, CheckedChangedEventArgs e)
    {
        var checkbox = (CheckBox)sender;

        bool valor = e.Value;
        if (valor)

        {
            EspecificaInfeccionesTransmisionSexualEntry.IsVisible = true;
            MedicamentosActualesEntry.IsVisible = true;
            textMedicamentosActualesEntry.IsVisible = true;
        }
        else
        {
            EspecificaInfeccionesTransmisionSexualEntry.IsVisible = false;
            MedicamentosActualesEntry.IsVisible = false;
            textMedicamentosActualesEntry.IsVisible = false;
        }
    }

    void OnCheckBoxCheckedChangedInfeccionesOtrosAntecedentes(object sender, CheckedChangedEventArgs e)
    {
        var checkbox = (CheckBox)sender;

        bool valor = e.Value;
        if (valor)

        {
            EspecificaOtrosAntecedentesEntry.IsVisible = true;
        }
        else
        {
            EspecificaOtrosAntecedentesEntry.IsVisible = false;
        }
    }

    void OnCheckBoxCheckedChangedInfeccionesGinecologicos(object sender, CheckedChangedEventArgs e)
    {
        var checkbox = (CheckBox)sender;

        bool valor = e.Value;
        if (valor)

        {
            textMenarcaEntry.IsVisible = true;
            MenarcaEntry.IsVisible = true;
            textInicioVidaSexualEntry.IsVisible = true;
            InicioVidaSexualEntry.IsVisible = true;
            textParejasSexualesEntry.IsVisible = true;
            ParejasSexualesEntry.IsVisible = true;

            textGestasEntry.IsVisible = true;
            GestasEntry.IsVisible = true;

            textPartosEntry.IsVisible = true;
            PartosEntry.IsVisible = true;

            textCesareasEntry.IsVisible = true;
            CesareasEntry.IsVisible = true;

            textAbortosEntry.IsVisible = true;
            AbortosEntry.IsVisible = true;

            textCitologiaCervicalEntry.IsVisible = true;
            CitologiaCervicalEntry.IsVisible = true;

            textExploracionMamariaEntry.IsVisible = true;
            ExploracionMamariaEntry.IsVisible = true;

            textMastografiaEntry.IsVisible = true;
            MastografiaEntry.IsVisible = true;

            textPlanificacionFamiliarEntry.IsVisible = true;
            PlanificacionFamiliarEntry.IsVisible = true;
        }
        else
        {
            textMenarcaEntry.IsVisible = false;
            MenarcaEntry.IsVisible = false;
            textInicioVidaSexualEntry.IsVisible = false;
            InicioVidaSexualEntry.IsVisible = false;
            textParejasSexualesEntry.IsVisible = false;
            ParejasSexualesEntry.IsVisible = false;
            textGestasEntry.IsVisible = false;
            GestasEntry.IsVisible = false;
            textPartosEntry.IsVisible = false;
            PartosEntry.IsVisible = false;
            textCesareasEntry.IsVisible = false;
            CesareasEntry.IsVisible = false;
            textAbortosEntry.IsVisible = false;
            AbortosEntry.IsVisible = false;
            textCitologiaCervicalEntry.IsVisible = false;
            CitologiaCervicalEntry.IsVisible = false;
            textExploracionMamariaEntry.IsVisible = false;
            ExploracionMamariaEntry.IsVisible = false;
            MastografiaEntry.IsVisible = false;
            textMastografiaEntry.IsVisible = false;
            textPlanificacionFamiliarEntry.IsVisible = false;
            PlanificacionFamiliarEntry.IsVisible = false;
        }
    }

    void OnCheckBoxCheckedChangedCirugiasPrevias(object sender, CheckedChangedEventArgs e)
    {
        var checkbox = (CheckBox)sender;

        bool valor = e.Value;
        if (valor)

        {
            EspecificaCirugiasPreviasEntry.IsVisible = true;
        }
        else
        {
            EspecificaCirugiasPreviasEntry.IsVisible = false;
        }
    }

    void OnCheckBoxCheckedChangedAlergias(object sender, CheckedChangedEventArgs e)
    {
        var checkbox = (CheckBox)sender;

        bool valor = e.Value;
        if (valor)

        {
            EspecificaAlergiasEntry.IsVisible = true;
        }
        else
        {
            EspecificaAlergiasEntry.IsVisible = false;
        }
    }

    private void OnEventIndexChangedVisitaSeguimientoPicker(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex != -1)
        {
            var selected = picker.Items[selectedIndex];
            if (selected == "Sí")
            {
                FechaSugeridaEntry.IsVisible = true;
            }
            else
            {
                FechaSugeridaEntry.IsVisible = false;
            }
        }
    }

    private void OnEventIndexChangedDeteccionesRevisionBucalPicker(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex != -1)
        {
            var selected = picker.Items[selectedIndex];
            if (selected == "Sí")
            {
                DeteccionesRevisionBucalEntry.IsVisible = true;
            }
            else
            {
                DeteccionesRevisionBucalEntry.IsVisible = false;
            }
        }
    }

    private void OnEventIndexChangedDeteccionesSaludMentalPicker(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex != -1)
        {
            var selected = picker.Items[selectedIndex];
            if (selected == "Sí")
            {
                DeteccionesSaludMentalEntry.IsVisible = true;
            }
            else
            {
                DeteccionesSaludMentalEntry.IsVisible = false;
            }
        }
    }

    private void OnEventIndexChangedDeteccionesEvaluacionEstadoNutricionalPicker(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex != -1)
        {
            var selected = picker.Items[selectedIndex];
            if (selected == "Sí")
            {
                DeteccionesEvaluacionEstadoNutricionalEntry.IsVisible = true;
            }
            else
            {
                DeteccionesEvaluacionEstadoNutricionalEntry.IsVisible = false;
            }       
        }
    }

    private void OnEventIndexChangedExploracionClinicaMamariaPicker(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex != -1)
        {
            var selected = picker.Items[selectedIndex];
            if (selected == "Sí")
            {
                ExploracionClinicaMamariaEntry.IsVisible = true;
            }
            else
            {
                ExploracionClinicaMamariaEntry.IsVisible = false;
            }     
        }
    }

    private void OnEventIndexChangedDeteccionesExploracionMamariaPicker(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex != -1)
        {
            var selected = picker.Items[selectedIndex];
            if (selected == "Sí")
            {
                DeteccionesExploracionMamariaEntry.IsVisible = true;
            }
            else
            {
                DeteccionesExploracionMamariaEntry.IsVisible = false;
            }      
        }
    }

    private void OnEventIndexChangedTamizajeDiabetesPicker(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex != -1)
        {
            var selected = picker.Items[selectedIndex];
            if (selected == "Sí")
            {
                TamizajeDiabetesEntry.IsVisible = true;
            }
            else
            {
                TamizajeDiabetesEntry.IsVisible = false;
            }        
        }
    }

    private void OnEventIndexChangedTamizajeHipertensionPicker(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex != -1)
        {
            var selected = picker.Items[selectedIndex];
            if (selected == "Sí")
            {
                TamizajeHipertensionEntry.IsVisible = true;
            }
            else
            {
                TamizajeHipertensionEntry.IsVisible = false;
            }        
        }
    }
    private void OnEventIndexChangedPsicologicoPicker(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex != -1)
        {
            var selected = picker.Items[selectedIndex];
            if (selected == "Otro")
            {
                EspecificaOtroPsicologicoEntry.IsVisible = true;
            }
            else
            {
                EspecificaOtroPsicologicoEntry.IsVisible = false;
            }      
        }
    }

    private void OnEventIndexChangedGenitourinarioPicker(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex != -1)
        {
            var selected = picker.Items[selectedIndex];
            if (selected == "Otro")
            {
                EspecificaOtroGenitourinarioEntry.IsVisible = true;
            }
            else
            {
                EspecificaOtroGenitourinarioEntry.IsVisible = false;
            }
        }
    }

    private void OnEventIndexChangedNeurologicoPicker(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex != -1)
        {
            var selected = picker.Items[selectedIndex];
            if (selected == "Otro")
            {
                EspecificaOtroNeurologicoEntry.IsVisible = true;
            }
            else
            {
                EspecificaOtroNeurologicoEntry.IsVisible = false;
            }         
        }
    }

    private void OnEventIndexChangedMusculoesqueleticoPicker(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex != -1)
        {
            var selected = picker.Items[selectedIndex];
            if (selected == "Otro")
            {
                EspecificaOtroMusculoesqueleticoEntry.IsVisible = true;
            }
            else
            {
                EspecificaOtroMusculoesqueleticoEntry.IsVisible = false;
            }   
        }
    }

    private void OnEventIndexChangedDigestivoPicker(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex != -1)
        {
            var selected = picker.Items[selectedIndex];
            if (selected == "Otro")
            {
                EspecificaOtroDigestivoEntry.IsVisible = true;
            }
            else
            {
                EspecificaOtroDigestivoEntry.IsVisible = false;
            }           
        }
    }

    private void OnEventIndexChangedCardiovascularPicker(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex != -1)
        {
            var selected = picker.Items[selectedIndex];
            if (selected == "Otro")
            {
                EspecificaOtroCardiovascularEntry.IsVisible = true;
            }
            else
            {
                EspecificaOtroCardiovascularEntry.IsVisible = false;
            }         
        }
    }

    private void OnEventIndexChangedRespiratorioPicker(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex != -1)
        {
            var selected = picker.Items[selectedIndex];
            if (selected == "Otro")
            {
                EspecificaOtroRespiratorioEntry.IsVisible = true;
            }
            else
            {
                EspecificaOtroRespiratorioEntry.IsVisible = false;
            }
        }
    }

    private void OnEventIndexChangedTieneMascotasPicker(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex != -1)
        {
            var selected = picker.Items[selectedIndex];
            if (selected == "Sí")
            {
                EstanVacunadosContainer.IsVisible = true;
            }
            else
            {
                EstanVacunadosContainer.IsVisible = false;
            }          
        }
    }

    private void OnEventIndexChangedConsumoAlcoholPicker(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex != -1)
        {
            var selected = picker.Items[selectedIndex];
            if (selected == "Sí")
            {
                ConsumoAlcoholFrecuenciaEntry.IsVisible = true;
            }
            else
            {
                ConsumoAlcoholFrecuenciaEntry.IsVisible = false;
            }         
        }
    }

    private void OnEventIndexChangedConsumoDrogasPicker(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex != -1)
        {
            var selected = picker.Items[selectedIndex];
            if (selected == "Sí")
            {
                ConsumoDrogasFrecuenciaEntry.IsVisible = true;
            }
            else
            {
                ConsumoDrogasFrecuenciaEntry.IsVisible = false;
            }          
        }
    }

    private void OnEventIndexChangedTabaquismoPicker(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex != -1)
        {
            var selected = picker.Items[selectedIndex];
            if (selected == "Sí")
            {
                TabaquismoNumEntry.IsVisible = true;
            }
            else
            {
                TabaquismoNumEntry.IsVisible = false;
            } 
        }
    }

    private void OnEventIndexChangedIdiomaIndigenaPicker(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex != -1)
        {
            var selected = picker.Items[selectedIndex];
            if (selected == "Sí")
            {
                CualIdiomaIndigenaEntry.IsVisible = true;
            }
            else
            {
                CualIdiomaIndigenaEntry.IsVisible = false;
            }          
        }
    }
    
    private void OnEventIndexChangedGeneroPicker(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex != -1)
        {
            var selected = picker.Items[selectedIndex];
            if (selected == "Masculino")
            {
                GinecologicosCheckBox.IsVisible = false;
                textGinecologicosCheckBox.IsVisible = false;
            }
            else
            {
                GinecologicosCheckBox.IsVisible = true;
                textGinecologicosCheckBox.IsVisible = true;
            }
        }
        ValidarFormularioCURP();
    }

    private void OnEventIndexChangedPuebloIndigenaPicker(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex != -1)
        {
            var selected = picker.Items[selectedIndex];
            if (selected == "Sí")
            {
                CualPuebloIndigenaEntry.IsVisible = true;
            }
            else
            {
                CualPuebloIndigenaEntry.IsVisible = false;
            }           
        }
    }

    void OnCheckBoxAntecedentesHipertensionCheckBox(object sender, CheckedChangedEventArgs e)
    {
        var checkbox = (CheckBox)sender;

        bool valor = e.Value;
        if (valor)

        {
            ParentescoHipertensionPicker.IsVisible = true;
        }
        else
        {
            ParentescoHipertensionPicker.IsVisible = false;
        }
    }

    void OnCheckBoxCheckedChangedAntecedentesDiabetesCheckBox(object sender, CheckedChangedEventArgs e)
    {
        var checkbox = (CheckBox)sender;

        bool valor = e.Value;
        if (valor)

        {
            ParentescoDiabetesPicker.IsVisible = true;
        }
        else
        {
            ParentescoDiabetesPicker.IsVisible = false;
        }
    }

    void OnCheckBoxCheckedChangedAntecedentesObesidadCheckBox(object sender, CheckedChangedEventArgs e)
    {
        var checkbox = (CheckBox)sender;

        bool valor = e.Value;
        if (valor)

        {
            ParentescoObesidadPicker.IsVisible = true;
        }
        else
        {
            ParentescoObesidadPicker.IsVisible = false;
        }
    }

    void OnCheckBoxCheckedChangedAntecedentesEnfermedadCerdiovascularCheckBox(object sender, CheckedChangedEventArgs e)
    {
        var checkbox = (CheckBox)sender;

        bool valor = e.Value;
        if (valor)

        {
            ParentescoEnfermedadCardiovascularPicker.IsVisible = true;
        }
        else
        {
            ParentescoEnfermedadCardiovascularPicker.IsVisible = false;
        }
    }

    void OnCheckBoxCheckedChangedAntecedentesDislipidemiaCheckBox(object sender, CheckedChangedEventArgs e)
    {
        var checkbox = (CheckBox)sender;

        bool valor = e.Value;
        if (valor)

        {
            ParentescoDislipidemiaPicker.IsVisible = true;
        }
        else
        {
            ParentescoDislipidemiaPicker.IsVisible = false;
        }
    }

    void OnCheckBoxCheckedChangedAntecedentesAsmaCheckBox(object sender, CheckedChangedEventArgs e)
    {
        var checkbox = (CheckBox)sender;

        bool valor = e.Value;
        if (valor)

        {
            ParentescoAsmaPicker.IsVisible = true;
        }
        else
        {
            ParentescoAsmaPicker.IsVisible = false;
        }
    }

    void OnCheckBoxCheckedChangedAntecedentesEpilepsiaCheckBox(object sender, CheckedChangedEventArgs e)
    {
        var checkbox = (CheckBox)sender;

        bool valor = e.Value;
        if (valor)

        {
            ParentescoEpilepsiaPicker.IsVisible = true;
        }
        else
        {
            ParentescoEpilepsiaPicker.IsVisible = false;
        }
    }

    void OnCheckBoxCheckedChangedAntecedentesCancerCheckBox(object sender, CheckedChangedEventArgs e)
    {
        var checkbox = (CheckBox)sender;

        bool valor = e.Value;
        if (valor)

        {
            ParentescoCancerPicker.IsVisible = true;
        }
        else
        {
            ParentescoCancerPicker.IsVisible = false;
        }
    }

    void OnCheckBoxCheckedChangedAntecedentesSaludMentalCheckBoxx(object sender, CheckedChangedEventArgs e)
    {
        var checkbox = (CheckBox)sender;

        bool valor = e.Value;
        if (valor)

        {
            ParentescoSaludMentalPicker.IsVisible = true;
        }
        else
        {
            ParentescoSaludMentalPicker.IsVisible = false;
        }
    }

    private void ConfigurarCalculoIMC()
    {
        // Cuando cambie el peso o la talla, calcular IMC automáticamente
        PesoEntry.TextChanged += (s, e) => CalcularIMC();
        TallaEntry.TextChanged += (s, e) => CalcularIMC();
        TallaEntry.Unfocused += (s, e) => CalcularIMC(); // Calcular también al perder foco
    }

    private void CalcularIMC()
    {
        try
        {
            // Validar que ambos campos tengan valores
            if (string.IsNullOrWhiteSpace(PesoEntry.Text) || string.IsNullOrWhiteSpace(TallaEntry.Text))
            {
                ImcEntry.Text = "";
                ImcPicker.SelectedItem = null;
                return;
            }

            // Convertir a números
            if (decimal.TryParse(PesoEntry.Text, out decimal peso) &&
                decimal.TryParse(TallaEntry.Text, out decimal tallaMetros))
            {
                // Validar que los valores sean positivos
                if (peso <= 0 || tallaMetros <= 0)
                {
                    ImcEntry.Text = "";
                    ImcPicker.SelectedItem = null;
                    return;
                }

                // Calcular IMC: peso / (talla²)
                decimal imc = peso / (tallaMetros * tallaMetros);

                // Mostrar IMC con 2 decimales
                ImcEntry.Text = imc.ToString("F2");

                // Determinar categoría automáticamente
                DeterminarCategoriaIMC(imc);
            }
        }
        catch (Exception ex)
        {
            // En caso de error, limpiar campos
            ImcEntry.Text = "";
            ImcPicker.SelectedItem = null;
            System.Diagnostics.Debug.WriteLine($"Error calculando IMC: {ex.Message}");
        }
    }

    private void DeterminarCategoriaIMC(decimal imc)
    {
        // Clasificación según OMS
        if (imc < 18.5m)
            ImcPicker.SelectedItem = "Bajo peso";
        else if (imc >= 18.5m && imc < 25.0m)
            ImcPicker.SelectedItem = "Normal";
        else if (imc >= 25.0m && imc < 30.0m)
            ImcPicker.SelectedItem = "Sobrepeso";
        else
            ImcPicker.SelectedItem = "Obesidad";
    }

    private void OcupacionEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        //Regular expressions are used to match whether the user input contains numbers
        string result = System.Text.RegularExpressions.Regex.Replace(e.NewTextValue, @"[^0-9]+", "");
        if (Regex.Match(result, @"^[0-9]+$").Success)
        {
            var entry = sender as Entry;
            if (e.OldTextValue == null)
            {
                entry.Text = string.Empty;
            }
            else
            {
                entry.Text = e.OldTextValue;
            }
        }
    }
  
    private void CualPuebloIndigenaEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        //Regular expressions are used to match whether the user input contains numbers
        string result = System.Text.RegularExpressions.Regex.Replace(e.NewTextValue, @"[^0-9]+", "");
        if (Regex.Match(result, @"^[0-9]+$").Success)
        {
            var entry = sender as Entry;
            if (e.OldTextValue == null)
            {
                entry.Text = string.Empty;
            }
            else
            {
                entry.Text = e.OldTextValue;
            }
        }
    }
    
   private void NombreEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        //Regular expressions are used to match whether the user input contains numbers
        string result = System.Text.RegularExpressions.Regex.Replace(e.NewTextValue, @"[^0-9]+", "");
        if (Regex.Match(result, @"^[0-9]+$").Success)
        {
            var entry = sender as Entry;
            if (e.OldTextValue == null)
            {
                entry.Text = string.Empty;
            }
            else
            {
                entry.Text = e.OldTextValue;
            }
        }
        ValidarFormularioCURP();
       

    }
    private void AcompananteEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        //Regular expressions are used to match whether the user input contains numbers
        string result = System.Text.RegularExpressions.Regex.Replace(e.NewTextValue, @"[^0-9]+", "");
        if (Regex.Match(result, @"^[0-9]+$").Success)
        {
            var entry = sender as Entry;
            if (e.OldTextValue == null)
            {
                entry.Text = string.Empty;
            }
            else
            {
                entry.Text = e.OldTextValue;
            }
        }
    }

    private void RelacionAcompananteEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        //Regular expressions are used to match whether the user input contains numbers
        string result = System.Text.RegularExpressions.Regex.Replace(e.NewTextValue, @"[^0-9]+", "");
        if (Regex.Match(result, @"^[0-9]+$").Success)
        {
            var entry = sender as Entry;
            if (e.OldTextValue == null)
            {
                entry.Text = string.Empty;
            }
            else
            {
                entry.Text = e.OldTextValue;
            }
        }
    }

    private void NombreEncuestadorEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        //Regular expressions are used to match whether the user input contains numbers
        string result = System.Text.RegularExpressions.Regex.Replace(e.NewTextValue, @"[^0-9]+", "");
        if (Regex.Match(result, @"^[0-9]+$").Success)
        {
            var entry = sender as Entry;
            if (e.OldTextValue == null)
            {
                entry.Text = string.Empty;
            }
            else
            {
                entry.Text = e.OldTextValue;
            }
        }
    }
    
    private void CualIdiomaIndigenaEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        //Regular expressions are used to match whether the user input contains numbers
        string result = System.Text.RegularExpressions.Regex.Replace(e.NewTextValue, @"[^0-9]+", "");
        if (Regex.Match(result, @"^[0-9]+$").Success)
        {
            var entry = sender as Entry;
            if (e.OldTextValue == null)
            {
                entry.Text = string.Empty;
            }
            else
            {
                entry.Text = e.OldTextValue;
            }
        }
    }

    private void TabaquismoNumEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        //Regular expressions are used to match whether the user input contains numbers
        string result = System.Text.RegularExpressions.Regex.Replace(e.NewTextValue, @"[A-Za-záéíóúÁÉÍÓÚñÑ\s]+", "");
        if (Regex.Match(result, @"^[A-Za-záéíóúÁÉÍÓÚñÑ\s]+$").Success)
        {
            var entry = sender as Entry;
            if (e.OldTextValue == null)
            {
                entry.Text = string.Empty;
            }
            else
            {
                entry.Text = e.OldTextValue;
            }
        }
    }

 
    private async void OnCodigoPostalChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue) || e.NewTextValue.Length != 5)
            return;

        var cp = e.NewTextValue.Trim();

        _registrosPorCP = await _db.GetColoniasByCPAsync(cp);

        if (_registrosPorCP == null || _registrosPorCP.Count == 0)
        {
            ColoniaSelectorEntry.Text = string.Empty;
            SeccionPicker.ItemsSource = null;

            //CARGAR TODOS LOS MUNICIPIOS DE SLP
            _municipiosPorCP = await _db.GetTodosLosMunicipiosAsync();

            MunicipioPicker.ItemsSource = _municipiosPorCP;
            MunicipioPicker.ItemDisplayBinding = new Binding("Municipio");
            MunicipioPicker.SelectedIndex = -1;

            _cveMunSeleccionado = null;
            _localidadesActuales = null;
            return;
        }


        _municipiosPorCP = _registrosPorCP
            .GroupBy(x => new { x.Municipio, x.cve_mun })
            .Select(g => g.First())
            .OrderBy(x => x.Municipio)
            .ToList();

        
        MunicipioPicker.ItemsSource = _municipiosPorCP;
        MunicipioPicker.ItemDisplayBinding = new Binding("Municipio");
        MunicipioPicker.SelectedIndex = 0;

       
        ColoniaSelectorEntry.Text = string.Empty;
        SeccionPicker.ItemsSource = null;
    }


    private async void OnMunicipioChanged(object sender, EventArgs e)
    {
        if (MunicipioPicker.SelectedItem is not CodigoPostalCat registro)
            return;

        _cveMunSeleccionado = registro.cve_mun;

        await CargarLocalidadesPorMunicipioAsync(_cveMunSeleccionado);
    }







    private void OnColoniaNoEncontradaChanged(object sender, CheckedChangedEventArgs e)
    {
        bool manual = e.Value;

        // Deshabilita selector visual (Entry)
        ColoniaSelectorEntry.IsEnabled = !manual;

        // Mostrar / ocultar captura manual
        ColoniaManualContainer.IsVisible = manual;

        if (manual)
        {
            ColoniaSelectorEntry.IsVisible = false;

            // Limpia selección automática
            ColoniaSelectorEntry.Text = string.Empty;

            // Limpia secciones
            SeccionPicker.ItemsSource = null;
        }
        else
        {
            ColoniaSelectorEntry.IsVisible = true;
            ColoniaManualContainer.IsVisible = false;
            // Limpia captura manual
            ColoniaManualEntry.Text = string.Empty;
        }
    }



    private void OnSeccionNoEncontradaChanged(object sender, CheckedChangedEventArgs e)
    {
        bool manual = e.Value;

        SeccionPicker.IsEnabled = !manual;
        SeccionManualEntry.IsVisible = manual;

        if (manual)
        {
            SeccionPicker.IsVisible = false;
            SeccionPicker.SelectedIndex = -1;
        }
        else
        {
            SeccionPicker.IsVisible= true;
            SeccionManualEntry.Text = string.Empty;
        }
    }

    private async void AbrirColoniaPopup(object sender, EventArgs e)
    {
        if(_registrosPorCP == null || _registrosPorCP.Count == 0)
        {
            await DisplayAlert(
                "Colonia",
                "No se encontró ninguna colonia para este código postal, ingresa manualmente",
                "Aceptar"
            );
                    return;
        }
        var colonias = _registrosPorCP
            .Select(x => x.Colonia)
            .Distinct()
            .OrderBy(x => x)
            .ToList();

        var popup = new ColoniaPopup(colonias);

        await this.ShowPopupAsync(popup);

        if (!string.IsNullOrWhiteSpace(popup.ColoniaSeleccionada))
        {
            ColoniaSelectorEntry.Text = popup.ColoniaSeleccionada;
            CargarSeccionesPorColonia(popup.ColoniaSeleccionada);
        }
    }

   

    private void CargarSeccionesPorColonia(string coloniaSeleccionada)
    {
        if (string.IsNullOrWhiteSpace(coloniaSeleccionada))
            return;

        var secciones = _registrosPorCP
            .Where(x => x.Colonia == coloniaSeleccionada)
            .Select(x => x.SeccionElectoral)
            .Distinct()
            .OrderBy(x => x)
            .ToList();

        SeccionPicker.ItemsSource = secciones;
        SeccionPicker.SelectedIndex = -1;
    }

    private async Task CargarLocalidadesPorMunicipioAsync(String cveMun)
    {
 


        // Limpiar selección previa
        LocalidadSelectorEntry.Text = string.Empty;

        _localidadesActuales = await _db.GetLocalidadesByCveMunAsync(cveMun);

        if (_localidadesActuales == null || _localidadesActuales.Count == 0)
            return;

        // Quitar duplicados reales
        _localidadesActuales = _localidadesActuales
            .GroupBy(l => new { l.Cve_Loc, l.LocalidadNombre })
            .Select(g => g.First())
            .OrderBy(l => l.LocalidadNombre)
            .ToList();

        // UX: si solo hay una localidad, se selecciona automáticamente
        if (_localidadesActuales.Count == 1)
        {
            LocalidadSelectorEntry.Text = _localidadesActuales[0].LocalidadNombre;
        }

       

    }
    /*
    private void OnLocalidadChanged(object sender, EventArgs e)
    {
        if (LocalidadPicker.SelectedIndex < 0)
            return;

        var localidad = _localidadesActuales[LocalidadPicker.SelectedIndex];

       
        var cveLoc = localidad.Cve_Loc;
        var ambito = localidad.Ambito;

      
    }*/

    private async void AbrirBuscadorLocalidad(object sender, EventArgs e)
    {
        if (_localidadesActuales == null || _localidadesActuales.Count == 0)
        {
            await DisplayAlert(
                "Localidad",
                "Primero seleccione un municipio para cargar las localidades.",
                "Aceptar"
            );
            return;
        }

        var popup = new LocalidadPopup(
            _localidadesActuales.Select(l => l.LocalidadNombre).ToList()
        );

        await this.ShowPopupAsync(popup);

        if (string.IsNullOrWhiteSpace(popup.LocalidadSeleccionada))
            return;

        LocalidadSelectorEntry.Text = popup.LocalidadSeleccionada;

        var localidad = _localidadesActuales
            .First(l => l.LocalidadNombre == popup.LocalidadSeleccionada);

        _cveLocSeleccionada = localidad.Cve_Loc;
        _ambitoLocalidad = localidad.Ambito;
    }

    private async void OnSeccionFocused(object sender, FocusEventArgs e)
    {
        if (SeccionPicker.ItemsSource == null || SeccionPicker.ItemsSource.Cast<object>().Any() == false)
        {
            await DisplayAlert(
                "Sección electoral",
                "No se encontró ninguna sección para esta colonia. Captura la sección manualmente.",
                "Aceptar"
            );

            SeccionNoEncontradaCheck.IsChecked = true;
            SeccionPicker.Unfocus();
        }
    }


    private void FechaNacimientoEntry_DateSelected(object sender, DateChangedEventArgs e)
    {
        FechaNacimientoEntry.Format = "dd/MM/yyyy";

        DateTime fechaNacimiento = e.NewDate;
        DateTime hoy = DateTime.Today;

        int anios = hoy.Year - fechaNacimiento.Year;

        if (fechaNacimiento.Date > hoy.AddYears(-anios))
            anios--;

        if (anios <= 0)
        {
            

            EdadEntry.Text = "0";
        }
        else
        {
            EdadEntry.Text = $"{anios}";
        }
        ValidarFormularioCURP();
    }

    private void EntidadPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        ValidarFormularioCURP();
    }


    private void OnGenerarCurpClicked(object sender, EventArgs e)
    {
        if (!GenerarCurpButton.IsEnabled)
            return;

        try
        {
            string entidadCodigo = ObtenerCodigoEntidad(
                EntidadPicker.SelectedItem?.ToString()
            );

            string curp = GeneraCurp.GenerarCurp(
                NombreEntry.Text,
                pApellidoEntry.Text,
                sApellidoEntry.Text,
                FechaNacimientoEntry.Date,
                GeneroPicker.SelectedItem?.ToString(),
                entidadCodigo
            );

            CurpEntry.Text = curp;
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", "No se pudo generar la CURP", "OK");
        }
    }

    private string ObtenerEntidadDesdeCodigo(string codigo)
    {
        return codigo switch
        {
            "AS" => "AGUASCALIENTES",
            "BC" => "BAJA CALIFORNIA",
            "BS" => "BAJA CALIFORNIA SUR",
            "CC" => "CAMPECHE",
            "CL" => "COAHUILA",
            "CM" => "COLIMA",
            "CS" => "CHIAPAS",
            "CH" => "CHIHUAHUA",
            "DF" => "CIUDAD DE MEXICO",
            "DG" => "DURANGO",
            "GT" => "GUANAJUATO",
            "GR" => "GUERRERO",
            "HG" => "HIDALGO",
            "JC" => "JALISCO",
            "MC" => "MEXICO",
            "MN" => "MICHOACAN",
            "MS" => "MORELOS",
            "NT" => "NAYARIT",
            "NL" => "NUEVO LEON",
            "OC" => "OAXACA",
            "PL" => "PUEBLA",
            "QT" => "QUERETARO",
            "QR" => "QUINTANA ROO",
            "SP" => "SAN LUIS POTOSI",
            "SL" => "SINALOA",
            "SR" => "SONORA",
            "TC" => "TABASCO",
            "TS" => "TAMAULIPAS",
            "TL" => "TLAXCALA",
            "VZ" => "VERACRUZ",
            "YN" => "YUCATAN",
            "ZS" => "ZACATECAS",
            "NE" => "NACIONAL",
            "EX" => "EXTRANJERO",
            _ => null
        };
    }

    private string ObtenerCodigoEntidad(string entidad)
    {
        return entidad switch
        {
            "AGUASCALIENTES" => "AS",
            "BAJA CALIFORNIA" => "BC",
            "BAJA CALIFORNIA SUR" => "BS",
            "CAMPECHE" => "CC",
            "COAHUILA" => "CL",
            "COLIMA" => "CM",
            "CHIAPAS" => "CS",
            "CHIHUAHUA" => "CH",
            "CIUDAD DE MEXICO" => "DF",
            "DURANGO" => "DG",
            "GUANAJUATO" => "GT",
            "GUERRERO" => "GR",
            "HIDALGO" => "HG",
            "JALISCO" => "JC",
            "MEXICO" => "MC",
            "MICHOACAN" => "MN",
            "MORELOS" => "MS",
            "NAYARIT" => "NT",
            "NUEVO LEON" => "NL",
            "OAXACA" => "OC",
            "PUEBLA" => "PL",
            "QUERETARO" => "QT",
            "QUINTANA ROO" => "QR",
            "SAN LUIS POTOSI" => "SP",
            "SINALOA" => "SL",
            "SONORA" => "SR",
            "TABASCO" => "TC",
            "TAMAULIPAS" => "TS",
            "TLAXCALA" => "TL",
            "VERACRUZ" => "VZ",
            "YUCATAN" => "YN",
            "ZACATECAS" => "ZS",
            "NACIONAL" => "NE",
            "EXTRANJERO" => "EX",
            _ => "NE"
        };
    }

    private async void ValidarFormularioCURP()
    {
        bool nombreOk = !string.IsNullOrWhiteSpace(NombreEntry.Text);
        bool apellido1Ok = !string.IsNullOrWhiteSpace(pApellidoEntry.Text);
        bool generoOk = GeneroPicker.SelectedIndex != -1;
        bool entidadOk = EntidadPicker.SelectedIndex != -1;
        bool fechaOk = FechaNacimientoEntry.Date != DateTime.MinValue;

        bool mostrar=
            nombreOk &&
            apellido1Ok &&
            generoOk &&
            entidadOk &&
            fechaOk;
        if (mostrar && !GenerarCurpButton.IsVisible)
        {
            GenerarCurpButton.IsVisible = true;

            await Task.WhenAll(
                GenerarCurpButton.FadeTo(1, 200),
                GenerarCurpButton.ScaleTo(1, 200, Easing.CubicOut)
            );
        }
        else if (!mostrar && GenerarCurpButton.IsVisible)
        {
            await Task.WhenAll(
                GenerarCurpButton.FadeTo(0, 150),
                GenerarCurpButton.ScaleTo(0.8, 150)
            );

            GenerarCurpButton.IsVisible = false;
        }
    }

    private async void CurpEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        string curp = CurpEntry.Text?.Trim().ToUpper();

        if (string.IsNullOrWhiteSpace(curp) || curp.Length != 18)
            return;

        if (GenerarCurpButton.IsVisible)
        {
            await GenerarCurpButton.FadeTo(0, 150);
            GenerarCurpButton.IsVisible = false;
        }
        try
        {
            AutocompletarDesdeCurp(curp);
        }
        catch
        {
            // si algo falla no hacemos nada
        }
    }

    private void AutocompletarDesdeCurp(string curp)
    {
        // --- FECHA ---
        string anio = curp.Substring(4, 2);
        string mes = curp.Substring(6, 2);
        string dia = curp.Substring(8, 2);
        string siglo = curp.Substring(16, 1);

        int anioCompleto = siglo == "0"
            ? 1900 + int.Parse(anio)
            : 2000 + int.Parse(anio);

        DateTime fecha = new DateTime(
            anioCompleto,
            int.Parse(mes),
            int.Parse(dia)
        );

        FechaNacimientoEntry.Date = fecha;

        // --- GENERO ---
        string sexo = curp.Substring(10, 1);

        if (sexo == "H")
            GeneroPicker.SelectedItem = "Masculino";
        else if (sexo == "M")
            GeneroPicker.SelectedItem = "Femenino";

        // --- ENTIDAD ---
        string entidadCodigo = curp.Substring(11, 2);

        EntidadPicker.SelectedItem = ObtenerEntidadDesdeCodigo(entidadCodigo);

        // recalcular edad
        //CalcularEdad(fecha);
    }

    private void NoTelefonoCheck_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (e.Value)
        {
            TelefonoEntry.Text = string.Empty;
            TelefonoEntry.IsEnabled = false;

            if (NoCelularCheck.IsChecked)
            {
                DisplayAlert("Aviso",
                    "Debe existir al menos un número de contacto",
                    "OK");

                NoTelefonoCheck.IsChecked = false;
            }
        }
        else
        {
            TelefonoEntry.IsEnabled = true;
        }

        ValidarContacto();
    }

    private void NoCelularCheck_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (e.Value)
        {
            CelularEntry.Text = string.Empty;
            CelularEntry.IsEnabled = false;

            if (NoTelefonoCheck.IsChecked)
            {
                DisplayAlert("Aviso",
                    "Debe existir al menos un número de contacto",
                    "OK");

                NoCelularCheck.IsChecked = false;
            }
        }
        else
        {
            CelularEntry.IsEnabled = true;
        }

        ValidarContacto();
    }

    private bool ValidarContacto()
    {
        string telefono = TelefonoEntry.Text ?? string.Empty;
        string celular = CelularEntry.Text ?? string.Empty;

        bool telefonoRespondido =
            NoTelefonoCheck.IsChecked ||
            (telefono.Length == 10 && telefono.All(char.IsDigit));

        bool celularRespondido =
            NoCelularCheck.IsChecked ||
            (celular.Length == 10 && celular.All(char.IsDigit));

        return telefonoRespondido && celularRespondido;
    }
    private void TelefonoEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            if (NoTelefonoCheck.IsChecked)
                NoTelefonoCheck.IsChecked = false;
        }

        ValidarContacto();
    }
    private void CelularEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            if (NoCelularCheck.IsChecked)
                NoCelularCheck.IsChecked = false;
        }

        ValidarContacto();
    }

    private void NoTelAcompCheck_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (e.Value)
        {
            TelefonoAcompananteEntry.Text = string.Empty;
            TelefonoAcompananteEntry.IsEnabled = false;

        }
        else
        {
            TelefonoAcompananteEntry.IsEnabled = true;
        }
    }

    private void MostrarLoading(bool mostrar)
    {
        LoadingOverlay.IsVisible = mostrar;
        this.IsEnabled = !mostrar; // bloquea toda la página
    }

    private async void OnColoniaInfoTapped(object sender, EventArgs e)
    {
        await DisplayAlert(
            "Ejemplo de captura",
            "Ejemplos correctos:\n\n• Balcones del Valle\n• San Ángel\n\nEvite abreviaturas como:\nB. del Valle\nCol. Balcoles del Valle.\n fracc. Himno Nacional",
            "Entendido");
    }

    private async void OnCalleInfoTapped(object sender, EventArgs e)
    {
        await DisplayAlert(
            "Cómo capturar la calle",
            "Debe incluir el tipo de vialidad.\n\n" +
            "Ejemplos correctos:\n\n" +
            "• Avenida Himno Nacional\n" +
            "• Calle 5 de Mayo\n" +
            "• Boulevard Españita\n\n" +
            "Para casos foraneos capturar Fuera del Estado\n" +
            "Incorrecto: Himno Nacional",
            "Entendido");
    }

    private async void OnVacunasInfoTapped(object sender, EventArgs e)
    {
        await DisplayAlert(
            "Ejemplo de captura",
            "Ejemplos correctos:\n\n" +
            "• 3 de 4 perros vacunados 4 de 4 perros esterilizados\n" +
            "• 0 de 2 perros vacunados 1 de 2 perros esterilizados 1 de 1 gato vacunados 0 de 1 gato esterilizados",
            "Entendido");
    }

}