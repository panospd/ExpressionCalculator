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
            Console.WriteLine("Please specify an expression below: ");
            var input = Console.ReadLine();

            var validator = new ExpressionsValidator();

            IList<ValidationResult> validationResults = validator.Validate(input);

            if (validationResults.Any())
            {
                ShowValidations(validationResults);
                return;
            }

            var expressionsCalculator = new ExpressionsCalculator();

            Console.WriteLine($"Result is: {expressionsCalculator.Calculate(input)}");
        }

        private static void ShowValidations(IList<ValidationResult> validationResults)
        {
            foreach (var validationResult in validationResults)
                Console.WriteLine(validationResult.ErrorMessage);
        }
        
    }
}
