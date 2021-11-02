namespace PhonebookApi.Dto.Responses
{
    public class ContactInDetailResponse
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string CountryCode { get; set; }

        public string Phone { get; set; }

        public bool IsOwnedContact { get; set; }

    }
}