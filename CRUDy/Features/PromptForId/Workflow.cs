using System;
using CRUDy.DataAccess;
using CRUDy.Domain;
using Optionally;

namespace CRUDy.Features.PromptForId
{
    public class Workflow
    {
        private readonly IItemRepository _repo;

        public Workflow(IItemRepository repo)
        {
            _repo = repo;
        }

        public IResult<Exception, IOption<Item>> Execute()
        {
            Console.WriteLine("What is the item's id?");
            return Console.ReadLine()
                .Apply(ParseId)
                .AndThen(_repo.GetById);
        }

        private IResult<Exception, int> ParseId(string s)
        {
            return Int32.TryParse(s, out int x)
                ? Result.Success<Exception, int>(x)
                : Result.Failure<Exception, int>(new Exception("Couldn't parse " + s + " as a valid id."));
        }
    }
}
