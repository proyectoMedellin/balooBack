using System.ComponentModel.DataAnnotations;

namespace SiecaAPI.DTO.Data
{
    public class DtoAccessUser
    {
        public Guid? Id { get; set; }
        public Guid OrganizationId { get; set; }
        public string UserName { get; set; } 
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string? OtherNames { get; set; }
        public string LastName { get; set; }
        public string? OtherLastName { get; set; }
        public bool RequirePaswordChange { get; set; }
        public string CreatedBy { get; set; }
        public string? Phone { get; set; }
        public Guid DocumentTypeId { get; set; }
        public string DocumentNo { get; set; }
        public Guid? TrainingCenterId   { get; set; }
        public List<Guid> CampusId { get; set; }
        public List<Guid> RolsId { get; set; }
        public bool GlobalUser { get; set; }
        public DtoAccessUser(string userName, string email, string firstName, string? otherNames, string lastName, string? otherLastName,
           bool requirePaswordChange, string createdBy, string? phone, Guid documentTypeId, string documentNo, Guid? trainingCenterId, bool globalUser)
        {
            UserName = userName;
            Email = email;
            FirstName = firstName;
            OtherNames = otherNames;
            LastName = lastName;
            OtherLastName = otherLastName;
            RequirePaswordChange = requirePaswordChange;
            CreatedBy = createdBy;
            Phone = phone;
            DocumentTypeId = documentTypeId;
            DocumentNo = documentNo;
            TrainingCenterId = trainingCenterId;
            GlobalUser = globalUser;
        }
        public DtoAccessUser(Guid? id, string userName, string email, string firstName, string? otherNames, string lastName, string? otherLastName,
           bool requirePaswordChange, string createdBy, string? phone, Guid documentTypeId, string documentNo, Guid? trainingCenterId, bool globalUser)
        {
            Id = id;
            UserName = userName;
            Email = email;
            FirstName = firstName;
            OtherNames = otherNames;
            LastName = lastName;
            OtherLastName = otherLastName;
            RequirePaswordChange = requirePaswordChange;
            CreatedBy = createdBy;
            Phone = phone;
            DocumentTypeId = documentTypeId;
            DocumentNo = documentNo;
            TrainingCenterId = trainingCenterId;
            GlobalUser = globalUser;
        }
        public DtoAccessUser( string userName, string email, string firstName, string? otherNames, string lastName, string? otherLastName,
            bool requirePaswordChange, string createdBy, string? phone, Guid documentTypeId, string documentNo, Guid? trainingCenterId, List<Guid>? campusId, List<Guid> rolsId, bool globalUser)
        {
            UserName = userName;
            Email = email;
            FirstName = firstName;
            OtherNames = otherNames;
            LastName = lastName;
            OtherLastName = otherLastName;
            RequirePaswordChange = requirePaswordChange;
            CreatedBy = createdBy;
            Phone = phone;
            DocumentTypeId = documentTypeId;
            DocumentNo = documentNo;
            TrainingCenterId = trainingCenterId;
            CampusId = campusId;
            RolsId = rolsId;
            GlobalUser = globalUser;
        }
        public DtoAccessUser(string userName, string email, string firstName, string? otherNames, string lastName, string? otherLastName,
            bool requirePaswordChange, string? phone, Guid documentTypeId, string documentNo, Guid? trainingCenterId, List<Guid>? campusId, List<Guid> rolsId, bool globalUser)
        {
            UserName = userName;
            Email = email;
            FirstName = firstName;
            OtherNames = otherNames;
            LastName = lastName;
            OtherLastName = otherLastName;
            RequirePaswordChange = requirePaswordChange;
            Phone = phone;
            DocumentTypeId = documentTypeId;
            DocumentNo = documentNo;
            TrainingCenterId = trainingCenterId;
            CampusId = campusId;
            RolsId = rolsId;
            GlobalUser = globalUser;
        }
        public DtoAccessUser(Guid? id, string userName, string email, string firstName, string? otherNames, string lastName, string? otherLastName,
            bool requirePaswordChange, string createdBy, string? phone, Guid documentTypeId, string documentNo, Guid? trainingCenterId, List<Guid> campusId, List<Guid> rolsId, bool globalUser)
        {
            Id = id;
            UserName = userName;
            Email = email;
            FirstName = firstName;
            OtherNames = otherNames;
            LastName = lastName;
            OtherLastName = otherLastName;
            RequirePaswordChange = requirePaswordChange;
            CreatedBy = createdBy;
            Phone = phone;
            DocumentTypeId = documentTypeId;
            DocumentNo = documentNo;
            TrainingCenterId = trainingCenterId;
            CampusId = campusId;
            RolsId = rolsId;
            GlobalUser = globalUser;
        }
    }
}
