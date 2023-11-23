using Dapper;
using EldudkaServer.Models.DAL;
using EldudkaServer.Context;
using EldudkaServer.Models.DTO;

namespace EldudkaServer.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DapperContext _context;

        public ProductRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<ProductDAL> GetByIdAsync(Guid id)
        {
            var productSqlQuery =
                @"
                    SELECT * FROM ""Product"" WHERE ""Id"" = @id;
                    SELECT ""Href"" FROM ""ProductImage"" WHERE ""ProductId"" = @id;
                ";

            var productStocksqlQuery =
                @"
                    SELECT * FROM ""ProductStock""
                    JOIN ""Shop"" ON ""Id"" = ""ShopId""
                    WHERE ""ProductId"" = @id;
                ";

            using var connection = _context.CreateConnection();
            using var multi = await connection.QueryMultipleAsync(productSqlQuery, new { id });

            var product = await multi.ReadSingleOrDefaultAsync<ProductDAL>();
            product.Images = await multi.ReadAsync<string>();
            product.Stock = await connection.QueryAsync<ProductStockDAL, ShopDAL, ProductStockDAL>(
                productStocksqlQuery,
                (productStock, shop) =>
                {
                    return new ProductStockDAL { Amount = productStock.Amount, Shop = shop };
                },
                new { id }
            );

            return product;
        }

        public async Task<IEnumerable<ProductDAL>> GetAllAsync()
        {
            var productSqlQuery =
                @"
                    SELECT * FROM ""Product"";
                    SELECT * FROM ""ProductImage"";
                    SELECT * FROM ""ProductStock"";
                    SELECT * FROM ""Shop"";
                ";

            using var connection = _context.CreateConnection();
            using var multi = await connection.QueryMultipleAsync(productSqlQuery);

            var products = await multi.ReadAsync<ProductDAL>();
            var productImages = await multi.ReadAsync<ProductIdWithHrefDTO>();
            var productStock = await multi.ReadAsync<ProductStockDTO>();
            var shops = await multi.ReadAsync<ShopDAL>();

            foreach (var product in products)
            {
                product.Images = productImages
                    .Where(i => i.ProductId == product.Id)
                    .Select(i => i.Href);
                product.Stock = productStock
                    .Where(s => s.ProductId == product.Id)
                    .Select(
                        s =>
                            new ProductStockDAL()
                            {
                                Amount = s.Amount,
                                Shop = shops.FirstOrDefault(shp => shp.Id == s.ShopId)
                            }
                    );
            }

            return products;
        }

        public async Task<int> DeleteAllAsync()
        {
            using var connection = _context.CreateConnection();

            var productImageSqlQuery = @"DELETE FROM ""ProductImage""";
            var productImageAffectedRows = await connection.ExecuteAsync(productImageSqlQuery);

            var productStockSqlQuery = @"DELETE FROM ""ProductStock""";
            var productStockAffectedRows = await connection.ExecuteAsync(productStockSqlQuery);

            var productSqlQuery = @"DELETE FROM ""Product""";
            var productAffectedRows = await connection.ExecuteAsync(productSqlQuery);

            return productAffectedRows;
        }

        public async Task<int> AddRangeAsync(IEnumerable<ProductForCreatingDTO> products)
        {
            var result = 0;

            if (products.Any())
            {
                using var connection = _context.CreateConnection();

                var productToAdd = products.Select(
                    (p) =>
                        new
                        {
                            p.Id,
                            p.Name,
                            p.Description,
                            p.Price
                        }
                );
                if (productToAdd.Any())
                {
                    var sqlQueryProduct =
                        @"
                            INSERT INTO ""Product"" (""Id"", ""Name"", ""Description"", ""Price"")
                            VALUES (@Id, @Name, @Description, @Price)
                        ";

                    result = await connection.ExecuteAsync(sqlQueryProduct, productToAdd);
                }

                var productImageToAdd = products.SelectMany(
                    p => p.Images.Select(i => new { Href = i, ProductId = p.Id })
                );
                if (productImageToAdd.Any())
                {
                    var sqlQueryProductImage =
                        @"
                            INSERT INTO ""ProductImage"" (""Href"", ""ProductId"")
                            VALUES (@Href, @ProductId)
                        ";

                    var productImageInsertedRows = await connection.ExecuteAsync(
                        sqlQueryProductImage,
                        productImageToAdd
                    );
                }

                var productStockToAdd = products.SelectMany(
                    p =>
                        p.Stock.Select(
                            s =>
                                new
                                {
                                    ProductId = p.Id,
                                    s.ShopId,
                                    s.Amount
                                }
                        )
                );
                if (productStockToAdd.Any())
                {
                    var sqlQueryProductStock =
                        @"
                        INSERT INTO ""ProductStock"" (""ProductId"", ""ShopId"", ""Amount"")
                        VALUES (@ProductId, @ShopId, @Amount)
                    ";
                    var productStockInsertedRows = await connection.ExecuteAsync(
                        sqlQueryProductStock,
                        productStockToAdd
                    );
                }
            }

            return result;
        }
    }
}
