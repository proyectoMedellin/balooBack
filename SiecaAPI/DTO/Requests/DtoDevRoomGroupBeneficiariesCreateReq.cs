namespace SiecaAPI.DTO.Requests
{
    public class DtoDevRoomGroupBeneficiariesCreateReq
    {
        public Guid OrganizationId { set; get; }
        public Guid TrainingCenterId { set; get; }
        public Guid CampusId { set; get; }
        public Guid DevelopmentRoomId { set; get; }
        public Guid DevelopmentRoomGroupByYearId { get; set; }
        public List<Guid> BeneficiariesIds { get; set; } = new();
        public string assignamentUser { get; set; } = string.Empty;
    }
}
