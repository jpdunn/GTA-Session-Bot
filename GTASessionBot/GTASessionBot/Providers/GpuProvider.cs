using OpenHardwareMonitor.Hardware;
using System.Collections.Generic;


namespace GTASessionBot.Providers
{
    public class GpuProvider
    {

        public static Dictionary<string, string> GetGpuTemperatures()
        {
            Computer thisComputer;
            Dictionary<string, string> rc;


            thisComputer = new Computer() { GPUEnabled = true };
            rc = new Dictionary<string, string>();

            thisComputer.Open();

            foreach (var hardwareItem in thisComputer.Hardware)
            {
                if (hardwareItem.HardwareType == HardwareType.GpuNvidia)
                {
                    hardwareItem.Update();

                    foreach (IHardware subHardware in hardwareItem.SubHardware)
                    {
                        subHardware.Update();
                    }

                    foreach (var sensor in hardwareItem.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Temperature)
                        {

                            rc.Add(
                                $"{sensor.Name} Temp",
                                sensor.Value.HasValue ? $"{sensor.Value.Value.ToString()}c" : "no value"
                            );

                        }
                    }
                }
            }

            thisComputer.Close();

            return rc;
        }


        public static Dictionary<string, string> GetGpuUsage()
        {
            Computer thisComputer;
            Dictionary<string, string> rc;


            thisComputer = new Computer() { GPUEnabled = true };
            rc = new Dictionary<string, string>();

            thisComputer.Open();

            foreach (var hardwareItem in thisComputer.Hardware)
            {
                if (hardwareItem.HardwareType == HardwareType.GpuNvidia)
                {
                    hardwareItem.Update();

                    foreach (IHardware subHardware in hardwareItem.SubHardware)
                    {
                        subHardware.Update();
                    }

                    foreach (var sensor in hardwareItem.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Load)
                        {

                            rc.Add(
                                $"{sensor.Name} Usage",
                                sensor.Value.HasValue ? $"{sensor.Value.Value.ToString()}% used" : "no value"
                            );

                        }
                    }
                }
            }

            thisComputer.Close();

            return rc;
        }
    }
}
