using SecurityLayerDotNetAPI.Models;

namespace SecurityLayerDotNetAPI.DTO.Response
{
    public class DtoAccessUserResp
    {
        public Guid? Id { get; set; }
        public Guid? OrganizationId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? OtherNames { get; set; }
        public string? LastName { get; set; }
        public string? OtherLastName { get; set; }
        public string? CreatedBy { get; set; }
        public Guid? DocumentTypeId { get; set; }
        public string? DocumentNo { get; set; }

        public DtoAccessUserResp(Guid? id, Guid? organizationId, string userName, string email, string firstName, string? otherNames, string lastName, string? otherLastName,Guid documentTypeId, string documentNo)
        {
            Id = id;
            OrganizationId = organizationId;
            UserName = userName;
            Email = email;
            FirstName = firstName;
            OtherNames = otherNames;
            LastName = lastName;
            OtherLastName = otherLastName;
            DocumentTypeId = documentTypeId;
            DocumentNo = documentNo;
        }
        public DtoAccessUserResp(Guid? organizationId, string userName, string email, string firstName, string? otherNames, string lastName, string? otherLastName, Guid documentTypeId, string documentNo)
        {
            OrganizationId = organizationId;
            UserName = userName;
            Email = email;
            FirstName = firstName;
            OtherNames = otherNames;
            LastName = lastName;
            OtherLastName = otherLastName;
            DocumentTypeId = documentTypeId;
            DocumentNo = documentNo;
        }
    }
}
