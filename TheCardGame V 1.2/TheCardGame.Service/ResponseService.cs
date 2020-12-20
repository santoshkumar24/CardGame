using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using TheCardGame.Models;
using TheCardGame.ServiceInterface;

namespace TheCardGame.Service
{
    public class ResponseService:IResponseService
    {

        public ResponseService()
        {
        }



        public ResultModel<T> SetResponse<T>(T data, bool isError, string message) where T : class
        {
            ResultModel<T> resultModel = new ResultModel<T>();
            resultModel.Data = data;
            resultModel.IsError = isError;
            resultModel.Message = message;
            return resultModel;
        }
    }
}
