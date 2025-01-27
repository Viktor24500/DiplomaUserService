using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using SystemUserService.Common.Results;
using SystemUserService.DataAccess.DTO.Users;
using SystemUserService.DataAccess.Repositories.Intefaces;
using SystemUserService.Common.Enums;
using SystemUserService.DataAccess.DTO.Login;

namespace SystemUserService.DataAccess.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly string? _connectionString;
        public UsersRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString(Constants.MainConnectionString);
        }
        public async Task<Result<int>> CreateUser(string username, string userPassword, string email, string firstName, string lastName,
            string? fatherName, DateTime dateRegistered, DateTime? lastLogin, string? lastToken, DateTime? tokenExpiration,
            bool isActive)
        {
            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                Result<int> result = new Result<int>();
                connection.Open();
                SqlCommand command = new SqlCommand("insertUser", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@userPassword", userPassword);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@firstName", firstName);
                command.Parameters.AddWithValue("@lastName", lastName);
                if (fatherName != null)
                {
                    command.Parameters.AddWithValue("@fatherName", fatherName);
                }
                else
                {
                    command.Parameters.AddWithValue("@fatherName", DBNull.Value);
                }
                command.Parameters.AddWithValue("@dateRegistered", dateRegistered);
                if (lastLogin != null)
                {
                    command.Parameters.AddWithValue("@lastLogin", lastLogin);
                }
                else
                {
                    command.Parameters.AddWithValue("@lastLogin", DBNull.Value);
                }
                if (tokenExpiration != null)
                {
                    command.Parameters.AddWithValue("@tokenExpiration", lastLogin);
                }
                else
                {
                    command.Parameters.AddWithValue("@tokenExpiration", DBNull.Value);
                }
                if (lastToken != null)
                {
                    command.Parameters.AddWithValue("@lastToken", lastToken);
                }
                else
                {
                    command.Parameters.AddWithValue("@lastToken", DBNull.Value);
                }
                command.Parameters.AddWithValue("@isActive", isActive);
                await using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Data = reader.GetInt32(reader.GetOrdinal("userId"));
                    }
                    if (result.Data == null)
                    {
                        result.ErrorCode = (int)ErrorCodes.NotFound;
                        return result;
                    }
                    return result;
                }
            }
        }

        public async Task<Result<List<UserDTO>>> GetAllUsers()
        {
            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("getAllUsers", connection);
                command.CommandType = CommandType.StoredProcedure;
                await using (SqlDataReader reader = command.ExecuteReader())
                {
                    Result<List<UserDTO>> result = new Result<List<UserDTO>>();
                    result.Data = new List<UserDTO>();
                    while (reader.Read())
                    {
                        string? fatherName;
                        if (reader.IsDBNull(reader.GetOrdinal("fatherName")))
                        {
                            fatherName = null;
                        }
                        else
                        {
                            fatherName = reader.GetString(reader.GetOrdinal("fatherName"));
                        }
                        DateTime? lastLogin;
                        if (reader.IsDBNull(reader.GetOrdinal("lastLogin")))
                        {
                            lastLogin = null;
                        }
                        else
                        {
                            lastLogin = reader.GetDateTime(reader.GetOrdinal("lastLogin"));
                        }
                        DateTime? tokenExpiration;
                        if (reader.IsDBNull(reader.GetOrdinal("tokenExpiration")))
                        {
                            tokenExpiration = null;
                        }
                        else
                        {
                            tokenExpiration = reader.GetDateTime(reader.GetOrdinal("tokenExpiration"));
                        }
                        string? lastToken;
                        if (reader.IsDBNull(reader.GetOrdinal("lastToken")))
                        {
                            lastToken = null;
                        }
                        else
                        {
                            lastToken = reader.GetString(reader.GetOrdinal("lastToken"));
                        }
                        UserDTO user = new UserDTO(
                            reader.GetInt32(reader.GetOrdinal("userId")),
                            reader.GetString(reader.GetOrdinal("username")),
                            reader.GetString(reader.GetOrdinal("userPassword")),
                            reader.GetString(reader.GetOrdinal("email")),
                            reader.GetString(reader.GetOrdinal("firstName")),
                            reader.GetString(reader.GetOrdinal("lastName")),
                            fatherName,
                            reader.GetDateTime(reader.GetOrdinal("dateRegistered")),
                            lastLogin,
                            lastToken,
                            tokenExpiration,
                            reader.GetBoolean(reader.GetOrdinal("isActive"))
                            );
                        result.Data.Add(user);
                    }
                    return result;
                }
            }
        }

        public async Task<Result<UserDTO>> GetUser(int id)
        {
            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("getUserById", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id", id);
                await using (SqlDataReader reader = command.ExecuteReader())
                {
                    Result<UserDTO> result = new Result<UserDTO>();
                    while (reader.Read())
                    {
                        string? fatherName;
                        if (reader.IsDBNull(reader.GetOrdinal("fatherName")))
                        {
                            fatherName = null;
                        }
                        else
                        {
                            fatherName = reader.GetString(reader.GetOrdinal("fatherName"));
                        }
                        DateTime? lastLogin;
                        if (reader.IsDBNull(reader.GetOrdinal("lastLogin")))
                        {
                            lastLogin = null;
                        }
                        else
                        {
                            lastLogin = reader.GetDateTime(reader.GetOrdinal("lastLogin"));
                        }
                        DateTime? tokenExpiration;
                        if (reader.IsDBNull(reader.GetOrdinal("tokenExpiration")))
                        {
                            tokenExpiration = null;
                        }
                        else
                        {
                            tokenExpiration = reader.GetDateTime(reader.GetOrdinal("tokenExpiration"));
                        }
                        string? lastToken;
                        if (reader.IsDBNull(reader.GetOrdinal("lastToken")))
                        {
                            lastToken = null;
                        }
                        else
                        {
                            lastToken = reader.GetString(reader.GetOrdinal("lastToken"));
                        }
                        result.Data = new UserDTO(
                            reader.GetInt32(reader.GetOrdinal("userId")),
                            reader.GetString(reader.GetOrdinal("username")),
                            reader.GetString(reader.GetOrdinal("userPassword")),
                            reader.GetString(reader.GetOrdinal("email")),
                            reader.GetString(reader.GetOrdinal("firstName")),
                            reader.GetString(reader.GetOrdinal("lastName")),
                            fatherName,
                            reader.GetDateTime(reader.GetOrdinal("dateRegistered")),
                            lastLogin,
                            lastToken,
                            tokenExpiration,
                            reader.GetBoolean(reader.GetOrdinal("isActive"))
                            );
                    }
                    if (result.Data == null)
                    {
                        result.ErrorCode = (int)ErrorCodes.NotFound;
                        return result;
                    }
                    return result;
                }
            }
        }

        public async Task<Result<UserDTO>> GetUserByEmail(string email)
        {
            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("getUserByEmail", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@email", email);
                await using (SqlDataReader reader = command.ExecuteReader())
                {
                    Result<UserDTO> result = new Result<UserDTO>();
                    while (reader.Read())
                    {
                        string? fatherName;
                        if (reader.IsDBNull(reader.GetOrdinal("fatherName")))
                        {
                            fatherName = null;
                        }
                        else
                        {
                            fatherName = reader.GetString(reader.GetOrdinal("fatherName"));
                        }
                        DateTime? lastLogin;
                        if (reader.IsDBNull(reader.GetOrdinal("lastLogin")))
                        {
                            lastLogin = null;
                        }
                        else
                        {
                            lastLogin = reader.GetDateTime(reader.GetOrdinal("lastLogin"));
                        }
                        DateTime? tokenExpiration;
                        if (reader.IsDBNull(reader.GetOrdinal("tokenExpiration")))
                        {
                            tokenExpiration = null;
                        }
                        else
                        {
                            tokenExpiration = reader.GetDateTime(reader.GetOrdinal("tokenExpiration"));
                        }
                        string? lastToken;
                        if (reader.IsDBNull(reader.GetOrdinal("lastToken")))
                        {
                            lastToken = null;
                        }
                        else
                        {
                            lastToken = reader.GetString(reader.GetOrdinal("lastToken"));
                        }
                        result.Data = new UserDTO(
                            reader.GetInt32(reader.GetOrdinal("userId")),
                            reader.GetString(reader.GetOrdinal("username")),
                            reader.GetString(reader.GetOrdinal("userPassword")),
                            reader.GetString(reader.GetOrdinal("email")),
                            reader.GetString(reader.GetOrdinal("firstName")),
                            reader.GetString(reader.GetOrdinal("lastName")),
                            fatherName,
                            reader.GetDateTime(reader.GetOrdinal("dateRegistered")),
                            lastLogin,
                            lastToken,
                            tokenExpiration,
                            reader.GetBoolean(reader.GetOrdinal("isActive"))
                            );
                    }
                    if (result.Data == null)
                    {
                        result.ErrorCode = (int)ErrorCodes.NotFound;
                        return result;
                    }
                    return result;
                }
            }
        }

        public async Task<Result<List<UserDTO>>> GetUserByActiveStatus(bool isActive)
        {
            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("getUserByIsActive", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@isActive", isActive);
                await using (SqlDataReader reader = command.ExecuteReader())
                {
                    Result<List<UserDTO>> result = new Result<List<UserDTO>>();
                    result.Data = new List<UserDTO>();
                    while (reader.Read())
                    {
                        string? fatherName;
                        if (reader.IsDBNull(reader.GetOrdinal("fatherName")))
                        {
                            fatherName = null;
                        }
                        else
                        {
                            fatherName = reader.GetString(reader.GetOrdinal("fatherName"));
                        }
                        DateTime? lastLogin;
                        if (reader.IsDBNull(reader.GetOrdinal("lastLogin")))
                        {
                            lastLogin = null;
                        }
                        else
                        {
                            lastLogin = reader.GetDateTime(reader.GetOrdinal("lastLogin"));
                        }
                        DateTime? tokenExpiration;
                        if (reader.IsDBNull(reader.GetOrdinal("tokenExpiration")))
                        {
                            tokenExpiration = null;
                        }
                        else
                        {
                            tokenExpiration = reader.GetDateTime(reader.GetOrdinal("tokenExpiration"));
                        }
                        string? lastToken;
                        if (reader.IsDBNull(reader.GetOrdinal("lastToken")))
                        {
                            lastToken = null;
                        }
                        else
                        {
                            lastToken = reader.GetString(reader.GetOrdinal("lastToken"));
                        }
                        UserDTO user = new UserDTO(
                            reader.GetInt32(reader.GetOrdinal("userId")),
                            reader.GetString(reader.GetOrdinal("username")),
                            reader.GetString(reader.GetOrdinal("userPassword")),
                            reader.GetString(reader.GetOrdinal("email")),
                            reader.GetString(reader.GetOrdinal("firstName")),
                            reader.GetString(reader.GetOrdinal("lastName")),
                            fatherName,
                            reader.GetDateTime(reader.GetOrdinal("dateRegistered")),
                            lastLogin,
                            lastToken,
                            tokenExpiration,
                            reader.GetBoolean(reader.GetOrdinal("isActive"))
                            );
                        result.Data.Add(user);
                    }
                    return result;
                }
            }
        }

        public async Task<Result<UserDTO>> GetUserByName(string name)
        {
            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("getUserByUsername", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@username", name);
                await using (SqlDataReader reader = command.ExecuteReader())
                {
                    Result<UserDTO> result = new Result<UserDTO>();
                    while (reader.Read())
                    {
                        string? fatherName;
                        if (reader.IsDBNull(reader.GetOrdinal("fatherName")))
                        {
                            fatherName = null;
                        }
                        else
                        {
                            fatherName = reader.GetString(reader.GetOrdinal("fatherName"));
                        }
                        DateTime? lastLogin;
                        if (reader.IsDBNull(reader.GetOrdinal("lastLogin")))
                        {
                            lastLogin = null;
                        }
                        else
                        {
                            lastLogin = reader.GetDateTime(reader.GetOrdinal("lastLogin"));
                        }
                        DateTime? tokenExpiration;
                        if (reader.IsDBNull(reader.GetOrdinal("tokenExpiration")))
                        {
                            tokenExpiration = null;
                        }
                        else
                        {
                            tokenExpiration = reader.GetDateTime(reader.GetOrdinal("tokenExpiration"));
                        }
                        string? lastToken;
                        if (reader.IsDBNull(reader.GetOrdinal("lastToken")))
                        {
                            lastToken = null;
                        }
                        else
                        {
                            lastToken = reader.GetString(reader.GetOrdinal("lastToken"));
                        }
                        result.Data = new UserDTO(
                            reader.GetInt32(reader.GetOrdinal("userId")),
                            reader.GetString(reader.GetOrdinal("username")),
                            reader.GetString(reader.GetOrdinal("userPassword")),
                            reader.GetString(reader.GetOrdinal("email")),
                            reader.GetString(reader.GetOrdinal("firstName")),
                            reader.GetString(reader.GetOrdinal("lastName")),
                            fatherName,
                            reader.GetDateTime(reader.GetOrdinal("dateRegistered")),
                            lastLogin,
                            lastToken,
                            tokenExpiration,
                            reader.GetBoolean(reader.GetOrdinal("isActive"))
                            );
                    }
                    if (result.Data == null)
                    {
                        result.ErrorCode = (int)ErrorCodes.NotFound;
                        return result;
                    }
                    return result;
                }
            }
        }

        public async Task<Result> UpdateLoginUser(int id, DateTime lastLogin, string lastToken, DateTime tokenExpiration)
        {
            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                Result result = new Result();
                connection.Open();

                SqlCommand command = new SqlCommand("updateLoginUser", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@lastLogin", lastLogin);
                command.Parameters.AddWithValue("@tokenExpiration", lastLogin);
                command.Parameters.AddWithValue("@lastToken", lastToken);

                if (command.ExecuteNonQuery() <= 0)
                {
                    result.ErrorCode = (int)ErrorCodes.NotFound;
                    result.ErrorMessage = $"User with {id} not found";
                    return result;
                }
                return result;
            }
        }

        public async Task<Result> UpdateUser(int id, string email, string firstName, string lastName, string? fatherName, bool isActive)
        {
            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                Result result = new Result();
                connection.Open();

                SqlCommand command = new SqlCommand("updateUser", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@firstName", firstName);
                command.Parameters.AddWithValue("@lastName", lastName);
                if (fatherName != null)
                {
                    command.Parameters.AddWithValue("@fatherName", fatherName);
                }
                else
                {
                    command.Parameters.AddWithValue("@fatherName", DBNull.Value);
                }
                command.Parameters.AddWithValue("@isActive", isActive);

                if (command.ExecuteNonQuery() <= 0)
                {
                    result.ErrorCode = (int)ErrorCodes.NotFound;
                    result.ErrorMessage = $"User with {id} not found";
                    return result;
                }
                return result;
            }
        }
        public async Task<Result<LoginDTO>> GetUserByToken(string token)
        {
            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                Result<LoginDTO> result = new Result<LoginDTO>();
                connection.Open();

                SqlCommand command = new SqlCommand("getUserByToken", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@token", token);
                await using (SqlDataReader reader = command.ExecuteReader())
                {
                    DateTime? tokenExpiration;
                    if (reader.IsDBNull(reader.GetOrdinal("tokenExpiration")))
                    {
                        tokenExpiration = null;
                        result.ErrorCode = (int)ErrorCodes.InternalServerError;
                        result.ErrorMessage = "token expiration is null";
                        return result;
                    }
                    else
                    {
                        tokenExpiration = reader.GetDateTime(reader.GetOrdinal("tokenExpiration"));
                    }
                    string? lastToken;
                    if (reader.IsDBNull(reader.GetOrdinal("tokenExpiration")))
                    {
                        lastToken = null;
                        result.ErrorCode = (int)ErrorCodes.InternalServerError;
                        result.ErrorMessage = "token is null";
                        return result;
                    }
                    else
                    {
                        lastToken = reader.GetString(reader.GetOrdinal("tokenExpiration"));
                    }
                    result.Data = new LoginDTO(
                        reader.GetInt32(reader.GetOrdinal("userId")), lastToken,
                        tokenExpiration.Value);
                }
                if (result.Data == null)
                {
                    result.ErrorCode = (int)ErrorCodes.NotFound;
                    return result;
                }
                return result;
            }
        }
    }
}
