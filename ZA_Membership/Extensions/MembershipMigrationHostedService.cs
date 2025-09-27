using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ZA_Membership.Data;

namespace ZA_Membership.Extensions
{
    /// <summary>
    /// HostedService که هنگام استارتاپ جداول Membership را به صورت ایمن ایجاد می‌کند
    /// </summary>
    public class MembershipMigrationHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public MembershipMigrationHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<MembershipDbContext>();
            await db.Database.MigrateAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
