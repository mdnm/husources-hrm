using HRM.Infra.Helpers;
using HRM.Infra.Enums;
using HRM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using HRM.Model.Entities.Interfaces;

namespace HRM.Service
{
    public class LoginService<T> where T : IUser
    {
        public BaseService<T> mBaseService;

        public LoginService()
        {
            mBaseService = new BaseService<T>();
        }

        public Response Register(T user)
        {
            var response = Responses.SignUpSuccess;
            object data = null;

            try
            {
                using (var sha256Hash = SHA256.Create())
                {
                    var getResponse = mBaseService.Get();
                    if (!getResponse.Error && getResponse.Data != null)
                    {
                        var databaseUser = ((IEnumerable<IUser>)getResponse.Data).Where(x => x.Username == user.Username).FirstOrDefault();
                        if (databaseUser != null)
                        {
                            response = Responses.SignUpFailure;
                            return ResponseHelper.GetResponse(response);
                        }
                    }
                    else
                    {
                        return getResponse;
                    }

                    var hash = GetHash(sha256Hash, user.PasswordHash);
                    user.PasswordHash = hash;
                    var postResponse = mBaseService.Post(user);
                    if (!postResponse.Error && postResponse.Data != null)
                    {
                        data = postResponse.Data;
                    }
                    else
                    {
                        return postResponse;
                    }
                }
            }
            catch
            {
                response = Responses.InternalServerError;
            }

            return ResponseHelper.GetResponse(response, data);
        }

        public Response Login(T user)
        {
            var response = Responses.SignInSuccess;
            object data = null;

            try
            {
                using (var sha256Hash = SHA256.Create())
                {
                    var getResponse = mBaseService.Get();
                    if (getResponse.Error || getResponse.Data == null)
                    {
                        return getResponse;
                    }

                    var databaseUser = ((IEnumerable<IUser>)getResponse.Data).Where(x => x.Username == user.Username).FirstOrDefault();
                    if (databaseUser == null)
                    {
                        response = Responses.UserNotFound;
                        return ResponseHelper.GetResponse(response);
                    }

                    var passwordsMatch = VerifyHash(sha256Hash, user.PasswordHash, databaseUser.PasswordHash);
                    if (!passwordsMatch)
                    {
                        response = Responses.SignInFailure;
                    }

                    data = databaseUser.Id;
                }
            }
            catch
            {
                response = Responses.InternalServerError;
            }

            return ResponseHelper.GetResponse(response, data);
        }

        private static bool VerifyHash(HashAlgorithm hashAlgorithm, string input, string hash)
        {
            var hashOfInput = GetHash(hashAlgorithm, input);

            var comparer = StringComparer.OrdinalIgnoreCase;
            return comparer.Compare(hashOfInput, hash) == 0;
        }

        private static string GetHash(HashAlgorithm hashAlgorithm, string input)
        {
            var data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            var stringBuilder = new StringBuilder();

            for (var i = 0; i < data.Length; i++)
            {
                stringBuilder.Append(data[i].ToString("x2"));
            }

            return stringBuilder.ToString();
        }
    }
}
