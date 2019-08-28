using Project.Domain.SeedWork;

namespace Project.Domain.AggregatesModel
{
    public class ProjectProperty:ValueObject
    {
        public ProjectProperty() { }

        public ProjectProperty(string key, string text, string value)
        {
            Key = key;
            Text = text;
            Value = value;
        }
        public int  ProjectId { get; set; }
        public string  Key { get; set; }
        public string  Text { get; set; }
        public string  Value { get; set; }
    }
}