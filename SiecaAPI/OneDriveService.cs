using Microsoft.EntityFrameworkCore;
using SiecaAPI.Data.SQLImpl;
using SiecaAPI.Data.SQLImpl.Entities;
using SiecaAPI.OneDrive.Manager;
using SiecaAPI.OneDrive.Models;
using System.Globalization;
using System.Text;

namespace SiecaAPI;

public class OneDriveService : BackgroundService
{
    private readonly IConfiguration configuration;

    public OneDriveService(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                if (DateTime.Now.Hour != int.Parse(configuration["ScheduleHour"]!))
                {
                    await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
                    continue;
                }

                var reqestManager = new RequestManager(configuration);

                var model = await reqestManager.GetFilesAsync(stoppingToken);

                if (model == null)
                {
                    continue;
                }

                var filesToDownload = model.Value.Where(x => x.DownloadUrl != null && !x.Name.Contains("_prc") &&
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
                    var text = Encoding.Default.GetString(file.Bytes).Split(Environment.NewLine).Skip(1).ToArray();

                    using var context = new SqlContext();

                    var models = await ToModels(context, text);

                    await context.BeneficiaryAnthropometricData.AddRangeAsync(models, stoppingToken);
                    await context.SaveChangesAsync(stoppingToken);

                    file.NewName = Path.GetFileNameWithoutExtension(file.Name) + "_prc" + Path.GetExtension(file.Name);
                }

                await reqestManager.UpdateFilesAsync(filesToDownload, stoppingToken);

            }
            catch (Exception e)
            {
                continue;
            }
        }
    }

    private static async Task<BeneficiaryAnthropometricDataEntity[]> ToModels(SqlContext context, string[] text)
    {
        var models = new BeneficiaryAnthropometricDataEntity[text.Length];

        for (int i = 0; i < text.Length - 1; i++)
        {
            var chunks = text[i].Split(';');

            try
            {
                models[i] = new BeneficiaryAnthropometricDataEntity
                {
                    Id = Guid.NewGuid(),
                    BeneficiaryId = await context.Beneficiaries.Where(x => x.DocumentNumber == chunks[7]).Select(x => x.Id).FirstOrDefaultAsync(),
                    TrainingCenterId = await context.TrainingCenters.Where(x => x.Code == chunks[2]).Select(x => x.Id).FirstOrDefaultAsync(),
                    Comment = chunks[4],
                    Weight = Convert.ToDecimal(chunks[296] == "-" ? null : chunks[296]),
                    Height = Convert.ToDecimal(chunks[299] == "-" ? null : chunks[299]),
                    Bmi = Convert.ToDecimal(chunks[9] == "-" ? null : chunks[9]),
                    CreatedOn = DateTime.ParseExact(chunks[3].Replace(" p. m.", "").Replace(" a. m.", ""), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                    ModifiedOn = chunks[6] != "-" ? DateTime.ParseExact(chunks[6].Replace(" p. m.", "").Replace(" a. m.", ""), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture) : null
                };
            }
            catch (Exception e)
            {
            }
        }

        return models;
    }
}
