using Repository.HelperFunction;
using System;

namespace Repository.Common
{
    public class CalorieInput
    {
        public string Gender { get; set; }
        public int Age { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public double ActivityFactor { get; set; }
    }
    public class CalorieOutput : CommonData
    {
        public string MaintainWeight { get; set; }
        public string MaintainWeightPer { get; set; }
        public string MildWeightLoss { get; set; }
        public string MildWeightPer { get; set; }
        public string WeightLoss { get; set; }
        public string WeightLossPer { get; set; }
        public string ExtremeWeightLoss { get; set; }
        public string ExtremeWeightper { get; set; }
    }
    public interface ICalorieCalculator
    {
        CalorieOutput Calculate(CalorieInput inp);
    }
    public class CalorieCalculator : ICalorieCalculator
    {
        Converter con = new Converter();
        public CalorieOutput Calculate(CalorieInput inp)
        {
            CalorieOutput ret = new CalorieOutput();
            int const_Set = 5;
            if (inp.Gender == "F")
            {
                const_Set = -161;
            }
            #region Default region
            ret.MaintainWeight = "0";
            ret.MildWeightLoss = "0";
            ret.WeightLoss = "0";
            ret.ExtremeWeightLoss = "0";

            ret.MaintainWeightPer = "0%";
            ret.MildWeightPer = "0%"; //100%
            ret.WeightLossPer = "0%"; //100%
            ret.ExtremeWeightper = "0%"; //100%
            #endregion
            try
            {
                var BMR = (10 * inp.Weight) + (6.25 * inp.Height) - (5 * inp.Age) + const_Set;
                var Calorie = BMR * inp.ActivityFactor;
                int CalorieResult = Convert.ToInt32(Calorie);
                if (CalorieResult <= 0)
                {
                    ret.CODE = "400";
                    ret.MESSAGE = "Conversion fail";
                    return ret;
                }
                if (CalorieResult < 1000)
                {
                    ret.CODE = "401";
                    ret.MESSAGE = "You probably do not need to lose weight!";
                    return ret;
                }
                var MildWtLoss = CalorieResult - 250;
                var WtLoss = MildWtLoss - 250;
                var ExtWtLoss = WtLoss - 500;

                ret.MaintainWeight = CalorieResult.ToString();
                ret.MildWeightLoss = MildWtLoss.ToString();
                ret.WeightLoss = WtLoss.ToString();
                ret.ExtremeWeightLoss = ExtWtLoss.ToString();

                ret.MaintainWeightPer = con.CalcPercentage(CalorieResult, CalorieResult) + "%"; //100%
                ret.MildWeightPer = con.CalcPercentage(MildWtLoss, CalorieResult) + "%"; //100%
                ret.WeightLossPer = con.CalcPercentage(WtLoss, CalorieResult) + "%"; //100%
                ret.ExtremeWeightper = con.CalcPercentage(ExtWtLoss, CalorieResult) + "%"; //100%
                ret.MESSAGE = "Success";
                ret.CODE = "0";
            }
            catch
            {
                ret.CODE = "400";
                ret.MESSAGE = "Conversion fail";
            }
            return ret;
        }
    }
}