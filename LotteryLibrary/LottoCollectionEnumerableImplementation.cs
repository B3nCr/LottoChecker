namespace LotteryLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    public class LottoCollectionEnumerableImplementation : ILottoCollection
    {
        #region Constants and Fields

        private bool initialised;

        #endregion

        #region Public Properties

        public IEnumerable<ILottoDraw> AllDraws { get; private set; }

        #endregion

        #region Public Methods

        public void LoadAllDraws(string fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException("File not found", fileName);
            }

            var list = new List<LottoDraw>();

            var skipFirstLine = true;
            foreach (var line in File.ReadLines(fileName))
            {
                if (skipFirstLine)
                {
                    skipFirstLine = false;
                    continue;
                }

                //0     1   2  3   4     5  6  7  8  9  10 11     12        13      14        15
                //No.       Date         Winning Numbers          Jackpot   Wins    Machine   Set
                //1668  Sat 17 Dec 2011  01 22 35 39 42 48 (12)   4,672,310     1    Merlin      5 
                var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                var draw = new LottoDraw();

                // get main numbers
                for (var i = 0; i < 6; i++)
                {
                    var ball = short.Parse(parts[i + 5]);
                    draw.MainNumbers[i] = ball;
                }

                //get bonus ball
                draw.BonusBall = short.Parse(parts[11].Substring(1, 2));

                // get draw date, force UK culture as it's UK data
                var drawDate = DateTime.Parse(string.Format("{0} {1} {2}", parts[2], parts[3], parts[4]), CultureInfo.CreateSpecificCulture("en-gb"));
                draw.DrawDate = drawDate;

                list.Add(draw);
            }

            AllDraws = list;
            initialised = true;
        }

        public bool WinsAnything(IEnumerable<short> numbers)
        {
            ValidateNumbers(numbers);

            if (!initialised)
            {
                throw new InvalidOperationException("Draw collection isn't initialised");
            }

            foreach (var n in this.AllDraws.Select(x => x.MainNumbers))
            {
                if (n.Intersect<short>(numbers).Count() >= 3)
                {
                    return true;
                }
            }

            return false;
        }

        public IEnumerable<LottoWin> GetWins(IEnumerable<short> numbers)
        {
            ValidateNumbers(numbers);

            if (!initialised)
            {
                throw new InvalidOperationException("Draw collection isn't initialised");
            }

            var wins = new List<LottoWin>();
            foreach (var n in this.AllDraws.Select(x => x.MainNumbers))
            {
                var intersectCount = n.Intersect(numbers).Count();
                if (intersectCount >= 3)
                {
                    wins.Add(
                        new LottoWin()
                        {
                            MatchingNumbers = (short)intersectCount
                        });
                }
            }
            return wins;
        }

        public bool WinsJackpot(IEnumerable<short> numbers)
        {
            ValidateNumbers(numbers);

            if (!initialised)
            {
                throw new InvalidOperationException("Draw collection isn't initialised");
            }

            return this.AllDraws.Any(draw => draw.MainNumbers.SequenceEqual(numbers));
        }

        #endregion

        #region Methods

        private static void ValidateNumbers(IEnumerable<short> numbers)
        {
            if (numbers.Count() != 6)
            {
                throw new InvalidOperationException("You must specify exactly 6 numbers");
            }

            if (numbers.Distinct().Count() != 6)
            {
                throw new InvalidOperationException("You can't specify the same number twice");
            }

            if (numbers.Where(x => x > 49).Any())
            {
                throw new InvalidOperationException("Numbers must be between 1 and 49, inclusive");
            }

            if (numbers.Where(x => x < 1).Any())
            {
                throw new InvalidOperationException("Numbers must be between 1 and 49, inclusive");
            }
        }

        #endregion
    }
}