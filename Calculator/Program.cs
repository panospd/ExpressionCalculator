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
            var input = "15 ++ 2 * 16 / 4 - 2 / 3";

            var validator = new ExpressionsValidator();

            IList<ValidationResult> validationResults = validator.Validate(input);

            if (validationResults.Any())
            {
                foreach (var validationResult in validationResults)
                    Console.WriteLine(validationResult.ErrorMessage);

                return;
            }

            var expressionsCalculator = new ExpressionsCalculator();

            Console.WriteLine(expressionsCalculator.Calculate(input));
        }

        
    }
}
