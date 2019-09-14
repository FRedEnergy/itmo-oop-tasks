using System;

namespace Lab1
{
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
}