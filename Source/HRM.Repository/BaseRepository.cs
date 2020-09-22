using Npgsql;
using Dapper;
using System;
using System.Linq;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using HRM.Model.Entities.Interfaces;

namespace Repository
{
    public class BaseRepository<T> where T : IEntity
    {
        protected NpgsqlConnection mConnection;

        public BaseRepository()
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        public T Get(Guid id)
        {
            using (mConnection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQLConnectionString"].ConnectionString))
            {
                var tableName = ((TableAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(TableAttribute))).Name;
                var command = $"SELECT * FROM public.{tableName} WHERE id = @id";
                return mConnection.Query<T>(command, new { id }).FirstOrDefault();
            }
        }

        public IEnumerable<T> Get()
        {
            using (mConnection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQLConnectionString"].ConnectionString))
            {
                var tableName = ((TableAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(TableAttribute))).Name;
                var command = $"SELECT * FROM public.{tableName}";
                return mConnection.Query<T>(command);
            }
        }

        public Guid Post(T entity)
        {
            using (mConnection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQLConnectionString"].ConnectionString))
            {
                var tableName = ((TableAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(TableAttribute))).Name;
                var parameters = GetInsertParameters();
                var command = $"INSERT INTO public.{tableName} {parameters} RETURNING id";
                return mConnection.Query<Guid>(command, entity).FirstOrDefault();
            }
        }

        public Guid Put(T entity)
        {
            using (mConnection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQLConnectionString"].ConnectionString))
            {
                var tableName = ((TableAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(TableAttribute))).Name;
                var parameters = GetUpdateParameters();
                var command = $"UPDATE public.{tableName} SET {parameters} WHERE id = @Id RETURNING id";
                return mConnection.Query<Guid>(command, entity).FirstOrDefault();
            }
        }

        public Guid Delete(T entity)
        {
            using (mConnection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQLConnectionString"].ConnectionString))
            {
                var tableName = ((TableAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(TableAttribute))).Name;
                var command = $"DELETE FROM public.{tableName} WHERE id = @Id RETURNING id";
                return mConnection.Query<Guid>(command, entity).FirstOrDefault();
            }
        }

        private string GetInsertParameters()
        {
            var columns = new List<string>();
            var parameters = new List<string>();
            var type = typeof(T);
            var propertyInfo = type.GetProperties();
            for (var i = 0; i < propertyInfo.Length; i++)
            {
                var propertyName = propertyInfo[i].Name;
                if (propertyName == "Id")
                {
                    continue;
                }

                var memberInfo = type.GetMember(propertyName);
                var column = (ColumnAttribute)Attribute.GetCustomAttribute(memberInfo[0], typeof(ColumnAttribute));
                columns.Add(column.Name);
                parameters.Add($"@{propertyName}");
            }
            return $"({string.Join(",", columns)}) VALUES ({string.Join(",", parameters)})";
        }

        private string GetUpdateParameters()
        {
            var columns = new List<string>();
            var type = typeof(T);
            var propertyInfo = type.GetProperties();
            for (var i = 0; i < propertyInfo.Length; i++)
            {
                var propertyName = propertyInfo[i].Name;
                if (propertyName == "Id")
                {
                    continue;
                }

                var memberInfo = type.GetMember(propertyName);
                var column = (ColumnAttribute)Attribute.GetCustomAttribute(memberInfo[0], typeof(ColumnAttribute));
                columns.Add($"{column.Name} = @{propertyName}");
            }
            return string.Join(",", columns);
        }
    }
}
