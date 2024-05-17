using MySql.Data.MySqlClient;
using receive_ID.Models;
using receive_ID.Interfaces;
using Newtonsoft.Json;

namespace receive_ID.Services
{
    public class DbService : IDbService
    {
        private readonly MySqlConnection _connection;

        public DbService(string connectionString)
        {
            _connection = new MySqlConnection(connectionString);
        }

        public async Task<UserData> GetUser(int userid)
        {
            try
            {
                await _connection.OpenAsync();

                using var httpClient = new HttpClient();
                var response = await httpClient.GetAsync($"https://reqres.in/api/users/{userid}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var externalUserData = JsonConvert.DeserializeObject<ExternalUserData>(json);

                    var userData = new UserData
                    {
                        userid = userid,
                        email = externalUserData.data.email,
                        first_name = externalUserData.data.first_name,
                        last_name = externalUserData.data.last_name,
                        avatar = externalUserData.data.avatar
                    };

                    return userData;
                }
                else
                {
                    return null;
                }
            }
            finally
            {
                _connection.Close();
            }
        }

        public async Task SaveUser(UserData userData)
        {
            try
            {
                await _connection.OpenAsync();

                using var cmd = new MySqlCommand("INSERT INTO User (userid, email, first_name, last_name, avatar) VALUES (@UserId, @Email, @FirstName, @LastName, @Avatar)", _connection);
                cmd.Parameters.AddWithValue("@UserId", userData.userid);
                cmd.Parameters.AddWithValue("@Email", userData.email);
                cmd.Parameters.AddWithValue("@FirstName", userData.first_name);
                cmd.Parameters.AddWithValue("@LastName", userData.last_name);
                cmd.Parameters.AddWithValue("@Avatar", userData.avatar);

                await cmd.ExecuteNonQueryAsync();
            }
            finally
            {
                _connection.Close();
            }
        }
    }
}