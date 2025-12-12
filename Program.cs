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

            //TestBisection(my_function, left, right, eps, max_iters);
            //TestGoldenRatio(my_function, left, right, eps, max_iters);
            //TestFibonacci(my_function, left, right, eps, max_iters);
            //TestPerCoordDescend(my_function, left, eps, max_iters);
            TestGradientDescend(my_function, left, eps, max_iters);
            TestConjGradientDescend(my_function, left, eps, max_iters);
            TestNewtoneRaphson(my_function, left, eps, max_iters);
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
                Console.WriteLine($"Ошибка в Bisection: {ex.Message}\n");
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
                Console.WriteLine($"Ошибка в Golden Ratio: {ex.Message}\n");
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
                Console.WriteLine($"Ошибка в Fibonacci: {ex.Message}\n");
            }
        }

        static void TestPerCoordDescend(FunctionND func, DoubleVector startPoint, double eps, int maxIters)
        {
            try
            {
                var coordParam = new MultiDimensional.PerCoordDescendParam(startPoint, eps, maxIters);
                var result = new MultiDimensional.SearchResult(MultiDimensional.MethodType.per_coord_descend, 0, eps, startPoint);
                
                MultiDimensional.FindMinimumByPerCoordDescend(ref result, func, ref coordParam);
                
                Console.WriteLine(result.GetAllValuesAsString());
                Console.WriteLine($"Ожидаемый минимум: (2.5, 1.5)\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка в PerCoordDescend: {ex.Message}\n");
            }
        }
        static void TestGradientDescend(FunctionND func, DoubleVector startPoint, double eps, int maxIters)
        {
            try
            {
                var coordParam = new MultiDimensional.GradientDescendParam(startPoint, eps, maxIters);
                var result = new MultiDimensional.SearchResult(MultiDimensional.MethodType.gradient_descend, 0, eps, startPoint);
                
                MultiDimensional.FindMinimumByGradientDescend(ref result, func, ref coordParam);
                
                Console.WriteLine(result.GetAllValuesAsString());
                Console.WriteLine($"Ожидаемый минимум: (2.5, 1.5)\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка в GradientDescend: {ex.Message}\n");
            }
        }
        static void TestConjGradientDescend(FunctionND func, DoubleVector startPoint, double eps, int maxIters)
        {
            try
            {
                var coordParam = new MultiDimensional.ConjGradientDescendParam(startPoint, eps, maxIters);
                var result = new MultiDimensional.SearchResult(MultiDimensional.MethodType.conj_gradient_descend, 0, eps, startPoint);
                
                MultiDimensional.FindMinimumByСonjGradientDescend(ref result, func, ref coordParam);
                
                Console.WriteLine(result.GetAllValuesAsString());
                Console.WriteLine($"Ожидаемый минимум: (2.5, 1.5)\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка в СonjGradientDescend: {ex.Message}\n");
            }
        }
        static void TestNewtoneRaphson(FunctionND func, DoubleVector startPoint, double eps, int maxIters)
        {
            try
            {
                var coordParam = new MultiDimensional.NewtoneRaphsonParam(startPoint, eps, maxIters);
                var result = new MultiDimensional.SearchResult(MultiDimensional.MethodType.newtone_raphson, 0, eps, startPoint);
                
                MultiDimensional.FindMinimumByNewtoneRaphson(ref result, func, ref coordParam);
                
                Console.WriteLine(result.GetAllValuesAsString());
                Console.WriteLine($"Ожидаемый минимум: (2.5, 1.5)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка в NewtoneRaphson: {ex.Message}\n");
            }
        }
    }
}


// using System;
// using MathUtils;
// using OptimizationMethods;
// class Program
// {
//     // Определение целевой функции (должно соответствовать FunctionND делегату)
//     // f(x) = (x[0] - 5) * x[0] + (x[1] - 3) * x[1]
//     public static double MyFunction(DoubleVector x)
//     {
//         // Убедимся, что вектор имеет достаточную размерность
//         if (x.Count < 2)
//         {
//             throw new ArgumentException("Вектор должен иметь как минимум 2 элемента.");
//         }
//         return (x[0] - 5) * x[0] + (x[1] - 3) * x[1];
//     }

//     static void Main(string[] args)
//     {
//         Console.WriteLine("--- Тестирование методов оптимизации (Лабораторная работа) ---\n");

//         // 1. Заданные параметры
//         FunctionND my_function = MyFunction;
//         DoubleVector left = new DoubleVector(0.0, 0.0);
//         DoubleVector right = new DoubleVector(5.0, 3.0);
        
//         // В методах 1D поиска (Дихотомия, Золотое сечение, Фибоначчи) left и right задают границы отрезка [A, B].
//         // В многомерных методах (Покоординатный спуск) требуется начальная точка.
//         DoubleVector start_point = (left + right) * 0.5; // Начальная точка для Покоординатного спуска
//         double eps = 1e-6;
//         int max_iters = 100;

//         // 2. Ожидаемый минимум для f(x) = (x[0] - 5) * x[0] + (x[1] - 3) * x[1]
//         // Чтобы найти минимум, приравняем частные производные к нулю:
//         // ∂f/∂x[0] = 2*x[0] - 5 = 0  => x[0] = 2.5
//         // ∂f/∂x[1] = 2*x[1] - 3 = 0  => x[1] = 1.5
//         Console.WriteLine($"Целевая функция: f(x) = (x[0] - 5) * x[0] + (x[1] - 3) * x[1]");
//         Console.WriteLine($"Точный минимум: x* = (2.5, 1.5)\n");
//         Console.WriteLine($"Начальный интервал (для 1D поиска): [{left}, {right}]");
//         Console.WriteLine($"Точность (eps): {eps:E2}\n");


