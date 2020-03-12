using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            var anotherRound = true;

            while (anotherRound)
            {
                Run();

                ConsoleKey keyPressed = PromptUserForAnotherRound();
                anotherRound = keyPressed == ConsoleKey.Enter;
            }
        }

        private static ConsoleKey PromptUserForAnotherRound()
        {
            Console.WriteLine("Press enter for another round or any other key to exit:");

            return Console.ReadKey(true).Key;
        }

        private static void Run()
        {
            Console.WriteLine("Please specify an expression below: ");
            var input = Console.ReadLine();

            IList<ValidationResult> validationResults = RunValidations(input);

            if (validationResults.Any())
            {
                ShowValidations(validationResults);
                return;
            }

            string result = RunCalculation(input);

            Console.WriteLine($"Result is: {result}");
        }

        private static IList<ValidationResult> RunValidations(string input)
        {
            var validator = new ExpressionsValidator();

            return validator.Validate(input);
        }

        private static string RunCalculation(string input)
        {
            var expressionsCalculator = new ExpressionsCalculator();
            return expressionsCalculator.Calculate(input);
        }

        private static void ShowValidations(IList<ValidationResult> validationResults)
        {
            foreach (var validationResult in validationResults)
                Console.WriteLine(validationResult.ErrorMessage);
        }
        
    }
}
