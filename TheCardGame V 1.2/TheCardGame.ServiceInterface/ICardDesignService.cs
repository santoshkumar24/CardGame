using System;
using System.Collections.Generic;
using System.Text;
using TheCardGame.Models;

namespace TheCardGame.ServiceInterface
{
    public interface ICardDesignService
    {
        ResultModel<CardDesignPropertiesModel> GetAsciiOfCard(string suitType, string cardValue);
        void DisplayCard(string cardAscii);
        void DisplayCard(CardDesignPropertiesModel cardProp);
        void DisplayGame();
        void DisplayCardFace(CardDesignPropertiesModel cardProp);
        void WriteAt(string text, int left, int top);
        void WriteAtNextLineSamePosition(string text);
        void ClearCurentLine(string text);
        void HandleResponse<T>(ResultModel<T> result, bool nextLine, bool clearCurrent, int timeOut) where T : class;
    }
}
