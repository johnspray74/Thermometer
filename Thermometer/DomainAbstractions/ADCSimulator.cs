using System;
using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// Simulate an 10-bit ADC input with 
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
        public int simulatedReading { get; set; } = 512; // 0 to 1023
        public int simulatedNoiseLevel { get; set; } = 10; // 0 to 1023

        private IDataFlow<int> output;

        public void Run()
        {
            var setAndForget = RunAsyncCatch();
        }


        public async Task RunAsyncCatch()
        {
            try
            {
                await RunAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


        public async Task RunAsync()
        {
            while (true)
            {
                await Task.Delay(period);
                var randomNumberGenerator = new Random(); // add a bit of noise to the adc readings
                int data = simulatedReading + randomNumberGenerator.Next(simulatedNoiseLevel) - simulatedNoiseLevel/2;
                if (data < 0) data = 0;
                if (data > 1023) data = 1023;
                output.Send(data);
                // throw new Exception("exception test");
            }
        }

    }
}
