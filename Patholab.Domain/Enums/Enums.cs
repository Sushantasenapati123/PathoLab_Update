namespace Patholab.Domain.Enums
{
    public enum OrderStatus
    {
        Registered,
        SamplePending,
        SampleCollected,
        Processing,
        ResultEntered,
        UnderVerification,
        Verified,
        Published,
        Cancelled
    }

    public enum SampleStatus
    {
        Pending,
        Collected,
        Received,
        Processing,
        Completed,
        Rejected,
        Cancelled
    }

    public enum PaymentStatus
    {
        Unpaid,
        PartiallyPaid,
        Paid,
        Refunded
    }

    public enum InvoiceStatus
    {
        Unpaid,
        PartiallyPaid,
        Paid,
        Cancelled
    }

    public enum HomeCollectionStatus
    {
        Requested,
        Assigned,
        OnTheWay,
        SampleCollected,
        Completed,
        Cancelled
    }

    public enum ResultStatus
    {
        Pending,
        Normal,
        Low,
        High,
        Critical,
        Abnormal
    }

    public enum CommissionType
    {
        None,
        Percentage,
        Fixed
    }

    public enum NotificationType
    {
        SMS,
        Email,
        WhatsApp
    }

    public enum NotificationStatus
    {
        Pending,
        Sent,
        Failed
    }

    public enum ReportStatus
    {
        Draft,
        UnderReview,
        Verified,
        Published,
        Cancelled
    }
}
