using System;
using System.Runtime.CompilerServices;

public delegate double Function1D(double x);

public static class OneDimensional
{

    public enum MethodType
    {
        bisect,
        golden_ratio,
        fibonacci
    };

    public struct BisactionParam
    {
        public double LeftBound;
        public double RightBound;
        public double Epsilon;
        public long MaxIteration;

        public double x_c => (LeftBound + RightBound) * 0.5;

        public BisactionParam(double lhs, double rhs, double eps, long max_it)
        {
            LeftBound = lhs;
            RightBound = rhs;
            Epsilon = eps;
            MaxIteration = max_it;
        }
    }

    public struct GoldenRatioParam
    {
        public double LeftBound;
        public double RightBound;
        public double Epsilon;
        public long MaxIteration;

        public double x_r;
        public double x_l;
        public int iteration;

        public double x_c => (LeftBound + RightBound) * 0.5;

        public const double Phi = 1.61803398874989484820;
        public const double Psi = 0.61803398874989484820;

        public GoldenRatioParam(double lhs, double rhs, double eps, long max_it)
        {
            LeftBound = lhs;
            RightBound = rhs;
            Epsilon = eps;
            MaxIteration = max_it;
            x_r = LeftBound + Psi * (RightBound - LeftBound);
            x_l = RightBound - Psi * (RightBound - LeftBound);
            iteration = 0;
        }
    }

    public struct FibonacciParam
    {
        public double LeftBound;
        public double RightBound;
        public double Epsilon;
        public long MaxIteration;
        public double fib_t;
        public double fib_1;
        public double fib_2;
        public int iteration;

        public double condition => (RightBound - LeftBound) / Epsilon;

        public FibonacciParam(double lhs, double rhs, double eps, long max_it)
        {
            LeftBound = lhs;
            RightBound = rhs;
            Epsilon = eps;
            MaxIteration = max_it;
            fib_t = 0.0;
            fib_1 = 1.0;
            fib_2 = 1.0;
            iteration = 0;
        }
    }

    public struct SearchResult
    {
        public MethodType Method { get; set; }
        public long FunctionCalls { get; set; }
        public long Iteration { get; set; }
        public double Accuracy { get; set; }
        public double Result { get; set; }
        public SearchResult(MethodType m, long iter, double acc, double ResultParam)
        {
            Method = m;
            Iteration = iter;
            Accuracy = acc;
            Result = ResultParam;
        }
    };


    public static void BisectSerchResult(ref SearchResult ResultParam, Function1D func, ref BisactionParam bisectParam)
    {
        CheckLeftAndRightBound(ref bisectParam);

        FindRootByBisection(ref ResultParam, func, ref bisectParam);

        NoteBisectSearchResult(ref ResultParam, bisectParam);
    }

    public static void CheckLeftAndRightBound(ref BisactionParam bisectParam)
    {
        if (bisectParam.LeftBound > bisectParam.RightBound)
        {
            swap(ref bisectParam.LeftBound, ref bisectParam.RightBound);
        }
    }

    public static void swap<TypeT>(ref TypeT x, ref TypeT y)
    {
        TypeT temp = x;
        x = y;
        y = temp;
    }

    public static void FindRootByBisection(ref SearchResult ResultParam, Function1D func, ref BisactionParam bisectParam)
    {
        for (long i = 0; Math.Abs(bisectParam.RightBound - bisectParam.LeftBound) > 2 * bisectParam.Epsilon && i < bisectParam.MaxIteration; i++)
        {
            ResultParam.FunctionCalls += 2;
            UpdateSearchBounds(func, ref bisectParam);
        }
    }

    public static void UpdateSearchBounds(Function1D func, ref BisactionParam bisectParam)
    {
        double mid = bisectParam.x_c;
        double f_left = func(mid + bisectParam.Epsilon);
        double f_right = func(mid - bisectParam.Epsilon);

        if (f_left < f_right)
            bisectParam.RightBound = mid;
        else
            bisectParam.LeftBound = mid;
    }

    public static void NoteBisectSearchResult(ref SearchResult ResultParam, BisactionParam bisectParam)
    {
        ResultParam.Method = MethodType.bisect;
        ResultParam.Iteration = bisectParam.MaxIteration;
        ResultParam.Accuracy = Math.Abs(bisectParam.LeftBound - bisectParam.RightBound) * 0.5;
        ResultParam.Result = (bisectParam.LeftBound + bisectParam.RightBound) * 0.5;
    }

    public static void GoldenRatioSearchResult(ref SearchResult ResultParam, Function1D func, ref GoldenRatioParam goldenParam)
    {
        CheckLeftAndRightBound(ref goldenParam);

        FindRootByGoldenRatio(ref ResultParam, func, ref goldenParam);

        NoteGoldenRatioSearchResult(ref ResultParam, goldenParam);

    }
    public static void CheckLeftAndRightBound(ref GoldenRatioParam goldenParam)
    {
        if (goldenParam.LeftBound > goldenParam.RightBound)
        {
            swap(ref goldenParam.LeftBound, ref goldenParam.RightBound);
        }
    }

