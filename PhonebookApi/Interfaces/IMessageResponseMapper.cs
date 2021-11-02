using PhonebookApi.Dto.Responses;

namespace PhonebookApi.Interfaces
{

    public interface IMessageResponseMapper
    {
        public MessageResponse Map(string message);
    }

}
