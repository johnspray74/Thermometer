using Features;
using Foundation;

namespace Application
{
    class Application
    {
        /// <summary>
        /// Instantiate two features: a temperature readout and a load readout.
        /// Also wire the Temperature to the LoadCell for temperature compensation 
        /// </summary>
        public static void Main()
        {
            // Tell the wiring abstraction to log what it is doing to the diagnostic output window
            Wiring.diagnosticOutput += (s) => System.Diagnostics.Debug.WriteLine(s);
            // Wiring.diagnosticOutput += Console.WriteLine;
            // Wiring.diagnosticOutput += wiringLoggerToFile.WriteLine;

            Console.WriteLine("Wiring application features");

            var temperature = new Temperature();
            var load = new LoadCell();

            temperature.WireTo(load); // for temperature compensation

            Console.WriteLine("Running application");
            Console.WriteLine("press any key to stop");
            temperature.Run();
            load.Run();
            Console.ReadKey();
        }
    }

}

