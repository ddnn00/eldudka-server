namespace EldudkaServer.Models.VO
{
    public class LabelObjectValuesVO
    {
        public string Label { get; set; }
        public IEnumerable<object> Values { get; set; }
    }
}
