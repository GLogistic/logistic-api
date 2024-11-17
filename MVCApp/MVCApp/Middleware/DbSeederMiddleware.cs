using DbAccess.Context;
using Entities;
using Route = Entities.Route;

namespace MVCApp.Middleware
{
    public class DbSeederMiddleware
    {
        private readonly RequestDelegate _next;

        public DbSeederMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, LogisticContext dbContext)
        {
            if (!dbContext.Cargos.Any() || !dbContext.Settlements.Any() || !dbContext.Routes.Any())
            {
                try
                {
                    var quantityEntity = 50;
                    var random = new Random();

                    var newSettlements = new Settlement[quantityEntity];

                    for (int i = 0; i < newSettlements.Length; i++)
                        newSettlements[i] = new Settlement
                        {
                            Title = $"Settlement {i}"
                        };

                    await dbContext.Settlements.AddRangeAsync(newSettlements);

                    dbContext.SaveChanges();

                    var newRoutes = new Route[quantityEntity];

                    for (int i = 0; i < newRoutes.Length; i++)
                    {
                        var startSettlementIndex = random.Next(newSettlements.Length);
                        var endSettlementIndex = Enumerable.Range(0, quantityEntity).Except([startSettlementIndex]).First();

                        var startSettlement = newSettlements[startSettlementIndex];
                        var endSettlement = newSettlements[endSettlementIndex];

                        var newRoute = new Route
                        {
                            Distance = random.Next(100, 1000),
                            StartSettlement = startSettlement,
                            EndSettlement = endSettlement
                        };

                        newRoutes[i] = newRoute;
                    }

                    await dbContext.Routes.AddRangeAsync(newRoutes);

                    var newCargos = new Cargo[quantityEntity];

                    for (int i = 0; i < newCargos.Length; i++)
                    {
                        var newCargo = new Cargo
                        {
                            Title = $"Cargo {i}",
                            Weight = random.Next(500, 2000),
                            RegistrationNumber = $"REG-{random.Next(1000, 9999)}"
                        };

                        newCargos[i] = newCargo;
                    }

                    await dbContext.Cargos.AddRangeAsync(newCargos);

                    dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            await _next(context);
        }
    }
}
