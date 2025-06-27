using Microsoft.Extensions.Time.Testing;

namespace FinanceManager.TestUtilities.Time;

public static class TestClockFactory
{
    public static FakeTimeProvider CreateFixed(DateTimeOffset fixedTime)
    {
        var fake = new FakeTimeProvider();
        fake.SetUtcNow(fixedTime);
        return fake;
    }
}