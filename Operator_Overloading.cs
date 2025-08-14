using System;

namespace OperatorOverloadingDemo{
    class Threed{
        public int X { get; }
        public int Y { get; }
        public int Z { get; }

        public Threed(int x, int y, int z){
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString(){
            return $"({X}, {Y}, {Z})";
        }

        public static Threed operator +(Threed a, Threed b){
            return new Threed(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Threed operator -(Threed a, Threed b){
            return new Threed(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Threed operator *(Threed a, Threed b){
            return new Threed(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
        }

        public static Threed operator /(Threed a, Threed b){
            return new Threed(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
        }

        public static Threed operator %(Threed a, Threed b){
            return new Threed(a.X % b.X, a.Y % b.Y, a.Z % b.Z);
        }

        public static Threed operator +(Threed a, int n){
            return new Threed(a.X + n, a.Y + n, a.Z + n);
        }

        public static Threed operator +(int n, Threed a){
            return a + n;
        }

        public static Threed operator +(Threed a){
            return new Threed(+a.X, +a.Y, +a.Z);
        }

        public static Threed operator -(Threed a){
            return new Threed(-a.X, -a.Y, -a.Z);
        }

        public static Threed operator ++(Threed a){
            return new Threed(a.X + 1, a.Y + 1, a.Z + 1);
        }

        public static Threed operator --(Threed a){
            return new Threed(a.X - 1, a.Y - 1, a.Z - 1);
        }

        private double Length => Math.Sqrt(X * X + Y * Y + Z * Z);

        public static bool operator >(Threed a, Threed b){
            return a.Length > b.Length;
        }

        public static bool operator <(Threed a, Threed b){
            return a.Length < b.Length;
        }

        public static bool operator ==(Threed a, Threed b){
            return a.Length == b.Length;
        }

        public static bool operator !=(Threed a, Threed b){
            return a.Length != b.Length;
        }

        public override bool Equals(object obj){
            return obj is Threed other && this == other;
        }

        public override int GetHashCode(){
            return HashCode.Combine(X, Y, Z);
        }

        public static bool operator true(Threed a){
            return (a.X != 0 || a.Y != 0 || a.Z != 0);
        }

        public static bool operator false(Threed a){
            return (a.X == 0 && a.Y == 0 && a.Z == 0);
        }
    }

    class Program{
        static void Main(){
            Threed V1 = new Threed(4, 6, 8);
            Threed V2 = new Threed(2, 3, 4);

            Console.WriteLine("v1 = " + V1);
            Console.WriteLine("v2 = " + V2);
            Console.WriteLine("v1 + v2 = " + (V1 + V2));
            Console.WriteLine("v1 - v2 = " + (V1 - V2));
            Console.WriteLine("v1 * v2 = " + (V1 * V2));
            Console.WriteLine("v1 / v2 = " + (V1 / V2));
            Console.WriteLine("v1 % v2 = " + (V1 % V2));
            Console.WriteLine("v1 + 5  = " + (V1 + 5));
            Console.WriteLine("10 + v2 = " + (10 + V2));
            Console.WriteLine("+v1     = " + (+V1));
            Console.WriteLine("-v2     = " + (-V2));
            Console.WriteLine("++v1    = " + (++V1));
            Console.WriteLine("--v2    = " + (--V2));
            Console.WriteLine("v1 > v2  ? " + (V1 > V2));
            Console.WriteLine("v1 < v2  ? " + (V1 < V2));
            Console.WriteLine("v1 == v2 ? " + (V1 == V2));
            Console.WriteLine("v1 != v2 ? " + (V1 != V2));

            Threed zero = new Threed(0, 0, 0);
            if (V1){
                Console.WriteLine("v1 is not zero");
            }
            else{
                Console.WriteLine("v1 is zero");
            }

            if (zero){
                Console.WriteLine("zero is not zero");
            }
            else{
                Console.WriteLine("zero is zero");
            }
            Console.ReadKey();
        }
    }
}
