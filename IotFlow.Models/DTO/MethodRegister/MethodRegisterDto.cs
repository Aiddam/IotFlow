using IotFlow.Models.DB;
using IotFlow.Models.Enum;

namespace IotFlow.Models.DTO.MethodRegister
{
    public class MethodRegisterDto
    {
        public Guid CorrelationId { get; set; } = Guid.NewGuid();
        public required string MethodName { get; set; }
        public RequestStatus Status { get; set; } = RequestStatus.Pending;
        public string Message { get; set; } = string.Empty;
        public DateTime RequestTime { get; set; } = DateTime.Now;
        public DateTime ResponseReceivedAt { get; set; }
    }
}
