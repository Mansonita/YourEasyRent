﻿using Amazon.Runtime.Internal.Transform;
using System.Security.Cryptography.X509Certificates;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using YourEasyRent.DataBase.Interfaces;
using YourEasyRent.Services.Buttons;
using YourEasyRent.Services.State;

namespace YourEasyRent.Services.Keyboard
{
    public class TelegramSender : ITelegramSender //  как мы понмаем какое кокренто меню вызвать, когда получаем ответ 
    {
        private readonly ITelegramBotClient _botClient;
        private Dictionary<MenuStatus, IButtonHandler> _menus;
        private readonly IProductRepository _productRepository;

        public TelegramSender(ITelegramBotClient botClient)
        {
            _botClient = botClient;
            _menus = new Dictionary<MenuStatus, IButtonHandler>()
            {
                { MenuStatus.MainMenu, new MainMenuButtonHandler() },
                { MenuStatus.BrandMenu, new BrandButtonHandler(_productRepository) },//  _botClient  можно и убрать и написать репозиторий, который ходит в базу
                { MenuStatus.CategoryMenu, new CategoryButtonHandler() },
                { MenuStatus.MenuAfterReceivingRresult, new ReturnToMMButtonHandler() }
            };
        }
        public async Task SendMainMenu(long chatId)
        { 
            var menu = await _menus[MenuStatus.MainMenu].SendMenuToTelegramHandle();
            await _botClient.SendTextMessageAsync(chatId, "Main menu. Choose one:", replyMarkup: menu);
            // надо ли тут return?
        }
        public async Task SendBrandMenu(long chatId)
        {
            var menu = await _menus[MenuStatus.BrandMenu].SendMenuToTelegramHandle();
            await _botClient.SendTextMessageAsync(chatId, "Сhoose a brand:", replyMarkup: menu);
        }

        public async Task SendCategoryMenu(long chatId)
        {
            var menu = await _menus[MenuStatus.CategoryMenu].SendMenuToTelegramHandle();
            await _botClient.SendTextMessageAsync(chatId, "Сhoose a category:", replyMarkup: menu);
        }

        public  async Task SendMenuAfterResult(long chatId)
        {
            var menu = await _menus[MenuStatus.MenuAfterReceivingRresult].SendMenuToTelegramHandle();
            await _botClient.SendTextMessageAsync(chatId, "What do you want to do next?", replyMarkup: menu);
        }
        public Task SendResults()
        {
            throw new NotImplementedException();
            
        }

    }
}
