using OpenHardwareMonitor.Hardware;
using System.Collections.Generic;


namespace GTASessionBot.Providers {

    /// <summary>
    /// Defines the provider for GPU related information.
    /// </summary>
    public class GpuProvider {

        /// <summary>
        /// Gets the temperature of the GPU cores.
        /// </summary>
        /// <returns>The temperature of the GPU cores.</returns>
        public static Dictionary<string, string> GetGpuTemperatures() {
            Computer thisComputer;
            Dictionary<string, string> rc;


            thisComputer = new Computer() { GPUEnabled = true };
            rc = new Dictionary<string, string>();

            thisComputer.Open();

            foreach (var hardwareItem in thisComputer.Hardware) {

                if (hardwareItem.HardwareType == HardwareType.GpuNvidia) {
                    hardwareItem.Update();

                    foreach (IHardware subHardware in hardwareItem.SubHardware) {
                        subHardware.Update();
                    }

                    foreach (var sensor in hardwareItem.Sensors) {
                        if (sensor.SensorType == SensorType.Temperature) {

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


        /// <summary>
        /// Gets the usage of the GPU cores.
        /// </summary>
        /// <returns>The usage of the GPU cores.</returns>
        public static Dictionary<string, string> GetGpuUsage() {
            Computer thisComputer;
            Dictionary<string, string> rc;


            thisComputer = new Computer() { GPUEnabled = true };
            rc = new Dictionary<string, string>();

            thisComputer.Open();

            foreach (var hardwareItem in thisComputer.Hardware) {
                if (hardwareItem.HardwareType == HardwareType.GpuNvidia) {
                    hardwareItem.Update();

                    foreach (IHardware subHardware in hardwareItem.SubHardware) {
                        subHardware.Update();
                    }

                    foreach (var sensor in hardwareItem.Sensors) {
                        if (sensor.SensorType == SensorType.Load) {

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
