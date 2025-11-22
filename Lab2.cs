using MathUtils;
using System;

namespace OptimizationMethods
{
    public static class MultiDimensional
    {
        public enum MethodType
        {
            bisect,
            golden_ratio,
            fibonacci,
            per_coord_descend,
            gradient_descend,
            conj_gradient_descend,
            newtone_raphson
        };

        public struct BisectionParam
        {
            public DoubleVector LeftBound;
            public DoubleVector RightBound;
            public double Epsilon;
            public long MaxIteration;
            public int Iteration;

            public DoubleVector x_c => (LeftBound + RightBound) * 0.5;

            public BisectionParam(DoubleVector lhs, DoubleVector rhs, double eps, long max_it)
            {
                LeftBound = new DoubleVector(lhs);
                RightBound = new DoubleVector(rhs);
                Epsilon = eps;
                MaxIteration = max_it;
                Iteration = 0;
            }
            public BisectionParam(DoubleVector lhs, DoubleVector rhs, double eps)
                : this(lhs, rhs, eps, NumericCommon.ITERATIONS_COUNT_HIGH)
            {
            }
            public BisectionParam(DoubleVector lhs, DoubleVector rhs)
                : this(lhs, rhs, NumericCommon.NUMERIC_ACCURACY_MIDDLE, NumericCommon.ITERATIONS_COUNT_HIGH)
            {
            }
        }

        public struct GoldenRatioParam
        {
            public DoubleVector LeftBound;
            public DoubleVector RightBound;
            public double Epsilon;
            public long MaxIteration;
            public int Iteration;

            public DoubleVector x_l;
            public DoubleVector x_r;

            public DoubleVector x_c => (LeftBound + RightBound) * 0.5;

            public GoldenRatioParam(DoubleVector lhs, DoubleVector rhs, double eps, long max_it)
            {
                LeftBound = new DoubleVector(lhs);
                RightBound = new DoubleVector(rhs);
                Epsilon = eps;
                MaxIteration = max_it;
                Iteration = 0;
                
                x_l = RightBound - (RightBound - LeftBound) * NumericCommon.PSI;
                x_r = LeftBound + (RightBound - LeftBound) * NumericCommon.PSI;
            }
            public GoldenRatioParam(DoubleVector lhs, DoubleVector rhs, double eps)
                : this(lhs, rhs, eps, NumericCommon.ITERATIONS_COUNT_HIGH)
            {
            }
            public GoldenRatioParam(DoubleVector lhs, DoubleVector rhs)
                : this(lhs, rhs, NumericCommon.NUMERIC_ACCURACY_MIDDLE, NumericCommon.ITERATIONS_COUNT_HIGH)
            {
            }
        }

        public struct FibonacciParam
        {
            public DoubleVector LeftBound;
            public DoubleVector RightBound;
            public double Epsilon;
            public double fib_t;
            public double fib_1;
            public double fib_2;
            public int Iteration;
            
            public DoubleVector x_c => (LeftBound + RightBound) * 0.5;
            public double condition => DoubleVector.Distance(RightBound, LeftBound) / Epsilon;

            public FibonacciParam(DoubleVector lhs, DoubleVector rhs, double eps)
            {
                LeftBound = new DoubleVector(lhs);
                RightBound = new DoubleVector(rhs);
                Epsilon = eps;
                fib_t = 0.0;
                fib_1 = 1.0;
                fib_2 = 1.0;
                Iteration = 0;
            }
            public FibonacciParam(DoubleVector lhs, DoubleVector rhs)
                : this(lhs, rhs, NumericCommon.NUMERIC_ACCURACY_MIDDLE)
            {
            }
        }

        public struct PerCoordDescendParam
        {
            public DoubleVector StartPoint;
            public double Epsilon;
            public long MaxIteration;
            public int Iteration;
            public int OptimizedCoordinatesCount;

            public PerCoordDescendParam(DoubleVector start, double eps, long max_it)
            {
                StartPoint = new DoubleVector(start);
                Epsilon = eps;
                MaxIteration = max_it;
                Iteration = 0;
                OptimizedCoordinatesCount = 0;
            }
        }

        public struct SearchResult
        {
            public MethodType Method;
            public long FunctionCalls;
            public long Iteration;
            public double Accuracy;
            public DoubleVector Result;

            public SearchResult(MethodType m, long iter, double acc, DoubleVector resultParam)
            {
                Method = m;
                Iteration = iter;
                Accuracy = acc;
                Result = new DoubleVector(resultParam);
            }
            public string GetAllValuesAsString()
            {
                return $"Method: {Method}\n" +
                    $"Function Calls: {FunctionCalls}\n" +
                    $"Iteration: {Iteration}\n" +
                    $"Accuracy: {Accuracy}\n" +
                    $"Result: {Result}";
            }
        };

