namespace Fklib.ApiRRLog.Models
{
    public class ExceptionLog
    {
        public string Message { get; set; }
        public string HelpLink { get; set; }
        public string Source { get; set; }
        public string TargetSiteName { get; set; }
        public string StackTrace { get; set; }
    }
}