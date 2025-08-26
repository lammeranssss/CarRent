namespace CarRental.ntier.API.Middleware
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Details { get; set; }
        public Dictionary<string, string[]>? Errors { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string TraceId { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"StatusCode: {StatusCode}, Message: {Message}, Timestamp: {Timestamp}";
        }
    }
}
