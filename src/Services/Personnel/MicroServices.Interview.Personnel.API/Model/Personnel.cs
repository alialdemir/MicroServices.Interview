namespace MicroServices.Interview.Personnel.API.Model
{
    public partial class Personnel : ModelBase
    {
        public int PersonnelId { get; set; }

        public string FullName { get; set; }

        public byte Age { get; set; }

        public string City { get; set; }
    }
}