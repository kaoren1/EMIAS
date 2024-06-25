namespace EMIAS.Model   
{
    public class DirectionsWithSpecialistDetails
    {
        public int? IdDirection { get; set; }

        public int? SpecialityId { get; set; }

        public string Name { get; set; }

        public long Oms { get; set; }
    }
}
