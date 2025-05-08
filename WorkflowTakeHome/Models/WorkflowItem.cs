namespace WorkflowTakeHome.Models
{
    public class WorkflowItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string SourceTool { get; set; } = string.Empty;
        public Dictionary<string, object> NormalizedData { get; set; } = new();
        public WorkflowStatus Status { get; set; } = WorkflowStatus.PendingReview;
        public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
    }
}
