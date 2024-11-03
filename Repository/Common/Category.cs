using System;
using System.Collections.Generic;

namespace Repository.Common
{
    public class Category
    {
        public String FLAG { get; set; }
        public String CAT_ID { get; set; }
        public String CATEGORY_NAME { get; set; }
        public String CATEGORY_DESCRIPTION { get; set; }
        public byte[] CATEGORY_IMAGE { get; set; }
        public String CREATE_BY { get; set; }
        public String CREATE_TS { get; set; }
        public Boolean IS_ENABLE { get; set; }
    }
    public class TblProduct
    {
        public String FLAG { get; set; }
        public String PRODUCTID { get; set; }
        public String CAT_ID { get; set; }
        public String CATEGORY_NAME { get; set; }
        public String CATEGORY_TAB { get; set; }
        public String NAME { get; set; }
        public String DESCRIPTION { get; set; }

        public String PRICE { get; set; }
        public String CURRENCY { get; set; }

        public byte[] IMAGE { get; set; }
        public String IMAGENAME { get; set; }

        public String PRODUCTURL { get; set; }

        public String DATEAVAILABLE { get; set; }

        public String CHANGEDATE { get; set; }
        public String STATUS { get; set; }

        public Boolean IS_ENABLE { get; set; }

        public String CREATE_TS { get; set; }
        public String CREATE_BY { get; set; }

        public String UPDATE_TS { get; set; }
        public String UPDATE_BY { get; set; }

        public Boolean IS_VERIFIED { get; set; }
        public String VERIFIED_BY { get; set; }
        public String CALORIES { get; set; }

    }

    public class TblChallenge
    {
        public String CHALLENGE_ID { get; set; }
        public String CAT_ID { get; set; }
        public String CATEGORY_NAME { get; set; }
        public String CATEGORY_TAB { get; set; }
        public String NAME { get; set; }
        public String DESCRIPTION { get; set; }
        public String DIFFICULTY_LEVEL { get; set; }
        public String POINTS { get; set; }
        public byte[] IMAGE { get; set; }
        public String IMAGENAME { get; set; }
        public String IMAGE_URL { get; set; }
        public String CHALLENGE_URL { get; set; }
        public String FILE_PATH { get; set; }
        public String CTF_FLAG { get; set; }

        public String FLAG { get; set; }
        public String DATEAVAILABLE { get; set; }
        public String CHANGEDATE { get; set; }
        public String STATUS { get; set; }
        public Boolean IS_ENABLE { get; set; }
        public String CREATE_TS { get; set; }
        public String CREATE_BY { get; set; }
        public String UPDATE_TS { get; set; }
        public String UPDATE_BY { get; set; }
        public Boolean IS_VERIFIED { get; set; }
        public String VERIFIED_BY { get; set; }
        public String HINT_1 { get; set; }
        public String HINT_2 { get; set; }
        public String HINT_3 { get; set; }
        public String INTENDED_LEARNING { get; set; }
        public String CHALLENGE_SOLUTION { get; set; }
        public String USER_SCORE { get; set; }
        public String SOLVED_AT { get; set; }
        public String TOTAL_SOLVES { get; set; }
        public String CHALLENGE_FOLDER { get; set; }
    }

    public class SubmitUserFlag
    {
        public String CHALLENGE_ID { get; set; }
        public String USER_FLAG { get; set; }
        public String USER_ID { get; set; }
    }

    public class ScoreBoard
    {
        public String TOTAL_SCORE { get; set; }
        public String PLAYER { get; set; }
        public String PLAYER_ID { get; set; }  
        public String TIME_STAMPS { get; set; }
        public int SCORE { get; set; }


    }
    public class TeamScore
    {
        public string TeamName { get; set; }
        public List<int> Scores { get; set; }
        public List<string> Timestamps { get; set; }
    }
    public class ScoreAndTime
    {
        public int Score { get; set; }
        public string Time { get; set; }

    }
}
