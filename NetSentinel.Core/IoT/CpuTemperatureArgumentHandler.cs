using Iot.Device.CpuTemperature;
using NetSentinel.ArgumentHandling;

namespace NetSentinel.IoT
{
    [ArgumentHandler("--cpu-temperature")]
    public sealed class CpuTemperatureArgumentHandler : IArgumentHandler
    {
        public void Execute()
        {
            var ct = new CpuTemperature();

            if (ct.IsAvailable)
            {
                foreach (var (Sensor, Temperature) in ct.ReadTemperatures())
                {
                    Console.WriteLine($"Sensor '{Sensor}': {Temperature.DegreesCelsius:0.00} °C");
                }
            }
            else
            {
                Console.WriteLine("Temperature reading is not available");
            }
        }

        public void Process(string[] arguments, ref int index)
        {
            // nothing to do
        }
    }
}
