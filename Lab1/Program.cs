using System;
using System.Collections.Generic;

namespace Lab1 {
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

            Console.WriteLine("Max: " + fractList.Max);
            Console.WriteLine("Min: " + fractList.Min);

            Console.WriteLine("Fractions > 9 / 3: " + fractList.CountGreater(new RationalFraction(9, 3)));
            Console.WriteLine("Fractions < 7 / 8: " + fractList.CounterLower(new RationalFraction(7, 8)));

            var polynomial = new Polynomial(fractList);
            var polynomialSum = new Polynomial(new List<RationalFraction>(new[]{new RationalFraction(10, 2)})) + polynomial;

            Console.WriteLine("Value of polynomial sum at x = 10: " + polynomialSum.Evaluate(10.0));

            var fractListFromFile =
                FractionList.ReadFromFile("./../../../input.txt");
            Console.WriteLine("Read from file: ");
            foreach (var fract in fractListFromFile.Fractions)
                Console.WriteLine(fract);
        }
    }

}