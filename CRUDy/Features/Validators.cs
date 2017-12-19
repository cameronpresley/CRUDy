using System;
using System.Collections.Generic;
using CRUDy.Domain;
using Optionally;

namespace CRUDy.Features
{
    public static class Validators
    {
        public static IResult<IEnumerable<string>, Item> CreateItem(string title, string description)
        {
            return Result.Apply(Item.Create, ValidateTitle(title), ValidateDescription(description));
        }

        public static IResult<string, string> ValidateTitle(string s)
        {
            if (s == null) return Result.Failure<string, string>("Title cannot be null");
            if (String.IsNullOrWhiteSpace(s)) return Result.Failure<string, string>("Title must be specified");
            if (s.Trim().Length > 75)
                return Result.Failure<string, string>("Title cannot be longer than 75 charactes.");
            return Result.Success<string, string>(s.Trim());
        }

        public static IResult<string, string> ValidateDescription(string s)
        {
            if (s == null) return Result.Failure<string, string>("Description cannot be null");
            if (String.IsNullOrWhiteSpace(s)) return Result.Failure<string, string>("Description must be specified");
            if (s.Trim().Length > 500)
                return Result.Failure<string, string>("Description cannot be longer than 500 charactes.");
            return Result.Success<string, string>(s.Trim());
        }
    }
}