        public static DoubleVector FindRootByBisection(ref SearchResult resultParam, FunctionND func, ref BisectionParam bisectParam)
        {
            DoubleVector direction = DoubleVector.Direction(bisectParam.LeftBound, bisectParam.RightBound) * bisectParam.Epsilon;
            
            for (; bisectParam.Iteration < bisectParam.MaxIteration && 
                   DoubleVector.Distance(bisectParam.RightBound, bisectParam.LeftBound) > 2 * bisectParam.Epsilon; 
                   bisectParam.Iteration++)
            {
                if (func(bisectParam.x_c + direction) > func(bisectParam.x_c - direction))
                    bisectParam.RightBound = bisectParam.x_c;
                else
                    bisectParam.LeftBound = bisectParam.x_c;
                
                resultParam.FunctionCalls += 2;
            }

            NoteBisectSearchResult(ref resultParam, bisectParam);
            return (bisectParam.LeftBound + bisectParam.RightBound) * 0.5;
        }
        public static void NoteBisectSearchResult(ref SearchResult resultParam, BisectionParam bisectParam)
        {
            resultParam.Method = MethodType.bisect;
            resultParam.Iteration = bisectParam.Iteration;
            resultParam.Accuracy = DoubleVector.Distance(bisectParam.RightBound, bisectParam.LeftBound) * 0.5;
            resultParam.Result = (bisectParam.LeftBound + bisectParam.RightBound) * 0.5;
        }

        public static DoubleVector FindRootByGoldenRatio(ref SearchResult resultParam, FunctionND func, ref GoldenRatioParam goldenParam)
        {
            double f_l = func(goldenParam.x_l);
            double f_r = func(goldenParam.x_r);
            resultParam.FunctionCalls += 2;

            for (; goldenParam.Iteration < goldenParam.MaxIteration && 
                   DoubleVector.Distance(goldenParam.RightBound, goldenParam.LeftBound) > 2 * goldenParam.Epsilon; 
                   goldenParam.Iteration++)
            {
                if (f_l > f_r)
                {
                    goldenParam.LeftBound = goldenParam.x_l;
                    goldenParam.x_l = goldenParam.x_r;
                    f_l = f_r;
                    goldenParam.x_r = goldenParam.LeftBound + (goldenParam.RightBound - goldenParam.LeftBound) * NumericCommon.PSI;
                    f_r = func(goldenParam.x_r);
                }
                else
                {
                    goldenParam.RightBound = goldenParam.x_r;
                    goldenParam.x_r = goldenParam.x_l;
                    f_r = f_l;
                    goldenParam.x_l = goldenParam.RightBound - (goldenParam.RightBound - goldenParam.LeftBound) * NumericCommon.PSI;
                    f_l = func(goldenParam.x_l);
                }
                resultParam.FunctionCalls += 1;
            }
            NoteGoldenRatioSearchResult(ref resultParam, goldenParam);
            return (goldenParam.LeftBound + goldenParam.RightBound) * 0.5;
        }
        public static void NoteGoldenRatioSearchResult(ref SearchResult resultParam, GoldenRatioParam goldenParam)
        {
            resultParam.Method = MethodType.golden_ratio;
            resultParam.Iteration = goldenParam.Iteration;
            resultParam.Accuracy = DoubleVector.Distance(goldenParam.RightBound, goldenParam.LeftBound) * 0.5;
            resultParam.Result = (goldenParam.LeftBound + goldenParam.RightBound) * 0.5;
        }

