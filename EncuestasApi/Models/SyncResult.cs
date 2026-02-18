namespace EncuestasApi.Models
{
    public class SyncResult
    {
        public int LocalId { get; set; }
        public bool Success { get; set; }
        public int ServerId { get; set; }
        public string Error { get; set; } = "";
    }
}
