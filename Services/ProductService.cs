using sqlapp.Models;
using System.Data.SqlClient;

namespace sqlapp.Services
{
    public class ProductService : IProductService
    {
        private readonly IConfiguration _configuration;

        private static string db_source = "appserver600.database.windows.net";
        private static string db_user = "sqlusr";
        private static string db_password = "Azure@81";
        private static string db_database = "appdb";

        public ProductService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SqlConnection GetConnection()
        {
            var builder = new SqlConnectionStringBuilder();
            builder.DataSource = db_source;
            builder.UserID = db_user;
            builder.Password = db_password;
            builder.InitialCatalog = db_database;

            return new SqlConnection(builder.ConnectionString);
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            List<Product> productList = new List<Product>();

            string query = "SELECT ProductID, ProductName, Quantity FROM Products";

            using (SqlConnection connection = GetConnection())
            {
                await connection.OpenAsync();

                SqlCommand cmd = new SqlCommand(query, connection);

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Product product = new Product()
                        {
                            ProductID = reader.GetInt32(0),
                            ProductName = reader.GetString(1),
                            Quantity = reader.GetInt32(2)
                        };

                        productList.Add(product);
                    }
                }
            }

            return productList;
        }
    }
}


