using System.Globalization;
using System.Runtime.CompilerServices;

namespace EventFlowAPI.Tests.Infrastructure
{
    public class TestRunSettings
    {
        public const string Culture = "pl-PL";

        [ModuleInitializer]
        public static void Initialize()
        {
            var culture = CultureInfo.GetCultureInfo(Culture);
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
        }
    }
}
