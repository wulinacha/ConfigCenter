using ConfigCenter.Dto;

namespace ConfigCenter.Admin
{
    public class ResponseResult
    {
        public bool Success { get; set; }
        public ResultEnum.ErrorType ErrorType { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public ResponseResult(ResultEnum.IsSuccess success, string message,
            object data=null,
            ResultEnum.ErrorType errorType = ResultEnum.ErrorType.无异常)
        {
            Success = ((int)success)==0?false:true;
            Message = message;
            Data = data;
            ErrorType = errorType;
        }
        public ResponseResult(bool success, string message,
        object data = null,
        ResultEnum.ErrorType errorType = ResultEnum.ErrorType.无异常)
        {
            Success = success;
            Message = message;
            Data = data;
            ErrorType = errorType;
        }
    }
}