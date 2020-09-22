using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Infra.Enums
{
    public enum Responses
    {
        InternalServerError,
        SignInSuccess,
        SignInFailure,
        SignUpSuccess,
        SignUpFailure,
        UserNotFound,
        RequiredFields,
        ActionSuccess,
    }
}
