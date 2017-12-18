using System;
using System.Collections.Generic;
using System.Linq;
using CRUDy.DataAccess;
using Optionally;

namespace CRUDy.Features
{
    internal class FeatureMap
    {
        private readonly Dictionary<int, Feature> _features;

        public FeatureMap()
        {
            var repo = new ItemRepository();
            _features = new Dictionary<int, Feature>
            {
                {1, new Add.Screen(repo).Create() },
                {2, new Delete.Screen(repo).Create() },
                {3, new Exit.Screen().Create() },
            };
        }

        public IEnumerable<string> GetNames()
        {
           return  _features.Keys
                            .OrderBy(FunctionHelpers.Identity)
                            .Select(x => _features[x].Name);
        }

        public IResult<string, Action> GetAction(int choice)
        {
            return _features.ContainsKey(choice)
                ? Result.Success<string, Action>(_features[choice].Workflow)
                : Result.Failure<string, Action>("Couldn't find the workflow for the requested feature.");
        }

        public bool DoesFeatureExist(int feature)
        {
            return _features.ContainsKey(feature);
        }
    }
}
