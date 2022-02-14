using System;

namespace ProgrammingParadigms
{
    interface IDataFlow<T>
    {
        void Push(T data);
    }








    public delegate void DataChangedDelegate();



    /// <summary>
    /// A reversed IDataFlow.
    /// IDataFlow pushes data to the destination whereas IDataFlowPull pulls data from source.
    /// The DataChanged event will notify the destination when change happens.
    /// This used if you really want to pull data instead of push it. We default to pushing, but pulling may be preferred in some cases for performance reasons
    /// when the source data changes much more frequently than we are interested in, or calculating the source data is expensive so we want to do it only on demand.
    /// IDataFlowPush can also be used to solve the problem that a class can't imlement an interface of the same type more than once.
    /// For example, consider implementing an AND gate with 4 inputs all IDatFlow&lt;bool&gt;
    /// Implementing IDataFlow&lt;bool&gt more than once would create a compiler error
    /// (It would make sense for the C# compiler to allow multiple named or indexed implementations of the same interface, and for references to be specific to one of them, like delegates)
    /// As a workaround, to implement the input ports we can use four private fields of type IDataFlowPullB&lt;bool&gt instead:
    /// IDataFlow_B&lt;bool&gt input1;
    /// IDataFlow_B&lt;bool&gt input2;
    /// IDataFlow_B&lt;bool&gt input3;
    /// IDataFlow_B&lt;bool&gt input4;
    /// These are inputs but they are wired up in the opposite direction to the direction data actually flows.
    /// Each one is generally wired to an instance of a DataFlowConnector - see below
    /// </summary>
    /// <typeparam name="T">Generic data type</typeparam>
    public interface IDataFlowPull<T>
    {
        T Pull();
        event DataChangedDelegate DataChanged;
    }




    /// <summary>
    /// DataFlowConnector has multiple uses:
    /// 1) Allows fanout from an output port (normally an output port that is not a list can be wired only once)
    /// 2) If the runtime order of fanout dataflow execution is important, DataFlowConnector instances can be chained, making the order explicit.
    /// 3) Allows an abstraction to have multiple input ports of the same type (normally a class an implement a given type of interface only once)
    /// ----------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IDataFlow<T> implemented interface: incoming data port
    /// 2. List<IDataFlow<T>> fanoutList: output port that can be wired to many places
    /// 3. IDataFlow<T> last: output port that will output after the fanoutList and IDataFlow_B data changed event.
    /// 4. IDataFlow_B<T> implemented interface: ouput port but is wired opposite way from normal.
    /// </summary>
    /// <typeparam name="T">Generic data type</typeparam>
    public class DataFlowFanout<T> : IDataFlow<T>, IDataFlowPull<T> // input, pull outputs
    {
        // properties
        public string InstanceName = "";

        // ports
        // IDataFlow<T> (implemented) is the only input
        // IDataFlowPull<T> (implemented) is an output port from which the input port pulls the data  
        private List<IDataFlow<T>> fanoutList = new List<IDataFlow<T>>(); // ouptut port that supports multiple wiring 
        private IDataFlow<T> last; // output ports that outputs after all other outputs to allow contrilling order of execution through chaining instances of these connectors.

        /// <summary>
        /// supports fan out, or connects an IDataFlow output to a IDataFlowPull input, control order with a "last" port
        /// </summary>
        public DataFlowFanout() { }


        // IDataFlow<T> implementation ---------------------------------
        private T data = default;

        void IDataFlow<T>.Push(T data)
        {
            this.data = data;
            foreach (var f in fanoutList) f.Push(data);
            DataChanged?.Invoke();
            last?.Push(data);
        }

        // IDataFlow_B<T> implementation ---------------------------------
        T IDataFlowPull<T>.Pull() { return data; }

        public event DataChangedDelegate DataChanged;
    }






    /// <summary>
    /// DataFlowExternalPort
    /// The situation is you are implementing an abstraction that has ports.
    /// You are composing instances of abstractions inside this abstraction that also have ports.
    /// You want to connect an internal output port to an external output port.
    /// Lets say your external port is named output: private IDataflow<double> outout;
    /// And your instance of an internal abstraction is: var filter = new Filter<double>();
    /// filter has an IDataFlow<double> output port
    /// You could just write: filter.WireTo(output);
    /// But that will cause an exception in WireTo becasue output is likely null because you are doing this in the contructor, and output hasn't been wired externally to anything yet. 
    /// You can solve this by writing: filter.WireTo(new DataFlowExternalPort<double>((d) => output?.Push(d)));
    /// ----------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IDataFlow<T> implemented interface: incoming data port
    /// </summary>
    /// <typeparam name="T">Generic data type</typeparam>
    class DataFlowExternalPort<T> : IDataFlow<T> // input
    {

        public DataFlowExternalPort(OutputMethod outputMethod)
        {
            this.outputMethod = outputMethod;
        }

        public delegate void OutputMethod(T data);

        private OutputMethod outputMethod;

        void IDataFlow<T>.Push(T data)
        {
            outputMethod(data);
        }
    }




    class DataFlowConvert<T1, T2> : IDataFlow<T1> // input
    {
        public DataFlowConvert(lambdaDelegate lambda)
        {
            this.lambda = lambda;   
        }

        // ports
        private IDataFlow<T2> output;


        public delegate T2 lambdaDelegate(T1 data);

        private lambdaDelegate lambda;


        void IDataFlow<T1>.Push(T1 data)
        {
            output?.Push(lambda(data));
        }
    }





    class DataFlowInitializer<T> : IDataFlow<T>
    {

        // ports
        private IDataFlow<T> output;

        void IDataFlow<T>.Push(T data)
        {
            output?.Push(data);
        }

        public void Push(T data)
        {
            output?.Push(data);
        }

    }





    class DataFlowDebugOutput<T> : IDataFlow<T> // input
    {
        public DataFlowDebugOutput(DiagnosticOutputDelegate diagnosticOutput) { this.diagnosticOutput += diagnosticOutput;  }

        // ports
        private IDataFlow<T> output;



        void IDataFlow<T>.Push(T data)
        {
            diagnosticOutput?.Invoke(data.ToString());
            output?.Push(data);
        }

        // diagnostics output port
        // doesn't have to be wired anywhere
        public delegate void DiagnosticOutputDelegate(string output);
        public event DiagnosticOutputDelegate diagnosticOutput;
    }




    /// <summary>
    /// Has one input port of type T1 and one ouptut port of type T2
    /// Wire into a Data flow to change the type
    /// e.g. you can change from double to int
    /// or anything the C# ChangeType can do.
    /// You need to understand the programming paradigm abstraction, IDataFlow, to understand this code.
    /// </summary>
    class ChangeType<T1, T2> : IDataFlow<T1>
    {
        private IDataFlow<T2> output;

        void IDataFlow<T1>.Push(T1 data)
        {
            output.Push((T2)Convert.ChangeType(data, typeof(T2)));
        }
    }

}
