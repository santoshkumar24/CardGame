using System;
using System.Collections.Generic;
using System.Text;
using TheCardGame.ServiceInterface;
using System.IO;
using Microsoft.Extensions.Configuration;
using TheCardGame.Models;

namespace TheCardGame.Service
{
    public class CardGameService
    {
        #region Private Variables
        /// <summary>
        /// Configuration Service
        /// </summary>
        private readonly IConfigurationService _configurationService;
        private readonly ICardService _cardService;
        private readonly ICardDesignService _cardDesignService;
        private readonly IResponseService _responseService;
        /// <summary>
        /// Card type like - SUITS, CLUBS, DIAMONDS, SPADES
        /// </summary>
        private string[] _cardTypes;
        /// <summary>
        /// Card Values - A, 2, 3, 4, 5, 6, 7, 8, 9, 10, J, Q, K 
        /// </summary>
        private string[] _cardValues;
        #endregion

        #region Constructor

        public CardGameService(IConfiguration configuration)
        {
            _configurationService = new ConfigurationService(configuration);
            GetConfiguration();
            _cardService = new CardService(_cardTypes, _cardValues);
            _cardDesignService = new CardDesignService();
            _responseService = new ResponseService();
        }

        #endregion

        #region Private Methods



        private void DisplayCard(CardModel card)
        {
            ResultModel<CardDesignPropertiesModel> cardAscii = _cardDesignService.GetAsciiOfCard(card.Suit,card.Value);
            if(!cardAscii.IsError)
                _cardDesignService.DisplayCardFace(cardAscii.Data);
        }

        private void HandlePlayCardDisplay(ResultModel<CardModel> response)
        {
            if (!response.IsError && response.Data != null)
            {
                DisplayCard(response.Data);
            }
        }

        

        #endregion

        #region Public Methods

        private void GetConfiguration()
        {
            _cardTypes = _configurationService.ReadDelimitedValue("SuitTypes", ",");
            _cardValues = _configurationService.ReadDelimitedValue("CardValues", ",");
        }

        private void RunApplication()
        {
            //Console.WriteLine("Press ESC to stop");
            do
            {
                while (!Console.KeyAvailable)
                {
                    if (Console.ReadKey(true).Key == ConsoleKey.P)
                    {
                        //*** Play Card
                        ResultModel<CardModel> response = _cardService.PlayCard();
                        HandlePlayCardDisplay(response);
                        _cardDesignService.HandleResponse(response,true,true,1000);
                    }
                    else if (Console.ReadKey(true).Key == ConsoleKey.S)
                    {
                        //*** Shuffle
                        ResultModel<CardModel> response = _cardService.ShuffleTheDeck();
                        _cardDesignService.HandleResponse(response,false,true,1000);
                    }
                    else if (Console.ReadKey(true).Key == ConsoleKey.R)
                    {
                        //*** Re-start
                        ResultModel<CardModel> response = _cardService.StartGame();
                        _cardDesignService.HandleResponse(response,false,true,1000);
                    }
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
        }

        public void Run()
        {
            _cardDesignService.DisplayGame();
            // 1. Configuration
            GetConfiguration();
            // 2. Initialize Card Service
            _cardService.StartGame();
            RunApplication();
        }
        #endregion
    }
}