    public static void FindRootByGoldenRatio(ref SearchResult ResultParam, Function1D func, ref GoldenRatioParam goldenParam)
    {

        for (; goldenParam.iteration < goldenParam.MaxIteration && (goldenParam.RightBound - goldenParam.LeftBound) > goldenParam.Epsilon; goldenParam.iteration++)
        {
            CheckLeftAndRightGoldenFunc(ref ResultParam, func, ref goldenParam);
        }
        ResultParam.Iteration = goldenParam.iteration;
    }
    public static void CheckLeftAndRightGoldenFunc(ref SearchResult ResultParam, Function1D func, ref GoldenRatioParam goldenParam)
    {
        double f_l = func(goldenParam.x_l);
        double f_r = func(goldenParam.x_r);

        if (f_l > f_r)
        {
            goldenParam.LeftBound = goldenParam.x_l;
            goldenParam.x_l = goldenParam.x_r;
            //f_l = f_r;
            goldenParam.x_r = goldenParam.LeftBound + (goldenParam.RightBound - goldenParam.LeftBound) * GoldenRatioParam.Psi;
            //f_r = func(goldenParam.x_r);
            ResultParam.FunctionCalls++;
        }
        else
        {
            goldenParam.RightBound = goldenParam.x_r;
            goldenParam.x_r = goldenParam.x_l;
            //f_r = f_l;
            goldenParam.x_l = goldenParam.RightBound - (goldenParam.RightBound - goldenParam.LeftBound) * GoldenRatioParam.Psi;
            //f_l = func(goldenParam.x_l);
            ResultParam.FunctionCalls++;
        }
    }

    public static void NoteGoldenRatioSearchResult(ref SearchResult ResultParam, GoldenRatioParam goldenParam)
    {
        ResultParam.Method = MethodType.golden_ratio;
        ResultParam.Accuracy = Math.Abs(goldenParam.LeftBound - goldenParam.RightBound) * 0.5;
        ResultParam.Result = (goldenParam.LeftBound + goldenParam.RightBound) * 0.5;
    }


    public static void FibonacciSearchResult(ref SearchResult ResultParam, Function1D func, ref FibonacciParam fibonacciParam)
    {
        CheckLeftAndRightBound(ref fibonacciParam);

        while (fibonacciParam.fib_2 < fibonacciParam.condition)
        {
            fibonacciParam.iteration++;
            fibonacciParam.fib_t = fibonacciParam.fib_1;
            fibonacciParam.fib_1 = fibonacciParam.fib_2;
            fibonacciParam.fib_2 = fibonacciParam.fib_t + fibonacciParam.fib_1;
        }
        double x_l = fibonacciParam.LeftBound + (fibonacciParam.RightBound - fibonacciParam.LeftBound) * ((fibonacciParam.fib_2 - fibonacciParam.fib_1) / fibonacciParam.fib_2);
        double x_r = fibonacciParam.LeftBound + (fibonacciParam.RightBound - fibonacciParam.LeftBound) * (fibonacciParam.fib_1 / fibonacciParam.fib_2);
        double f_l = func(x_l);
        double f_r = func(x_r);

        for (int index = fibonacciParam.iteration; index > 0; index--)
        {
            fibonacciParam.fib_t = fibonacciParam.fib_2 - fibonacciParam.fib_1;
            fibonacciParam.fib_2 = fibonacciParam.fib_1;
            fibonacciParam.fib_1 = fibonacciParam.fib_t;

            if (f_l > f_r)
            {
                fibonacciParam.LeftBound = x_l;
                f_l = f_r;
                x_l = x_r;
                x_r = fibonacciParam.LeftBound + (fibonacciParam.RightBound - fibonacciParam.LeftBound) * (fibonacciParam.fib_1 / fibonacciParam.fib_2);
                f_r = func(x_r);
            }
            else
            {
                fibonacciParam.RightBound = x_r;
                x_r = x_l;
                f_r = f_l;
                x_l = fibonacciParam.LeftBound + (fibonacciParam.RightBound - fibonacciParam.LeftBound) * ((fibonacciParam.fib_2 - fibonacciParam.fib_1) / fibonacciParam.fib_2);
                f_l = func(x_l);
            }
        }
        ResultParam.Iteration = fibonacciParam.iteration;
        NoteFibonacciSearchResult(ref ResultParam, fibonacciParam);

    }
    public static void CheckLeftAndRightBound(ref FibonacciParam fibonacciParam)
    {
        if (fibonacciParam.LeftBound > fibonacciParam.RightBound)
        {
            swap(ref fibonacciParam.LeftBound, ref fibonacciParam.RightBound);
        }
    }
    public static void NoteFibonacciSearchResult(ref SearchResult ResultParam, FibonacciParam fibonacciParam)
    {
        ResultParam.Method = MethodType.golden_ratio;
        ResultParam.Accuracy = Math.Abs(fibonacciParam.LeftBound - fibonacciParam.RightBound) * 0.5;
        ResultParam.Result = (fibonacciParam.LeftBound + fibonacciParam.RightBound) * 0.5;
    }
}
