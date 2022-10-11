﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerModel.UserModels
{
    public class UserDTO
    {
        public string Name { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public ICollection<UserChats> UserChats { get; set; } = new List<UserChats>();
        public ICollection<UserContacts> Contacts { get; set; } = new List<UserContacts>();
    }
}
