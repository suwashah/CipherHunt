using System.ComponentModel.DataAnnotations;

namespace CipherHunt.Models
{
    public class CalorieModel
    {
        [Required(ErrorMessage = "Please select Gender")]
        public string Gender { get; set; }
        public string Height_Feet { get; set; }
        public string Height_Inch { get; set; }
        public float Weight { get; set; }
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter valid Number")]
        public int Age { get; set; }
        [Required(ErrorMessage = "Please select activity factor")]
        public string ActivityFactor { get; set; }
    }
}