namespace BIA.Net.MVC
{
    using System;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;

    /// <summary>
    /// Summary description for Class1
    /// </summary>
    public class BigJsonResult : ActionResult
    {
        private readonly object data;

        public BigJsonResult(object data)
        {
            this.data = data;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.RequestContext.HttpContext.Response;
            response.ContentType = "application/json";
            var serializer = new JavaScriptSerializer();
            // You could set the MaxJsonLength to the desired size - 10MB in this example
            serializer.MaxJsonLength = Int32.MaxValue;
            response.Write(serializer.Serialize(this.data));
        }
    }
}