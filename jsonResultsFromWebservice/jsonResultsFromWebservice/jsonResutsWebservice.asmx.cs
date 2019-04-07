using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;

namespace jsonResultsFromWebservice
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class jsonResutsWebservice : System.Web.Services.WebService
    {

        [WebMethod]
        public void queryToReturnResultsWithSequence(string input)
        {
            getResultsFromWebservice.I_GetResultsClient client = new getResultsFromWebservice.I_GetResultsClient();
            string result = client.queryToReturnResultsWithSequence(input);
            client.Close();
            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.Write(result);
        }

        [WebMethod]
        public void queryToReturnResults(string input)
        {
            getResultsFromWebservice.I_GetResultsClient client = new getResultsFromWebservice.I_GetResultsClient();
            string result = client.queryToReturnResults(input);
            client.Close();
            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.Write(result);
        }
    }
}
