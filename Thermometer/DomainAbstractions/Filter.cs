using System;
using ProgrammingParadigms;

namespace DomainAbstractions
{
    class Filter : IDataFlow<double>
    {
        public Filter(double strength)
        {
            this.strength = strength;
        }

        // ports
        private IDataFlow<double> output;

        private double strength;
        private double state = 0.0;
        private double slowOuptut = 0;

        void IDataFlow<double>.Send(double data)
        {
            state = strength * data + (1.0-strength) * state;
            slowOuptut += strength;
            if (slowOuptut > 1.0)
            {
                slowOuptut -= 1.0;
                output.Send(state);
            }
        }
    }
}
