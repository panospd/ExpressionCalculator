using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Calculator
{
    public class ExpressionsValidator
    {
        public string[] _validOperators = {"/", "*", "+", "-"};

        public IList<ValidationResult> Validate(string input)
        {
            var validationResults = new List<ValidationResult>();

            string inputToValidate = input.Replace(" ", string.Empty);

            bool hasValidCharacters =  inputToValidate.All(c => char.IsDigit(c) || _validOperators.Contains(c.ToString()));

            if(!hasValidCharacters)
                validationResults.Add(new ValidationResult("Input can only contain digits, empty spaces and any of the operators \"+\", \"-\", \"*\" and \"/\""));

            bool containsAtLeastOneDigit = inputToValidate.Any(char.IsDigit);

            if (!containsAtLeastOneDigit)
                validationResults.Add(new ValidationResult("Input must contain at least one number"));

            bool ooeratorsHaveConsecutivePositions = OperatorsHaveConsecutivePositions(inputToValidate);

            if(ooeratorsHaveConsecutivePositions)
                validationResults.Add(new ValidationResult("Input contains two or more consecutive operators"));

            return validationResults;
        }

        private bool OperatorsHaveConsecutivePositions(string inputToValidate)
        {
            int?[] operatorPositions = inputToValidate
                .ToCharArray()
                .Select((c, i) =>
                {
                    bool charIsValidOperator = _validOperators.Contains(c.ToString());
                    return charIsValidOperator ? (int?)i : null;
                })
                .Where(i => i != null)
                .ToArray();

            for (int i = 0; i < operatorPositions.Length - 1; i++)
            {
                if (operatorPositions[i] == operatorPositions[i + 1] - 1)
                    return true;
            }

            return false;
        }
    }
}