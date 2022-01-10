using DomainAbstractions;
using ProgrammingParadigms;
using Foundation;



namespace Application
{
    class Thermometer
    {
        public static async Task Main()
        {
            Console.WriteLine("Wiring application");

            var adc = new ADCSimulator(channel: 2, period: 200) { simulatedNoiseLevel = 50 };
            adc
            .WireIn(new ChangeType<int, double>())
            .WireIn(new Filter(strength: 0.1))
            .WireIn(new OffsetAndScale(offset: -31.2, scale: 0.097))
            .WireIn(new DisplayNumeric<double>(label: "Temperature"));

            Console.WriteLine("Running application");
            adc.Run();

            Console.WriteLine("press any key to stop");
            Console.ReadKey();
        }
    }
}

