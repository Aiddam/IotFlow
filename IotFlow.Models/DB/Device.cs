using IotFlow.Abstractions.Abstrations;

namespace IotFlow.Models.DB
{
    public class Device : BaseEntity
    {
        public Guid DeviceGuid { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public bool IsAlive { get; set; } = false;
        public DateTime? LastSeen { get; set; }
        public ICollection<DeviceMethod> Methods { get; set; } = new List<DeviceMethod>();
        public int UserId { get; set; }
        public User User { get; set; }

    }
}