//         // 3. Вызов и вывод результатов для каждого метода

//         // --- 1. Дихотомия (Bisection) ---
//         try
//         {
//             OptimizationMethodss.SearchResult resultBisect = OptimizationMethodss.OptimizationMethods.Bisect(my_function, left, right, eps, max_iters);
//             Console.WriteLine("------------------------------------------");
//             Console.WriteLine($"[Метод Дихотомии (Bisection)]\n{resultBisect.ToString()}");
//         }
//         catch (Exception ex)
//         {
//             Console.WriteLine($"Ошибка в Дихотомии: {ex.Message}");
//         }
        
//         // --- 2. Золотое сечение (Golden Ratio) ---
//         try
//         {
//             OptimizationMethodss.SearchResult resultGolden = OptimizationMethodss.OptimizationMethods.GoldenRatio(my_function, left, right, eps, max_iters);
//             Console.WriteLine("------------------------------------------");
//             Console.WriteLine($"[Метод Золотого Сечения (Golden Ratio)]\n{resultGolden.ToString()}");
//         }
//         catch (Exception ex)
//         {
//             Console.WriteLine($"Ошибка в Золотом Сечении: {ex.Message}");
//         }


//         // --- 3. Фибоначчи (Fibonacci) ---
//         try
//         {
//             // Метод Фибоначчи не принимает max_iters, только eps
//             OptimizationMethodss.SearchResult resultFib = OptimizationMethodss.OptimizationMethods.Fibonacci(my_function, left, right, eps);
//             Console.WriteLine("------------------------------------------");
//             Console.WriteLine($"[Метод Фибоначчи (Fibonacci)]\n{resultFib.ToString()}");
//         }
//         catch (Exception ex)
//         {
//             Console.WriteLine($"Ошибка в Фибоначчи: {ex.Message}");
//         }


//         // --- 4. Покоординатный спуск (Per Coordinate Descent) ---
//         try
//         {
//             // Используем усредненную начальную точку
//             OptimizationMethodss.SearchResult resultCoord = OptimizationMethodss.OptimizationMethods.PerCoordinateDescend(my_function, start_point, eps, max_iters);
//             Console.WriteLine("------------------------------------------");
//             Console.WriteLine($"[Покоординатный Спуск]\n{resultCoord.ToString()}");
//         }
//         catch (Exception ex)
//         {
//             Console.WriteLine($"Ошибка в Покоординатном Спуске: {ex.Message}");
//         }

//         Console.WriteLine("------------------------------------------");
//     }
// }



// // using System;
// // class Program
// // {
// //     static void Main()
// //     {
// //         Function1D func = x => Math.Pow((x-2),2)+1;
// //         //Function1D func = x => (x-1)*(x-3);
// //         var bisactParam = new OneDimensional.BisactionParam(
// //             lhs: 0.0,     
// //             rhs: 5.0,       
// //             eps: 0.0001,   
// //             max_it: 100  
// //         );

// //         var goldenParam = new OneDimensional.GoldenRatioParam(
// //             lhs: 0.0,     
// //             rhs: 5.0,       
// //             eps: 0.0001,    
// //             max_it: 100   
// //         );
        
// //         var fibonacciParam = new OneDimensional.FibonacciParam(
// //             lhs: 0.0,     
// //             rhs: 5.0,       
// //             eps: 0.0001,   
// //             max_it: 100  
// //         );

// //         OneDimensional.SearchResult searchResult = new OneDimensional.SearchResult();

// //         OneDimensional.BisectSerchResult(ref searchResult, func, ref bisactParam);
// //         Console.WriteLine("Метод: " + searchResult.Method);
// //         Console.WriteLine("Результат: " + searchResult.Result);
// //         Console.WriteLine("Точность: " + searchResult.Accuracy);
// //         Console.WriteLine("Итерации: " + searchResult.Iteration);
// //         Console.WriteLine("Вызовы функции: " + searchResult.FunctionCalls);
// //         Console.WriteLine(" ");

// //         OneDimensional.GoldenRatioSearchResult(ref searchResult, func, ref goldenParam);
// //         Console.WriteLine("Метод: " + searchResult.Method);
// //         Console.WriteLine("Результат: " + searchResult.Result);
// //         Console.WriteLine("Точность: " + searchResult.Accuracy);
// //         Console.WriteLine("Итерации: " + searchResult.Iteration);
// //         Console.WriteLine("Вызовы функции: " + searchResult.FunctionCalls);
// //         Console.WriteLine(" ");

// //         OneDimensional.FibonacciSearchResult(ref searchResult, func, ref fibonacciParam);
// //         Console.WriteLine("Метод: " + searchResult.Method);
// //         Console.WriteLine("Результат: " + searchResult.Result);
// //         Console.WriteLine("Точность: " + searchResult.Accuracy);
// //         Console.WriteLine("Итерации: " + searchResult.Iteration);
// //         Console.WriteLine("Вызовы функции: " + searchResult.FunctionCalls);
// //         Console.WriteLine(" ");
        
// //         //Console.WriteLine("x_c: " + fibonacciParam.x_c);
// //     }
// // }