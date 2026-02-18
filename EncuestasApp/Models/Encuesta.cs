//using Android.Health.Connect.DataTypes.Units;
//using Java.Nio.Channels;
using SQLite;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static System.Collections.Specialized.BitVector32;

namespace EncuestaApp.Models;

public class Encuesta : IValidatableObject
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    // Id del dispositivo
    public string DeviceId { get; set; } = "";

    // 1. Datos Generales del Paciente
    [Required(ErrorMessage = "El campo Nombre completo es obligatorio")]
    [RegularExpression(@"(^[A-ZÁÉÍÓÚÑ\s]+$)", ErrorMessage = "El Nombre no tiene un formato válido. No puede tener números")]
    public string NombreCompleto { get; set; } = "";

    [Range(0, 120, ErrorMessage = "La edad debe ser mayor a 0 y menor a 120")]
    public int Edad { get; set; }

    [Required(ErrorMessage = "El campo Género es obligatorio")]
    public string Genero { get; set; } = "";

    [Required(ErrorMessage = "El campo Fecha de nacimiento es obligatorio")]
    public DateTime FechaNacimiento { get; set; }

    [Required(ErrorMessage = "El campo CURP es obligatorio")]
    [StringLength(18, ErrorMessage = "La CURP debe contener 18 caracteres")]
    [RegularExpression(@"([A-Z][AEIOUX][A-Z]{2}\d{2}(?:0[1-9]|1[0-2])(?:0[1-9]|[12]\d|3[01])[HM](?:AS|B[CS]|C[CLMSH]|D[FG]|G[TR]|HG|JC|M[CNS]|N[ETL]|OC|PL|Q[TR]|S[PLR]|T[CSL]|VZ|YN|ZS)[B-DF-HJ-NP-TV-Z]{3}[A-Z\d])(\d)$", ErrorMessage = " La CURP no tiene un formato válido. Ejemplo: TEST150502HSPDTN10\"")]
    [DisplayName("CURP")]
    public string Curp { get; set; } = "";

    [Required(ErrorMessage = "El campo Sección electoral es obligatorio")]
    [RegularExpression(@"^\d{4}$", ErrorMessage = "La Sección electoral no tiene un formato válido. Ejemplo: 0001")]
    [StringLength(4)]
    public string SeccionElectoral { get; set; } = "";

    [Required(ErrorMessage = "El campo Calle es obligatorio")]
    public string Calle { get; set; }

    [Required(ErrorMessage = "El campo Número exterior es obligatorio")]
    [RegularExpression(@"(^[0-9]+$)",
                            ErrorMessage = "El Número exterior no tiene un formato válido es número")]
    public string NumExterior { get; set; }

    public string? NumInterior { get; set; }

    [Required(ErrorMessage = "El campo Colonia es obligatorio")]
    public string Colonia { get; set; }

    [Required(ErrorMessage = "El campo Localidad es obligatorio")]
    public string Localidad { get; set; }

    [Required(ErrorMessage = "El campo Municipio es obligatorio")]
    public string Municipio { get; set; }

    [Required(ErrorMessage = "El campo Código Postal es obligatorio")]
    [StringLength(5)]
    [RegularExpression(@"(^[0-9]+$)", ErrorMessage = "El Código Postal no tiene un formato válido. Ejemplo: 83560")]
    public string CodigoPostal { get; set; }

    [StringLength(10, ErrorMessage = "El Teléfono de casa debe contener 10 dígitos")]
    [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "El Teléfono de casa no tiene un formato válido")]
    [DisplayName("Teléfono fijo")]
    public string Telefono { get; set; } = "";

    [StringLength(10, ErrorMessage = "El Teléfono celular debe contener 10 dígitos")]
    [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "El Teléfono celular no tiene un formato válido")]
    public string Celular { get; set; } = "";

    [Required(ErrorMessage = "El campo Estado civil es obligatorio")]
    public string EstadoCivil { get; set; } = "";

    [Required(ErrorMessage = "El campo Ocupación es obligatorio")]
    //[RegularExpression(@"(^[A-ZÁÉÍÓÚÑ\s]+$)", ErrorMessage = "La ocupación no tiene un formato válido. No puede tener números")]
    public string Ocupacion { get; set; } = "";

    [Required(ErrorMessage = "El campo ¿Pertenece a algún pueblo indígena? es obligatorio")]
    public string PuebloIndigena { get; set; }

    //[RegularExpression(@"(^[A-ZÁÉÍÓÚÑ\s]+$)", ErrorMessage = "El campo Cuál pueblo indigena no tiene un formato válido. No puede tener números")]
    public string? CualPuebloIndigena { get; set; }

    [Required(ErrorMessage = "El campo ¿Habla alguna lengua indígena? es obligatorio")]
    public string IdiomaIndigena { get; set; }

    //[RegularExpression(@"(^[A-ZÁÉÍÓÚÑ\s]+$)", ErrorMessage = "El campo Cuál idioma no tiene un formato válido. No puede tener números")]
    public string? CualIdiomaIndigena { get; set; }

    [RegularExpression(@"(^[A-Za-záéíóúÁÉÍÓÚñÑ\s]+$)", ErrorMessage = "El Acompanante no tiene un formato válido. No puede tener números")]
    public string? Acompanante { get; set; }

    [RegularExpression(@"(^[A-Za-záéíóúÁÉÍÓÚñÑ\s]+$)", ErrorMessage = "La Relación  de acompanante no tiene un formato válido.  No puede tener números")]
    public string? RelacionAcompanante { get; set; }

    [StringLength(10, ErrorMessage = "El Teléfono de acompañante debe contener 10 dígitos, sin espacios ni guiones")]
    [RegularExpression(@"(^[0-9]+$)", ErrorMessage = "El Teléfono de acompañante no tiene un formato válido. Solo puede tener números")]
    public string? TelefonoAcompanante { get; set; }

    // 2. Antecedentes Personales
    public bool HipertensionArterial { get; set; } = false;
    public bool Diabetes { get; set; } = false;
    public bool Obesidad { get; set; } = false;
    public bool EnfermedadCerdiovascular { get; set; } = false;
    public bool Dislipidemia { get; set; } = false;
    public bool Asma { get; set; } = false;
    public bool Epilepsia { get; set; } = false;
    public bool Tuberculosis { get; set; } = false;
    public bool Cancer { get; set; } = false;
    public string? EspecificaCancer { get; set; }
    public bool SaludMental { get; set; } = false;
    public string? EspecificaSaludMental { get; set; }
    public bool Caries { get; set; } = false;
    public bool Alergias { get; set; } = false;
    public string? EspecificaAlergias { get; set; }
    public bool CirugiasPrevias { get; set; } = false;
    public string? EspecificaCirugiasPrevias { get; set; }
    public bool InfeccionesTransmisionSexual { get; set; } = false;
    public string? EspecificaInfeccionesTransmisionSexual { get; set; }
    public string? MedicamentosActuales { get; set; }
    public bool OtrosAntecedentes { get; set; }
    public string? EspecificaOtrosAntecedentes { get; set; }
    public bool Ginecologicos { get; set; } = false;
    public string? Menarca { get; set; }
    public string? InicioVidaSexual { get; set; }
    [RegularExpression(@"(^[0-9]+$)", ErrorMessage = "El No. de parejas sexuales no tiene un formato válido. Solo puede tener números")]
    public string? ParejasSexuales { get; set; }
    [RegularExpression(@"(^[0-9]+$)", ErrorMessage = "El No. de gestas no tiene un formato válido. Solo puede tener números")]
    public string? Gestas { get; set; }
    [RegularExpression(@"(^[0-9]+$)", ErrorMessage = "El No. de partos no tiene un formato válido. Solo puede tener números")]
    public string? Partos { get; set; }
    [RegularExpression(@"(^[0-9]+$)", ErrorMessage = "El No. de cesareas no tiene un formato válido. Solo puede tener números")]
    public string? Cesareas { get; set; }
    [RegularExpression(@"(^[0-9]+$)", ErrorMessage = "El No. de abortos no tiene un formato válido. Solo puede tener números")]
    public string? Abortos { get; set; }
    public string? CitologiaCervical { get; set; }
    public string? ExploracionMamariaObservaciones { get; set; }
    public string? Mastografia { get; set; }
    public string? PlanificacionFamiliar { get; set; }

    // 3. Antecedentes Familiares
    public bool AntecedentesHipertension { get; set; } = false;
    public string? ParentescoHipertension { get; set; }
    public bool AntecedentesDiabetes { get; set; } = false;
    public string? ParentescoDiabetes { get; set; }
    public bool AntecedentesObesidadCheckBox { get; set; } = false;
    public string? ParentescoObesidad { get; set; }
    public bool AntecedentesEnfermedadCardiovascular { get; set; } = false;
    public string? ParentescoEnfermedadCardiovascular { get; set; }
    public bool AntecedentesDislipidemia { get; set; } = false;
    public string? ParentescoDislipidemian { get; set; }
    public bool AntecedentesAsma { get; set; } = false;
    public string? ParentescoAsma { get; set; }
    public bool AntecedentesEpilepsia { get; set; } = false;
    public string? ParentescoEpilepsia { get; set; }
    public bool AntecedentesCancer { get; set; } = false;
    public string? ParentesCancer { get; set; }
    public bool AntecedentesSaludMental { get; set; } = false;
    public string? ParentescoSaludMental { get; set; }

    // 4. Habitos y Estilo de Vida
    [Required(ErrorMessage = "El campo Tabaquismo es obligatorio")]
    public string Tabaquismo { get; set; }
    [RegularExpression(@"(^[0-9]+$)", ErrorMessage = "El No. de cuantos cigarros no tiene un formato válido. Solo puede tener números")]
    public string? TabaquismoNum { get; set; }
    [Required(ErrorMessage = "El campo Consumo de alcohol es obligatorio")]
    public string ConsumoAlcohol { get; set; }
    //[RegularExpression(@"(^[A-ZÁÉÍÓÚÑ\s]+$)",
    //                        ErrorMessage = "El campo frecuencia consumo alcohol no tiene un formato válido. No puede tener números")]
    public string? ConsumoAlcoholFrecuencia { get; set; }
    [Required(ErrorMessage = "El campo Consumo de drogas es obligatorio")]
    public string ConsumoDrogas { get; set; }
    //[RegularExpression(@"(^[A-ZÁÉÍÓÚÑ\s]+$)",
    //                        ErrorMessage = "El campo frecuencia consumo drogas no tiene un formato válido. No puede tener números")]
    public string? ConsumoDrogasFrecuencia { get; set; }
    [Required(ErrorMessage = "El campo Actividad física es obligatorio")]
    public string AcividadFisica { get; set; }
    [Required(ErrorMessage = "El campo Alimentación es obligatorio")]
    public string Alimentacion { get; set; }
    [Required(ErrorMessage = "El campo Nivel de estrés es obligatorio")]
    public string NivelEstres { get; set; }
    [Required(ErrorMessage = "El campo Apoyo familiar es obligatorio")]
    public string ApoyoFamiliar { get; set; }
    [Required(ErrorMessage = "El campo ¿Tiene mascotas? es obligatorio")]
    public string TieneMascotas { get; set; }
    public string? EstanVacunados { get; set; }

    // 5. Evaluación social y condiciones del entorno
    [Required(ErrorMessage = "El campo Tipo de vivienda es obligatorio")]
    public string TipoVivienda { get; set; }
    [Required(ErrorMessage = "El campo Condiciones del hogar es obligatorio")]
    public string CondicionesHogar { get; set; }
    public bool ServicioAgua { get; set; } = false;
    public bool ServicioLuz { get; set; } = false;
    public bool ServicioGas { get; set; } = false;
    public bool ServicioDrenaje { get; set; } = false;
    public bool PersonasMayores { get; set; } = false;
    public bool PersonasDiscapacidad { get; set; } = false;
    public bool Menores5anios { get; set; } = false;
    public bool ViolenciaFamiliar { get; set; } = false;
    public bool Abandono { get; set; } = false;
    public bool InseguridadAlimentaria { get; set; } = false;
    public bool Desempleo { get; set; } = false;
    public string? RiegosSocialesObservaciones { get; set; }

    // 6. Motivo de consulta y padecimiento actual 
    public string MotivoConsulta { get; set; } = "";

    // 7. Interrogatorio por Aparatos y Sistemas (breve)
    public string Respiratorio { get; set; }
    public string EspecificaOtroRespiratorio { get; set; }
    public string Cardiovascular { get; set; }
    public string EspecificaOtroCardiovascular { get; set; }
    public string Digestivo { get; set; }
    public string EspecificaOtroDigestivo { get; set; }
    public string Musculoesqueletico { get; set; }
    public string EspecificaOtroMusculoesqueletico { get; set; }
    public string Neurologico { get; set; }
    public string EspecificaOtroNeurologico { get; set; }
    public string Genitourinario { get; set; }
    public string EspecificaOtroGenitourinario { get; set; }
    public string Psicologico { get; set; }
    public string EspecificaOtroPsicologico { get; set; }

    // 8. Examen físico
    [Required(ErrorMessage = "El campo Tensíon Arterial es obligatorio")]
    public string TensionArterial { get; set; }
    [Required(ErrorMessage = "El campo Frecuencia Cardiaca es obligatorio")]
    [RegularExpression(@"(^[0-9]+$)", ErrorMessage = "El campo de frecuencia cardiaca no tiene un formato válido. Solo puede tener números")]
    //[Range(0, 220, ErrorMessage = "El campo de frecuencia cardiaca debe ser mayor a 40 y menor a 220")]
    public string FrecuenciaCardiaca { get; set; }
    [Required(ErrorMessage = "El campo Temperatura es obligatorio")]
    [StringLength(4, ErrorMessage = "La temperatura debe contener 4 caracteres")]
    public string Temperatura { get; set; }
    [Range(0, 40, ErrorMessage = "El campo de frecuencia respiratoria debe ser mayor a 10 y menor a 40")]
    [Required(ErrorMessage = "El campo Frecuencia Respiratoria es obligatorio")]
    public string FrecuenciaRespiratoria { get; set; }
    [Required(ErrorMessage = "El campo Saturación es obligatorio")]
    [Range(0, 100, ErrorMessage = "El campo Saturación debe ser mayor a 10 y menor a 100")]
    public string SaturacionO2 { get; set; }
    [Required(ErrorMessage = "El campo Peso es obligatorio")]
   
    public string Peso { get; set; }
    [Required(ErrorMessage = "El campo Talla es obligatorio")]
    public string Talla { get; set; }

    [Required(ErrorMessage = "El campo Imc es obligatorio")]
    public string Imc { get; set; }
    public string ImcPicker { get; set; }
    public string EstadoGeneral { get; set; }
    [Required(ErrorMessage = "El campo Exploración Física es obligatorio")]
    public string ExploracionFisica { get; set; }

    // 9. Detecciones (aplicar según edad y género)
    public string TamizajeHipertension { get; set; }
    public string TamizajeHipertensionObservaciones { get; set; }
    public string TamizajeDiabetes { get; set; }
    public string TamizajeDiabetesObservaciones { get; set; }
    public string DeteccionesExploracionMamaria { get; set; }
    public string DeteccionesExploracionMamariaObservaciones { get; set; }
    public string DeteccionesExploracionClinicaMamaria { get; set; }
    public string DeteccionesExploracionClinicaMamariaObservaciones { get; set; }
    public string DeteccionesEvaluacionEstadoNutricional { get; set; }
    public string DeteccionesEvaluacionEstadoNutricionalObservaciones { get; set; }
    public string DeteccionesSaludMental { get; set; }
    public string DeteccionesSaludMentalObservaciones { get; set; }
    public string DeteccionesRevisionBucal { get; set; }
    public string DeteccionesRevisionBucalObservaciones { get; set; }

    // 10. Impresión Diagnóstica / Problemas Identificados
    public string ImpresionDiagnostica1 { get; set; }
    public string ImpresionDiagnostica2 { get; set; }
    public string ImpresionDiagnostica3 { get; set; }

    // 11. Plan de manejo
    [Required(ErrorMessage = "El Tratamiento de medicamentos entregado es obligatorio")]
    public string TratamientoMedicamentosEntregados { get; set; }
    [Required(ErrorMessage = "La entrega de suplementos e insumos es obligatorio")]

    public string EntregaSuplementosInsumos { get; set; }
    [Required(ErrorMessage = "La educación sexual es obligatorio")]
    public string EducacionSalud { get; set; }
    public string ReferenciasNecesarias { get; set; }
    [Required(ErrorMessage = "El Lugar de atención es obligatorio")]
     public string Lugar { get; set; }
    [Required(ErrorMessage = "El campo visita de seguimiento es obligatorio")]

    public string VisitaSeguimiento { get; set; }
    public DateTime FechaSugerida { get; set; }

    // 12. Profesional que Realiza la Atención
    [Required(ErrorMessage = "El Nombre completo de quien realiza la atención es obligatorio")]
    [RegularExpression(@"(^[A-ZÁÉÍÓÚÑ\s]+$)", ErrorMessage = "El Nombre del encuestador no tiene un formato válido. No puede tener números")]
    public string NombreEncuestador { get; set; }
    [Required(ErrorMessage = "El campo Cargo de quien realiza la atención es obligatorio")]
    public string CargoEncuestador { get; set; }
    public DateTime FechaEncuesta { get; set; }
    [Required(ErrorMessage = "El campo ¿Qué tanta disposición mostró la persona entrevistada hacía el brigadista? es obligatorio")]
    public string DisposicionEncuestado { get; set; }


    // Id del usuario logueado
    public int UsuarioId { get; set; }

    // Geolocalización
    public double? Latitud { get; set; }
    public double? Longitud { get; set; }

    // Bandera de sincronización
    public bool Sincronizada { get; set; } = false;

    // Obtiene el domicilio completo para mostrarlo en el listado de encuestas guardadas.
    [Ignore] // Para que SQLite lo ignore y no se guarde en BD.
    public string DomicilioCompleto
    {
        get
        {
            // Manejar nulls para que no reviente
            return $"{Calle ?? ""} {NumExterior ?? ""}" +
               $"{(string.IsNullOrWhiteSpace(NumInterior) ? "" : " Int. " + NumInterior)}, " +
               $"{Colonia ?? ""}, {Localidad ?? ""}, {Municipio ?? ""}, {CodigoPostal ?? ""}"
               .Trim().Trim(',');
        }
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(Telefono) &&
            string.IsNullOrWhiteSpace(Celular))
        {
            yield return new ValidationResult(
                "Debe proporcionar al menos un número de contacto (Teléfono o Celular).",
                new[] { nameof(Telefono), nameof(Celular) }
            );
        }
    }
}