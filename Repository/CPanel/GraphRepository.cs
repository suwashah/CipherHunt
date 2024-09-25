using Repository.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace Repository.CPanel
{
    public interface IGraphRepository
    {
        string CreateGraphString(string chartOBj, string chart_type, string dataLabel, DataTable dt, string Caption, string xAxisName, string yAxisName, bool disableScale = false, int borderWidth = 1, string legendPosition = "top");
        dynamic CreateDataSetGraph(DataTable dt, string caption, string chartType, int borderWidth);
        string DrawWeeklyCustomerGraph(string chartObj, string chartType);
    }
    public class GraphRepository : IGraphRepository
    {
        RepositoryDao dao;
        JavaScriptSerializer js = new JavaScriptSerializer();
        public GraphRepository()
        {
            dao = new RepositoryDao();
        }
        private string[] ColorsCollection(bool isBorder = false)
        {
            string[] ret = new string[]
            {
                "rgba(96, 196, 150, 1)",
                "rgba(0, 188, 212, 1)",
                "rgba(116, 199, 209, 1)",
                "rgba(255, 82, 82, 1)",
                "rgba(0, 53, 169, 0.36)",
                "rgba(196, 37, 15, 0.66)"
            };
            if (isBorder == true)
            {
                ret = new string[]
                {
                    "rgb(255, 82, 82)",
                    "rgb(96, 196, 150)",
                    "rgb(0, 188, 212)",
                    "rgb(116, 199, 209)",
                    "rgb(76, 175, 80)",
                    "rgb(227, 136, 27)"
                };
            }

            return ret;
        }
        public string CreateGraphString(string chartOBj, string chart_type, string dataLabel, DataTable dt, string Caption, string xAxisName, string yAxisName, bool disableScale = false, int borderWidth = 2, string legendPosition = "top")
        {
            String[] Labels = dt.AsEnumerable().Select(r => r.Field<string>(dataLabel)).ToArray();
            int cutoutPercentage = 75;
            bool responsive = true;
            string fontcolor = "white";
            bool display = true;
            bool yAxesBeginAtZero = true;
            bool xAxesBeginAtZero = true;

            StringBuilder sb = new StringBuilder();
            #region SCRIPT STARTED
            //sb.AppendFormat(@"<canvas id=""{0}"" aria-label=""{1}"" role=""img""></canvas>",chartOBj,Caption);
            sb.AppendFormat(@"<script type=""text/javascript"">");
            sb.AppendFormat(@"var color = Chart.helpers.color;");
            sb.AppendFormat(@"var bg1=""rgba(196, 37, 15, 0.66)"";");
            sb.AppendFormat(@"var bg2=""rgba(0, 53, 169, 0.36)"";");
            sb.AppendFormat(@"var ctx = document.getElementById({0});", dao.singleQuote(chartOBj));
            sb.AppendFormat(@"var myChart = null;");
            sb.AppendFormat(@"if(myChart !== null){{removeData(myChart);}}");
            #region CHART REGION STARTED
            sb.AppendFormat(@"myChart = new Chart(ctx, {{");//CHART STARTED
            sb.AppendFormat(@"type:{0},", dao.singleQuote(chart_type));
            #region DataRegion
            sb.AppendFormat(@"data: {{");//Data Start
            sb.AppendFormat(@"labels:{0},", js.Serialize(Labels));
            #region DataSetRegion
            sb.AppendFormat(@"datasets: {0},", CreateDataSetGraph(dt, Caption, chart_type, borderWidth));//DataSet Start
            #endregion
            sb.AppendFormat(@"onAnimationComplete: function () {{ctx.fillText(""Hello"", 100 - 20, 100, 200);}}");
            sb.AppendFormat(@"}},");//Data End
            #endregion
            #region OPTIONS
            sb.AppendFormat(@"options:{{");//Option Starts
            if (chart_type == "doughnut")
            {
                sb.AppendFormat(@"cutoutPercentage:{0},", cutoutPercentage);
            }
            sb.AppendFormat(@"responsive:{0},", responsive.ToString().ToLower());
            #region Legends
            sb.AppendFormat(@"legend:{{");//legend start
            sb.AppendFormat(@"position:{0},", dao.singleQuote(legendPosition));
            sb.AppendFormat(@"fontColor:{0},", dao.singleQuote(fontcolor));
            sb.AppendFormat(@"}},");//end legend
            #endregion
            #region Title
            sb.AppendFormat(@"title:{{");//Title start
            sb.AppendFormat(@"display:{0},", display.ToString().ToLower());
            sb.AppendFormat(@"text:{0},", dao.singleQuote(Caption));
            sb.AppendFormat(@"}},");//end Title
            #endregion
            if (disableScale == false)
            {
                sb.AppendFormat(@"scales:{{");
                #region Y-Axis
                sb.AppendFormat(@"yAxes: [{{");
                sb.AppendFormat("ticks:{{");
                sb.AppendFormat(@"beginAtZero: {0}", yAxesBeginAtZero.ToString().ToLower());
                sb.AppendFormat(@"}},");
                sb.AppendFormat(@"scaleLabel:{{");
                sb.AppendFormat(@"display: true,");
                sb.AppendFormat(@"labelString:{0}", dao.singleQuote(yAxisName));
                sb.AppendFormat(@"}}");
                sb.AppendFormat(@"}}],");
                #endregion
                #region X-Axis
                sb.AppendFormat(@"XAxes: [{{");
                sb.AppendFormat("ticks:{{");
                sb.AppendFormat(@"beginAtZero: {0}", xAxesBeginAtZero.ToString().ToLower());
                sb.AppendFormat(@"}},");
                sb.AppendFormat(@"scaleLabel:{{");
                sb.AppendFormat(@"display: true,");
                sb.AppendFormat(@"labelString:{0}", dao.singleQuote(xAxisName));
                sb.AppendFormat(@"}}");
                sb.AppendFormat(@"}}],");
                #endregion
                sb.AppendFormat(@"}},");
            }
            #region Tooltips
            sb.AppendFormat(@"tooltips:{{");//Tooltips start
            sb.AppendFormat(@"mode:{0},", dao.singleQuote("point"));
            sb.AppendFormat(@"axis:{0},", dao.singleQuote("y"));
            sb.AppendFormat(@"}}");//end Tooltips
            #endregion
            sb.AppendFormat(@"}}");//end Options
            #endregion
            sb.AppendFormat(@"}});");//CHART ENDED
            #endregion
            sb.AppendFormat(@"</script>");
            #endregion
            //return sb.ToString();
            //HttpContext.Current.Response.Write((sb.ToString()));
            return sb.ToString();
        }
        public dynamic CreateDataSetGraph(DataTable dt, string caption, string chartType, int borderWidth)
        {
            Random random = new Random();
            List<string> colors = new List<string>();
            List<string> borders = new List<string>();
            dynamic bg = null;
            dynamic border = null;
            bool bgFill = false;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            int start = random.Next(0, ColorsCollection().Length);
            if (chartType == "doughnut" || chartType == "pie")
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    colors.Add(ColorsCollection()[i]);
                }
                bg = colors;
                bgFill = true;
            }
            List<object> ard = new List<object>();
            List<Dictionary<object, object>> rows = new List<Dictionary<object, object>>();
            Dictionary<object, object> row;
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                ard = new List<object>();
                row = new Dictionary<object, object>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (j != 0)
                    {
                        ard.Add(dt.Rows[i][j]);
                    }
                }
                if (chartType == "line" || chartType == "bar")
                {
                    start = random.Next(0, ColorsCollection().Length);
                    bg = ColorsCollection()[start];
                    border = ColorsCollection()[start];
                }
                if (ard.ToArray().Length != 0)
                {
                    row.Add("label", caption + ":" + dt.Columns[j].ColumnName);
                    row.Add("data", ard);
                    row.Add("fill", bgFill.ToString().ToLower());
                    //row.Add("fill", false);
                    row.Add("borderColor", border);
                    row.Add("backgroundColor", bg);
                    row.Add("borderWidth", borderWidth);
                    row.Add("pointRadius", 4);
                    row.Add("pointHoverRadius", 4);
                    row.Add("pointBorderWidth", 8);
                    rows.Add(row);
                }
            }
            return serializer.Serialize(rows);
        }
        public string DrawWeeklyCustomerGraph(string chartObj, string chartType)
        {
            DataTable dt = dao.ExecuteDataTable("spa_matrix_report @flag='1'");
            if (dt != null && dt.Rows.Count > 0)
            {
                return CreateGraphString(chartObj, chartType, "dw", dt, "Weekly customer graph", "Week Name", "No. of registered customer", false, 2);
            }
            else
            {
                return "";
            }
        }
    }
}
