using System.Collections.Generic;

namespace Patholab.Domain.Entities
{
    public class SampleType : BaseEntity
    {
        public string SampleTypeCode { get; set; } = string.Empty;
        public string SampleTypeName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;

        public virtual ICollection<Sample> Samples { get; set; } = new List<Sample>();
    }
}
