using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Application.DTOs
{
    public record AppUserDTO(
        int Id,
        [Required] string Name,
        [Required] int TelephoneNumber,
        [Required, Range(1, int.MaxValue)] int Age,
        [Required] string Address,
        [Required, EmailAddress] string Email,
        [Required, DataType(DataType.Password)] string Password,
        [Required] string Role
        );
}
