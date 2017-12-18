using System;
using System.Collections.Generic;
using System.Linq;
using CRUDy.Features;
using Optionally;

namespace CRUDy
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                var features = new FeatureMap();
                DisplayMenu(features.GetNames());

                features.Apply(GetChoice)
                    .AndThen(features.GetAction)
                    .Match(s => () => Console.WriteLine(s),
                        FunctionHelpers.Identity)
                    .Invoke();
            }
        }

        private static void DisplayMenu(IEnumerable<string> names)
        {
            var i = 0;
            names
                .Select(x => $"{++i}). {x}")
                .ToList()
                .ForEach(Console.WriteLine);
        }

        private static IResult<string, int> GetChoice(FeatureMap featureMap)
        {
            var choice = Console.ReadLine();
            return !Int32.TryParse(choice, out int x)
                ? Result.Failure<string, int>("Couldn't parse " + choice + " as an integer")
                : featureMap.DoesFeatureExist(x) ?
                    Result.Success<string, int>(x)
                    : Result.Failure<string, int>("Don't know the feature for choice #" + x);
        }
    }
}
