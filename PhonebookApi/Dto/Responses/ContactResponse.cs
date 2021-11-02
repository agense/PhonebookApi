namespace PhonebookApi.Dto.Responses
{
    public class ContactResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public bool? IsOwnedContact { get; set; } = null;

    }
}