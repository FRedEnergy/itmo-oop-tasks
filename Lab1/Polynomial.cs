using System;
using System.Collections.Generic;

namespace Lab1
{
    public class Polynomial {

        public readonly FractionList Coefs;

        public Polynomial(List<RationalFraction> coefs) {
            this.Coefs = new FractionList(coefs);
        }

        public Polynomial(FractionList coefs)
        {
            this.Coefs = coefs;
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
}