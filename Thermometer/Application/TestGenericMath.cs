using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public interface IParseable<TSelf> where TSelf : IParseable<TSelf>
    {
        static abstract TSelf Parse(string s, IFormatProvider? provider);
    }



    internal class TestGenericMath
   {
        T Add1<T>(T x) // where T : INumber<T>
        {
            // return x+x;
            return x;   
        }
    }
}
