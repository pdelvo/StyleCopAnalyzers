namespace StyleCop.Analyzers.Test.SpacingRules
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Analyzers.SpacingRules;

    [TestClass]
    public class SA1021UnitTests : NumberSignSpacingTestBase<SA1021NegativeSignsMustBeSpacedCorrectly, SA1021CodeFixProvider>
    {
        protected override string DiagnosticId
        {
            get
            {
                return SA1021NegativeSignsMustBeSpacedCorrectly.DiagnosticId;
            }
        }

        protected override string Sign
        {
            get
            {
                return "-";
            }
        }

        protected override string SignName
        {
            get
            {
                return "Negative";
            }
        }
    }
}
