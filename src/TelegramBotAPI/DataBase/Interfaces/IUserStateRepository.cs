﻿using Telegram.Bot.Types;
using MongoDB.Driver;
using YourEasyRent.Entities;
using YourEasyRent.UserState;

namespace YourEasyRent.DataBase.Interfaces
{
    public interface IUserStateRepository
    {
        Task<UserSearchState> GetForUser(string userId);
        Task CreateAsync(UserSearchState userSearchState);
        Task<bool> UpdateAsync(UserSearchState userSearchState);
        Task<MenuStatus> GetCurrentStateForUser(string userId);
        Task<bool> CheckFieldsFilledForUser(string userId);
        Task<(string Brand, string Category)> GetBrandAndCategoryForSearch(string userId);
       
    }
}
