using System;
using DomainAbstractions;
using ProgrammingParadigms;
using Foundation;

namespace Features
{
    /// <summary>
    /// Class:
    /// Feature to coninuously measure a load from a load cell and display it in kg on the console.
    /// Displays with one decimal place.
    /// Has temperature compensation for better accuracy (optionally feed temperature into the input port in degress C) 
    /// </summary>
    class LoadCell : IDataFlow<double> // input for temperature compensation
    {
        private ADCSimulator adc;
        private DataFlowInitializer<double> defaultTemperature;
        private OffsetAndScale offsetAndScaleTemperature;

        /// <summary>
        /// Constructor:
        /// Feature to coninuously measure a load from a load cell and display it in kg on the console.
        /// Displays with one decimal place.
        /// Has temperature compensation for better accuracy (optionally feed temperature into the input port in degress C) 
        /// </summary>
        public LoadCell()
        {
            // Wire an adc to an OffsetAndScale to an Add to a DislayNumeric.
            adc = new ADCSimulator(channel: 3, period: 500) { simulatedLevel = 200, simulatedNoise = 0 };
            var add = new Add();
            adc.WireIn(new ChangeType<int, double>())
                .WireIn(new OffsetAndScale(offset: 0, scale: 0.5))
                //.WireIn(new DataFlowDebugOutput<double>((s)=> System.Diagnostics.Debug.WriteLine(s)))
                // .WireIn(new DataFlowDebugOutput<double>(Console.WriteLine))
                .WireIn(add)
                .WireTo(new DisplayNumeric<double>(label: "Load") { fixPoints = 1, units = "kg" } );

            // Wire the inut port for temperature to another OffsetAndScale to the other input of the Add.
            defaultTemperature = new DataFlowInitializer<double>();
            offsetAndScaleTemperature = new OffsetAndScale(offset: -20, scale: -0.1); // compensate -0.1 kg/C from 20 C
            defaultTemperature.WireIn(offsetAndScaleTemperature)
                .WireIn(new DataFlowConvert<double, Double2>((d)=>new Double2(d)))
                .WireIn(add);

        }



        void IDataFlow<double>.Push(double data)
        {
            ((IDataFlow<double>)offsetAndScaleTemperature).Push(data); 
        }


        public void Run()
        {
            defaultTemperature.Push(20);  // in case no temperture is connected to the input port, set it to 20 C
            adc.Run();
        }
    }
}
