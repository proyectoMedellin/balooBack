using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Kiota.Abstractions;
using Microsoft.VisualBasic.FileIO;
using Quartz;
using SiecaAPI.Data.SQLImpl;
using SiecaAPI.Data.SQLImpl.Entities;
using SiecaAPI.OneDrive.Manager;
using SiecaAPI.OneDrive.Models;
using System.Globalization;
using System.IO;
using System.Text;

namespace SiecaAPI.OneDrive;

public class OneDriveServiceJob : IJob
{
    private readonly IConfiguration configuration;

    public OneDriveServiceJob(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        CancellationToken stoppingToken = default;

        try
        {
            var reqestManager = new RequestManager(configuration);

            var model = await reqestManager.GetFilesAsync(stoppingToken);

            if (model == null)
            {
                return;
            }

            var filesToDownload = model.Value.Where(x => x.DownloadUrl != null && !x.Name.Contains(configuration["Suffix"]!) &&
                string.Equals(Path.GetExtension(x.Name), ".csv", StringComparison.InvariantCultureIgnoreCase))
                .Select(x => new FileDataModel
                {
                    DownloadUrl = x.DownloadUrl,
                    Name = x.Name,
                    ItemId = x.Id
                }).ToArray();

            await reqestManager.ReadFilesAsync(filesToDownload, stoppingToken);

            foreach (var file in filesToDownload)
            {
                using var sqlContext = new SqlContext();

                var models = await ToModelsAsync(sqlContext, file.Bytes);

                await sqlContext.BeneficiaryAnthropometricData.AddRangeAsync(models, stoppingToken);
                await sqlContext.SaveChangesAsync(stoppingToken);

                file.NewName = Path.GetFileNameWithoutExtension(file.Name) + configuration["Suffix"]! + Path.GetExtension(file.Name);
            }

            await reqestManager.UpdateFilesAsync(filesToDownload, stoppingToken);

        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed due to -> " + ex.ToString());
        }
    }

    private static async Task<IList<BeneficiaryAnthropometricDataEntity>> ToModelsAsync(SqlContext context, byte[] data)
    {
        var models = new List<BeneficiaryAnthropometricDataEntity>();
        using var ms = new MemoryStream(data);

        try
        {
            var csv = new CsvReader(reader: new StreamReader(ms, Encoding.UTF8), new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ";",
            });

            csv.Read();
            csv.ReadHeader();

            while (await csv.ReadAsync())
            {
                csv.TryGetField("id", out string? docNumber);
                csv.TryGetField("doctor", out string? tcCode);

                if (!string.IsNullOrEmpty(docNumber) && !string.IsNullOrEmpty(tcCode))
                {
                    List<BeneficiariesEntity> benList = await context.Beneficiaries.Where(x => x.DocumentNumber == docNumber).ToListAsync();
                    List<TrainingCenterEntity> tcList = await context.TrainingCenters.Where(x => x.Code == tcCode).ToListAsync();

                    if (benList.Count > 0 && tcList.Count > 0)
                    {
                        DateTime CreatedOn = DateTime.ParseExact(csv.GetField("created")!.Replace(" p. m.", "").Replace(" a. m.", ""), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                        models.Add(new BeneficiaryAnthropometricDataEntity
                        {
                            Id = Guid.NewGuid(),
                            BeneficiaryId = benList[0].Id,
                            Beneficiaries = benList[0],
                            TrainingCenterId = tcList[0].Id,
                            TrainingCenter = tcList[0],
                            Comment = csv.GetField("comment"),
                            Weight = Convert.ToDecimal(csv.GetField("Weight value") == "-" ? null : csv.GetField("Weight value")),
                            Height = Convert.ToDecimal(csv.GetField("Height value") == "-" ? null : csv.GetField("Height value")),
                            Bmi = Convert.ToDecimal(csv.GetField("BMI value") == "-" ? null : csv.GetField("BMI value")),
                            CreatedOn = CreatedOn.ToUniversalTime(),
                            ModifiedOn = csv.GetField("last modified") != "-" ? DateTime.ParseExact(csv.GetField("last modified")!.Replace(" p. m.", "").Replace(" a. m.", ""), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture) : null
                        });
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed due to -> " + ex.ToString());
        }

        return models;
    }
}
