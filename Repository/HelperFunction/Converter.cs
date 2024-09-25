using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace Repository.HelperFunction
{
    public interface IConverter
    {
        Int32 convertToInt(object input);
        Decimal convertToDecimal(object input);
        DateTime convertToDateTime(object input);
        Boolean convertToBool(object input);
        Boolean convertCharToBool(object input);
        bool isDecimal(string input);
        string DecimalRounder(string amount, int roundvalue = 2);
        string CalcPercentage(double dividend, double divisor);
        String BytesToBase64(dynamic data);
        String GetBytesImage(dynamic data);
        String GetCompressedBytesImage(dynamic data);
    }
    public class Converter : IConverter
    {
        public Int32 convertToInt(object input)
        {
            Int32 ret = 0;
            if (Int32.TryParse(input.ToString(), out ret))
            {
                ret = Convert.ToInt32(input);
            }
            return ret;
        }
        public Decimal convertToDecimal(object input)
        {
            Decimal ret = 0;
            if (Decimal.TryParse(input.ToString(), out ret))
            {
                ret = Convert.ToDecimal(input);
            }
            return ret;
        }
        public DateTime convertToDateTime(object input)
        {
            DateTime ret = new DateTime();
            if (DateTime.TryParse(input.ToString(), out ret))
            {
                ret = Convert.ToDateTime(input);
            }
            return ret;
        }
        public Boolean convertToBool(object input)
        {
            Boolean ret = false;
            if (Boolean.TryParse(input.ToString(), out ret))
            {
                ret = Convert.ToBoolean(input);
            }
            return ret;
        }

        public Boolean convertCharToBool(object input)
        {
            if (input.ToString() == "y")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool isDecimal(string input)
        {
            Decimal a;
            if (Decimal.TryParse(input, out a))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public string DecimalRounder(string amount, int roundvalue = 2)
        {
            //It returns the value of amount seperated by comma after 3 digits
            if (String.IsNullOrEmpty(amount))
            {
                amount = "0";
            }
            if (isDecimal(amount))
            {
                Decimal ConvertAmount = Decimal.Round(Convert.ToDecimal(amount), roundvalue);
                amount = ConvertAmount.ToString();
            }
            return amount;
        }
        public string CalcPercentage(double dividend, double divisor)
        {
            try
            {
                double res = (dividend / divisor) * 100;
                int res1 = Convert.ToInt32(res);
                return res1.ToString();
            }
            catch
            {
                return "";
            }
        }
        public String BytesToBase64(dynamic data)
        {
            try
            {
                return Convert.ToBase64String(data);
            }
            catch
            {

                return "";
            }
        }
        public String GetBytesImage(dynamic data)
        {
            try
            {
                string imageBase64 = Convert.ToBase64String(data);
                string imgSrc = string.Format("data:image/jpeg;base64,{0}", imageBase64);
                return imgSrc;
            }
            catch
            {

                return "";
            }
        }

        public String GetCompressedBytesImage(dynamic data)
        {
            try
            {
                using (var ms = new MemoryStream(data))
                {
                    var image = Image.FromStream(ms);

                    var ratioX = (double)350 / image.Width;
                    var ratioY = (double)250 / image.Height;
                    var ratio = Math.Min(ratioX, ratioY);

                    var width = (int)(image.Width * ratio);
                    var height = (int)(image.Height * ratio);

                    var newImage = new Bitmap(width, height);
                    Graphics.FromImage(newImage).DrawImage(image, 0, 0, width, height);
                    Bitmap bmp = new Bitmap(newImage);

                    ImageConverter converter = new ImageConverter();

                    data = (byte[])converter.ConvertTo(bmp, typeof(byte[]));
                    return "data:image/jpeg;base64," + Convert.ToBase64String(data);
                }
            }
            catch
            {
                return "";
            }

        }

    }

    public static class CurrencyCodeMapper
    {
        private static readonly Dictionary<string, string> SymbolsByCode;
        public static string GetSymbol(string code) { return SymbolsByCode[code]; }
        static CurrencyCodeMapper()
        {
            SymbolsByCode = new Dictionary<string, string>();
            var regions = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                          .Select(x => new RegionInfo(x.LCID));
            foreach (var region in regions)
                if (!SymbolsByCode.ContainsKey(region.ISOCurrencySymbol))
                    SymbolsByCode.Add(region.ISOCurrencySymbol, region.CurrencySymbol);
        }
    }
}
