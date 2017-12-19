using System;
using System.Collections.Generic;
using CRUDy.DataAccess;
using CRUDy.Domain;
using Optionally;

namespace CRUDy.Features.Add
{
    public class Screen : Feature
    {
        private readonly IItemRepository _repo;

        public Screen(IItemRepository repo)
        {
            _repo = repo;
        }

        public string Name => "Add an Item";
        public Action Workflow => Display;

        public void Display()
        {
            Console.WriteLine("What's the title?");
            var title = Console.ReadLine();
            Console.WriteLine("What's the description?");
            var description = Console.ReadLine();

            Validators.CreateItem(title, description)
                .BiMap(FormatErrors, FunctionHelpers.Identity)
                .AndThen(_repo.Add)
                .Match(ex => ex.Message, item => $"Successfully added item #{item.Id}")
                .Apply(Console.WriteLine);
        }

        private Exception FormatErrors(IEnumerable<string> errors)
        {
            var e = new List<string>() {"Failed to create item."};
            e.AddRange(errors);
            return new Exception(String.Join(Environment.NewLine, e));
        }
    }
}
