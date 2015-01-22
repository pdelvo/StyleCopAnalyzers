namespace StyleCop.Analyzers.Test.SpacingRules
{
    using Analyzers.SpacingRules;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SA1022UnitTests : NumberSignSpacingTestBase<SA1022PositiveSignsMustBeSpacedCorrectly, SA1022CodeFixProvider>
    {
        protected override string DiagnosticId
        {
            get
            {
                return SA1022PositiveSignsMustBeSpacedCorrectly.DiagnosticId;
            }
        }

        protected override string Sign
        {
            get
            {
                return "+";
            }
        }

        protected override string SignName
        {
            get
            {
                return "Positive";
            }
        }
    }
}
