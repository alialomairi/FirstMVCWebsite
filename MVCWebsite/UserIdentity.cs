using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace MVCWebsite
{
    public class UserIdentity: GenericIdentity
    {
        public UserIdentity(int userId, string displayName,bool isFacebook) : base(displayName,isFacebook?"facebook":"system") { this.userId = userId; }
        private int userId;
        public int UserId { get { return userId; } }
    }
}