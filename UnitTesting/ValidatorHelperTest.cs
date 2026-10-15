using HabitTracker;

namespace UnitTesting
{
    public class ValidatorHelperTest
    {
        private static readonly object[] isValidData =
        {
            new TestCaseData("19-03-1994", true),
            new TestCaseData("19-03-2054", true),
            new TestCaseData("19-03-1204", true),
            new TestCaseData("24-12-2020", true),

            new TestCaseData("x", false),
            new TestCaseData("", false),
            new TestCaseData("19-03", false),
        };

        private static readonly object[] isValidInteger =
        {
            new TestCaseData("19", true),
            new TestCaseData("500", true),
            new TestCaseData("1231234124", true),

            new TestCaseData("123456789012345", false),
            new TestCaseData("x", false),
            new TestCaseData("", false),
            new TestCaseData("-4", false),
        };

        private InputHandler _inputHandler;

        [SetUp]
        public void Setup()
        {
            _inputHandler = new InputHandler();
        }

        [TestCaseSource(nameof(isValidData))]
        public void Data_CorrectInput_ReturnsTrue(string input, bool expectedResult)
        {
            var result = _inputHandler.IsValidData(input);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [TestCaseSource(nameof(isValidInteger))]
        public void Int_CorrectInput_ReturnsTrue(string input, bool expectedResult)
        {
            var result = _inputHandler.IsValidPositiveInteger(input);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

    }
}