using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace TechShopSolution.Application.Configurations {
    public class DiagnosticsConfig {
        public static void Initialize(string serviceName) {
            ServiceName = serviceName;
        }
        public static string ServiceName = "default-fpl-serviceinfo-service";
        public static Meter Meter = new Meter(ServiceName);
        public ActivitySource ActivitySource  = new ActivitySource(ServiceName);
    }
}

