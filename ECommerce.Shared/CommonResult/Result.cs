using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.CommonResult
{
    public class Result
    {
        protected readonly List<Error> _errors = [];    
        public bool IsSuccess => _errors.Count()==0;
        public bool IsFailure => !IsSuccess;
        public IReadOnlyList<Error> Errors => _errors;
        protected Result() { }
        protected Result(Error error)
        {
            _errors.Add(error);
        }
        protected Result(List<Error> errors)
        {
            _errors.AddRange(errors);
        }
        public static Result Ok()
        {
            return new Result();
        }
        public static Result Fail(Error error)
        {
            return new Result(error);
        }
        public static Result Fail(List<Error> errors)
        {
            return new Result(errors);
        }

    }
    public class Result<T> : Result
    {
        private readonly T _value;
        public T Value=> IsSuccess ? _value : throw new InvalidOperationException("Cannot access the value of a failed result.");
        private Result(T value)
        {
            _value = value;
        }
        private Result(Error error) : base (error)
        {
            _value = default!;
        }
        private Result(List<Error> errors) : base (errors)
        {
            _value = default!;
        }
        public static Result<T> Ok(T value)
        {
            return new Result<T>(value);
        }
        public static new Result<T> Fail(Error error)
        {
            return new Result<T>(error);
        }
        public static new Result<T> Fail(List<Error> errors)
        {
            return new Result<T>(errors);
        }
    }
}
