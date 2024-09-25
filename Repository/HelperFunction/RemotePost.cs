using System.Collections.Specialized;
using System.Text;

namespace Repository.HelperFunction
{
    public class RemotePost
    {
        public RemotePost()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private NameValueCollection Inputs = new NameValueCollection();
        public string Url = "";
        public string Method = "";
        public string FormName = "";


        public void AddField(string name, string value)
        {
            Inputs.Add(name, value);
        }

        public string Post()
        {
            string returnData = string.Empty;

            StringBuilder sb = new StringBuilder();

            if (Method.ToLower() == "post")
            {
                sb.Append("<html>");
                sb.AppendFormat(@"<body onload='document.forms[" + "\"" + FormName + "\"" + "].submit()'>");
                sb.AppendFormat("<form name='" + FormName + "' action='" + Url + "' method='post'>");
                for (int i = 0; i < Inputs.Keys.Count; i++)
                {
                    System.Web.HttpContext.Current.Response.Write(string.Format("", Inputs.Keys[i], Inputs[Inputs.Keys[i]]));
                    sb.AppendFormat("<input type='hidden' name='" + Inputs.Keys[i] + "' value='" + Inputs[Inputs.Keys[i]] + "'>");
                }

                sb.Append("</form>");
                sb.Append("</body>");
                sb.Append("</html>");
            }

            if (Method.ToLower() == "get")
            {
                sb.Append(Url);
                sb.AppendFormat("?");

                for (int i = 0; i < Inputs.Keys.Count; i++)
                {
                    System.Web.HttpContext.Current.Response.Write(string.Format("", Inputs.Keys[i], Inputs[Inputs.Keys[i]]));
                    if (i > 0)
                    {
                        sb.AppendFormat("&");
                    }
                    sb.AppendFormat(Inputs.Keys[i] + "=" + Inputs[Inputs.Keys[i]]);
                }
            }


            returnData = sb.ToString();
            return returnData;
        }

        public string GetUrl()
        {
            string returnData = string.Empty;

            StringBuilder sb = new StringBuilder();

            sb.Append(Url);
            sb.AppendFormat("?");

            for (int i = 0; i < Inputs.Keys.Count; i++)
            {
                System.Web.HttpContext.Current.Response.Write(string.Format("", Inputs.Keys[i], Inputs[Inputs.Keys[i]]));
                if (i > 0)
                {
                    sb.AppendFormat("&");
                }
                sb.AppendFormat(Inputs.Keys[i] + "=" + Inputs[Inputs.Keys[i]]);
            }

            returnData = sb.ToString();
            return returnData;
        }

        public string digipay_redirection_url()
        {
            return "";
        }
        public string GetRedirectionUrl()
        {
            string returnData = string.Empty;

            StringBuilder sb = new StringBuilder();

            if (Method.ToLower() == "post")
            {
                sb.Append("<html>");
                sb.AppendFormat("<script src=" + "\"" + "/Scripts/jquery-1.11.1.min.js" + "\"" + " type=" + "\"" + "text/javascript" + "\"" + "></script>");
                sb.AppendFormat("<script src=" + "\"" + "/Scripts/blockUI.js" + "\"" + " type=" + "\"" + "text/javascript" + "\"" + "></script>");
                sb.AppendFormat("<script type=" + "\"" + "text/javascript" + "\"" + ">");
                sb.Append("$(document).ready(function () {$.blockUI({css: { border: 'none',padding: '15px',backgroundColor: '#000','-webkit-border-radius': '10px','-moz-border-radius': '10px',opacity: .5,color: '#fff'}})});");
                sb.Append("</script>");

                sb.AppendFormat(@"<body onload='document.forms[" + "\"" + FormName + "\"" + "].submit()'>");
                sb.AppendFormat("<form name='" + FormName + "' action='" + Url + "' method='post' >");
                for (int i = 0; i < Inputs.Keys.Count; i++)
                {
                    System.Web.HttpContext.Current.Response.Write(string.Format("", Inputs.Keys[i], Inputs[Inputs.Keys[i]]));
                    sb.AppendFormat("<input type='hidden'" + " name='" + Inputs.Keys[i] + "' value='" + Inputs[Inputs.Keys[i]] + "'>");
                }
                sb.Append("</form>");
                sb.Append("</body>");
                sb.Append("</html>");
            }

            if (Method.ToLower() == "get")
            {
                sb.Append(Url);
                sb.AppendFormat("?");

                for (int i = 0; i < Inputs.Keys.Count; i++)
                {
                    System.Web.HttpContext.Current.Response.Write(string.Format("", Inputs.Keys[i], Inputs[Inputs.Keys[i]]));
                    if (i > 0)
                    {
                        sb.AppendFormat("&");
                    }
                    sb.AppendFormat(Inputs.Keys[i] + "=" + Inputs[Inputs.Keys[i]]);
                }
            }

            returnData = sb.ToString();
            return returnData;
        }
    }
}
