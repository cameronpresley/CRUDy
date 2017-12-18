using System;
using CRUDy.DataAccess;
using CRUDy.Domain;
using Optionally;

namespace CRUDy.Features.View
{
    public class Screen : Feature
    {
        public string Name => "View Item";
        public Action Workflow => Display;

        private readonly IItemRepository _repo;

        public Screen(IItemRepository repo)
        {
            _repo = repo;
        }

        public void Display()
        {
            new PromptForId.Workflow(_repo).Execute()
                .Match(FormatError, FormatItemOption)
                .Apply(Console.WriteLine);
        }

        private string FormatItemOption(IOption<Item> arg) => 
         arg.Match(() => "Failed to find requested item",
                item => $"Id: {item.Id}\nTitle: {item.Title}\nDescription: {item.Description}");

        private string FormatError(Exception ex) => ex.Message;
    }
}
