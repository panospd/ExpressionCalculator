﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Calculator
{
    public class ExpressionsCalculator
    {
        private readonly Operator _operator = new Operator();

        public string Calculate(string input)
        {
            string[] subtractionExpressionBlocks = input
                .Replace(" ", string.Empty)
                .Split(_operator.Minus)
                .Select((b, i) => 
                    i == 0 
                    ? CalculateSubtractionExpressionBlock(b, SumNumbers) 
                    : CalculateSubtractionExpressionBlock(b, SubtractNumbers))
                .ToArray();

            return PerformOperationForExpressionBlocks(subtractionExpressionBlocks, SubtractNumbers);
        }

        private static long SubtractNumbers(long[] numbers)
        {
            return numbers
                .Select((n, i) => i == 0 ? n : n * -1)
                .Sum();
        }

        private static long SumNumbers(long[] numbers)
        {
            return numbers.Sum();
        }

        private string CalculateSubtractionExpressionBlock(string subtractionExpressionBlock, Func<long[], long> operation)
        {
            string[] summationExpressionBlocks = subtractionExpressionBlock
                .Split(_operator.Plus)
                .Select(CalculateSummationExpressionBlock)
                .ToArray();

            return PerformOperationForExpressionBlocks(summationExpressionBlocks, operation);
        }

        private string PerformOperationForExpressionBlocks(string[] blocks, Func<long[], long> operation)
        {
            if (blocks.Length == 1)
                return blocks[0];

            string[] fractionBlocks = blocks
                .Where(b => b.Contains(_operator.Divisor))
                .ToArray();

            return !fractionBlocks.Any() 
                ? PerformOperationOnWholeNumbersBlock(blocks, operation) 
                : PerformOperationOnNumbersExpressionWithFractions(blocks, operation);
        }

        private string PerformOperationOnNumbersExpressionWithFractions(string[] blocks, Func<long[], long> operation)
        {
            string[] fractionBlocks = blocks
                .Where(b => b.Contains(_operator.Divisor))
                .ToArray();

            long[] denominators = fractionBlocks
                .Select(s => Convert.ToInt64(s.Split(_operator.Divisor)[1]))
                .ToArray();

            long leastCommonMultiple = FindLeastCommonMultiple(denominators);

            long[] adjustedNumerators = blocks
                .Select((f, i) =>
                {
                    string[] fraction = f.Split(_operator.Divisor);

                    if (fraction.Length == 1)
                        return Convert.ToInt64(fraction[0]) * leastCommonMultiple;

                    long denominator = Convert.ToInt64(fraction[1]);
                    long numerator = Convert.ToInt64(fraction[0]);

                    return numerator * leastCommonMultiple / denominator;
                })
                .ToArray();

            long calculatedNumerator = operation(adjustedNumerators.ToArray());
            long greatestCommonDivisor = GreatestCommonDivisor(calculatedNumerator, leastCommonMultiple);

            return greatestCommonDivisor == leastCommonMultiple
                ? $"{greatestCommonDivisor}"
                : $"{calculatedNumerator / greatestCommonDivisor}/{leastCommonMultiple / greatestCommonDivisor}";
        }

        private static long FindLeastCommonMultiple(params long[] numbers)
        {
            if (numbers.Length == 1)
                return numbers[0];

            return numbers
                .Skip(1)
                .Aggregate(numbers[0], LeastCommonMultiple);
        }

        private static long LeastCommonMultiple(long a, long b)
        {
            long number1, number2;

            if (a > b)
            {
                number1 = a; 
                number2 = b;
            }
            else
            {
                number1 = b; 
                number2 = a;
            }

            for (int i = 1; i < number2; i++)
            {
                if ((number1 * i) % number2 == 0)
                    return i * number1;
            }

            return number1 * number2;
        }

        private string CalculateSummationExpressionBlock(string summationExpressionBlock)
        {
            string[] multiplyExpressionBlocks = summationExpressionBlock.Split(_operator.Multiplier);

            string[] fractions = multiplyExpressionBlocks
                .Where(ma => ma.Contains(_operator.Divisor))
                .ToArray();
            
            return fractions.Any() 
                ? NumbersWithFractionsSummationBlock(multiplyExpressionBlocks) 
                : PerformOperationOnWholeNumbersBlock(multiplyExpressionBlocks, MultiplyNumbers);
        }

        private string NumbersWithFractionsSummationBlock(string[] multiplyExpressionBlocks)
        {
            string[] fractions = multiplyExpressionBlocks
                .Where(ma => ma.Contains(_operator.Divisor))
                .ToArray();

            IEnumerable<long> firstElementsFromFractions = fractions.Select(s => Convert.ToInt64(s.Split(_operator.Divisor)[0]));

            List<long> numeratorsContainer = multiplyExpressionBlocks
                .Except(fractions)
                .Select(s => Convert.ToInt64(s))
                .ToList();

            numeratorsContainer.AddRange(firstElementsFromFractions);

            long numerator = MultiplyNumbers(numeratorsContainer.ToArray());
            long denominator = CalculateDenominatorForFractions(fractions);

            long greatCommonDivisor = GreatestCommonDivisor(numerator, denominator);

            return greatCommonDivisor == denominator
                ? $"{numerator / greatCommonDivisor}"
                : $"{numerator / greatCommonDivisor}/{denominator / greatCommonDivisor}";
        }

        private static string PerformOperationOnWholeNumbersBlock(string[] expressionBlocks, Func<long[], long> operation)
        {
            long[] numbers = expressionBlocks
                .Select(s => Convert.ToInt64(s))
                .ToArray();

            return $"{operation(numbers)}";
        }

        private long CalculateDenominatorForFractions(IEnumerable<string> fractions)
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
            long result = numbers[0];

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
