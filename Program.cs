using System;

class Program
{
    static void Main()
    {
        // Создаем функцию для тестирования
        Function1D func = x => x * x + 2 * x + 1;
        
        // Создаем параметры для метода бисекции
        var goldenParam = new OneDimensional.GoldenRatioParam(
            lhs: -5.0,     // левая граница
            rhs: 5.0,      // правая граница  
            eps: 0.0001,   // точность
            max_it: 1000   // максимальное число итераций
        );
        
        // Создаем структуру для результата
        OneDimensional.SearchResult searchResult = new OneDimensional.SearchResult();
        
        // Вызываем метод бисекции (убедитесь, что он public)
        OneDimensional.GoldenRatioSearchResult(ref searchResult, func, ref goldenParam);
    
        // Выводим результаты
        Console.WriteLine("Метод: " + searchResult.Method);
        Console.WriteLine("Результат: " + searchResult.Result);
        Console.WriteLine("Точность: " + searchResult.Accuracy);
        Console.WriteLine("Итерации: " + searchResult.Iteration);
        Console.WriteLine("Вызовы функции: " + searchResult.FunctionCalls);
        
        // Также можно посмотреть x_c из параметров
        Console.WriteLine("x_c: " + goldenParam.x_c);
    }
}