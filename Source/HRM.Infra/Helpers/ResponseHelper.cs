using HRM.Infra.Enums;
using Microsoft.Win32;
using HRM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Infra.Helpers
{
    public static class ResponseHelper
    {
        public static Response GetResponse(Responses desiredResponse, object data = null)
        {
            switch (desiredResponse)
            {
                case Responses.SignInSuccess:
                    return new Response() { Error = false, Message = Resources.SignInSuccess, Data = data };
                case Responses.SignInFailure:
                    return new Response() { Error = true, Message = Resources.SignInFailure, Data = data };
                case Responses.SignUpSuccess:
                    return new Response() { Error = false, Message = Resources.SignUpSuccess, Data = data };
                case Responses.SignUpFailure:
                    return new Response() { Error = true, Message = Resources.SignUpFailure, Data = data };
                case Responses.UserNotFound:
                    return new Response() { Error = true, Message = Resources.UserNotFound, Data = data };
                case Responses.ActionSuccess:
                    return new Response() { Error = false, Message = Resources.ActionSuccess, Data = data };
                case Responses.InternalServerError:
                default:
                    return new Response() { Error = true, Message = Resources.InternalServerError, Data = data };
            }
        }
    }
}
