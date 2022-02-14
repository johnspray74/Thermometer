using System;
using ProgrammingParadigms;

namespace DomainAbstractions
{

    /// <summary>
    /// ALA domain abstraction to add two numeric data flows
    /// Currently only supports doubles
    /// Two input ports are implemented interfaces
    /// One output port called "output"
    /// Both inputs must receive at least one data before output begins
    /// Thereafter output occurs when either input receives data
    /// One of the inouts is type Double2, which is a struct containing a double.
    /// This is a work around for can't implement the same interface twice
    /// When wiring to the Double2 port, do it via an instance of DataFlowConvert like this:
    /// .WireIn(new DataFlowConvert<double, Double2>((d)=>new Double2(d))).WireIn(new Add());
    /// Later make a generic version for numeric primitive types (int, float, double) when C# supports generic math
    /// You need to understand the programming paradigm abstraction, IDataFlow, to understand this code.
    /// </summary>
    class Add : IDataFlow<double>, IDataFlow<Double2>
    {
        private IDataFlow<double> output;

        private double? operand1;
        private double? operand2;

        void IDataFlow<double>.Push(double data)
        {
            operand1 = data;
            if (operand2.HasValue)
            {
                output.Push(operand1.Value + operand2.Value);
            }
        }

        void IDataFlow<Double2>.Push(Double2 data)
        {
            operand2 = data.Value;
            if (operand1.HasValue)
            {
                output.Push(operand1.Value + operand2.Value);
            }
        }
    }




    /// <summary>
    /// wrap a double in a struct
    /// we do this only to get a different type of double to effectively get multple inputs for the "Add" class
    /// because C# wont allow implementing the same interface twice (it should though)
    /// </summary>
    struct Double2
    {
        public Double2(double value) { this.value = value; }

        private readonly double value;

        public double Value { get { return value; } }


        public override string ToString() => $"{value}";
    }

}