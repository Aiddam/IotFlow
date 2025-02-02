using System.ComponentModel.DataAnnotations;


namespace IotFlow.Abstractions.Abstrations
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
