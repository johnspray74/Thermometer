using System;
using DomainAbstractions;
using ProgrammingParadigms;
using Foundation;

namespace Features
{
    /// <summary>
    /// Feature to coninuously measure temperature and periodically display it in degrees C on the console.
    /// Has an output port that outputs the temperature. 
    /// </summary>
    class Temperature
    {
        private IDataFlow<double> output; // temperature in celcius

        private ADCSimulator adc;

        public Temperature()
        {
            const int adcLevel = 400;  // 40 C
            adc = new ADCSimulator(channel: 2, period: 1000) { simulatedLevel = adcLevel, simulatedNoise = 100 };
            adc.WireIn(new ChangeType<int, double>())
                .WireIn(new LowPassFilter(strength: 10, initialState: adcLevel))
                .WireIn(new OffsetAndScale(offset: -200, scale: 0.2)) // 200 adc counts is 0 C, 300 adc counts is 20 C
                .WireIn(new DataFlowFanout<double>())  
                .WireTo(new DisplayNumeric<double>(label: "Temperature") { units = "C"} )
                .WireTo(new DataFlowExternalPort<double>((d) => output?.Push(d)));
        }


        public void Run()
        {
            adc.Run();
        }
    }
}
