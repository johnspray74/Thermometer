using System;
using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// ALA Domain abstraction
    /// Has one DataFlow input port and one DataFlow output port, both type double
    /// Smooths the incoming data and outputs it at a lower frequency
    /// The strength parameter sets the degree of filtering (cutoff frequency relative to input frequency)
    /// and also sets the lower rate of output.
    /// e.g. if Strength is set to 10, then there is one output for every 10 input datas received.
    /// You need to understand the programming paradigm abstraction, IDataFlow, to understand this code.
    /// </summary>
    public class LowPassFilter : IDataFlow<double>
    {
        public LowPassFilter(int strength, double initialState = 0.0)
        {
            this.strength = strength;
            this.lastOutput = initialState;
        }

        // ports
        private IDataFlow<double> output;

        private int strength;
        private double lastOutput = 0.0;
        private int resampleCounter = 0;

        void IDataFlow<double>.Push(double data)
        {
            lastOutput = (data + strength * lastOutput) / (strength + 1);
            if (resampleCounter == 0)
            {
                resampleCounter += strength;
                output?.Push(lastOutput);
            }
            resampleCounter--;
        }
    }
}
