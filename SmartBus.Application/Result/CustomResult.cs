using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.Result
{
    public class CustomResult<T>
    {
        public bool IsSuccess { get; private set; }
        public bool IsFailed => !IsSuccess;
        private T? _value;
        public CustomError? Error { get; set; }
        public T? Value 
        { 
            get => IsSuccess ? _value : default;  
            private init => _value =  value ; 
        }
        private CustomResult(T value)
        {
            IsSuccess = true;
            Value = value;
        }
        private CustomResult(CustomError error)
        {
            IsSuccess = false;
            Error = error;
        }
        public static CustomResult<T> Success(T value) => new CustomResult<T>(value);
        public static CustomResult<T> Failure(CustomError error) => new CustomResult<T>(error);
    }
    public class CustomResult
    {
        public bool IsSuccess { get; private set; }
        public bool IsFailed => !IsSuccess;
        
        public CustomError? Error { get; set; }
        
        private CustomResult()
        {
            IsSuccess = true;
            
        }
        private CustomResult(CustomError error)
        {
            IsSuccess = false;
            Error = error;
        }
        public static CustomResult Success() => new CustomResult();
        public static CustomResult Failure(CustomError error) => new CustomResult(error);
    }
}
