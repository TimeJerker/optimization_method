using System;
using MathUtils;
using System.Collections.Generic;

namespace OptimizationMethodss
{
    public enum OptimizationMethodType
    {
        Bisect,
        GoldenRatio,
        Fibonacci,
        PerCoordinateDescend
    }

    public struct SearchResult
    {
        public readonly OptimizationMethodType Method;
        public readonly long Iterations;
        public readonly long FunctionCalls;
        public readonly double Accuracy;
        public readonly DoubleVector Result;

        public SearchResult(OptimizationMethodType method, long iters, long calls, double acc, DoubleVector res)
        {
            Method = method;
            Iterations = iters;
            FunctionCalls = calls;
            Accuracy = acc;
            Result = res;
        }

        public override string ToString()
        {
            string resStr = Result != null ? Result.ToString() : "null";
            return $"Method: {Method}\n" +
                   $"Iterations:     {Iterations}\n" +
                   $"Function Calls: {FunctionCalls}\n" +
                   $"Accuracy:       {Accuracy:E4}\n" +
                   $"Result:         {resStr}\n";
        }
    }

    public static class OptimizationMethods
    {
        // Дихотомия (Bisection)
        public static SearchResult Bisect(FunctionND func, DoubleVector left, DoubleVector right, double eps = 1e-6, int maxIterations = 1000)
        {
            DoubleVector dir = DoubleVector.Direction(left, right) * eps;
            DoubleVector lhs = new DoubleVector(left);
            DoubleVector rhs = new DoubleVector(right);
            DoubleVector result = new DoubleVector(lhs.Count);

            long iterations = 0;
            long functionCalls = 0;

            while (iterations < maxIterations && DoubleVector.Distance(lhs, rhs) > 2 * eps)
            {
                result = (lhs + rhs) * 0.5;
                
                double f_minus = func(result - dir);
                double f_plus = func(result + dir);
                functionCalls += 2;

                if (f_minus > f_plus)
                    lhs = result;
                else
                    rhs = result;

                iterations++;
            }

            if (iterations > 0) result = (lhs + rhs) * 0.5;
            else result = (lhs + rhs) * 0.5;

            return new SearchResult(
                OptimizationMethodType.Bisect,
                iterations,
                functionCalls,
                DoubleVector.Distance(lhs, rhs),
                result
            );
        }

        // Золотое сечение (Golden Ratio)
        public static SearchResult GoldenRatio(FunctionND func, DoubleVector left, DoubleVector right, double eps = 1e-6, int maxIterations = 1000)
        {
            DoubleVector lhs = new DoubleVector(left);
            DoubleVector rhs = new DoubleVector(right);

            double PSI = NumericCommon.PSI;

            DoubleVector x_l = rhs - (rhs - lhs) * PSI;
            DoubleVector x_r = lhs + (rhs - lhs) * PSI;

            double f_l = func(x_l);
            double f_r = func(x_r);

            long iterations = 0;
            long functionCalls = 2;

            while (iterations < maxIterations && DoubleVector.Distance(lhs, rhs) > 2 * eps)
            {
                iterations++;
                functionCalls++;

                if (f_l > f_r)
                {
                    lhs = x_l;
                    x_l = x_r;
                    f_l = f_r;
                    x_r = lhs + (rhs - lhs) * PSI;
                    f_r = func(x_r);
                }
                else
                {
                    rhs = x_r;
                    x_r = x_l;
                    f_r = f_l;
                    x_l = rhs - (rhs - lhs) * PSI;
                    f_l = func(x_l);
                }
            }

            return new SearchResult(
                OptimizationMethodType.GoldenRatio,
                iterations,
                functionCalls,
                DoubleVector.Distance(lhs, rhs),
                (lhs + rhs) * 0.5
            );
        }

        // Метод Фибоначчи (Fibonacci)
        public static SearchResult Fibonacci(FunctionND func, DoubleVector left, DoubleVector right, double eps = 1e-6)
        {
            DoubleVector lhs = new DoubleVector(left);
            DoubleVector rhs = new DoubleVector(right);

            double condition = DoubleVector.Distance(lhs, rhs) / eps;
            double fib_t = 0.0, fib_1 = 1.0, fib_2 = 1.0;
            long iterations = 0;

            while (fib_2 < condition)
            {
                fib_t = fib_1;
                fib_1 = fib_2;
                fib_2 += fib_t;
                iterations++;
            }

            long functionCalls = iterations + 2; 

            DoubleVector x_l = lhs + (rhs - lhs) * ((fib_2 - fib_1) / fib_2);
            DoubleVector x_r = lhs + (rhs - lhs) * (fib_1 / fib_2);

            double f_l = func(x_l);
            double f_r = func(x_r);
            
            long currentIter = 0;
            for (long ind_jex = iterations; ind_jex > 0; ind_jex--)
            {
                currentIter++;
                fib_t = fib_2 - fib_1;
                fib_2 = fib_1;
                fib_1 = fib_t;

                if (f_l > f_r)
                {
                    lhs = x_l;
                    f_l = f_r;
                    x_l = x_r;
                    x_r = lhs + (rhs - lhs) * (fib_1 / fib_2);
                    f_r = func(x_r);
                }
                else
                {
                    rhs = x_r;
                    x_r = x_l;
                    f_r = f_l;
                    x_l = lhs + (rhs - lhs) * ((fib_2 - fib_1) / fib_2);
                    f_l = func(x_l);
                }
            }
            
            
            return new SearchResult(
                OptimizationMethodType.Fibonacci,
                iterations, 
                functionCalls + iterations,
                DoubleVector.Distance(lhs, rhs),
                (lhs + rhs) * 0.5
            );
        }

        // Покоординатный спуск (Per Coordinate Descent)
        public static SearchResult PerCoordinateDescend(FunctionND func, DoubleVector x_start, double eps = 1e-6, int maxIterations = 1000)
        {
            long totalProbes = 0;
            double step = 1.0;
            int optCoordN = 0;
            long iterations = 0;
            
            DoubleVector x_curr = new DoubleVector(x_start);
            DoubleVector x_next = new DoubleVector(x_start);
            double accuracy = double.PositiveInfinity;

            for (iterations = 0; iterations < maxIterations; ++iterations)
            {
                int coordId = (int)(iterations % x_curr.Count);
                double originalValue = x_curr[coordId];

                x_curr[coordId] -= eps;
                double f_left = func(x_curr);
                
                x_curr[coordId] += 2 * eps;
                double f_right = func(x_curr);
                
                x_curr[coordId] = originalValue;
                totalProbes += 2;

                double searchDirection = (f_left > f_right) ? step : -step;
                x_next[coordId] = x_curr[coordId] + searchDirection;

                SearchResult lineSearchResult = Fibonacci(func, x_curr, x_next, eps);
                
                if (lineSearchResult.Accuracy < accuracy) accuracy = lineSearchResult.Accuracy;
                totalProbes += lineSearchResult.FunctionCalls;

                DoubleVector x_prev = new DoubleVector(x_curr);
                x_curr = lineSearchResult.Result;
                x_next = new DoubleVector(x_curr);

                if (DoubleVector.Distance(x_curr, x_prev) < 2 * eps)
                {
                    optCoordN++;
                    if (optCoordN == x_curr.Count)
                        break;
                }
                else
                {
                    optCoordN = 0;
                }
            }

            return new SearchResult(
                OptimizationMethodType.PerCoordinateDescend,
                iterations,
                totalProbes,
                accuracy,
                x_curr
            );
        }
    }
}