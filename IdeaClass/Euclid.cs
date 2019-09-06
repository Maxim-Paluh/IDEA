namespace IdeaClass
{
   public class Euclid
    {
        public struct ExtendedEuclideanResult { public int X; public int Y; public int Gcd; }
        public static ExtendedEuclideanResult ExtendedEuclide(int a, int b)
        {
            int x = 1;
            int d = a;
            int v1 = 0;
            int v3 = b;
            while (v3 > 0)
            {
                int q0 = d / v3;
                int q1 = d % v3;
                int tmp = v1 * q0;
                int tn = x - tmp;
                x = v1;
                v1 = tn;
                d = v3;
                v3 = q1;
            }
            int tmp2 = x * (a);
            tmp2 = d - (tmp2);
            int res = tmp2 / (b);
            var result = new ExtendedEuclideanResult() { X = x, Y = res, Gcd = d };
            return result;
        }
    }
}
