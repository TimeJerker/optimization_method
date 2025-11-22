using System;
using MathUtils;
using OptimizationMethods;

namespace OptimizationTest
{
    class Program
    {
        static void Main(string[] args)
        {
            FunctionND my_function = (DoubleVector x) => (x[0] - 5) * x[0] + (x[1] - 3) * x[1];
            DoubleVector left = new DoubleVector(0, 0);
            DoubleVector right = new DoubleVector(5, 3);
            double eps = 1e-6;
            int max_iters = 100;

            Console.WriteLine($"Функция: f(x,y) = (x - 5)*x + (y - 3)*y");
            Console.WriteLine($"Интервал: [{left}] - [{right}]");
            Console.WriteLine($"Точность: {eps}");
            Console.WriteLine($"Максимум итераций: {max_iters}\n");

            TestBisection(my_function, left, right, eps, max_iters);

            TestGoldenRatio(my_function, left, right, eps, max_iters);

            TestFibonacci(my_function, left, right, eps, max_iters);

            TestPerCoordDescend(my_function, left, eps, max_iters);
        }

        static void TestBisection(FunctionND func, DoubleVector left, DoubleVector right, double eps, int maxIters)
        {
            try
            {
                var bisectParam = new MultiDimensional.BisectionParam(left, right, eps, maxIters);
                var result = new MultiDimensional.SearchResult(MultiDimensional.MethodType.bisect, 0, eps, left);
                
                var optimizationResult = MultiDimensional.FindRootByBisection(ref result, func, ref bisectParam);
                
                Console.WriteLine(result.GetAllValuesAsString());
                Console.WriteLine($"Ожидаемый минимум: (2.5, 1.5)\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}\n");
            }
        }

        static void TestGoldenRatio(FunctionND func, DoubleVector left, DoubleVector right, double eps, int maxIters)
        {
            try
            {
                var goldenParam = new MultiDimensional.GoldenRatioParam(left, right, eps, maxIters);
                var result = new MultiDimensional.SearchResult(MultiDimensional.MethodType.golden_ratio, 0, eps, left);
                
                var optimizationResult = MultiDimensional.FindRootByGoldenRatio(ref result, func, ref goldenParam);
                
                Console.WriteLine(result.GetAllValuesAsString());
                Console.WriteLine($"Ожидаемый минимум: (2.5, 1.5)\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}\n");
            }
        }

        static void TestFibonacci(FunctionND func, DoubleVector left, DoubleVector right, double eps, int maxIters)
        {   
            try
            {
                var fibonacciParam = new MultiDimensional.FibonacciParam(left, right, eps);
                var result = new MultiDimensional.SearchResult(MultiDimensional.MethodType.fibonacci, 0, eps, left);
                
                var optimizationResult = MultiDimensional.FindRootByFibonacci(ref result, func, ref fibonacciParam);
                
                Console.WriteLine(result.GetAllValuesAsString());
                Console.WriteLine($"Ожидаемый минимум: (2.5, 1.5)\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}\n");
            }
        }

        static void TestPerCoordDescend(FunctionND func, DoubleVector startPoint, double eps, int maxIters)
        {
            try
            {
                var coordParam = new MultiDimensional.PerCoordDescendParam(startPoint, eps, maxIters);
                var result = new MultiDimensional.SearchResult(MultiDimensional.MethodType.per_coord_descend, 0, eps, startPoint);
                
                MultiDimensional.PerCoordDescendSearchResult(ref result, func, ref coordParam);
                
                Console.WriteLine(result.GetAllValuesAsString());
                Console.WriteLine($"Ожидаемый минимум: (2.5, 1.5)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}\n");
            }
        }
    }
}
// using System;

// class Program
// {
//     static void Main()
//     {
//         Function1D func = x => Math.Pow((x-2),2)+1;
//         //Function1D func = x => (x-1)*(x-3);
//         var bisactParam = new OneDimensional.BisactionParam(
//             lhs: 0.0,     
//             rhs: 5.0,       
//             eps: 0.0001,   
//             max_it: 100  
//         );

//         var goldenParam = new OneDimensional.GoldenRatioParam(
//             lhs: 0.0,     
//             rhs: 5.0,       
//             eps: 0.0001,    
//             max_it: 100   
//         );
        
//         var fibonacciParam = new OneDimensional.FibonacciParam(
//             lhs: 0.0,     
//             rhs: 5.0,       
//             eps: 0.0001,   
//             max_it: 100  
//         );

//         OneDimensional.SearchResult searchResult = new OneDimensional.SearchResult();

//         OneDimensional.BisectSerchResult(ref searchResult, func, ref bisactParam);
//         Console.WriteLine("Метод: " + searchResult.Method);
//         Console.WriteLine("Результат: " + searchResult.Result);
//         Console.WriteLine("Точность: " + searchResult.Accuracy);
//         Console.WriteLine("Итерации: " + searchResult.Iteration);
//         Console.WriteLine("Вызовы функции: " + searchResult.FunctionCalls);
//         Console.WriteLine(" ");

//         OneDimensional.GoldenRatioSearchResult(ref searchResult, func, ref goldenParam);
//         Console.WriteLine("Метод: " + searchResult.Method);
//         Console.WriteLine("Результат: " + searchResult.Result);
//         Console.WriteLine("Точность: " + searchResult.Accuracy);
//         Console.WriteLine("Итерации: " + searchResult.Iteration);
//         Console.WriteLine("Вызовы функции: " + searchResult.FunctionCalls);
//         Console.WriteLine(" ");

//         OneDimensional.FibonacciSearchResult(ref searchResult, func, ref fibonacciParam);
//         Console.WriteLine("Метод: " + searchResult.Method);
//         Console.WriteLine("Результат: " + searchResult.Result);
//         Console.WriteLine("Точность: " + searchResult.Accuracy);
//         Console.WriteLine("Итерации: " + searchResult.Iteration);
//         Console.WriteLine("Вызовы функции: " + searchResult.FunctionCalls);
//         Console.WriteLine(" ");
        
//         //Console.WriteLine("x_c: " + fibonacciParam.x_c);
//     }
// }