using System;

namespace ShopApp.Models
{
    public class AccountData
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime TimeOfRegistration { get; set; }
        public DateTime? TimeOfRemovingAccount { get; set; }

        public User User { get; set; }
    }
}