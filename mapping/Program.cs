using library;
using mapping.models;

internal class Program
{
    private static void Main(string[] args)
    {
        //Console.WriteLine("Hello, World!");
        var test = new ExampleModel()
        {
            PropOne = "Test Value",
            PropThree = DateTime.Now,
            PropFive = "Another Test Value"
        };

        var test2 = new ExampleModel()
        {
            PropOne = "value of test 2",
            PropTwo = 2,
            PropFour = true
        };

        //test.DisplayData();
        var newClass = test.ToClone();
        //newClass.DisplayData();
        var merged = test.ToMerged(test2);
        merged.DisplayData();
    }
}