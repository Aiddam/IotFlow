
using IotFlow.Models.DB;
using IotFlow.Models.Enum;

namespace IotFlow.Models.DTO.MethodRegister
{
    public class MethodRegisterResultDto
    {
        public Guid CorrelationId { get; set; } = Guid.NewGuid();
        public int MethodId { get; set; }
        public DeviceMethod Method { get; set; }
        public RequestStatus Status { get; set; } = RequestStatus.Pending;
        public string Message { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty ;
        public MethodType ResultType {  get; set; }
        public DateTime ResponseReceivedAt { get; set; }
    }
}
