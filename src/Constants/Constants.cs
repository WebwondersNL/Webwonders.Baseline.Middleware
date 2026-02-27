namespace Webwonders.Baseline.Middleware;

public static partial class Constants
{
    public static class Headers
    {
        public const string Hsts = "Enable_HSTS";
        public const string XFrameOptions = "Enable_X-Frame-Options";
        public const string XContentTypeOptions = "Enable_X-ContentType-Options";
        public const string ReferrerPolicy = "Enable_Referrer-Policy";
        
        public static class XFrameOptionsValues
        {
            public const string SameOrigin = "SAMEORIGIN";
        }
        
        public static class XContentTypeOptionsValues
        {
            public const string Nosniff = "nosniff";
        }

        public static class ReferrerPolicyValues
        {
            public const string StrictOriginWhenCrossOrigin = "strict-origin-when-cross-origin";
        }
    }
}