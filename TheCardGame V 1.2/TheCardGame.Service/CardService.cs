using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheCardGame.Models;
using TheCardGame.Models.Constants;
using TheCardGame.ServiceInterface;

namespace TheCardGame.Service
{
    public class CardService:ICardService
    {
        #region Private Variables
        /// <summary>
        /// Card types like CLUBS, SPADES, HEARTS, DIAMONDS
        /// </summary>
        private string[] _cardTypes;
        private string[] _values;
        private readonly IResponseService _responseService;

        #endregion

        #region Properties
        /// <summary>
        /// This property represents Complete deck of cards
        /// </summary>
        public List<CardModel> Deck { get; set; }

        #endregion

        #region Constructor
        public CardService(string[] cardTypes, string[] values)
        {
            _cardTypes = cardTypes;
            _values = values;
            Deck = InitializeDeck(cardTypes, values);
            _responseService = new ResponseService();
        }
        #endregion

        #region Private Methods
        private List<CardModel> InitializeDeck(string[] cardTypes, string[] values)
        {
            List<CardModel> cards = new List<CardModel>();
            foreach (string cardType in cardTypes)
            {
                foreach (string value in values)
                {
                    cards.Add(new CardModel { Suit = cardType.Trim(), Value = value.Trim() });
                }
            }
            return cards;
        }

        private void RemoveCard(CardModel toBeRemovedCard)
        {
            if (toBeRemovedCard != null)
            {
                Deck.Remove(toBeRemovedCard);
            }
        }

        private void ShuffleDeck()
        {
            if (Deck.Count > 0)
            {
                Deck.Shuffle();
            }
        }


        #endregion

        #region Public Methods

        public ResultModel<CardModel> StartGame()
        {
            ResultModel<CardModel> response;
            try
            {
                //
                //1. InitializeDeck
                Deck = InitializeDeck(_cardTypes, _values);
                //2. Shuffle The Cards
                ShuffleDeck();
                response = _responseService.SetResponse<CardModel>(null, false, DisplayMessageConstatns.START_GAME_SUCCESS);
            }
            catch (Exception ex)
            {
                // Log the error
                response = _responseService.SetResponse<CardModel>(null, true, DisplayMessageConstatns.START_GAME_FAILED);
            }
            return response;
        }

        public ResultModel<CardModel> PlayCard()
        {
            ResultModel<CardModel> response;
            try
            {
                if (Deck.Count > 0)
                {
                    //Deck is not empty
                    CardModel playedCard = Deck.FirstOrDefault();
                    response = _responseService.SetResponse(playedCard, false, DisplayMessageConstatns.PLAY_CARD_SUCCESS);
                    RemoveCard(playedCard);
                }
                else
                {
                    //Deck is empty
                    response = _responseService.SetResponse<CardModel>(null, false, DisplayMessageConstatns.PLAY_CARD_DECK_EMPTY);
                }
            }
            catch (Exception ex)
            {
                // Log the error
                response = _responseService.SetResponse<CardModel>(null, true, DisplayMessageConstatns.PLAY_CARD_FAIL);
            }
            return response;
        }

        public ResultModel<CardModel> ShuffleTheDeck()
        {
            ResultModel<CardModel> response;
            try
            {
                ShuffleDeck();
                response= _responseService.SetResponse<CardModel>(null,false,DisplayMessageConstatns.SHUFFLE_CARD_SUCCESS);
            }
            catch (Exception)
            {
                // Log the error
                response = _responseService.SetResponse<CardModel>(null, true, DisplayMessageConstatns.SHUFFLE_CARD_FAILED);
            }
            return response;
        }

        #endregion
    }
}
