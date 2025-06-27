using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.DTO
{
    public sealed class BaseResponseDto<T> where T :class
    {
        public bool IsSuccess { get; set; }
        public T Data { get; set; }
        public string Message { get; set; } = "";
        public string key { get; set; } = "";
        public int ResponseCode { get; set; }
        public List<ValidationError>? ValidationErrors { get; set; }
        public BaseResponseDto()
        {
            
        }
        public BaseResponseDto(bool isSuccess, T data, string message, string key, int responseCode, List<ValidationError>? validationErrors)
        {
            IsSuccess = isSuccess;
            Data = data;
            Message = message;
            this.key = key;
            ValidationErrors = validationErrors;
            ResponseCode = responseCode;
        }

    }

    public class ValidationError
    {
        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }
    }
}
