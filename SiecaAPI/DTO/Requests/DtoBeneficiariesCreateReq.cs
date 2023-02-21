﻿using SiecaAPI.DTO.Requests;
using SiecaAPI.Models;
using System.Reflection;

namespace SiecaAPI.DTO.Data
{
    public class DtoBeneficiariesCreateReq
    {
        public Guid DocumentTypeId { get; set; } = Guid.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string OtherNames { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string OtherLastName { get; set; } = string.Empty;
        public Guid GenderId { get; set; } = Guid.Empty;
        public DateTime BirthDate { get; set; } = DateTime.UtcNow;
        public Guid BirthCountryId { get; set; } = Guid.Empty;
        public Guid BirthDepartmentId { get; set; } = Guid.Empty;
        public Guid BirthCityId { get; set; } = Guid.Empty;
        public Guid RhId { get; set; } = Guid.Empty;
        public Guid BloodTypeId { get; set; } = Guid.Empty;
        public string EmergencyPhoneNumber { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
        public Guid AdressZoneId { get; set; } = Guid.Empty;
        public string Adress { get; set; } = string.Empty;
        public string Neighborhood { get; set; } = string.Empty;
        public string AdressPhoneNumber { get; set; } = string.Empty;
        public string AdressObservations { get; set; } = string.Empty;
        public string CreationUser { get; set; } = string.Empty;
        public List<DtoBeneficiariesCreateReqFamilyMember> Family { get; set; } = new();
    }
}
