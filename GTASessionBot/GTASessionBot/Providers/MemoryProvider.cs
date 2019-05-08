using OpenHardwareMonitor.Hardware;
using System.Collections.Generic;


namespace GTASessionBot.Providers {
    public class MemoryProvider {

        public static Dictionary<string, string> GetRamUsage() {
            Computer thisComputer;
            Dictionary<string, string> rc;


            thisComputer = new Computer() { RAMEnabled = true };
            rc = new Dictionary<string, string>();

            thisComputer.Open();

            foreach (var hardwareItem in thisComputer.Hardware) {
                if (hardwareItem.HardwareType == HardwareType.RAM) {
                    hardwareItem.Update();

                    foreach (IHardware subHardware in hardwareItem.SubHardware) {
                        subHardware.Update();
                    }

                    foreach (var sensor in hardwareItem.Sensors) {
                        if (sensor.SensorType == SensorType.Data) {

                            rc.Add(
                                sensor.Name,
                                sensor.Value.HasValue ? $"{sensor.Value.Value.ToString("0.00")}GB" : "no value"
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
