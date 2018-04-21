namespace DotFlow.Example
{
    class ProcessObject
    {
        public string Value { get; set; }
        public string Name { get; set; }

        public ProcessObject(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
