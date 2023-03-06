namespace SiecaAPI.DTO.Requests
{
    public class DtoAccessUserReq
    {
        public string? oldUserName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? OtherNames { get; set; }
        public string? LastName { get; set; }
        public string? OtherLastName { get; set; }
        public string? CreatedBy { get; set; }
        public string? Phone { get; set; }
        public Guid? DocumentTypeId { get; set; }
        public string? DocumentNo { get; set; }
        public Guid? TrainingCenterId { get; set; } = Guid.Empty;
        public List<Guid>? CampusId { get; set; } = new List<Guid>();
        public List<Guid> RolsId { get; set; }
        public bool GlobalUser { get; set; }
    }
}
