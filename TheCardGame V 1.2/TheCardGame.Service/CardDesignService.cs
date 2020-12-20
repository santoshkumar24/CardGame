using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TheCardGame.Models;
using TheCardGame.Models.Constants;
using TheCardGame.ServiceInterface;

namespace TheCardGame.Service
{
    public class CardDesignService:ICardDesignService
    {
        //List<string> suits_name = new List<string> { "SPADES", "DIAMONDS", "HEARTS", "CLUBS" };
        //List<string> suits_symbols = new List<string> { "♠", "♦", "♥", "♣" };
        IDictionary<string, string> suitsSymbol = new Dictionary<string, string>() {
            { "SPADES","♠" },
            { "DIAMONDS","♦" },
            { "HEARTS","♥" },
            { "CLUBS","♣" }
        };

        List<ConsoleColor> suits_colors = new List<ConsoleColor> { ConsoleColor.Black, ConsoleColor.Red, ConsoleColor.Red, ConsoleColor.Black };
        private readonly IResponseService _responseService;
        public CardDesignService()
        {
            _responseService = new ResponseService();
        }
        public ResultModel<CardDesignPropertiesModel> GetAsciiOfCard(string suitType, string cardValue)
        {
            List<string> cardAscii = new List<string>();
            string rank = string.Empty;
            string space = string.Empty;
            List<string> lines = new List<string>(); //create an empty list of list, each sublist is a line
            CardDesignPropertiesModel cardDesignPropertiesModel = new CardDesignPropertiesModel();
            ResultModel<CardDesignPropertiesModel> result = new ResultModel<CardDesignPropertiesModel>();
            try
            {
                if (cardValue == "10") //10 is the only one who's rank is 2 char long
                {
                    rank = cardValue;
                    space = "";
                }
                //if we write "10" on the card that line will be 1 char to long
                else
                {
                    rank = cardValue;
                    space = " "; // no "10", a blank space to fill the void
                }
                //get the cards suit in two steps
                //int suit = suits_name.IndexOf(suitType);
                string suitSymbol = suitsSymbol[suitType];
                ConsoleColor suitColor = suits_colors[suitsSymbol.Values.ToList().IndexOf(suitSymbol)];
                //Add the individual card on a line by line basis
                cardAscii = GetAsciiForFaceOfCard(rank, space, suitSymbol);
                cardDesignPropertiesModel.CardText = string.Join("\n", cardAscii);
                cardDesignPropertiesModel.Color = suitColor;
                cardDesignPropertiesModel.CardPattern = cardAscii;
                cardDesignPropertiesModel.CardWidth = 26;
                cardDesignPropertiesModel.CardHeight = 22;
                result = _responseService.SetResponse<CardDesignPropertiesModel>(cardDesignPropertiesModel, false, DisplayMessageConstatns.ASCII_CARD_RETRIEVAL_SUCCESS);
            }
            catch (Exception ex)
            {
                // Log The Error 
               result =  _responseService.SetResponse<CardDesignPropertiesModel>(null, true, DisplayMessageConstatns.ASCII_CARD_RETRIEVAL_FAILED);
            }
            return result;
        }

        public List<string> GetAsciiForFaceOfCard(string rank, string space, string suitSymbol)
        {
            List<string> cardAscii = new List<string>();
            //cardAscii.Add("┌─────────┐");
            //cardAscii.Add(String.Format("│{0}{1}       │", rank, space)); // # use two {} one for char, one for space or char
            //cardAscii.Add("│         │");
            //cardAscii.Add("│         │");
            //cardAscii.Add(String.Format("│    {0}    │", suitSymbol));
            //cardAscii.Add("│         │");
            //cardAscii.Add("│         │");
            //cardAscii.Add(String.Format("│       {0}{1}│", space, rank));
            //cardAscii.Add("└─────────┘");


            cardAscii.Add("┌─────────────────────────┐");
            cardAscii.Add(String.Format("│{0}{1}                       │",rank,space));
            cardAscii.Add("│                         │");
            cardAscii.Add("│                         │");
            cardAscii.Add("│                         │");
            cardAscii.Add("│                         │");
            cardAscii.Add("│                         │");
            cardAscii.Add("│                         │");
            cardAscii.Add("│                         │");
            cardAscii.Add(String.Format("│          {0} {0} {0}          │",suitSymbol));
            cardAscii.Add("│                         │");
            cardAscii.Add("│                         │");
            cardAscii.Add("│                         │");
            cardAscii.Add("│                         │");
            cardAscii.Add("│                         │");
            cardAscii.Add("│                         │");
            cardAscii.Add("│                         │");
            cardAscii.Add("│                         │");
            cardAscii.Add("│                         │");
            cardAscii.Add("│                         │");
            cardAscii.Add(String.Format("│                       {0}{1}│",space,rank));
            cardAscii.Add("└─────────────────────────┘");
            return cardAscii;
        }

