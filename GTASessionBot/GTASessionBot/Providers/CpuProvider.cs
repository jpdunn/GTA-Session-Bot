using GTASessionBot.Windows_Libraries;
using OpenHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;

namespace GTASessionBot.Providers {
    public class CpuProvider {

        public static string GetCoreCount() {
            int cores = 0;
            Computer thisComputer;


            thisComputer = new Computer() { CPUEnabled = true };

            thisComputer.Open();

            foreach (var hardwareItem in thisComputer.Hardware) {
                if (hardwareItem.HardwareType == HardwareType.CPU) {
                    hardwareItem.Update();

                    foreach (IHardware subHardware in hardwareItem.SubHardware) {
                        subHardware.Update();
                    }

                    foreach (var sensor in hardwareItem.Sensors) {
                        // Only look at the load sensors, and ignore the sensor if it has an index
                        // of zero. This allows us to get the number of cores that the CPU has 
                        // and allows us to ignore the 'CPU Total' sensor.
                        if ((sensor.SensorType == SensorType.Load) && (sensor.Index > 0)) {
                            cores += 1;
                        }
                    }
                }
            }

            thisComputer.Close();

            return $"{cores} cores";
        }


        public static string GetUpTime() {
            return TimeSpan.FromMilliseconds(Kernel32.GetTickCount64()).ToString("c");
        }


        public static string GetProcessorInformation() {
            Computer thisComputer;
            string rc = "";


            thisComputer = new Computer() { CPUEnabled = true };

            thisComputer.Open();
            foreach (var hardwareItem in thisComputer.Hardware) {
                if (hardwareItem.HardwareType == HardwareType.CPU) {
                    hardwareItem.Update();

                    rc = hardwareItem.Name;
                }
            }

            thisComputer.Close();
            return rc;
        }


        public static Dictionary<string, string> GetCpuTemperatures() {
            Computer thisComputer;
            Dictionary<string, string> rc;


            thisComputer = new Computer() { CPUEnabled = true };
            rc = new Dictionary<string, string>();

            thisComputer.Open();
            foreach (var hardwareItem in thisComputer.Hardware) {
                if (hardwareItem.HardwareType == HardwareType.CPU) {
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


        public static Dictionary<string, string> GetCpuLoad() {
            Computer thisComputer;
            Dictionary<string, string> rc;


            thisComputer = new Computer() { CPUEnabled = true };
            rc = new Dictionary<string, string>();

            thisComputer.Open();

            foreach (var hardwareItem in thisComputer.Hardware) {
                if (hardwareItem.HardwareType == HardwareType.CPU) {
                    hardwareItem.Update();

                    foreach (IHardware subHardware in hardwareItem.SubHardware) {
                        subHardware.Update();
                    }

                    foreach (var sensor in hardwareItem.Sensors) {
                        if (sensor.SensorType == SensorType.Load) {
                            if (sensor.Name.Equals("CPU Total")) {
                                rc.Add(
                                    $"{sensor.Name} Usage",
                                    sensor.Value.HasValue ? $"{sensor.Value.Value.ToString()}% used" : "no value"
                                );
                            }

                        }
                    }
                }
            }

            thisComputer.Close();

            return rc;
        }

    }
}
