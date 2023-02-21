using Microsoft.EntityFrameworkCore;
using SiecaAPI.Data.Interfaces;
using SiecaAPI.Data.SQLImpl.Entities;
using SiecaAPI.DTO.Data;
using SiecaAPI.Errors;
using SiecaAPI.Models;
using System.Collections.Generic;

namespace SiecaAPI.Data.SQLImpl
{
    public class DaoBeneficiariesSqlImpl : IDaoBeneficiaries
    {
        public async Task<DtoBeneficiaries> CreateAsync(DtoBeneficiaries beneficiary)
        {
            using SqlContext context = new();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                OrganizationEntity org = await context.Organizations
                    .Where(o => o.Id.Equals(beneficiary.OrganizationId)).FirstAsync();
                DocumentTypeEntity bDocType = await context.DocumentTypes
                    .Where(dt => dt.Id.Equals(beneficiary.DocumentTypeId)).FirstAsync();
                BeneficiariesParametersEntity gender = await context.BeneficiariesParameters
                    .Where(g => g.Id.Equals(beneficiary.GenderId)).FirstAsync();
                BeneficiariesParametersEntity rh = await context.BeneficiariesParameters
                    .Where(rh => rh.Id.Equals(beneficiary.RhId)).FirstAsync();
                BeneficiariesParametersEntity bloodType = await context.BeneficiariesParameters
                    .Where(bt => bt.Id.Equals(beneficiary.BloodTypeId)).FirstAsync();
                BeneficiariesParametersEntity adressZone = await context.BeneficiariesParameters
                    .Where(az => az.Id.Equals(beneficiary.AdressZoneId)).FirstAsync();

                CountryEntity birthContry = await context.Countries
                    .Where(c => c.Id.Equals(beneficiary.BirthCountryId)).FirstAsync();
                DepartmentEntity birthDepartment = await context.Departments
                    .Where(d => d.Id.Equals(beneficiary.BirthDepartmentId)).FirstAsync();
                CityEntity birthCity = await context.Cities
                    .Where(c => c.Id.Equals(beneficiary.BirthCityId)).FirstAsync();

                BeneficiariesEntity newBeneficiary = new()
                {
                    OrganizationId = beneficiary.OrganizationId,
                    Organization = org,
                    DocumentTypeId = beneficiary.DocumentTypeId,
                    DocumentType = bDocType,
                    DocumentNumber = beneficiary.DocumentNumber,
                    FirstName = beneficiary.FirstName,  
                    OtherNames = beneficiary.OtherNames,
                    LastName = beneficiary.LastName,
                    OtherLastName = beneficiary.OtherLastName,
                    GenderId = gender.Id,
                    Gender = gender,
                    BirthDate = beneficiary.BirthDate,
                    BirthCountryId = birthContry.Id,
                    BirthCountry = birthContry,
                    BirthDepartmentId = birthDepartment.Id,
                    BirthDepartment = birthDepartment,
                    BirthCityId = birthCity.Id,
                    BirthCity = birthCity,
                    RhId = rh.Id,
                    Rh = rh,
                    BloodTypeId = bloodType.Id,
                    BloodType = bloodType,
                    EmergencyPhoneNumber = beneficiary.EmergencyPhoneNumber,
                    PhotoUrl = beneficiary.PhotoUrl,
                    AdressZoneId = adressZone.Id,
                    AdressZone = adressZone,
                    Adress = beneficiary.Adress,
                    Neighborhood = beneficiary.Neighborhood,
                    AdressPhoneNumber = beneficiary.AdressPhoneNumber,
                    AdressObservations = beneficiary.AdressObservations,
                    Enabled = beneficiary.Enabled,
                    CreatedBy = beneficiary.CreatedBy,
                    CreatedOn = DateTime.UtcNow,
                };
                await context.Beneficiaries.AddAsync(newBeneficiary);
                await context.SaveChangesAsync();
                beneficiary.Id = newBeneficiary.Id;

                //se adicionan los familiares
                for (int fcount = 0; fcount < beneficiary.FamilyMembers.Count; fcount++)
                {
                    DtoBeneficiariesFamily f = beneficiary.FamilyMembers[fcount];

                    DocumentTypeEntity bDocTypeFM = await context.DocumentTypes
                    .Where(dt => dt.Id.Equals(f.DocumentTypeId)).FirstAsync();

                    BeneficiariesParametersEntity fRelation = await context.BeneficiariesParameters
                    .Where(fr => fr.Id.Equals(f.FamilyRelationId)).FirstAsync();

                    BeneficiariesFamilyEntity newFmember = new()
                    {
                        OrganizationId = org.Id,
                        Organization = org,
                        BeneficiaryId = newBeneficiary.Id,
                        Beneficiary = newBeneficiary,
                        DocumentTypeId = bDocTypeFM.Id,
                        DocumentType = bDocTypeFM,
                        DocumentNumber = f.DocumentNumber,
                        FirstName = f.FirstName,
                        OtherNames = f.OtherNames,
                        LastName = f.LastName,
                        OtherLastName = f.OtherLastName,
                        FamilyRelationId = fRelation.Id,
                        FamilyRelation = fRelation,
                        Attendant = f.Attendant,
                        Enabled = f.Enabled,
                        CreatedBy = newBeneficiary.CreatedBy,
                        CreatedOn = DateTime.UtcNow
                    };

                    await context.BeneficiariesFamilies.AddAsync(newFmember);
                    await context.SaveChangesAsync();
                    beneficiary.FamilyMembers[fcount].Id = newFmember.Id;
                    beneficiary.FamilyMembers[fcount].BeneficiaryId = newFmember.BeneficiaryId;
                    beneficiary.FamilyMembers[fcount].OrganizationId = newFmember.OrganizationId;
                }

                transaction.Commit();
                return beneficiary;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            using SqlContext context = new();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                context.BeneficiariesFamilies
                    .RemoveRange(await context.BeneficiariesFamilies
                        .Where(bf => bf.BeneficiaryId.Equals(id))
                        .ToListAsync());
                await context.SaveChangesAsync();

                context.Beneficiaries
                    .RemoveRange(await context.Beneficiaries
                        .Where(b => b.Id.Equals(id)).ToListAsync());
                await context.SaveChangesAsync();

                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                return false;
            }
        }

