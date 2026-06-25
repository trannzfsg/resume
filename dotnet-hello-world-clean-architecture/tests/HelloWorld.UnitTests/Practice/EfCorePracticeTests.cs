using Microsoft.EntityFrameworkCore;

namespace HelloWorld.UnitTests.Practice;

public sealed class EfCorePracticeTests
{
    [Fact]
    public async Task Can_query_project_and_order_related_data()
    {
        await using var database = await PracticeDatabase.CreateAsync();

        var summaries = await database.Context.Customers
            .AsNoTracking()
            .Where(customer => customer.Orders.Any(order => order.Status == OrderStatus.Paid))
            .Select(customer => new CustomerSummary(
                customer.Name,
                customer.Orders.Count(order => order.Status == OrderStatus.Paid),
                customer.Orders
                    .Where(order => order.Status == OrderStatus.Paid)
                    .Sum(order => order.Amount)))
            .OrderByDescending(summary => summary.PaidTotal)
            .ThenBy(summary => summary.CustomerName)
            .ToListAsync();

        Assert.Collection(
            summaries,
            summary =>
            {
                Assert.Equal("Sam", summary.CustomerName);
                Assert.Equal(220m, summary.PaidTotal);
            },
            summary =>
            {
                Assert.Equal("Tran", summary.CustomerName);
                Assert.Equal(200m, summary.PaidTotal);
                Assert.Equal(2, summary.PaidOrderCount);
            });
    }

    [Fact]
    public async Task Can_update_an_entity_and_persist_changes()
    {
        await using var database = await PracticeDatabase.CreateAsync();

        var order = await database.Context.Orders
            .SingleAsync(order => order.Reference == "ORD-003");

        order.MarkPaid();

        await database.Context.SaveChangesAsync();

        var paidOrderCount = await database.Context.Orders
            .AsNoTracking()
            .CountAsync(order => order.Customer.Name == "Alex" && order.Status == OrderStatus.Paid);

        Assert.Equal(1, paidOrderCount);
    }

    [Fact]
    public async Task Can_eager_load_related_entities_when_the_object_graph_is_needed()
    {
        await using var database = await PracticeDatabase.CreateAsync();

        var tran = await database.Context.Customers
            .AsNoTracking()
            .Include(customer => customer.Orders)
            .SingleAsync(customer => customer.Name == "Tran");

        Assert.Equal(2, tran.Orders.Count);
        Assert.All(tran.Orders, order => Assert.Equal(tran.Id, order.CustomerId));
    }

    private sealed record CustomerSummary(
        string CustomerName,
        int PaidOrderCount,
        decimal PaidTotal);

    private sealed class PracticeDatabase : IAsyncDisposable
    {
        private PracticeDatabase(SalesDbContext context)
        {
            Context = context;
        }

        public SalesDbContext Context { get; }

        public static async Task<PracticeDatabase> CreateAsync()
        {
            var options = new DbContextOptionsBuilder<SalesDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new SalesDbContext(options);
            await SeedAsync(context);

            return new PracticeDatabase(context);
        }

        public async ValueTask DisposeAsync()
        {
            await Context.DisposeAsync();
        }

        private static async Task SeedAsync(SalesDbContext context)
        {
            var tran = new Customer("Tran");
            tran.AddOrder(new Order("ORD-001", 120m, OrderStatus.Paid));
            tran.AddOrder(new Order("ORD-002", 80m, OrderStatus.Paid));

            var alex = new Customer("Alex");
            alex.AddOrder(new Order("ORD-003", 50m, OrderStatus.Pending));

            var sam = new Customer("Sam");
            sam.AddOrder(new Order("ORD-004", 220m, OrderStatus.Paid));

            context.Customers.AddRange(tran, alex, sam);
            await context.SaveChangesAsync();
        }
    }

    private sealed class SalesDbContext(DbContextOptions<SalesDbContext> options)
        : DbContext(options)
    {
        public DbSet<Customer> Customers => Set<Customer>();

        public DbSet<Order> Orders => Set<Order>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(customer =>
            {
                customer.HasKey(entity => entity.Id);
                customer.Property(entity => entity.Name).HasMaxLength(100).IsRequired();
                customer
                    .HasMany(entity => entity.Orders)
                    .WithOne(entity => entity.Customer)
                    .HasForeignKey(entity => entity.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Order>(order =>
            {
                order.HasKey(entity => entity.Id);
                order.Property(entity => entity.Reference).HasMaxLength(40).IsRequired();
                order.Property(entity => entity.Amount).HasPrecision(18, 2);
                order.Property(entity => entity.Status).HasConversion<string>().HasMaxLength(20);
            });
        }
    }

    private sealed class Customer
    {
        private readonly List<Order> _orders = [];

        private Customer()
        {
        }

        public Customer(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; } = string.Empty;

        public IReadOnlyCollection<Order> Orders => _orders;

        public void AddOrder(Order order)
        {
            ArgumentNullException.ThrowIfNull(order);
            _orders.Add(order);
        }
    }

    private sealed class Order
    {
        private Order()
        {
        }

        public Order(string reference, decimal amount, OrderStatus status)
        {
            Id = Guid.NewGuid();
            Reference = reference;
            Amount = amount;
            Status = status;
        }

        public Guid Id { get; private set; }

        public string Reference { get; private set; } = string.Empty;

        public decimal Amount { get; private set; }

        public OrderStatus Status { get; private set; }

        public Guid CustomerId { get; private set; }

        public Customer Customer { get; private set; } = null!;

        public void MarkPaid()
        {
            Status = OrderStatus.Paid;
        }
    }

    private enum OrderStatus
    {
        Pending,
        Paid,
        Refunded
    }
}
