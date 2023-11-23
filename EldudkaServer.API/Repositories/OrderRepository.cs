using Dapper;
using EldudkaServer.Context;
using EldudkaServer.Models.DAL;
using EldudkaServer.Models.DTO;

namespace EldudkaServer.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DapperContext _context;

        public OrderRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(IEnumerable<ProductIdWithAmountDTO> products)
        {
            var sqlOrderQuery =
                @"
                    INSERT INTO ""Order"" (""CreatedDate"") VALUES (NOW()) RETURNING ""Id"";
               ";

            var sqlOrderProductQuery =
                @"
                    INSERT INTO ""OrderProduct"" (""OrderId"", ""ProductId"", ""Amount"")
                    VALUES (@OrderId, @ProductId, @Amount)
                ";

            using var connection = _context.CreateConnection();
            var createdOrderId = await connection.QueryFirstAsync<int>(sqlOrderQuery);
            var addedOrderProductRows = await connection.ExecuteAsync(
                sqlOrderProductQuery,
                products.Select(
                    (p) =>
                        new
                        {
                            OrderId = createdOrderId,
                            p.ProductId,
                            p.Amount
                        }
                )
            );

            return createdOrderId;
        }

        public async Task<IEnumerable<OrderDAL>> GetAllAsync()
        {
            var sqlQuery =
                @"
                    SELECT * FROM ""Order"";
                    SELECT * FROM ""OrderProduct"";
                ";

            using var connection = _context.CreateConnection();
            using var multi = await connection.QueryMultipleAsync(sqlQuery);

            var orders = await multi.ReadAsync<OrderDAL>();
            var orderProducts = await multi.ReadAsync<OrderProductDTO>();

            return orders.Select(
                o =>
                    new OrderDAL()
                    {
                        Id = o.Id,
                        CreatedDate = o.CreatedDate,
                        Products = orderProducts
                            .Where(op => op.OrderId == o.Id)
                            .Select(
                                op =>
                                    new ProductIdWithAmountDTO()
                                    {
                                        ProductId = op.ProductId,
                                        Amount = op.Amount
                                    }
                            )
                    }
            );
        }
    }
}
