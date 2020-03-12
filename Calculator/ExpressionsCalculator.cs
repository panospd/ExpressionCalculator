using System;
using System.Collections.Generic;
using System.Linq;

namespace Calculator
{
    public class ExpressionsCalculator
    {
        private readonly Operator _operator = new Operator();

        public string Calculate(string input)
        {
            string inputWithNoSpaces = input.Replace(" ", string.Empty);

            string[] subtractAggregates = inputWithNoSpaces.Split(_operator.Minus);

            string[] subtractBlocks = subtractAggregates
                .Select(CalculateSubtractBlock)
                .ToArray();

            return PerformOperationForBlocks(subtractBlocks, SubtractNumbers);
        }

        private static long SubtractNumbers(params long[] numbers)
        {
            if (numbers.Length == 0)
                return default;

            var result = numbers[0];

            return numbers
                .Skip(1)
                .Aggregate(result, (current, number) => current - number);
        }

        private static long SumOfNumbers(long[] numbers)
        {
            return numbers.Sum();
        }

        private string CalculateSubtractBlock(string subtractAggregate)
        {
            string[] sumAggregates = subtractAggregate.Split(_operator.Plus);

            string[] sumBlocks = sumAggregates
                .Select(CalculateSumBlock)
                .ToArray();

            return PerformOperationForBlocks(sumBlocks, SumOfNumbers);
        }

        private string PerformOperationForBlocks(string[] blocks, Func<long[], long> operation)
        {
            if (blocks.Length == 1)
                return blocks[0];

            string[] fractionBlocks = blocks
                .Where(b => b.Contains(_operator.Divisor))
                .ToArray();

            if (!fractionBlocks.Any())
            {
                long[] blocksToPerformOperationOn = blocks
                    .Select(s => Convert.ToInt64(s))
                    .ToArray();

                return $"{operation(blocksToPerformOperationOn)}";
            }

            long[] denominators = fractionBlocks
                .Select(s => Convert.ToInt64(s.Split(_operator.Divisor)[1]))
                .ToArray();

            long leastCommonMultiple = FindLeastCommonMultiple(denominators);

            var adjustedNumerators = blocks
                .Select(f =>
                {
                    string[] fraction = f.Split(_operator.Divisor);

                    if (fraction.Length == 1)
                        return Convert.ToInt64(fraction[0]) * leastCommonMultiple;

                    long denominator = Convert.ToInt64(fraction[1]);
                    long numerator = Convert.ToInt64(fraction[0]);

                    return numerator * leastCommonMultiple / denominator;
                })
                .ToList();

            var calculatedNumerator = operation(adjustedNumerators.ToArray());

            var greatestCommonDivisor = GreatestCommonDivisor(calculatedNumerator, leastCommonMultiple);

            return greatestCommonDivisor == leastCommonMultiple
                ? $"{greatestCommonDivisor}"
                : $"{calculatedNumerator / greatestCommonDivisor}/{leastCommonMultiple / greatestCommonDivisor}";
        }

        private static long FindLeastCommonMultiple(params long[] numbers)
        {
            if (numbers.Length == 1)
                return numbers[0];

            long result = LeastCommonMultiple(numbers[0], numbers[1]);

            return numbers
                .Skip(2)
                .Aggregate(result, LeastCommonMultiple);
        }

        private static long LeastCommonMultiple(long a, long b)
        {
            long num1, num2;

            if (a > b)
            {
                num1 = a; 
                num2 = b;
            }
            else
            {
                num1 = b; 
                num2 = a;
            }

            for (int i = 1; i < num2; i++)
            {
                if ((num1 * i) % num2 == 0)
                    return i * num1;
            }

            return num1 * num2;
        }

        private string CalculateSumBlock(string sumAggregate)
        {
            string[] multiplyAggregates = sumAggregate.Split(_operator.Multiplier);

            string[] fractions = multiplyAggregates
                .Where(ma => ma.Contains(_operator.Divisor))
                .ToArray();

            if (!fractions.Any())
            {
                long[] numbers = multiplyAggregates
                    .Select(s => Convert.ToInt64(s))
                    .ToArray();

                long block = MultiplyNumbers(numbers);

                return $"{block}";
            }

            IEnumerable<long> firstElementsFromFractions = fractions.Select(s => Convert.ToInt64(s.Split(_operator.Divisor)[0]));

            List<long> numeratorsContainer = multiplyAggregates
                .Except(fractions)
                .Select(s => Convert.ToInt64(s))
                .ToList();

            numeratorsContainer.AddRange(firstElementsFromFractions);

            long numerator = MultiplyNumbers(numeratorsContainer.ToArray());
            long denominator = CalculateDenominatorFromFractions(fractions);

            long greatCommonDivisor = GreatestCommonDivisor(numerator, denominator);

            return greatCommonDivisor == denominator
                ? $"{numerator / greatCommonDivisor}"
                : $"{numerator / greatCommonDivisor}/{denominator / greatCommonDivisor}";
        }

        private long CalculateDenominatorFromFractions(IEnumerable<string> fractions)
        {
            long[] fractionElements = fractions
                .Select(f =>
                {
                    long[] numbersToMultiply = f.Split(_operator.Divisor)
                        .Skip(1)
                        .Select(s => Convert.ToInt64(s))
                        .ToArray();

                    return MultiplyNumbers(numbersToMultiply);
                })
                .ToArray();

            return MultiplyNumbers(fractionElements);
        }

        private static long MultiplyNumbers(long[] numbers)
        {
            var result = numbers[0];

            for (int i = 1; i < numbers.Length; i++)
                result *= numbers[i];

            return result;
        }

        private static long GreatestCommonDivisor(long a, long b)
        {
            long number1 = Math.Abs(a);
            long number2 = Math.Abs(b);

            return number1 == 0 ? number2 : GreatestCommonDivisor(number2 % number1, number1);
        }
    }
}
