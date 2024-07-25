// See https://aka.ms/new-console-template for more information

namespace Pipline;

internal class Program
{
    static void Main(string[] args)
    {
        var nums = new List<int> {1, 2, 3, 4, 5, 6};

        var result = nums
            .Where(num => num % 2 == 0)
            .Select(num => num);
    }
}