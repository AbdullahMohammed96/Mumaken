using Microsoft.AspNetCore.Mvc;
using Mumaken.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.Common.Helpers.ResponseHelper
{
    public static class ResponseHelper
    {
        public static IActionResult CreateResponse<T>(this BaseResponseDto<T> response, int responseCode, string errorMessage = null) where T : class
        {
            if (!response.IsSuccess && errorMessage != null)
            {
                response.Message = errorMessage;
            }

            return response.IsSuccess
                ? new ObjectResult(response) { StatusCode = responseCode }
                : new ObjectResult(response) { StatusCode = responseCode };
        }

        public static BaseResponseDto<T> Success<T>(T data, string message = "", string key = "success", int responseCode = 200) where T : class
        {
            return new BaseResponseDto<T>(true, data, message, key, responseCode, null);
        }
        public static BaseResponseDto<T> Failure<T>(string errorMessage, int responseCode = 400, string key = "failed", List<ValidationError>? validationErrors = null) where T : class
        {
            return new BaseResponseDto<T>(false, default(T), errorMessage, key, responseCode, validationErrors);
        }
    }
}
