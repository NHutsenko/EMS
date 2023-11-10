// ReSharper disable once CheckNamespace
namespace EMS.Protos;

public partial class DecimalValue
{
    private const decimal NanoFactor = 1_000_000_000;
    public DecimalValue(long units, int nanos) {
        Units = units;
        Nanos = nanos;
    }

    public static implicit operator decimal(DecimalValue decimalValue) => decimalValue.ToDecimal();

    public static implicit operator DecimalValue(decimal value) => FromDecimal(value);

    public decimal ToDecimal()
    {
        return Units + Nanos / NanoFactor;
    }

    public static DecimalValue FromDecimal(decimal value)
    {
        long units = decimal.ToInt64(value);
        int nanos = decimal.ToInt32((value - units) * NanoFactor);
        return new DecimalValue(units, nanos);
    }
}