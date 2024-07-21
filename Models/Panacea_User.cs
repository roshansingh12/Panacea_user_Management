using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Panacea_User_Management.Models
{
    public class Panacea_User
    {
            [Key]
            public string Id { get; set; }
            
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }
            public string Address { get; set; }
            public string PhoneNumber { get; set; }
            public string Email { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string PinCode { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public bool IsActive { get; set; }

    }
}
