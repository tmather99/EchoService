namespace GloboTicket.Catalog.Model
{
    public class ExampleContract
    {
        /// <summary>
        /// SimpleProperty description.
        /// </summary>
        public string SimpleProperty { get; set; }

        public ExampleContractInnerContract ComplexProperty { get; set; }

        /// <summary>
        /// SimpleCollection description.
        /// </summary>
        public ICollection<string> SimpleCollection { get; set; }

        /// <summary>
        /// ComplexCollection description.
        /// </summary>
        public ICollection<ExampleContractArrayInnerContract> ComplexCollection { get; set; }
    }


    public class ExampleContractInnerContract
    {
        /// <summary>
        /// Name description.
        /// </summary>
        public string Name { get; set; }
    }

    public class ExampleContractArrayInnerContract
    {
        /// <summary>
        /// Name description.
        /// </summary>
        public string Name { get; set; }
    }
}
