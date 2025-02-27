using IotFlow.Abstractions.Abstrations;
using IotFlow.Models.Enum;

namespace IotFlow.Models.DB
{
    public class MethodRegister : BaseEntity
    {
        public Guid CorrelationId { get; set; } = Guid.NewGuid();
        public int MethodId { get; set; }
        public DeviceMethod Method { get; set; }
        public required string MethodName { get; set; }
        public required RequestStatus Status { get; set; } = RequestStatus.Pending;
        public string Message { get; set; } = string.Empty;
        public DateTime RequestTime { get; set; } = DateTime.UtcNow;
        public DateTime ResponseReceivedAt { get; set; }
        public string? Result { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int DeviceId { get; set; }
        public Device Device { get; set; }
    }
}
