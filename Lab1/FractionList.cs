using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security;
using System.Security.Claims;

namespace Lab1
{
    public class FractionList {

        public readonly List<RationalFraction> Fractions = new List<RationalFraction>();

        private RationalFraction _max;
        public RationalFraction Max
        {
            get
            {
                if (_max != null)
                    return _max;
                return _max = Fractions.Max();
            }
        }

        private RationalFraction _min;

        public RationalFraction Min
        {
            get
            {
                if (_min != null)
                    return _min;
                return _min = Fractions.Min();
            }
        }

        public int Count => Fractions.Count;

        public FractionList() { }

        public FractionList(List<RationalFraction> list)
        {
            this.Fractions.AddRange(list);
        }

        public void AddFraction(int m, int n) {
            Fractions.Add(new RationalFraction(m, n));
            ClearCache();
        }

        public void AddFraction(RationalFraction fraction) {
            Fractions.Add(fraction);
            ClearCache();
        }

        public RationalFraction this[int i]
        {
            get => Fractions[i];
            set
            {
                Fractions[i] = value;
                ClearCache();
            }
        }

        public int CountGreater(RationalFraction fraction)
            => Fractions.Count(it => it > fraction);

        public int CounterLower(RationalFraction fraction)
            => Fractions.Count(it => it < fraction);


        public void ClearCache()
        {
            this._min = null;
            this._max = null;
        }

        public static FractionList ReadFromFile(String fileName)
        {
            var list = new FractionList();
            var lines = System.IO.File.ReadAllLines(fileName);
            foreach (var line in lines)
            {
                var trimmed = line.Trim();
                var splitted = trimmed.Split("/");
                list.AddFraction(Int32.Parse(splitted[0].Trim()), Int32.Parse(splitted[1].Trim()));
            }

            return list;
        }
    }
}