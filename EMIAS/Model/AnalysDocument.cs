namespace EMIAS.Model
{
    public class AnalysDocument
    {
        public int? IdAnalysDocument { get; set; }

        public int AppointmentId { get; set; }

        public string Rtf { get; set; } = null!;
    }
}
