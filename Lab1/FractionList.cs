using System.Collections.Generic;

namespace Lab1
{
    public class FractionList {

        public readonly List<RationalFraction> Fractions = new List<RationalFraction>();

        public int Count
        {
            get { return Fractions.Count; }
            
        }
        public FractionList() { }

        public FractionList(List<RationalFraction> list)
        {
            this.Fractions.AddRange(list);
        }

        public void AddFraction(int m, int n) {
            Fractions.Add(new RationalFraction(m, n));
        }
        
        public void AddFraction(RationalFraction fraction) {
            Fractions.Add(fraction);
        }
        
        public RationalFraction this[int i]
        {
            get { return Fractions[i]; }
            set { Fractions[i] = value; }
        }
        public RationalFraction GetMaxFraction() {
            RationalFraction max = null;

            foreach (var fraction in Fractions) 
                if (max == null || fraction > max) 
                    max = fraction;

            return max;
        }
        
        public RationalFraction GetMinFraction() {
            RationalFraction min = null;

            foreach (var fraction in Fractions) 
                if (min == null || fraction < min) 
                    min = fraction;

            return min;
        }

        public int CountGreater(RationalFraction fraction) {
            var n = 0;

            foreach (var containedFraction in Fractions) 
                if (containedFraction > fraction)
                    n++;
            
            return n;
        }
        
        public int CounterLower(RationalFraction fraction) {
            var n = 0;

            foreach (var containedFraction in Fractions) 
                if (containedFraction < fraction)
                    n++;
            
            return n;
        }
    }
}