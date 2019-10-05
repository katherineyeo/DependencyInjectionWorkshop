﻿using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace DependencyInjectionWorkshop.Models
{
    public interface IProfileDao
    {
        string GetPassword(string accountId);
    }
    
    public class ProfileDao : IProfileDao
    {
        public string GetPassword(string accountId)
        {
            string passwordFromDb;
            using (var connection = new SqlConnection("my connection string"))
            {
                var password1 = connection.Query<string>("spGetUserPassword", new { Id = accountId },
                    commandType: CommandType.StoredProcedure).SingleOrDefault();

                passwordFromDb = password1;
            }

            return passwordFromDb;
        }
    }
}