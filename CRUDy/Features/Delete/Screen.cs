using System;
using CRUDy.DataAccess;
using CRUDy.Domain;
using Optionally;

namespace CRUDy.Features.Delete
{
    public class Screen : Feature
    {
        private readonly IItemRepository _repo;

        public Screen(IItemRepository repo)
        {
            _repo = repo;
        }

        public void Display()
        {
            new PromptForId.Workflow(_repo).Execute()
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

        public string Name => "Delete an Item";
        public Action Workflow => Display;
    }
}
