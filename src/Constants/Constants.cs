namespace Webwonders.Baseline.Middleware;

public static partial class Constants
{
    public static class Headers
    {
        public const string Hsts = "Enable_HSTS";
        public const string XFrameOptions = "Enable_X-Frame-Options";
        public const string XContentTypeOptions = "Enable_X-ContentType-Options";
        public const string XxssProtection = "Enable_X-XSS-Protection";
        
        public static class XFrameOptionsValues
        {
            public const string SameOrigin = "SAMEORIGIN";
            public const string Deny = "DENY";
            public const string AllowFrom = "ALLOW-FROM";
        }
        
        public static class XContentTypeOptionsValues
        {
            public const string Nosniff = "nosniff";
        }
        
        public static class XxssProtectionValues
        {
            public const string Enabled = "1; mode=block";
            public const string Disabled = "0";
        }
    }
}