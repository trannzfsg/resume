namespace HelloWorld.UnitTests.Practice;

public sealed class LinqPracticeTests
{
    private static readonly DateTimeOffset BaseTime = new(2026, 6, 25, 0, 0, 0, TimeSpan.Zero);

    [Fact]
    public void Can_filter_group_order_and_project_recent_sales()
    {
        var orders = new[]
        {
            new Order("cust-1", "Tran", 120m, "Paid", BaseTime.AddHours(-4)),
            new Order("cust-2", "Alex", 50m, "Pending", BaseTime.AddHours(-3)),
            new Order("cust-1", "Tran", 80m, "Paid", BaseTime.AddHours(-2)),
            new Order("cust-3", "Sam", 220m, "Paid", BaseTime.AddHours(-1)),
            new Order("cust-2", "Alex", 75m, "Paid", BaseTime.AddHours(-6))
        };

        var summaries = orders
            .Where(order => order.Status == "Paid")
            .GroupBy(order => new { order.CustomerId, order.CustomerName })
            .Select(group => new CustomerSalesSummary(
                group.Key.CustomerId,
                group.Key.CustomerName,
                group.Count(),
                group.Sum(order => order.Amount),
                group.Max(order => order.CreatedAtUtc)))
            .OrderByDescending(summary => summary.TotalAmount)
            .ThenBy(summary => summary.CustomerName)
            .ToArray();

        Assert.Collection(
            summaries,
            summary =>
            {
                Assert.Equal("cust-3", summary.CustomerId);
                Assert.Equal(220m, summary.TotalAmount);
            },
            summary =>
            {
                Assert.Equal("cust-1", summary.CustomerId);
                Assert.Equal(200m, summary.TotalAmount);
                Assert.Equal(2, summary.OrderCount);
            },
            summary =>
            {
                Assert.Equal("cust-2", summary.CustomerId);
                Assert.Equal(75m, summary.TotalAmount);
            });
    }

    [Fact]
    public void Can_left_join_customers_to_orders_and_find_customers_without_paid_orders()
    {
        var customers = new[]
        {
            new Customer("cust-1", "Tran"),
            new Customer("cust-2", "Alex"),
            new Customer("cust-3", "Sam")
        };

        var orders = new[]
        {
            new Order("cust-1", "Tran", 120m, "Paid", BaseTime.AddHours(-4)),
            new Order("cust-1", "Tran", 80m, "Refunded", BaseTime.AddHours(-2)),
            new Order("cust-2", "Alex", 50m, "Pending", BaseTime.AddHours(-3))
        };

        var customersWithoutPaidOrders = customers
            .GroupJoin(
                orders.Where(order => order.Status == "Paid"),
                customer => customer.Id,
                order => order.CustomerId,
                (customer, paidOrders) => new
                {
                    Customer = customer,
                    PaidOrders = paidOrders
                })
            .Where(result => !result.PaidOrders.Any())
            .Select(result => result.Customer.Name)
            .Order()
            .ToArray();

        Assert.Equal(["Alex", "Sam"], customersWithoutPaidOrders);
    }

    private sealed record Customer(string Id, string Name);

    private sealed record Order(
        string CustomerId,
        string CustomerName,
        decimal Amount,
        string Status,
        DateTimeOffset CreatedAtUtc);

    private sealed record CustomerSalesSummary(
        string CustomerId,
        string CustomerName,
        int OrderCount,
        decimal TotalAmount,
        DateTimeOffset LastOrderAtUtc);
}
