using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Application.DTOs
{
    public record OrderDetailsDTO(
        int Id,
        [Required, Range(1, int.MaxValue)] int ProductId,
        [Required, Range(1, int.MaxValue)] int ClientId,
        [Required] string ClientName,
        [Required] int Client,
        [Required, EmailAddress] string Email,
        [Required] string TelephoneNumber,
        [Required] string Address,
        [Required] string ProductName,
        [Required, Range(1, int.MaxValue)] int PurchaseQuantity,
        [Required, DataType(DataType.Currency)] decimal UnitPrice,
        [Required, DataType(DataType.Currency)] decimal TotalPrice,
        [Required] DateTime OrderDate
        );
}
