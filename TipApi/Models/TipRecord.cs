namespace TipApi.Models
{
    public class TipRecord
    {
        public int Id { get; set; }
        public double TotalBill { get; set; }
        public double Tip { get; set; }
        public string Sex { get; set; } = string.Empty;
        public string Smoker { get; set; } = string.Empty;
        public string Day { get; set; } = string.Empty;
        public string Time { get; set; } = string.Empty;
        public int Size { get; set; }
    }
}
