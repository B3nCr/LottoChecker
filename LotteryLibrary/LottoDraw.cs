namespace LotteryLibrary
{
    using System;

    public class LottoDraw : ILottoDraw
    {
        public LottoDraw()
        {
            MainNumbers = new short[6];
        }

        public DateTime DrawDate { get; set; }

        public short[] MainNumbers { get; set; }

        public short BonusBall { get; set; }
    }
}