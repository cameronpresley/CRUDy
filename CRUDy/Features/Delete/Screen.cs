using System;
using CRUDy.DataAccess;
using CRUDy.Domain;
using Optionally;

namespace CRUDy.Features.Delete
{
    public class Screen
    {
        private readonly IItemRepository _repo;

        public Screen(IItemRepository repo)
        {
            _repo = repo;
        }

        public Feature Create()
        {
            return new Feature("Delete an item", Display);
        }

        public void Display()
        {
            Console.WriteLine("What's the id?");
            Console.ReadLine()
                .Apply(ParseId)
                .AndThen(_repo.GetById)
                .AndThen(DeleteItem)
                .Match(FormatError, FormatItem)
                .Apply(Console.WriteLine);

        }

        private string FormatError(Exception ex) => ex.Message;
        private string FormatItem(Item item) => $"Deleted item #{item.Id}";

        private IResult<Exception, Item> DeleteItem(IOption<Item> optItem)
        {
            return optItem
                .Match(
                    () => Result.Failure<Exception, Item>(new Exception("Failed to find the requested record"))
                    , item => _repo.Delete(item));
        }

        private IResult<Exception, int> ParseId(string s)
        {
            return Int32.TryParse(s, out int x)
                ? Result.Success<Exception, int>(x)
                : Result.Failure<Exception, int>(new Exception("Failed to parse the id as a number."));
        }
    }
}
