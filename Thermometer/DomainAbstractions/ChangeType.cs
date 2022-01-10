using System;
using ProgrammingParadigms;

namespace DomainAbstractions
{
    class ChangeType<T1, T2> : IDataFlow<T1>
    {
        private IDataFlow<T2> output;

        void IDataFlow<T1>.Send(T1 data)
        {
            output.Send((T2)Convert.ChangeType(data, typeof(T2)));
        }
    }
}
