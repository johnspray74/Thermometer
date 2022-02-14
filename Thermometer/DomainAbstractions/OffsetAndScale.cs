using System;
using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// ALA domain abstraction
    /// Has one input port of type IDataflow and one output port of type IDataflow (both type double)
    /// Performs y = m(x+c) like operation where x is the input and y is the output
    /// If visualized as a straight line on an x,y graph, -c is the x axis intercept and m is the slope. 
    /// You need to understand the programming paradigm abstraction, IDataFlow, to understand this code.
    /// </summary>
    class OffsetAndScale : IDataFlow<double>
    {
        public OffsetAndScale(double offset, double scale)
        {
            this.offset = offset;
            this.scale = scale;
        }

        private double offset;
        private double scale;


        private IDataFlow<double> output;

        void IDataFlow<double>.Push(double data)
        {
            output.Push((data + offset) * scale);
        }


    }
}