        public void DisplayCard(string cardAscii)
        {
            Console.WriteLine(cardAscii);
        }

        public void DisplayCard(CardDesignPropertiesModel cardProp)
        {
            Console.ForegroundColor = cardProp.Color;
            Console.WriteLine(cardProp.CardText);
        }

        

        public void DisplayGame()
        {
            Console.BackgroundColor= ConsoleColor.Black;
            DisplayTitleBanner();
            DisplayFrame();
            DisplayDeck();
            DisplayManual();
        }

        public void DisplayManual()
        {
            DisplayText(DisplayMessageConstatns.GAME_KEY_PLAY_CARD_MANUAL, 70, 15, ConsoleColor.DarkMagenta);
            DisplayText(DisplayMessageConstatns.GAME_KEY_SHUFFLE_MANUAL, 70, 18, ConsoleColor.DarkMagenta);
            DisplayText(DisplayMessageConstatns.GAME_KEY_RESTART_MANUAL, 70, 21, ConsoleColor.DarkMagenta);

        }

        public void WriteAt(string text, int left, int top)
        {
            if (left >= 0 && top >= 0)
            {
                Console.SetCursorPosition(left, top);
                Console.Write(text);
            }
        }

        public void WriteAtNextLineSamePosition(string text)
        {

                Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop+2);
                Console.Write(text);
        }

        public void DisplayText(string text, int left, int top, ConsoleColor? foreGroundColor = null, ConsoleColor? backgroundColor=null)
        {
            if (backgroundColor.HasValue)
            {
                Console.BackgroundColor = backgroundColor.Value;
            }
            if (foreGroundColor.HasValue)
            {
                Console.ForegroundColor = foreGroundColor.Value;
            }
            WriteAt(text, left, top);
        }



        public void DisplayFrame()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.DarkCyan;

