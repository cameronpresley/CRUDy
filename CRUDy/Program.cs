using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRUDy.DataAccess;
using CRUDy.Domain;
using Optionally;

namespace CRUDy
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                DisplayMenu();
                GetChoice()
                    .Map(GetAction)
                    .Match(s => () => Console.WriteLine(s),
                        FunctionHelper.Identity)
                    .Invoke();
            }
        }

        private static Action GetAction(int choice)
        {
            var repo = new ItemRepository();
            Action exit = () => Environment.Exit(0);
            Action add = new Add.Screen(repo).Display;

            return choice == 1 ? add : exit;
        }

        private static void DisplayMenu()
        {
            new List<string>
            {
                "1). Add an item.",
                "2). Quit"
            }.ForEach(Console.WriteLine);
        }

        private static IResult<string,int> GetChoice()
        {
            var choice = Console.ReadLine();
            return !Int32.TryParse(choice, out int x)
                ? Result.Failure<string, int>("Couldn't parse " + choice + " as an integer")
                : x == 1 || x == 2
                    ? Result.Success<string, int>(x)
                    : Result.Failure<string, int>("Don't know the feature for choice #" + x);
        }
    }
}
