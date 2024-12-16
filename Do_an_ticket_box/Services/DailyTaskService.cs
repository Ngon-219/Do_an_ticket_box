using Do_an_ticket_box.Services;

public class DailyTaskService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<DailyTaskService> _logger;

    public DailyTaskService(IServiceScopeFactory scopeFactory, ILogger<DailyTaskService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Running daily task at: {time}", DateTimeOffset.Now);

            // Tạo scope mới
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Thực hiện công việc với dbContext
                var handleBanner = dbContext.Events
                    .Where(e => e.Event_date_end < DateTime.Now && e.status == "Banner")
                    .ToList();

                foreach (var data in handleBanner)
                {
                    data.status = "vertify";
                }
                await dbContext.SaveChangesAsync();
            }

            // Chờ đến lần chạy tiếp theo (ví dụ: 6h sáng hôm sau)
            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }
}
