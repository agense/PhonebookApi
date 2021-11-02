using PhonebookApi.Dto.Responses;
using PhonebookApi.Interfaces;

namespace PhonebookApi.Mappers
{

    public class MessageResponseMapper : IMessageResponseMapper
    {

        public MessageResponse Map(string message)
        {
            return new MessageResponse()
            {
                Message = message,
            };
        }
    }

}
