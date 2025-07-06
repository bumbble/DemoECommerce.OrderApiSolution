using OrderAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Application.DTOs.Conversions
{
    public static class OrderConversion
    {
        public static OrderDTO ToDTO(this Order entity)
        {
            return entity == null
                ? throw new ArgumentNullException(nameof(entity))
                : new OrderDTO(
                    entity.Id,
                    entity.ProductId,
                    entity.ClientId,
                    entity.PurchaseQuantity,
                    entity.OrderDate
            );
        }

        public static IEnumerable<OrderDTO> ToDTOs(this IEnumerable<Order> entities)
        {
            return entities == null
                ? throw new ArgumentNullException(nameof(entities))
                : entities.Select(e => e.ToDTO());
        }

        public static Order ToEntity(this OrderDTO order)
        {
            return order == null
                ? throw new ArgumentNullException(nameof(order))
                : new Order()
                {
                    Id = order.Id,
                    ProductId = order.ProductId,
                    ClientId = order.ClientId,
                    PurchaseQuantity = order.PurchaseQuantity,
                    OrderDate = order.OrderDate
                };
        }

        public static IEnumerable<Order> ToEntities(this IEnumerable<OrderDTO> orders)
        {
            return orders == null
                ? throw new ArgumentNullException(nameof(orders))
                : orders.Select(o => o.ToEntity());
        }

    }
}
