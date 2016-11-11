using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace JagBot
{
    public static class UserHelper
    {     
        public static bool IsJames(User user)
        {
            return user?.Id == GlobalSettings.James?.ID;
        }
    }
}
