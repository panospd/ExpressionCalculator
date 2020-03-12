using NUnit.Framework;

namespace Calculator.Tests
{
    [TestFixture]
    [TestOf(nameof(ExpressionsCalculator))]
    public class ExpressionsCalculatorTest
    {
        [TestCase("1 + 2", ExpectedResult = "3")]
        [TestCase("1 + 2/ 3", ExpectedResult = "5/3")]
        [TestCase("1 -   2 / 3", ExpectedResult = "1/3")]
        [TestCase(" 2 / 3 - 1", ExpectedResult = "-1/3")]
        [TestCase("50 * 2 + 25 / 4 - 40", ExpectedResult = "265/4")]

        public string Calculate_WhenCalled_ReturnsCorrectExpression(string input)
        {
            // Arrange
            ExpressionsCalculator classUnderTest = CreateClassUnderTest();

            // Act
            string result = classUnderTest.Calculate(input);

            // Assert
            return result;
        }

        private ExpressionsCalculator CreateClassUnderTest()
        {
            return new ExpressionsCalculator();
        }
    }
}