using System.Net;
using Hangfire;
using ZZCompanyNameZZ.ZZProjectNameZZ.WindowsService.Job.Shared;

namespace ZZCompanyNameZZ.ZZProjectNameZZ.WindowsService.Job
{

    class WakeUpTask : BaseJob
    {
        public WakeUpTask()
        {

        }

        [Queue("zzprojectnamezz")]
        [AutomaticRetry(Attempts = 0)]
        public void Run()
        {
            var webRequest = WebRequest.Create("https://simudm.eu.labinal.snecma/ZZProjectNameZZ");
            webRequest.Credentials = CredentialCache.DefaultCredentials;
            HttpWebResponse response = (HttpWebResponse) webRequest.GetResponse();
        }
    }
}