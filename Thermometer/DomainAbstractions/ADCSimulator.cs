using System;
using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// ALA Domain Abstraction
    /// Simulate a 10-bit ADC Analog to Digital converter)
    /// Normally an ADC is a hardware peripheral, but here we just do a software simulation of one
    /// to use as a source of data for example applications.
    /// A real ADC driver would have properties for setting the channel and period.
    /// You would create one instance of this driver for each ADC channel.
    /// It would output raw data in adc counts. Since it is a 10 bit ADC, the adc counts are in teh range 0-1023.
    /// We retain the channel although it is not used by the simulated version.
    /// 
    /// The simulated version has two simulation properties, one to set the simulated ADC reading
    /// and one to set the level of noise in the simulated readings.
    /// You need to understand the programming paradigm abstraction, IDataFlow, to understand this code.
    /// </summary>
    class ADCSimulator
    {
        public ADCSimulator(int channel, int period = 100)
        {
            this.channel = channel;
            this.period = period;
        }

        private int channel;  // unused on simulated ADC
        private int period;   // milliseconds
        public int simulatedLevel { get; set; } = 512; // 0 to 1023
        public int simulatedNoise { get; set; } = 0; // 0 to 1023

        private IDataFlow<int> output;

        public void Run()
        {
            RunAsyncCatch();
        }


        public async Task RunAsyncCatch()
        {
            // because we are the outermost async method, if we let exceptions go, they will be lost
            try
            {
                await RunAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        Random randomNumberGenerator = new Random();


        public async Task RunAsync()
        {
            while (true)
            {
                // add a bit of noise to the adc readings
                int data = simulatedLevel + randomNumberGenerator.Next(simulatedNoise) - simulatedNoise/2;
                if (data < 0) data = 0;
                if (data > 1023) data = 1023;
                output.Push(data);
                // throw new Exception("exception test");
                await Task.Delay(period);
            }
        }

    }
}
