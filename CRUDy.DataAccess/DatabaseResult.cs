using System;
using Optionally;

namespace CRUDy.DataAccess
{
    public static class DatabaseResult
    {
        public static IDatabaseResult<T> Success<T>(T value)
        {
            return new DatabaseResult<T>(Result.Success<Exception, T>(value));
        }

        public static IDatabaseResult<T> Failure<T>(Exception ex)
        {
            return new DatabaseResult<T>(Result.Failure<Exception, T>(ex));
        }
    }

    public interface IDatabaseResult<T>
    {
        IDatabaseResult<U> Map<U>(Func<T, U> mapper);
        IDatabaseResult<U> AndThen<U>(Func<T, IResult<Exception, U>> binder);
        IDatabaseResult<T> Do(Action<Exception> onFailure, Action<T> onSuccess);
        T1 Match<T1>(Func<Exception, T1> onFailure, Func<T, T1> onSuccess);
    }
    internal class DatabaseResult<T> : IDatabaseResult<T>
    {
        private readonly IResult<Exception, T> _result;

        public DatabaseResult(IResult<Exception, T> result)
        {
            _result = result;
        }

        public IDatabaseResult<U> Map<U>(Func<T, U> mapper)
        {
            return new DatabaseResult<U>(_result.Map(mapper));
        }

        public IDatabaseResult<U> AndThen<U>(Func<T, IResult<Exception, U>> binder)
        {
            return new DatabaseResult<U>(_result.AndThen(binder));
        }

        public IDatabaseResult<T> Do(Action<Exception> onFailure, Action<T> onSuccess)
        {
            _result.Do(onFailure, onSuccess);
            return this;
        }

        public T1 Match<T1>(Func<Exception, T1> onFailure, Func<T, T1> onSuccess)
        {
            return _result.Match(onFailure, onSuccess);
        }
    }
}