            Console.WriteLine("                    ¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦");
            Console.WriteLine("                    ¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦");
            Console.WriteLine("                    ¦¦¦¦¦                                                                                                                         ¦¦¦¦¦");
            Console.WriteLine("                    ¦¦¦¦¦                                                                                                                         ¦¦¦¦¦");
            Console.WriteLine("                    ¦¦¦¦¦                                                                                                                         ¦¦¦¦¦");
            Console.WriteLine("                    ¦¦¦¦¦                                                                                                                         ¦¦¦¦¦");
            Console.WriteLine("                    ¦¦¦¦¦                                                                                                                         ¦¦¦¦¦");
            Console.WriteLine("                    ¦¦¦¦¦                                                                                                                         ¦¦¦¦¦");
            Console.WriteLine("                    ¦¦¦¦¦                                                                                                                         ¦¦¦¦¦");
            Console.WriteLine("                    ¦¦¦¦¦                                                                                                                         ¦¦¦¦¦");
            Console.WriteLine("                    ¦¦¦¦¦                                                                                                                         ¦¦¦¦¦");
            Console.WriteLine("                    ¦¦¦¦¦                                                                                                                         ¦¦¦¦¦");
            Console.WriteLine("                    ¦¦¦¦¦                                                                                                                         ¦¦¦¦¦");
            Console.WriteLine("                    ¦¦¦¦¦                                                                                                                         ¦¦¦¦¦");
            Console.WriteLine("                    ¦¦¦¦¦                                                                                                                         ¦¦¦¦¦");
            Console.WriteLine("                    ¦¦¦¦¦                                                                                                                         ¦¦¦¦¦");
            Console.WriteLine("                    ¦¦¦¦¦                                                                                                                         ¦¦¦¦¦");
            Console.WriteLine("                    ¦¦¦¦¦                                                                                                                         ¦¦¦¦¦");
            Console.WriteLine("                    ¦¦¦¦¦                                                                                                                         ¦¦¦¦¦");
            Console.WriteLine("                    ¦¦¦¦¦                                                                                                                         ¦¦¦¦¦");
            Console.WriteLine("                    ¦¦¦¦¦                                                                                                                         ¦¦¦¦¦");
            Console.WriteLine("                    ¦¦¦¦¦                                                                                                                         ¦¦¦¦¦");
            Console.WriteLine("                    ¦¦¦¦¦                                                                                                                         ¦¦¦¦¦");
            Console.WriteLine("                    ¦¦¦¦¦                                                                                                                         ¦¦¦¦¦");
            Console.WriteLine("                    ¦¦¦¦¦                                                                                                                         ¦¦¦¦¦");
            Console.WriteLine("                    ¦¦¦¦¦                                                                                                                         ¦¦¦¦¦");
            Console.WriteLine("                    ¦¦¦¦¦                                                                                                                         ¦¦¦¦¦");
            Console.WriteLine("                    ¦¦¦¦¦                                                                                                                         ¦¦¦¦¦");
            Console.WriteLine("                    ¦¦¦¦¦                                                                                                                         ¦¦¦¦¦");
            Console.WriteLine("                    ¦¦¦¦¦                                                                                                                         ¦¦¦¦¦");
            Console.WriteLine("                    ¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦");
            Console.WriteLine("                    ¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦");
        }


        public void DisplayDeck()
        {
            int left = 35;
            int top = 12;
            Console.ForegroundColor = ConsoleColor.Green;
            List<string> deck = GetAsciiForDeck();
            foreach (string design in deck)
            {
                Console.SetCursorPosition(left, top);
                Console.Write(design);
                top++;
            }
        }

        public void DisplayCardFace(CardDesignPropertiesModel cardProp)
        {
            int left = 100;
            int top = 12;
            foreach (string design in cardProp.CardPattern)
            {
                Console.SetCursorPosition(left, top);
                Console.Write(design);
                top++;
            }
            Console.SetCursorPosition(Console.CursorLeft - cardProp.CardWidth,top++);
        }

        public List<string> GetAsciiForDeck()
        {
            List<string> deck = new List<string>();
            deck.Add("┌─────────────────────────┐");
            deck.Add("│                         │");
            deck.Add("│                         │");
            deck.Add("│                         │");
            deck.Add("│                         │");
            deck.Add("│                         │");
            deck.Add("│                         │");
            deck.Add("│                         │");
            deck.Add("│                         │");
            deck.Add("│                         │");
            deck.Add("│                         │");
            deck.Add("│                         │");
            deck.Add("│                         │");
            deck.Add("│                         │");
            deck.Add("│                         │");
            deck.Add("│                         │");
            deck.Add("│                         │");
            deck.Add("│                         │");
            deck.Add("│                         │");
            deck.Add("│                         │");
            deck.Add("│                         │");
            deck.Add("└─────────────────────────┘");
            return deck;
        }

        public void DisplayTitleBanner()
        {
            Console.Title = "The Card Game";
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(@"                                                     _____   _              ____               _    _____");
            Console.WriteLine(@"                                                    |_  _ | | |            / __ \             | |  |  __ \");
            Console.WriteLine(@"                                                      | |   | |__   ___   | /  \/ __ _ _ __ __| |  | |  \/ __ _ _ __ ___   ___");
            Console.WriteLine(@"                                                      | |   | '_ \ / _ \  | |    / _` | '__/ _` |  | | __ / _` | '_ ` _ \ / _ \");
            Console.WriteLine(@"                                                      | |   | | | |  __/  | \__/\ (_| | | | (_| |  | |_\ \ (_| | | | | | |  __/ ");
            Console.WriteLine(@"                                                      \_/   |_| |_|\___|   \____/\__,_|_|  \__,_|   \____/\__,_|_| |_| |_|\___|");
            Console.WriteLine("\n");
        }

        public void ClearCurentLine(string text)
        {
            int left = Console.CursorLeft;
            Console.CursorLeft = left - text.Length;
            for (int i = 0; i < text.Length; i++)
            {
                Console.Write(" ");
            }
            Console.CursorLeft = Console.CursorLeft - text.Length;
        }
        public void HandleResponse<T>(ResultModel<T> result, bool nextLine, bool clearCurrent, int timeOut) where T : class
        {
            if (result.IsError)
            {
                //Error handling logic
            }
            else
            {
                // Success Handling Logic
                if (nextLine)
                {
                    WriteAtNextLineSamePosition(result.Message);
                }
                else
                {
                    Console.Write(result.Message);   
                }
                if (timeOut > 0)
                {
                    Thread.Sleep(timeOut);
                }
                if (clearCurrent)
                {
                    ClearCurentLine(result.Message);
                }

            }
        }
    }
}
