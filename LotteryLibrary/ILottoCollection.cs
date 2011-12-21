namespace LotteryLibrary
{
    using System.Collections.Generic;

    public interface ILottoCollection
    {
        IEnumerable<ILottoDraw> AllDraws { get; }

        void LoadAllDraws(string fileName);

        bool WinsJackpot(IEnumerable<short> numbers);

        bool WinsAnything(IEnumerable<short> numbers);

        IEnumerable<LottoWin> GetWins(IEnumerable<short> numbers);
    }
}
