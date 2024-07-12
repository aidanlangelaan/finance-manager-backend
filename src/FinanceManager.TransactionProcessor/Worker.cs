namespace FinanceManager.TransactionProcessor;

public class Worker(ILogger<Worker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Worker started running at: {Time}", DateTimeOffset.Now);
            
            
            
            logger.LogInformation("Worker completed running at: {Time}", DateTimeOffset.Now);

            await Task.Delay(1000, stoppingToken);
        }
    }
}