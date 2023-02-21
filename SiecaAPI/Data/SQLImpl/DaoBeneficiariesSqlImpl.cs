using Microsoft.EntityFrameworkCore;
using SiecaAPI.Data.Interfaces;
using SiecaAPI.Data.SQLImpl.Entities;
using SiecaAPI.DTO.Data;
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
                OrganizationEntity org = await context.Organizations.Where(o => o.Id.Equals(beneficiary.Id)).FirstAsync();
                DocumentTypeEntity bDocType = await context.DocumentTypes
                    .Where(dt => dt.Id.Equals(beneficiary.DocumentTypeId)).FirstAsync();

                BeneficiariesEntity newB = new()
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
                };
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
            return new DtoBeneficiaries();
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
    }
}
