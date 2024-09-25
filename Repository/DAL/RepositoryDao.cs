using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.IO;
using System.Xml;
using System.Data.SqlClient;
using Repository.HelperFunction;

namespace Repository.DAL
{
    public class RepositoryDao : UtilityHelper
    {
        System.Data.SqlClient.SqlConnection _connection;
        public RepositoryDao()
        {
            Init();
        }
        private void Init()
        {
            _connection = new System.Data.SqlClient.SqlConnection(GetConnectionString());
        }
        private void OpenConnection()
        {
            if (_connection.State == System.Data.ConnectionState.Open)
                _connection.Close();
            _connection.Open();
        }
        private void CloseConnection()
        {
            if (_connection.State == System.Data.ConnectionState.Open)
                this._connection.Close();
        }
        private string GetConnectionString()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["WebDBConnString"].ConnectionString;
        }
        public System.Data.DataSet ExecuteDataset(string sql)
        {
            var ds = new System.Data.DataSet();
            var cmd = new System.Data.SqlClient.SqlCommand(sql, _connection);
            cmd.CommandTimeout = 120;
            System.Data.SqlClient.SqlDataAdapter da;

            try
            {
                OpenConnection();
                da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                da.Dispose();
                CloseConnection();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                da = null;
                CloseConnection();
            }
            return ds;
        }
        public System.Data.DataRow ExecuteDataRow(string sql)
        {
            using (var ds = ExecuteDataset(sql))
            {
                if (ds == null || ds.Tables.Count == 0)
                    return null;

                if (ds.Tables[0].Rows.Count == 0)
                    return null;

                return ds.Tables[0].Rows[0];
            }
        }
        public System.Data.DataTable ExecuteDataTable(string sql)
        {
            using (var ds = ExecuteDataset(sql))
            {
                if (ds == null || ds.Tables.Count == 0)
                    return null;

                if (ds.Tables[0].Rows.Count == 0)
                    return null;

                return ds.Tables[0];
            }
        }
        public DataTable ExecuteDataTableSP(string sql, SqlParameter[] param, CommandType cmdType)
        {
            using (SqlConnection con = _connection)
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.CommandType = cmdType;
                    if (param != null)
                    {
                        cmd.Parameters.AddRange(param);
                    }
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandTimeout = 950;
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
        }
        public int ExecuteNonQuery(string sql)
        {
            using (SqlConnection con = _connection)
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    //OpenConnection();
                    //cmd.CommandTimeout = 950;
                    return cmd.ExecuteNonQuery();
                }
            }
        }
        public System.Collections.Generic.List<object> DropdownList(string sql)
        {
            var dt = ExecuteDataTable(sql);
            var list = new System.Collections.Generic.List<object>();
            if (null != dt)
            {
                foreach (System.Data.DataRow item in dt.Rows)
                {
                    list.Add(new { id = item["Value"], text = item["Name"].ToString() });
                }
            }
            return list;
        }

        public string DBNullToValue(object obj)
        {
            if (obj != DBNull.Value)
            {
                return obj.ToString();
            }
            return null;
        }

        public DbResponse ParseDbResponse(System.Data.DataTable dt)
        {
            var res = new DbResponse();
            if (dt.Rows.Count > 0)
            {
                res.ErrorCode = Convert.ToInt32(dt.Rows[0][0].ToString());
                res.Message = dt.Rows[0][1].ToString();
                res.Id = dt.Rows[0][2].ToString();
                if (dt.Columns.Count > 3)
                {
                    res.Extra = dt.Rows[0][3].ToString();
                }
            }
            return res;
        }
        public DbResponse ParseDbResponse(string sql)
        {
            return ParseDbResponse(ExecuteDataTable(sql));
        }
        public System.Collections.Generic.Dictionary<string, string> ParseDictionary(string sql)
        {
            var dictionary = new System.Collections.Generic.Dictionary<string, string>();
            var dt = ExecuteDataTable(sql);
            if (null == dt || dt.Rows.Count == 0)
            {
                return dictionary;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //for (int j = 0; j < dt.Columns.Count; j++)
                //{
                dictionary.Add(dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString());
                //}
            }
            return dictionary;
        }
        public System.Collections.Generic.Dictionary<string, string> ParseDictionary(DataTable dt)
        {
            var dictionary = new System.Collections.Generic.Dictionary<string, string>();
            if (null == dt || dt.Rows.Count == 0)
            {
                return dictionary;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //for (int j = 0; j < dt.Columns.Count; j++)
                //{
                dictionary.Add(dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString());
                //}
            }
            return dictionary;
        }
        public DbResponse ExceptionDbResponse(string msg)
        {
            DbResponse dr = new DbResponse()
            {
                ErrorCode = 1,
                Message = msg,
                Id = ""
            };
            return dr;
        }
        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable("dt");

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        public string GetDataTableToString(DataTable dt)
        {
            string xml = null;
            using (TextWriter writer = new StringWriter())
            {
                dt.WriteXml(writer);
                xml = writer.ToString();
            }
            return xml;
        }
        public bool IsDBConnected()
        {
            using (var db_con = new SqlConnection(GetConnectionString()))
            {
                try
                {
                    db_con.Open();
                    return true;
                }
                catch (SqlException)
                {
                    return false;
                }
            }
        }
        #region getXMLvalue
        public string getXMLValue(XmlDocument xmlObj, string NodeName)
        {
            string nodeValue = string.Empty;
            XmlNodeList xmlNodeObj = xmlObj.GetElementsByTagName(NodeName);
            if (xmlNodeObj.Count > 0)
            {
                nodeValue = xmlObj.GetElementsByTagName(NodeName).Item(0).InnerText;

            }
            else
                nodeValue = "-1";
            return nodeValue;
        }
        #endregion
        #region getXMLAttributevalue
        public string getXMLAttributevalue(XmlDocument xmlObj, string NodeName, string AttributeName)
        {
            string nodeAttributeValue = string.Empty;
            XmlNodeList xmlNodeObj = xmlObj.GetElementsByTagName(NodeName);

            if (xmlNodeObj.Count > 0)
            {
                nodeAttributeValue = xmlObj.GetElementsByTagName(NodeName).Item(0).Attributes[AttributeName].Value;

            }
            else
                nodeAttributeValue = "-1";
            return nodeAttributeValue;
        }
        #endregion
    }
}
