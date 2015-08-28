﻿namespace StyleCop.Analyzers.NamingRules
{
    using System.Collections.Immutable;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;

    /// <summary>
    /// There are currently no situations in which this rule will fire.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    [NoDiagnostic("This rule has no behavior by design.")]
    [NoCodeFix("Don't fix what isn't broken.")]
    public class SA1301ElementMustBeginWithLowerCaseLetter : StyleCopDiagnosticAnalyzer
    {
        /// <summary>
        /// The ID for diagnostics produced by the <see cref="SA1301ElementMustBeginWithLowerCaseLetter"/> analyzer.
        /// </summary>
        public const string DiagnosticId = "SA1301";
        private const string Title = "Element must begin with lower-case letter";
        private const string MessageFormat = "Element must begin with lower-case letter";
        private const string Description = "There are currently no situations in which this rule will fire.";
        private const string HelpLink = "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/blob/master/documentation/SA1301.md";

        private static readonly DiagnosticDescriptor Descriptor =
            new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, AnalyzerCategory.NamingRules, DiagnosticSeverity.Warning, AnalyzerConstants.DisabledByDefault, Description, HelpLink, WellKnownDiagnosticTags.NotConfigurable);

        private static readonly ImmutableArray<DiagnosticDescriptor> SupportedDiagnosticsValue =
            ImmutableArray.Create(Descriptor);

        /// <inheritdoc/>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get
            {
                return SupportedDiagnosticsValue;
            }
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("AnalyzerPerformance", "RS1012:Start action has no registered actions.", Justification = "This diagnostic is not implemented (by design) in StyleCopAnalyzers.")]
        protected override void InitializeOnCompilationStart(CompilationStartAnalysisContext context)
        {
            // Intentionally empty
        }
    }
}