        public async Task<bool> ExistAsync(Guid docmentTypeId, string documentNumber)
        {
            using SqlContext context = new();
            List<BeneficiariesEntity> ben = await context.Beneficiaries
                .Where(b => b.DocumentTypeId.Equals(docmentTypeId) && b.DocumentNumber.Equals(documentNumber))
                .ToListAsync();

            return ben.Count > 0;
        }

        public async Task<List<DtoBeneficiariesParameters>> GetBeneficiaryParameterInfoByType(string type)
        {
            List<DtoBeneficiariesParameters> response = new();
            
            using SqlContext context = new();
            List<BeneficiariesParametersEntity> paramsData = await context.BeneficiariesParameters
                .Where(bp => bp.ParamType.Equals(type)).ToListAsync();

            foreach (BeneficiariesParametersEntity p in paramsData)
            {
                response.Add(new DtoBeneficiariesParameters() { 
                    Id = p.Id,
                    OrganizationId = p.OrganizationId,
                    ParamType = p.ParamType,
                    ParamCode = p.ParamCode,
                    ParamValue = p.ParamValue
                });
            }
            return response;
        }

        public async Task<DtoBeneficiaries> GetById(Guid id)
        {
            using SqlContext context = new();
            try
            {
                BeneficiariesEntity benReq = await context
                    .Beneficiaries.Where(b => b.Id.Equals(id)).FirstAsync();

                DtoBeneficiaries response = new() { 
                    Id = benReq.Id,
                    OrganizationId = benReq.OrganizationId,
                    DocumentTypeId = benReq.DocumentTypeId,
                    DocumentNumber = benReq.DocumentNumber,
                    FirstName = benReq.FirstName,
                    OtherNames = benReq.OtherNames,
                    LastName = benReq.LastName,
                    OtherLastName = benReq.OtherLastName,
                    GenderId = benReq.GenderId,
                    BirthDate = benReq.BirthDate,
                    BirthCountryId = benReq.BirthCountryId,
                    BirthDepartmentId = benReq.BirthDepartmentId,
                    BirthCityId = benReq.BirthCityId,
                    RhId = benReq.RhId,
                    BloodTypeId = benReq.BloodTypeId,
                    EmergencyPhoneNumber = benReq.EmergencyPhoneNumber,
                    PhotoUrl = benReq.PhotoUrl,
                    AdressZoneId = benReq.AdressZoneId,
                    Adress = benReq.Adress,
                    Neighborhood = benReq.Neighborhood,
                    AdressPhoneNumber = benReq.AdressPhoneNumber,
                    AdressObservations = benReq.AdressObservations,
                    Enabled = benReq.Enabled,
                    FamilyMembers = new()
                };

                List<BeneficiariesFamilyEntity> family = await context
                    .BeneficiariesFamilies.Where(bf => bf.BeneficiaryId.Equals(benReq.Id)).ToListAsync();

                foreach(BeneficiariesFamilyEntity f in family)
                {
                    response.FamilyMembers.Add(new DtoBeneficiariesFamily() { 
                        Id = f.Id,
                        BeneficiaryId = f.BeneficiaryId,
                        OrganizationId = f.OrganizationId,
                        FirstName = f.FirstName,
                        OtherNames = f.OtherNames,
                        LastName = f.LastName,
                        OtherLastName = f.OtherLastName,
                        FamilyRelationId = f.FamilyRelationId,
                        Attendant = f.Attendant,
                        Enabled = f.Enabled
                    });
                }

                return response;
            }
            catch
            {
                throw new NoDataFoundException("the requested beneficiary dosent exist");
            }
        }
    }
}
