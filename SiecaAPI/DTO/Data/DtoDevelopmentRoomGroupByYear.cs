namespace SiecaAPI.DTO.Data
{
    public class DtoDevelopmentRoomGroupByYear : GenericDataDto
    {
        public Guid DevelopmentRoomId { get; set; } = Guid.Empty;
        public string DevelopmentRoomCode { get; set; } = string.Empty;
        public string DevelopmentRoomName { get; set; } = string.Empty;
        public int Year { get; set; }
        public string GroupCode { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;
        public List<string> Agents { get; set; } = new();
    }
}
