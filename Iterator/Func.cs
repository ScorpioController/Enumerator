using System.Runtime.CompilerServices;

namespace Enumerator;

public class Func
{
    public static IEnumerable<int> GenerateEvenSeries(int limit)
    {
        for (int i = 0; i <= limit; i++)
            yield return i * 2;
    }
}