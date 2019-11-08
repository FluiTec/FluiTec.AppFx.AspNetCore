using FluiTec.DbLocalizationProvider.Abstractions;
using FluiTec.DbLocalizationProvider.Abstractions.Refactoring;

namespace FluiTec.AppFx.AspNetCore.Examples.LocalizationExample.Resources
{
    [LocalizedResource]
    public class Resource
    {
        // Sample for a normal, localized property
        [TranslationForCulture("NormaleEigenschaft-Wert", "de")] // this defines localization for culture: de
        public string NormalProperty => "NormalProperty-Value"; // this defines localization for cultures: invariant, en

        // Sample for a static, localized property
        [TranslationForCulture("StatischeEigenschaft-Wert", "de")] // this defines localization for culture: de
        public static string StaticProperty =>
            "StaticProperty-Value"; // this defines localization for cultures: invariant, en

        // Sample for refactoring
        [RenamedResource("OldyPropertyName")]
        [TranslationForCulture("RefaktorisierteEigenschaft-Wert", "de")] // this defines localization for culture: de
        public string RefactoredProperty =>
            "RefactoredProperty-Value"; // this defines localization for cultures: invariant, en
    }
}