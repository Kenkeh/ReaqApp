using reaquisites.Managers;
public class RegisterTicketerService : IHostedService, IDisposable
{
    private readonly ILogger<RegisterTicketerService> _logger;
    private Timer _timer = null!;

    public RegisterTicketerService(ILogger<RegisterTicketerService> logger)
    {
        _logger = logger;
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service running.");

        _timer = new Timer(DoWork, null, TimeSpan.Zero, 
            TimeSpan.FromSeconds(5));

        return Task.CompletedTask;
    }

    private void DoWork(object? state)
    {
        UsersManager.checkTickets();
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Register Ticketer Service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}