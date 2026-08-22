using System;
using Patholab.Domain.Enums;

namespace Patholab.Domain.Entities
{
    public class Notification : BaseEntity
    {
        public int? PatientId { get; set; }
        public virtual Patient? Patient { get; set; }
        public NotificationType NotificationType { get; set; }
        public string Recipient { get; set; } = string.Empty;
        public string? Subject { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? ReferenceType { get; set; }
        public int? ReferenceId { get; set; }
        public NotificationStatus Status { get; set; } = NotificationStatus.Pending;
        public DateTime? SentOn { get; set; }
        public string? FailureReason { get; set; }
    }
}
