using System;
using Microsoft.AspNetCore.Mvc;

namespace NetApiStarterLibrary.Models
{
	public class ApiResponse
	{
        public bool Success { get; set; }

        public object Data { get; set; }

        public string? Message { get; set; }

        public List<ApiErrorField> Errors { get; set; }
        
    }

    public class ApiErrorField
    {
        public string? Field { get; set; }

        public string? Message { get; set; }
    }

    public class ApiError : ApiResponse
    {

        public ApiError(string OutputMessage)
        {
            Success = false;
            Message = OutputMessage;
        }

        public ApiError(string OutputMessage, List<ApiErrorField> OutputErrors)
        {
            Success = false;
            Message = OutputMessage;
            Errors = OutputErrors;
        }

        //public ApiModelValidationError(ModelState modelState)
        //{
        //    Success = false;
        //    Message = OutputMessage;
        //    Errors = OutputErrors;
        //}


    }

    public class ApiSuccess : ApiResponse
    {

        public ApiSuccess()
        {
            Success = true;
        }

        public ApiSuccess(object OutputData, string OutputMessage)
        {
            Success = true;
            Data = OutputData;
            Message = OutputMessage;
        }

        public ApiSuccess(object OutputData)
        {
            Success = true;
            Data = OutputData;
        }
    }

    public static class ApiMessageHandler
    {
        public static BadRequestObjectResult CustomModelstateErrorResponse(ActionContext actionCtx)
        {
            var errors = actionCtx.ModelState
             .Where(modelError => modelError.Value.Errors.Count > 0)
             .Select(modelError => new ApiErrorField
             {
                 Field = modelError.Key,
                 Message = modelError.Value.Errors.FirstOrDefault().ErrorMessage
             }).ToList();

            return new BadRequestObjectResult(new ApiError("Invalid Input",errors));
        }
    }
}

