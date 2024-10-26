using Repository.Common;
using Repository.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace Repository.Challenge
{
    public interface IChallengeRepository
    {
        CommonData SaveChallenge(TblChallenge inp);
        List<TblChallenge> GetAllChallenges(string flag);
        TblChallenge GetChallengeDetail(string Challenge_ID);
        CommonData VerifyChallenge(string ID, string UserName);
        CommonData SubmitFlag(SubmitUserFlag inp);
    }
    public class ChallengeRepository : IChallengeRepository
    {
        RepositoryDao dao;
        public ChallengeRepository()
        {
            dao = new RepositoryDao();
        }    

        public CommonData SaveChallenge(TblChallenge inp)
        {
            var ret = new CommonData();
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@flag",inp.FLAG),
                new SqlParameter("@CHALLENGE_ID",inp.CHALLENGE_ID),
                new SqlParameter("@CAT_ID",inp.CAT_ID),
                new SqlParameter("@NAME",inp.NAME),
                new SqlParameter("@DESCRIPTION",inp.DESCRIPTION),
                new SqlParameter("@DIFFICULTY_LEVEL",inp.DIFFICULTY_LEVEL),
                new SqlParameter("@POINTS",inp.POINTS),
                new SqlParameter("@IMAGE",inp.IMAGE),
                new SqlParameter("@IMAGENAME",inp.IMAGENAME),
                new SqlParameter("@CHALLENGE_URL",inp.CHALLENGE_URL),
                new SqlParameter("@FILE_PATH",inp.FILE_PATH),
                new SqlParameter("@CTF_FLAG",inp.CTF_FLAG),
                new SqlParameter("@IS_ENABLE",inp.IS_ENABLE),
                new SqlParameter("@CREATE_BY",inp.CREATE_BY),
                new SqlParameter("@UPDATE_BY",inp.UPDATE_BY),
                new SqlParameter("@HINT_1",inp.HINT_1),
                new SqlParameter("@HINT_2",inp.HINT_2),
                new SqlParameter("@HINT_3",inp.HINT_3),
                new SqlParameter("@INTENDED_LEARNING",inp.INTENDED_LEARNING),
                new SqlParameter("@CHALLENGE_SOLUTION",inp.CHALLENGE_SOLUTION)
            };
            //sqlParam.DbType = DbType.Binary;
            DataTable dt = dao.ExecuteDataTableSP("[dbo].[spa_Challenge]", param, CommandType.StoredProcedure);
            if (dt != null && dt.Rows.Count > 0)
            {
                ret.CODE = dt.Rows[0]["Code"].ToString();
                ret.MESSAGE = dt.Rows[0]["Message"].ToString();
            }
            else
            {
                ret.CODE = "404";
                ret.MESSAGE = "Data not found";
            }
            return ret;
        }
        public List<TblChallenge> GetAllChallenges(string flag)
        {
            var lst = new List<TblChallenge>();
            string sql = "spa_Challenge @flag=" + dao.singleQuote(flag);
            DataTable dt = dao.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    lst.Add(new TblChallenge
                    {
                        CHALLENGE_ID = dr["CHALLENGE_ID"].ToString(),
                        CAT_ID = dr["CAT_ID"].ToString(),
                        CATEGORY_NAME = dr["CATEGORY_NAME"].ToString(),
                        NAME = dr["NAME"].ToString(),
                        DESCRIPTION = dr["DESCRIPTION"].ToString(),
                        POINTS =dr["POINTS"].ToString(),
                        CTF_FLAG= dr["FLAG"].ToString(),
                        IMAGE_URL = GetChallengeImage(dr["IMAGENAME"].ToString()),
                        CHALLENGE_URL = dr["CHALLENGE_URL"].ToString(),
                        IMAGENAME = dr["IMAGENAME"].ToString(),
                        STATUS = dr["STATUS"].ToString(),
                        IS_ENABLE = Convert.ToBoolean(dr["IS_ENABLE"]),
                        CREATE_BY = dr["CREATE_BY"].ToString(),
                        CREATE_TS = dr["CREATE_TS"].ToString(),
                        UPDATE_BY = dr["UPDATE_BY"].ToString(),
                        UPDATE_TS = dr["UPDATE_TS"].ToString(),
                        IS_VERIFIED = Convert.ToBoolean(dr["IS_VERIFIED"]),
                        VERIFIED_BY = dr["VERIFIED_BY"].ToString(),
                        HINT_1 = dr["HINT_1"].ToString(),
                        HINT_2 = dr["HINT_2"].ToString(),
                        HINT_3 = dr["HINT_3"].ToString(),
                        INTENDED_LEARNING = dr["INTENDED_LEARNING"].ToString(),
                        CHALLENGE_SOLUTION = dr["CHALLENGE_SOLUTION"].ToString(),
                        DIFFICULTY_LEVEL = dr["DIFFICULTY_LEVEL"].ToString(),
                    });
                }
            }
            return lst;
        }
        public TblChallenge GetChallengeDetail(string Challenge_ID)
        {
            var lst = new TblChallenge();
            string sql = "spa_Challenge @flag='s'" +
                ",@Challenge_ID=" + dao.singleQuote(Challenge_ID);
            DataTable dt = dao.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                lst.CHALLENGE_ID = dt.Rows[0]["CHALLENGE_ID"].ToString();
                lst.CAT_ID = dt.Rows[0]["CAT_ID"].ToString();
                lst.NAME = dt.Rows[0]["NAME"].ToString();
                lst.CATEGORY_NAME = dt.Rows[0]["CATEGORY_NAME"].ToString();
                lst.DESCRIPTION = dt.Rows[0]["DESCRIPTION"].ToString();
                lst.CREATE_TS = dt.Rows[0]["CREATE_TS"].ToString();
                lst.CREATE_BY = dt.Rows[0]["CREATE_BY"].ToString();
                lst.UPDATE_TS = dt.Rows[0]["UPDATE_TS"].ToString();
                lst.UPDATE_BY = dt.Rows[0]["UPDATE_BY"].ToString();
                lst.DIFFICULTY_LEVEL = dt.Rows[0]["DIFFICULTY_LEVEL"].ToString();
                lst.POINTS = dao.DecimalRounder(dt.Rows[0]["POINTS"].ToString());              
                lst.IMAGE_URL = GetChallengeImage(dt.Rows[0]["IMAGENAME"].ToString());
                lst.CHALLENGE_URL = dt.Rows[0]["CHALLENGE_URL"].ToString();
                lst.FILE_PATH = dt.Rows[0]["FILE_PATH"].ToString();
                lst.IMAGENAME = dt.Rows[0]["IMAGENAME"].ToString();
                lst.STATUS = dt.Rows[0]["STATUS"].ToString();
                lst.IS_ENABLE = Convert.ToBoolean(dt.Rows[0]["IS_ENABLE"]);
                lst.IS_VERIFIED = Convert.ToBoolean(dt.Rows[0]["IS_VERIFIED"]);
                lst.VERIFIED_BY = dt.Rows[0]["VERIFIED_BY"].ToString();
                lst.CTF_FLAG = dt.Rows[0]["FLAG"].ToString();
                lst.HINT_1 = dt.Rows[0]["HINT_1"].ToString();
                lst.HINT_2 = dt.Rows[0]["HINT_2"].ToString();
                lst.HINT_3 = dt.Rows[0]["HINT_3"].ToString();
                lst.INTENDED_LEARNING = dt.Rows[0]["INTENDED_LEARNING"].ToString();
                lst.CHALLENGE_SOLUTION = dt.Rows[0]["CHALLENGE_SOLUTION"].ToString();
            }
            return lst;
        }
        public CommonData VerifyChallenge(string ID, string UserName)
        {
            var lst = new CommonData();
            string sql = "spa_Challenge @flag='ve'" +
                ",@CHALLENGE_ID = " + dao.singleQuote(ID) +
                ",@UPDATE_BY = " + dao.singleQuote(UserName);
            DataTable dt = dao.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                lst.CODE = dt.Rows[0]["Code"].ToString();
                lst.MESSAGE = dt.Rows[0]["Message"].ToString();
            }
            return lst;
        }
       
        private string GetChallengeImage(string ImageName)
        {
            string img = "";
            if (dao.GetAppsettingValue("SaveImageBytes") == "n")
            {
                img = dao.GetImageBase64FromPath(ImageName, dao.GetAppsettingValue("ChallengePath"));
                img = dao.GetAppsettingValue("ChallengePath") + "/" + ImageName;
            }
            else
            {
                img = dao.GetCompressedBytesImage(ImageName);
            }
            return img;
        }

        public CommonData SubmitFlag(SubmitUserFlag inp)
        {
            CommonData ret = new CommonData();
            string sql = "spa_Challenge @flag='sf'"+
                ",@CHALLENGE_ID = " + dao.singleQuote(inp.CHALLENGE_ID) +
                ",@USER_ID = " + dao.singleQuote(inp.USER_ID) +
                ",@CTF_FLAG=" + dao.singleQuote(inp.USER_FLAG);
            DataTable dt = dao.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                ret.CODE = dt.Rows[0]["Code"].ToString();
                ret.MESSAGE = dt.Rows[0]["Message"].ToString();
            }
            return ret;
        }
    }
}