        public static DoubleVector FindRootByFibonacci(ref SearchResult resultParam, FunctionND func, ref FibonacciParam fibonacciParam)
        {
            while (fibonacciParam.fib_2 < fibonacciParam.condition)
            {
                fibonacciParam.fib_t = fibonacciParam.fib_1;
                fibonacciParam.fib_1 = fibonacciParam.fib_2;
                fibonacciParam.fib_2 += fibonacciParam.fib_t;
                fibonacciParam.Iteration++;
            }
            
            DoubleVector x_l = fibonacciParam.LeftBound + (fibonacciParam.RightBound - fibonacciParam.LeftBound) * 
                            ((fibonacciParam.fib_2 - fibonacciParam.fib_1) / fibonacciParam.fib_2);
            DoubleVector x_r = fibonacciParam.LeftBound + (fibonacciParam.RightBound - fibonacciParam.LeftBound) * 
                            (fibonacciParam.fib_1 / fibonacciParam.fib_2);

            double f_l = func(x_l);
            double f_r = func(x_r);
            resultParam.FunctionCalls += 2;
            
            for (int i = fibonacciParam.Iteration; i > 0; i--)
            {
                fibonacciParam.fib_t = fibonacciParam.fib_2 - fibonacciParam.fib_1;
                fibonacciParam.fib_2 = fibonacciParam.fib_1;
                fibonacciParam.fib_1 = fibonacciParam.fib_t;

                if (f_l > f_r)
                {
                    fibonacciParam.LeftBound = x_l;
                    f_l = f_r;
                    x_l = x_r;
                    x_r = fibonacciParam.LeftBound + (fibonacciParam.RightBound - fibonacciParam.LeftBound) * 
                        (fibonacciParam.fib_1 / fibonacciParam.fib_2);
                    f_r = func(x_r);
                }
                else
                {
                    fibonacciParam.RightBound = x_r;
                    x_r = x_l;
                    f_r = f_l;
                    x_l = fibonacciParam.LeftBound + (fibonacciParam.RightBound - fibonacciParam.LeftBound) * 
                        ((fibonacciParam.fib_2 - fibonacciParam.fib_1) / fibonacciParam.fib_2);
                    f_l = func(x_l);
                }
                resultParam.FunctionCalls += 1;
                fibonacciParam.Iteration++;
            }
            NoteFibonacciSearchResult(ref resultParam, fibonacciParam);
            return (fibonacciParam.LeftBound + fibonacciParam.RightBound) * 0.5;
        }

        public static void NoteFibonacciSearchResult(ref SearchResult resultParam, FibonacciParam fibonacciParam)
        {
            resultParam.Method = MethodType.fibonacci;
            resultParam.Iteration = fibonacciParam.Iteration;
            resultParam.Accuracy = DoubleVector.Distance(fibonacciParam.RightBound, fibonacciParam.LeftBound) * 0.5;
            resultParam.Result = (fibonacciParam.LeftBound + fibonacciParam.RightBound) * 0.5;
        }
        public static void PerCoordDescendSearchResult(ref SearchResult resultParam, FunctionND func, ref PerCoordDescendParam coordParam)
        {
            FindMinimumByPerCoordDescend(ref resultParam, func, ref coordParam);
            NotePerCoordDescendSearchResult(ref resultParam, coordParam);
        }

        public static void FindMinimumByPerCoordDescend(ref SearchResult resultParam, FunctionND func, ref PerCoordDescendParam coordParam)
        {
            DoubleVector x_curr = new (coordParam.StartPoint);
            DoubleVector x_next = new (coordParam.StartPoint);
            double step = 1.0;
            double accuracy = double.PositiveInfinity;
            int opt_coord_n = 0;

            for (; coordParam.Iteration < coordParam.MaxIteration; coordParam.Iteration++)
            {
                int coord_id = coordParam.Iteration % x_curr.Count;
                double original_value = x_curr[coord_id];

                x_curr[coord_id] -= coordParam.Epsilon;
                double f_left = func(x_curr);
                x_curr[coord_id] += 2 * coordParam.Epsilon;
                double f_right = func(x_curr);
                x_curr[coord_id] = original_value;
                resultParam.FunctionCalls += 2;

                double search_direction = f_left > f_right ? step : -step;
                x_next[coord_id] = x_curr[coord_id] + search_direction;

                var fibonacciParam = new FibonacciParam(x_curr, x_next, coordParam.Epsilon);
                var lineSearchResult = new SearchResult(MethodType.fibonacci, 0, coordParam.Epsilon, x_curr);
                
                DoubleVector line_search_result = FindRootByFibonacci(ref lineSearchResult, func, ref fibonacciParam);
                accuracy = Math.Min(accuracy, lineSearchResult.Accuracy);
                resultParam.FunctionCalls += lineSearchResult.FunctionCalls;

                DoubleVector x_prev = new DoubleVector(x_curr);
                x_curr = new DoubleVector(line_search_result);
                x_next = new DoubleVector(x_curr);

                if (DoubleVector.Distance(x_curr, x_prev) < 2 * coordParam.Epsilon)
                {
                    opt_coord_n++;
                    if (opt_coord_n == x_curr.Count)
                        break;
                }
                else
                {
                    opt_coord_n = 0;
                }
            }

            coordParam.StartPoint = new DoubleVector(x_curr);
            coordParam.OptimizedCoordinatesCount = opt_coord_n;
            resultParam.Accuracy = accuracy;
            NotePerCoordDescendSearchResult(ref resultParam, coordParam);
        }

        public static void NotePerCoordDescendSearchResult(ref SearchResult resultParam, PerCoordDescendParam coordParam)
        {
            resultParam.Method = MethodType.per_coord_descend;
            resultParam.Iteration = coordParam.Iteration;
            resultParam.Result = new DoubleVector(coordParam.StartPoint);
        }

    }
}