﻿using SecurityLayerDotNetAPI.Data.SQLImpl.Entities;
using SecurityLayerDotNetAPI.DTO.Data;

namespace SecurityLayerDotNetAPI.Models
{
    public class Rol
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public virtual OrganizationEntity Organization { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public Boolean NewAccessUserDefaultRol { get; set; }
        public Boolean Enable { get; set; } = true;
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Rol(Guid id, Guid organizationId, string name, string? description, bool newAccessUserDefaultRol, bool enable, string createdBy, DateTime? createdOn, string? modifiedBy, DateTime? modifiedOn)
        {
            Id = id;
            OrganizationId = organizationId;
            Name = name;
            Description = description;
            NewAccessUserDefaultRol = newAccessUserDefaultRol;
            Enable = enable;
            CreatedBy = createdBy;
            CreatedOn = createdOn;
            ModifiedBy = modifiedBy;
            ModifiedOn = modifiedOn;
        }

    }
}
