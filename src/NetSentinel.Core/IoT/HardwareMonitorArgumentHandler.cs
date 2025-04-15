using Iot.Device.HardwareMonitor;
using NetSentinel.ArgumentHandling;

namespace NetSentinel.IoT
{
    [ArgumentHandler("--hardware-monitor")]
    public sealed class HardwareMonitorArgumentHandler : IArgumentHandler
    {
        public void Execute(IGlobalOptions options)
        {
            var monitor = new OpenHardwareMonitor();

            var load = monitor.GetCpuLoad();
            Console.WriteLine($"CPU load: {load:0.00} %");

            if (monitor.TryGetAverageCpuTemperature(out var temperature))
                Console.WriteLine($"Average CPU temperature: {temperature.DegreesCelsius:0.00} °C");
            else
                Console.WriteLine("CPU temperature reading is not available");

            if (monitor.TryGetAverageGpuTemperature(out temperature))
                Console.WriteLine($"Average GPU temperature: {temperature.DegreesCelsius:0.00} °C");
            else
                Console.WriteLine("GPU temperature reading is not available");

            foreach (var item in monitor.GetHardwareComponents())
            {
                Console.WriteLine($"Hardware component: {item.Name} ({item.Type}/{item.Identifier})");
            }

            foreach (var item in monitor.GetSensorList())
            {
                Console.WriteLine($"Sensor '{item.Name}': {item.Value:0.00} ({item.SensorType}/{item.Identifier})");
            }
        }

        public void Process(string[] arguments, ref int index)
        {
            // nothing to do
        }

        public void GenerateHelp(int indent)
        {
            // nothing to do
        }
    }
}
