﻿using MongoDB.Driver;
using Telegram.Bot.Types;
using YourEasyRent.DataBase.Interfaces;
using YourEasyRent.Entities;
using YourEasyRent.UserState;

namespace YourEasyRent.DataBase
{
    public class UserStateManagerRepository : IUserStateManagerRepository
    {
        private readonly IMongoCollection<UserSearchStateDTO> _collectionOfUserSearchState;
        public UserStateManagerRepository(DataBaseConfig dateBaseConfig, IMongoClient mongoClient)
        {
            var dataBase = mongoClient.GetDatabase(dateBaseConfig.DataBaseName);
            _collectionOfUserSearchState = dataBase.GetCollection<UserSearchStateDTO>(dateBaseConfig.CollectionName);
        }

        public async Task<UserSearchState> GetForUser(string userId)
        {

            
            var filter = Builders<UserSearchStateDTO>.Filter.Eq(u => u.UserId.ToString(), userId);// ошибка MongoDB.Driver.Linq.ExpressionNotSupportedException: 'Expression not supported: u.UserId.ToString().'

            var dto = await _collectionOfUserSearchState.Find(filter).FirstOrDefaultAsync();

            var state = new UserSearchState(dto);

            return state;
        }

        public async Task CreateAsync(UserSearchState userSearchState)//  мы должны выполнить этот метод когда все поля будут заполены или 
            // достаточно и одного поля??
        {
            var dto = userSearchState.TOMongoRepresintation();
            await _collectionOfUserSearchState.InsertOneAsync(dto);
        }

        public async Task UpdateAsync(UserSearchState userSearchState)
        {
            var filter = Builders<UserSearchStateDTO>.Filter.Eq(u => u.UserId, userSearchState.UserId);
            var dto = userSearchState.TOMongoRepresintation();
            await _collectionOfUserSearchState.ReplaceOneAsync(filter, dto);
        }

        public async Task<MenuStatus> GetCurrentStateForUser(string userId)
        {
            var filter = Builders<UserSearchStateDTO>.Filter.Eq(u =>u.UserId, userId);
            var dto = await _collectionOfUserSearchState.Find(filter).FirstOrDefaultAsync();

            var state = new UserSearchState(dto);

            return state.CurrentMenuStatus;// не уверена, надо при запуске посмотреть что возвращает метод, точно ли корректный статус

        }
    }
}
