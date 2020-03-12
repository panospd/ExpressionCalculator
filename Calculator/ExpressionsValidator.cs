using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Calculator
{
    public class ExpressionsValidator
    {
        public string[] _validOperators = new[] {"/", "*", "+", "-"};

        public IList<ValidationResult> Validate(string input)
        {
            var validationResults = new List<ValidationResult>();

            string inputToValidate = input.Replace(" ", string.Empty);

            var hasValidCharacters = !string.IsNullOrEmpty(inputToValidate) && inputToValidate
                .All(c => char.IsDigit(c) || _validOperators.Contains(c.ToString()));

            if(!hasValidCharacters)
                validationResults.Add(new ValidationResult("Input can only contain digits, empty spaces and any of the operators \"+\", \"-\", \"*\" and \"/\""));

            bool operatorsHaveNonConsecutivePositions = OperatorsHaveNonConsecutivePositions(inputToValidate);

            if(!operatorsHaveNonConsecutivePositions)
                validationResults.Add(new ValidationResult("Input contains two or more consecutive operators"));

            return validationResults;
        }

        private bool OperatorsHaveNonConsecutivePositions(string inputToValidate)
        {
            int[] operatorPositions = inputToValidate
                .Select((c, i) => _validOperators.Contains(c.ToString()) ? i : default)
                .Where(i => i != default)
                .ToArray();

            for (int i = 0; i < operatorPositions.Length - 1; i++)
            {
                if (operatorPositions[i] == operatorPositions[i + 1] - 1)
                    return false;
            }

            return true;
        }
    }
}