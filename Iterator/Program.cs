// See https://aka.ms/new-console-template for more information

namespace Enumerator;

internal class Program
{
    static void Main(string[] args)
    {
        foreach (int element in Func.GenerateEvenSeries(10))
        {
            Console.WriteLine(element);
        }
        
        var ite = Func.GenerateEvenSeries(10).GetEnumerator();
        while (ite.MoveNext())
        {
            int current = ite.Current;
            Console.WriteLine(current);      
        }
        
        // 等效的调用
        Iterator iterator = Iterator.GenerateEvenSeriesInter(10);
        while (iterator.MoveNext())
        {
            int current = iterator.Current;
            Console.WriteLine(current);
        }
    }
}