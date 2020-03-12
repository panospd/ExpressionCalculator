using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NUnit.Framework;

namespace Calculator.Tests
{
    [TestFixture]
    [TestOf(nameof(ExpressionsValidator))]

    public class ExpressionsValidatorTest
    {
        [TestCase("1 + 2 _ 3 ^")]
        [TestCase("1 + 2 _ 3")]
        public void Validate_WhenInputIsEmptyOrContainsInvalidOperators_ShouldThrowValidationError(string input)
        {
            // Arrange
            ExpressionsValidator classUnderTest = CreateClassUnderTest();

            // Act
            IList<ValidationResult> result = classUnderTest.Validate(input);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(1, result.Count);

            Assert.AreEqual("Input can only contain digits, empty spaces and any of the operators \"+\", \"-\", \"*\" and \"/\"", result[0].ErrorMessage);
        }

        [TestCase("1 ++ 2 --")]
        [TestCase("1 + 2 *+ 3")]
        public void Validate_WhenInputContainsTwoOrMoreConsecutuveOperators_ShouldThrowValidationError(string input)
        {
            // Arrange
            ExpressionsValidator classUnderTest = CreateClassUnderTest();

            // Act
            IList<ValidationResult> result = classUnderTest.Validate(input);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(1, result.Count);

            Assert.AreEqual("Input contains two or more consecutive operators", result[0].ErrorMessage);
        }

        [TestCase("+")]
        [TestCase("")]
        public void Validate_WhenInputIsEmptyOrIsSingleOperator_ShouldThrowValidationError(string input)
        {
            // Arrange
            ExpressionsValidator classUnderTest = CreateClassUnderTest();

            // Act
            IList<ValidationResult> result = classUnderTest.Validate(input);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(1, result.Count);

            Assert.AreEqual("Input must contain at least one number", result[0].ErrorMessage);
        }

        [TestCase("++")]
        [TestCase("/////")]
        public void Validate_WhenInputContainsOnlyRepeatingOperators_ShouldThrowValidationErrors(string input)
        {
            // Arrange
            ExpressionsValidator classUnderTest = CreateClassUnderTest();

            // Act
            IList<ValidationResult> result = classUnderTest.Validate(input);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(2, result.Count);

            ValidationResult firstValidationError = result[0];

            Assert.AreEqual("Input must contain at least one number", firstValidationError.ErrorMessage);

            ValidationResult secondValidationError = result[1];

            Assert.AreEqual("Input contains two or more consecutive operators", secondValidationError.ErrorMessage);
        }

        [TestCase("1")]
        [TestCase("1 + 2 + 2 -25")]
        [TestCase("1 + 2 * 3")]
        [TestCase("1 + 2 * 3/2/2/2 + 4*5-    3")]
        public void Validate_WhenInputISValid_ShouldReturnEmptyValidationResultList(string input)
        {
            // Arrange
            ExpressionsValidator classUnderTest = CreateClassUnderTest();

            // Act
            IList<ValidationResult> result = classUnderTest.Validate(input);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        private static ExpressionsValidator CreateClassUnderTest()
        {
            return new ExpressionsValidator();
        }
    }
}