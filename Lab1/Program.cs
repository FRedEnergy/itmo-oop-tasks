using System;
using System.Collections.Generic;

namespace Lab1 {
    
    public class RationalFraction : IComparable<RationalFraction> {

        public readonly int m, n;

        public RationalFraction(int m, int n) {
            this.m = m;
            this.n = n;
        }

        public static bool operator <(RationalFraction a, RationalFraction b) {
            return a.CompareTo(b) < 0;
        }

        public static bool operator >(RationalFraction a, RationalFraction b) {
            return a.CompareTo(b) > 0;
        }
        
        public static RationalFraction operator +(RationalFraction a, RationalFraction b) {
            return new RationalFraction(a.m * b.n + a.n * b.m, a.n * b.n); //a/b + c/d = (ad + bc) / bd
        }
        
        public int CompareTo(RationalFraction other) {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            
            var lhs = m * other.n;
            var rhs = n * other.m;
            if (lhs < rhs) return -1;
            return lhs > rhs ? 1 : 0;
        }

        public bool Equals(RationalFraction other) {
            return m == other.m && n == other.n;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RationalFraction) obj);
        }

        public override int GetHashCode() {
            return (m * 397) ^ n;
        }

        public override string ToString() {
            return $"({m} / {n})";
        }
    }

    public class FractionList {

        public readonly List<RationalFraction> Fractions = new List<RationalFraction>();

        public void AddFraction(int m, int n) {
            Fractions.Add(new RationalFraction(m, n));
        }
        
        public void AddFraction(RationalFraction fraction) {
            Fractions.Add(fraction);
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

    public class Polynomial {

        public readonly List<RationalFraction> Coefs;

        public Polynomial(List<RationalFraction> coefs) {
            this.Coefs = coefs;
        }

        public Polynomial(FractionList coefs) {
            this.Coefs = new List<RationalFraction>(coefs.Fractions);
        }

        public double Evaluate(double x) {
            var k = x;
            var result = 0.0;

            for (var i = Coefs.Count - 1; i >= 0; i--) {
                var fract = Coefs[i];
                result += fract.m * k / fract.n;
                k *= x;
            }
            
            return result;
        }
        
        public static Polynomial operator +(Polynomial a, Polynomial b) {
            var num = Math.Max(a.Coefs.Count, b.Coefs.Count);

            var nextCoefs = new List<RationalFraction>(num);

            for (var i = 0; i < num; i++) {
                var coefA = a.Coefs.Count - i - 1 >= 0 ? a.Coefs[a.Coefs.Count - i - 1] : null;
                var coefB = b.Coefs.Count - i - 1 >= 0 ? b.Coefs[b.Coefs.Count - i - 1] : null;
                
                if(coefA != null && coefB != null)
                    nextCoefs.Add(coefA + coefB);
                else if(coefA != null)
                    nextCoefs.Add(coefA);
                else 
                    nextCoefs.Add(coefB);
            }

            nextCoefs.Reverse();
            
            return new Polynomial(nextCoefs);
        }
    }
    
    class Program {
        static void Main(string[] args) {
            var fractA = new RationalFraction(3, 5);
            Console.WriteLine("fractA.m = " + fractA.m + ", fractA.n = " + fractA.n);
            
            var fractList = new FractionList();
            fractList.AddFraction(22, 7);
            fractList.AddFraction(fractA);
            
            Console.WriteLine("Factions: ");
            foreach (var fract in fractList.Fractions)
                Console.WriteLine(fract);
            
            Console.WriteLine("Max: " + fractList.GetMaxFraction());
            Console.WriteLine("Min: " + fractList.GetMinFraction());
            
            Console.WriteLine("Fractions > 9 / 3: " + fractList.CountGreater(new RationalFraction(9, 3)));
            Console.WriteLine("Fractions < 7 / 8: " + fractList.CounterLower(new RationalFraction(7, 8)));

            var polynomial = new Polynomial(fractList);
            var polynomialSum = new Polynomial(new List<RationalFraction>(new[]{new RationalFraction(10, 2)})) + polynomial;
            
            Console.WriteLine("Value of polynomial sum at x = 10: " + polynomialSum.Evaluate(10.0));
        }
    }
    
}