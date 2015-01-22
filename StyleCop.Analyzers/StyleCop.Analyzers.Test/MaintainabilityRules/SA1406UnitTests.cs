namespace StyleCop.Analyzers.Test.MaintainabilityRules
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Analyzers.MaintainabilityRules;

    [TestClass]
    public class SA1406UnitTests : DebugMessagesUnitTestsBase<SA1406DebugFailMustProvideMessageText>
    {
        protected override string DiagnosticId
        {
            get
            {
                return SA1406DebugFailMustProvideMessageText.DiagnosticId;
            }
        }

        protected override string MethodName
        {
            get
            {
                return nameof(Debug.Fail);
            }
        }

        protected override IEnumerable<string> InitialArguments
        {
            get
            {
                return Enumerable.Empty<string>();
            }
        }
    }
}