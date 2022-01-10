using System;

namespace ProgrammingParadigms
{
    interface IDataFlow<T>
    {
        void Send(T data);
    }
}
