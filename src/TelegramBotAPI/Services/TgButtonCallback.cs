﻿using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualBasic;
using System.Numerics;
using Telegram.Bot.Types;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace YourEasyRent.Services
{
    public class TgButtonCallback
    {
        private readonly Update _update;
        public bool IsStart => IsBotStart();
        public bool IsValidMessage => IsValidMsg();
        public bool IsValueMenuMessage  => IsMenuButton();
        public bool IsBrandMenu => IsBrandMenuButton();
        public bool IsCategoryMenu => IsCategoryMenuButton();
        public bool IsValueProductButton => IsProductButton();
        public bool IsProductBrand=> IsProductBrandButton();
        public bool IsProductCategory=> IsProductCategoryButton();
        public bool IsSubscribeToProduct => IsSubscribeButton();


        public TgButtonCallback(Update update)
        {
            _update = update;
        }

        private bool IsSubscribeButton()
        {
            var nameOfButton = _update.CallbackQuery?.Data;
            if(nameOfButton == "Subscribe")
            {
                return true;
            }
            return false;

        }
        public bool IsBotStart()
        {
            var messageText = _update.Message?.Text;
            var nameOfButton = _update.CallbackQuery?.Data;
            return messageText != null && messageText.Contains("/start") || nameOfButton == "StartNewSearch";

        }
        public string GetUserId()
        {
            var userId = _update.CallbackQuery?.From.Id.ToString();
            if (userId == null)
            {
                var startedForUserId = _update.Message?.From.Id.ToString();
                return startedForUserId;
            }
            return userId;
        }
        public string GetChatId()
        {
            var chatId = _update.Message?.Chat.Id.ToString();
            if(chatId == null)
            {
                chatId = _update.CallbackQuery?.From.Id.ToString();
                return chatId;
            }
            return chatId;
        }

        public bool IsValidMsg() 
        {
            try
            {
                var nameOfButton = _update.CallbackQuery?.Data;
                return nameOfButton.All(c => char.IsLetter(c) || c == '_' || c == '/'  || c == ' ');
            }
            catch (Exception ex)
            {
                throw new Exception("The user did not send a message", ex);
            }
                        
        }

        public bool IsMenuButton()
        {
            var nameOfButton = _update.CallbackQuery?.Data;
            return nameOfButton == "BrandMenu" || nameOfButton == "CategoryMenu";        
        }

        public bool IsBrandMenuButton()
        {
            var menuButton = _update.CallbackQuery.Data;
            if (menuButton == "BrandMenu")
            {
                return true;
            }
            return false;
        }

        public bool IsCategoryMenuButton()
        {
            var menuButton = _update.CallbackQuery.Data;
            if (menuButton == "CategoryMenu")
            {
                return true;
            }
            return false;
        }
  
        public bool IsProductButton()
        {
            var nameOfButton = _update.CallbackQuery.Data;
            if( nameOfButton.StartsWith("Brand_") || nameOfButton.StartsWith("Category_"))
            {
                return true;
            }
            return false;
        }


        public bool IsProductBrandButton()
        {
            var productButton = _update.CallbackQuery.Data;
            if (productButton.StartsWith("Brand_"))
            {
                return true;
            }
            return false;
        }

        public bool IsProductCategoryButton()
        {
            var productButton = _update.CallbackQuery.Data;
            if (productButton.StartsWith("Category_"))
            {
                return true;
            }
            return false;
        }

        public string GetProductButton()
        {

            var productButton = _update.CallbackQuery.Data;
            if (productButton.StartsWith("Brand_"))
            {
                return productButton.Replace("Brand_", "");
            }
            if (productButton.StartsWith("Category_"))
            {
                return productButton.Replace("Category_", "");
            }
            return productButton;
        }
    }
}
