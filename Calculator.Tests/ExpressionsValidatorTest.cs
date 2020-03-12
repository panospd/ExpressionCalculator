using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NUnit.Framework;

namespace Calculator.Tests
{
    [TestFixture]
    [TestOf(nameof(ExpressionsValidator))]

    public class ExpressionsValidatorTest
    {
        [TestCase("")]
        [TestCase("1 + 2 _ 3")]
        public void Validate_WhenInputIsEmptyOrContainsInvalidOperators_ShouldThrowValidationError(string input)
        {
            // Arrange
            var classUnderTest = CreateClassUnderTest();

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
            var classUnderTest = CreateClassUnderTest();

            // Act
            IList<ValidationResult> result = classUnderTest.Validate(input);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(1, result.Count);

            Assert.AreEqual("Input contains two or more consecutive operators", result[0].ErrorMessage);
        }

        [TestCase("1 + 2 + 2 -25")]
        [TestCase("1 + 2 * 3")]
        [TestCase("1 + 2 * 3/2/2/2 + 4*5-    3")]
        public void Validate_WhenInputISValid_ShouldReturnEmptyValidationResultList(string input)
        {
            // Arrange
            var classUnderTest = CreateClassUnderTest();

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