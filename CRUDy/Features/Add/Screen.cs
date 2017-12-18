using System;
using System.Collections.Generic;
using CRUDy.DataAccess;
using CRUDy.Domain;
using Optionally;

namespace CRUDy.Add
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
            return new Feature("Add an item", Display);
        }

        public void Display()
        {
            Console.WriteLine("What's the title?");
            var title = Console.ReadLine();
            Console.WriteLine("What's the description?");
            var description = Console.ReadLine();

            CreateItem(title, description)
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

        public IResult<IEnumerable<string>, Item> CreateItem(string title, string description)
        {
            return Result.Apply(Item.Create, ValidateTitle(title), ValidateDescription(description));
        }

        public IResult<string, string> ValidateTitle(string s)
        {
            if (s == null) return Result.Failure<string, string>("Title cannot be null");
            if (String.IsNullOrWhiteSpace(s)) return Result.Failure<string, string>("Title must be specified");
            if (s.Trim().Length > 75)
                return Result.Failure<string, string>("Title cannot be longer than 75 charactes.");
            return Result.Success<string, string>(s.Trim());
        }

        public IResult<string, string> ValidateDescription(string s)
        {
            if (s == null) return Result.Failure<string, string>("Description cannot be null");
            if (String.IsNullOrWhiteSpace(s)) return Result.Failure<string, string>("Description must be specified");
            if (s.Trim().Length > 500)
                return Result.Failure<string, string>("Description cannot be longer than 500 charactes.");
            return Result.Success<string, string>(s.Trim());
        }
    }
}
