using System;
using System.Collections.Generic;
using CRUDy.DataAccess;
using CRUDy.Domain;
using Optionally;

namespace CRUDy.Features.Edit
{
    public class Screen : Feature
    {
        private readonly IItemRepository _repo;

        public string Name => "Edit an Item";
        public Action Workflow => Display;

        public Screen(IItemRepository repo)
        {
            _repo = repo;
        }

        public void Display()
        {
            new PromptForId.Workflow(_repo).Execute()
                .Map(AskForNewItem)
                .AndThen(EditItem)
                .Match(FormatFailures, FormatItem)
                .Apply(Console.WriteLine);
        }

        private string FormatItem(Item arg)
        {
            return $"Successfully updated item #{arg.Id}";
        }

        private string FormatFailures(Exception arg)
        {
            return arg.Message;
        }

        private IResult<Exception, Item> EditItem(IOption<IResult<IEnumerable<string>, Item>> arg)
        {
            return arg
                .Match(
                    () => Result.Failure<Exception, Item>(new Exception("Couldn't find the requested record")), 
                    result => result
                                .BiMap(ConvertToException, FunctionHelpers.Identity)
                                .AndThen(_repo.Edit)
                                .BiMap(FormatDatabaseError, FunctionHelpers.Identity)
                    );
        }

        private Exception FormatDatabaseError(Exception arg)
        {
            return new Exception("Failed to update item due to database error" + Environment.NewLine + arg.Message);
        }

        private IEnumerable<string> AddNewMessage(string message, IEnumerable<string> errors)
        {
            var e = new List<string> {message};
            e.AddRange(errors);
            return e;
        }

        private Exception ConvertToException(IEnumerable<string> arg)
        {
            return new Exception(String.Join(Environment.NewLine, arg));
        }

        private IOption<IResult<IEnumerable<string>, Item>> AskForNewItem(IOption<Item> optItem)
        {
            return optItem.Map(CreateNewItem);
        }

        private IResult<IEnumerable<string>, Item> CreateNewItem(Item item)
        {
            Console.WriteLine($"Current title: {item.Title}");
            Console.Write("New Title: ");
            var title = Console.ReadLine();

            Console.WriteLine($"Current Description: {item.Description}");
            Console.WriteLine(Environment.NewLine + "New Description: ");
            var description = Console.ReadLine();

            Func<Item, Item> updateId = x =>
            {
                x.Id = item.Id;
                return x;
            };
            Func<IEnumerable<string>, IEnumerable<string>> addFailedMessage =
                errors => AddNewMessage("Failed to update item due to:", errors);

            return Validators.CreateItem(title, description).BiMap(addFailedMessage, updateId);
        }
    }
}