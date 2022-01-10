using System;
using ProgrammingParadigms;

namespace DomainAbstractions
{
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

        void IDataFlow<double>.Send(double data)
        {
            output.Send(data * scale + offset);
        }


    }
}
