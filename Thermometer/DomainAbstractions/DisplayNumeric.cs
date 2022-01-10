using System;
using ProgrammingParadigms;

namespace DomainAbstractions
{
    class DisplayNumeric<T> : IDataFlow<T>
    {
        public DisplayNumeric(string label)
        {
            this.label = label;
        }

        public int fixPoints { get; set; } = 0;
        
        private string label;

        void IDataFlow<T>.Send(T data)
        {
            Console.Write("\r" + label + ":" + Math.Round((double)Convert.ChangeType(data,typeof(double)), fixPoints));
        }
    }
}
