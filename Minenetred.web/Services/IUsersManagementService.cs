﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minenetred.web.Services
{
    public interface IUsersManagementService
    {
        bool CheckReisteredUser(string userEmail);
        void RegisterUser(string userEmail);
        bool CheckRedmineKey(string userEmail);
        void UpdateKey(string apiKey, string userEmail);
        string GetUserKey(string userEmail);
        Task AddRedmineIdAsync(string key);
    }
}