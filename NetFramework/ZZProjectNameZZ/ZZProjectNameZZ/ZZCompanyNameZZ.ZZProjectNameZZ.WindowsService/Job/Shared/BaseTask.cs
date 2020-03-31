using System;
using System.Linq.Expressions;
using Hangfire;

namespace ZZCompanyNameZZ.ZZProjectNameZZ.WindowsService.Job.Shared
{
    public interface IBaseJob
    { 
        void Run();
    }
    public class BaseJob : IBaseJob
    {
        protected static string projectName = "ZZCompanyNameZZ.ZZProjectNameZZ";
        protected void launchMySelf(string functionName, Expression<Action> action, string cronExpression)
        {
            RecurringJob.AddOrUpdate($"{projectName}.{functionName}",
                action
                , cronExpression);
        }


        void IBaseJob.Run()
        {
            throw new NotImplementedException();
        }
    }
}
