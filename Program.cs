using System;

class Program
{
    static void Main()
    {
        Function1D func = x => Math.Pow((x-2),2)+1;
        //Function1D func = x => (x-1)*(x-3);
        var bisactParam = new OneDimensional.BisactionParam(
            lhs: 0.0,     
            rhs: 5.0,       
            eps: 0.0001,   
            max_it: 100  
        );

        var goldenParam = new OneDimensional.GoldenRatioParam(
            lhs: 0.0,     
            rhs: 5.0,       
            eps: 0.0001,    
            max_it: 100   
        );
        
        var fibonacciParam = new OneDimensional.FibonacciParam(
            lhs: 0.0,     
            rhs: 5.0,       
            eps: 0.0001,   
            max_it: 100  
        );

        OneDimensional.SearchResult searchResult = new OneDimensional.SearchResult();

        OneDimensional.BisectSerchResult(ref searchResult, func, ref bisactParam);
        Console.WriteLine("Метод: " + searchResult.Method);
        Console.WriteLine("Результат: " + searchResult.Result);
        Console.WriteLine("Точность: " + searchResult.Accuracy);
        Console.WriteLine("Итерации: " + searchResult.Iteration);
        Console.WriteLine("Вызовы функции: " + searchResult.FunctionCalls);
        Console.WriteLine(" ");

        OneDimensional.GoldenRatioSearchResult(ref searchResult, func, ref goldenParam);
        Console.WriteLine("Метод: " + searchResult.Method);
        Console.WriteLine("Результат: " + searchResult.Result);
        Console.WriteLine("Точность: " + searchResult.Accuracy);
        Console.WriteLine("Итерации: " + searchResult.Iteration);
        Console.WriteLine("Вызовы функции: " + searchResult.FunctionCalls);
        Console.WriteLine(" ");

        OneDimensional.FibonacciSearchResult(ref searchResult, func, ref fibonacciParam);
        Console.WriteLine("Метод: " + searchResult.Method);
        Console.WriteLine("Результат: " + searchResult.Result);
        Console.WriteLine("Точность: " + searchResult.Accuracy);
        Console.WriteLine("Итерации: " + searchResult.Iteration);
        Console.WriteLine("Вызовы функции: " + searchResult.FunctionCalls);
        Console.WriteLine(" ");
        
        //Console.WriteLine("x_c: " + fibonacciParam.x_c);
    }
}