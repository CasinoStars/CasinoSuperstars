﻿using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;
using Dapper;

namespace Superstars.DAL
{
    public class UserGateway
    {
        readonly string _connectionString;

        public UserGateway(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<UserData> FindById(int userId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                return await con.QueryFirstOrDefaultAsync<UserData>(
                    "select u.UserId, u.Email, u.UserName, u.UserPassword from vUser u where u.UserId = @UserId",
                    new { UserId = userId });
            }
        }

        public async Task<UserData> FindByName(string pseudo)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                return await con.QueryFirstOrDefaultAsync<UserData>(
                    "select u.UserId, u.Email, u.UserName, u.UserPassword from vUser u where u.UserName = @UserName",
                    new { UserName = pseudo });
            }
        }

        public async Task<UserData> FindByEmail(string email)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                return await con.QueryFirstOrDefaultAsync<UserData>(
                    "select u.UserId, u.Email, u.UserName, u.UserPassword from vUser u where u.Email = @Email",
                    new { Email = email });
            }
        }

        public async Task<Result<int>> CreateUser(string pseudo, byte[] password)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                var p = new DynamicParameters();
                p.Add("@UserName", pseudo);
                p.Add("@UserPassword", password);
                p.Add("@UserId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
                await con.ExecuteAsync("sp.sUserCreate", p, commandType: CommandType.StoredProcedure);

                int status = p.Get<int>("@Status");
                if (status == 1) return Result.Failure<int>(Status.BadRequest, "An account with this email or this pseudo already exists.");

                Debug.Assert(status == 0);
                return Result.Success(p.Get<int>("@UserId"));
            }
        }

        public async Task Delete(int userId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.ExecuteAsync("sp.sUserDelete", new { UserId = userId }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task UpdateEmail(int userId, string email)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.ExecuteAsync(
                    "sp.sUserUpdate",
                    new { UserId = userId, Email = email },
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task UpdateName(int userId, string pseudo)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.ExecuteAsync(
                    "sp.sUserUpdate",
                    new { UserId = userId, UserName = pseudo },
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task UpdatePassword(int userId, byte[] password)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.ExecuteAsync(
                    "sp.sUserUpdate",
                    new { UserId = userId, UserPassword = password },
                    commandType: CommandType.StoredProcedure);
            }
        }

        //public async Task<Result<int>> IdentityVerify(string pseudo, byte[] password)
        //{
        //    using (SqlConnection con = new SqlConnection(_connectionString))
        //    {
        //        var p = new DynamicParameters();
        //        p.Add("@UserName", pseudo);
        //        p.Add("@UserPassword", password);
        //        p.Add("@UserId", dbType: DbType.Int32, direction: ParameterDirection.Output);
        //        p.Add("@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

        //        await con.ExecuteAsync("sp.sUserVerify", p, commandType: CommandType.StoredProcedure);

        //        int status = p.Get<int>("@Status");

        //        if (status == 1) return Result.Failure<int>(Status.BadRequest, "Wrong Username or Password");

        //        Debug.Assert(status == 0);
        //        return Result.Success(p.Get<int>("@UserId"));
        //    }
        //}
    }
}