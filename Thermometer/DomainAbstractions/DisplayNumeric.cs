using System;
using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// ALA Domain Abstraction
    /// Ouptuts incoming data to the console with a preceding label and optional units.
    /// Has one input port of type IDataFlow which can take int, float, double
    /// The label must be passed in the constructor.
    /// The units property may be used to set the units.
    /// fixPoint Property sets the number of decimal places.
    /// You need to understand the programming paradigm abstraction, IDataFlow, to understand this code.
    /// </summary>
    class DisplayNumeric<T> : IDataFlow<T>
    {
        public DisplayNumeric(string label)
        {
            this.label = label;
        }

        public int fixPoints { get; set; } = 0;

        private string label;
        public string units { get; set; }


        void IDataFlow<T>.Push(T data)
        {
            double d = (double)Convert.ChangeType(data, typeof(double));
            Console.WriteLine($"{label}: { d.ToString($"F{fixPoints}") } {units}");
        }
    }
}
