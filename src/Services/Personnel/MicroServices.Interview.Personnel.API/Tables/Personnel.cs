namespace MicroServices.Interview.Personnel.API.Tables
{
    public partial class Personnel : EntityBase
    {
        public int PersonnelId { get; set; }

        public string FullName { get; set; }

        public byte Age { get; set; }

        public string City { get; set; }
    }
}