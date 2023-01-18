namespace GrpcStreamBenchmark.Core;

public partial class Record
{
    private static readonly Random Random = new();

    public static Record New()
    {
        return new()
        {
            Id = String(),
            AuditId = String(),
            IsActive = Random.Next(0, 2) == 1,
            Status1 = Int32(),
            Status2 = Int32(),
            Symbol = String(),
            SymbolId = Int32(),

            Value1 = Double(),
            Value2 = Double(),
            Value3 = Double(),
            Value4 = Double(),
            Value5 = Double(),
            Value6 = Double(),
            Value7 = Double(),
            Value8 = Double(),
            Value9 = Double(),
            Value10 = Double(),
            Value11 = Double(),

            Ts1 = Int64(),
            Ts2 = Int64(),
            Ts3 = Int64(),
            Ts4 = Int64(),
            Ts5 = Int64(),
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
        };

        static int Int32() => Random.Next(int.MaxValue);
        static long Int64() => Random.Next(int.MaxValue);
        static double Double() => Random.NextDouble() * 100;
        static string String() => Guid.NewGuid().ToString();
    }
}
