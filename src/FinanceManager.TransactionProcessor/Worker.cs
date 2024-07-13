using FinanceManager.Business.Interfaces;

namespace FinanceManager.TransactionProcessor;

public class Worker(ILogger<Worker> logger, IImportService importService) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Worker started running at: {Time}", DateTimeOffset.Now);
            
            var processResult = await importService.HandleImports();
            switch (processResult)
            {
                case null:
                    logger.LogInformation("No imports to process");
                    break;
                case { IsSuccess: true }:
                    logger.LogInformation("Successfully processed {FileName} - {RecordCount} records imported", processResult.FileName, processResult.Imported);
                    break;
                default:
                    logger.LogWarning("Failed to process imports");
                    break;
            }
            
            logger.LogInformation("Worker completed running at: {Time}", DateTimeOffset.Now);

            await Task.Delay(1000, stoppingToken);
        }
    }
}