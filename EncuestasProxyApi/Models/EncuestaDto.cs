namespace EncuestasProxyApi.Models
{
    public class EncuestaDto
    {
        public int Id { get; set; }

        // Id del dispositivo
        public string DeviceId { get; set; } = "";

        // 1. Datos Generales del Paciente
        public string NombreCompleto { get; set; } = "";

        public int Edad { get; set; }

        public string Genero { get; set; } = "";
        public DateTime FechaNacimiento { get; set; }
        public string Curp { get; set; } = "";

        public string SeccionElectoral { get; set; } = "";
        public string Calle { get; set; }

        public string NumExterior { get; set; }

        public string? NumInterior { get; set; }
        public string Colonia { get; set; }
        public string Localidad { get; set; }
        public string Municipio { get; set; }
        public string CodigoPostal { get; set; }
        public string Telefono { get; set; } = "";
        public string Celular { get; set; } = "";
        public string EstadoCivil { get; set; } = "";
        public string Ocupacion { get; set; } = "";
        public string PuebloIndigena { get; set; }
        public string? CualPuebloIndigena { get; set; }
        public string IdiomaIndigena { get; set; }
        public string? CualIdiomaIndigena { get; set; }
        public string? Acompanante { get; set; }
        public string? RelacionAcompanante { get; set; }
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
        public string? ParejasSexuales { get; set; }
        public string? Gestas { get; set; }
        public string? Partos { get; set; }
        public string? Cesareas { get; set; }
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
        public string Tabaquismo { get; set; }
        public string? TabaquismoNum { get; set; }
        public string ConsumoAlcohol { get; set; }
        public string? ConsumoAlcoholFrecuencia { get; set; }
        public string ConsumoDrogas { get; set; }
        public string? ConsumoDrogasFrecuencia { get; set; }
        public string AcividadFisica { get; set; }
        public string Alimentacion { get; set; }
        public string NivelEstres { get; set; }
        public string ApoyoFamiliar { get; set; }
        public string TieneMascotas { get; set; }
        public string? EstanVacunados { get; set; }

        // 5. Evaluación social y condiciones del entorno
        public string TipoVivienda { get; set; }
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
        public string? EspecificaOtroRespiratorio { get; set; }
        public string Cardiovascular { get; set; }
        public string? EspecificaOtroCardiovascular { get; set; }
        public string Digestivo { get; set; }
        public string? EspecificaOtroDigestivo { get; set; }
        public string Musculoesqueletico { get; set; }
        public string? EspecificaOtroMusculoesqueletico { get; set; }
        public string Neurologico { get; set; }
        public string? EspecificaOtroNeurologico { get; set; }
        public string Genitourinario { get; set; }
        public string? EspecificaOtroGenitourinario { get; set; }
        public string Psicologico { get; set; }
        public string? EspecificaOtroPsicologico { get; set; }

        // 8. Examen físico
        public string TensionArterial { get; set; }
        public string FrecuenciaCardiaca { get; set; }
        public string Temperatura { get; set; }
        public string FrecuenciaRespiratoria { get; set; }
        public string SaturacionO2 { get; set; }

        public string Peso { get; set; }
        public string Talla { get; set; }
        public string Imc { get; set; }
        public string ImcPicker { get; set; }
        public string EstadoGeneral { get; set; }
        public string ExploracionFisica { get; set; }

        // 9. Detecciones (aplicar según edad y género)
        public string? TamizajeHipertension { get; set; }
        public string? TamizajeHipertensionObservaciones { get; set; }
        public string? TamizajeDiabetes { get; set; }
        public string? TamizajeDiabetesObservaciones { get; set; }
        public string? DeteccionesExploracionMamaria { get; set; }
        public string? DeteccionesExploracionMamariaObservaciones { get; set; }
        public string? DeteccionesExploracionClinicaMamaria { get; set; }
        public string? DeteccionesExploracionClinicaMamariaObservaciones { get; set; }
        public string? DeteccionesEvaluacionEstadoNutricional { get; set; }
        public string? DeteccionesEvaluacionEstadoNutricionalObservaciones { get; set; }
        public string? DeteccionesSaludMental { get; set; }
        public string? DeteccionesSaludMentalObservaciones { get; set; }
        public string? DeteccionesRevisionBucal { get; set; }
        public string? DeteccionesRevisionBucalObservaciones { get; set; }

        // 10. Impresión Diagnóstica / Problemas Identificados
        public string ImpresionDiagnostica1 { get; set; }
        public string ImpresionDiagnostica2 { get; set; }
        public string ImpresionDiagnostica3 { get; set; }

        // 11. Plan de manejo
        public string TratamientoMedicamentosEntregados { get; set; }
        public string EntregaSuplementosInsumos { get; set; }
        public string? EducacionSalud { get; set; }
        public string ReferenciasNecesarias { get; set; }
        public string Lugar { get; set; }
        public string VisitaSeguimiento { get; set; }
        public DateTime FechaSugerida { get; set; }

        // 12. Profesional que Realiza la Atención

        public string NombreEncuestador { get; set; }

        public string CargoEncuestador { get; set; }
        public DateTime FechaEncuesta { get; set; }

        public string DisposicionEncuestado { get; set; }


        // Id del usuario logueado
        public int UsuarioId { get; set; }

        // Geolocalización
        public double? Latitud { get; set; }
        public double? Longitud { get; set; }

        // Bandera de sincronización
        public bool Sincronizada { get; set; } = false;
    }
}
