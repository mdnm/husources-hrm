using HRM.Infra.Enums;
using HRM.Infra.Helpers;
using HRM.Model;
using HRM.Model.Entities.Interfaces;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Service
{
    public class BaseService<T> where T : IEntity
    {
        protected BaseRepository<T> Repository { get; set; }

        public BaseService()
        {
            Repository = new BaseRepository<T>();
        }

        public Response Get(Guid id)
        {
            var response = Responses.ActionSuccess;
            object data = null; 

            try
            {
                data = Repository.Get(id);
            }
            catch 
            {
                response = Responses.InternalServerError;
            }

            return ResponseHelper.GetResponse(response, data);
        }

        public Response Get()
        {
            var response = Responses.ActionSuccess;
            object data = null;

            try
            {
                data = Repository.Get();
            }
            catch
            {
                response = Responses.InternalServerError;
            }

            return ResponseHelper.GetResponse(response, data);
        }

        public Response Post(T entity)
        {
            var response = Responses.ActionSuccess;
            object data = null;

            try
            {
                data = Repository.Post(entity);
            }
            catch
            {
                response = Responses.InternalServerError;
            }

            return ResponseHelper.GetResponse(response, data);
        }

        public Response Put(T entity)
        {
            var response = Responses.ActionSuccess;
            object data = null;

            try
            {
                data = Repository.Put(entity);
            }
            catch
            {
                response = Responses.InternalServerError;
            }

            return ResponseHelper.GetResponse(response, data);
        }

        public Response Delete(T entity)
        {
            var response = Responses.ActionSuccess;
            object data = null;

            try
            {
                data = Repository.Delete(entity);
            }
            catch
            {
                response = Responses.InternalServerError;
            }

            return ResponseHelper.GetResponse(response, data);
        }
    }
}
