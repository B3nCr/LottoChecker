namespace LotteryLibrary
{
    using System;

    public interface ILottoDraw
    {
        DateTime DrawDate { get; }

        Int16[] MainNumbers { get; }

        Int16 BonusBall { get; }
    }
}
