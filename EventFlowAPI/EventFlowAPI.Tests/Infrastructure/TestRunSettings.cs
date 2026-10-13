using System.Globalization;
using System.Runtime.CompilerServices;

namespace EventFlowAPI.Tests.Infrastructure
{
    public class TestRunSettings
    {
        public const string AcceptVariable = "EVENTFLOW_ACCEPT_SNAPSHOTS";
        public const string Culture = "pl-PL";

        [ModuleInitializer]
        public static void Initialize()
        {
            var culture = CultureInfo.GetCultureInfo(Culture);
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;

            Environment.SetEnvironmentVariable("DiffEngine_Disabled", "true");
            if (Environment.GetEnvironmentVariable(AcceptVariable) == "1")
            {
                VerifierSettings.AutoVerify(includeBuildServer: false);
            }
        }
    }
}
