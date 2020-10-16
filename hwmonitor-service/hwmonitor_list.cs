/*
 * 
 * Based on a tutorial at:
 * https://www.lattepanda.com/topic-f11t3004.html
 * and uses openhardwaremonitorlib.dll from openhardwaremonitor project
 * 
 */

using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenHardwareMonitor.Hardware;

namespace hwmonitor_service
{
    class hwmonitor_list
    {
        public class UpdateVisitor : IVisitor
        {
            public void VisitComputer(IComputer computer)
            {
                computer.Traverse(this);
            }
            public void VisitHardware(IHardware hardware)
            {
                hardware.Update();
                foreach (IHardware subHardware in hardware.SubHardware) subHardware.Accept(this);
            }
            public void VisitSensor(ISensor sensor) { }
            public void VisitParameter(IParameter parameter) { }
        }
        public string GetSystemInfo()
        {
            string retval;
            UpdateVisitor updateVisitor = new UpdateVisitor();
            Computer computer = new Computer();
            computer.Open();
            computer.CPUEnabled = true;
            computer.GPUEnabled = true;
            computer.FanControllerEnabled = true;
            computer.MainboardEnabled = true;
            computer.RAMEnabled = true;
            computer.HDDEnabled = true;
            computer.Accept(updateVisitor);
            retval = "{";
            retval += "\"Computer-name\":\"" + System.Environment.MachineName + "\", ";
            if (computer.Hardware.Length > 0)retval += "\"Hardware\":[";
            for (int i = 0; i < computer.Hardware.Length; i++)
            {
                retval += "{\"Type\":";
                switch (computer.Hardware[i].HardwareType)
                {
                    case HardwareType.CPU:
                        retval += "\"CPU\"";
                        break;
                    case HardwareType.GpuAti:
                        retval += "\"GPU Ati\"";
                        break;
                    case HardwareType.GpuNvidia:
                        retval += "\"GPU NVidia\"";
                        break;
                    case HardwareType.HDD:
                        retval += "\"HDD\"";
                        break;
                    case HardwareType.Heatmaster:
                        retval += "\"Heatmaster\"";
                        break;
                    case HardwareType.Mainboard:
                        retval += "\"Mainboard\"";
                        break;
                    case HardwareType.RAM:
                        retval += "\"RAM\"";
                        break;
                    case HardwareType.SuperIO:
                        retval += "\"SuperIO\"";
                        break;
                    case HardwareType.TBalancer:
                        retval += "\"TBalancer\"";
                        break;
                    default:
                        break;

                }
                retval += ",\"Name\":\"" + computer.Hardware[i].Name + "\",";
                if (computer.Hardware[i].SubHardware.Length > 0) 
                    retval += "\"SubHardware\":[";
                if (retval.EndsWith(","))
                    retval = retval.Remove(retval.LastIndexOf(","));

                for (int k = 0; k < computer.Hardware[i].SubHardware.Length; k++)
                {
                    retval += "{\"Type\":";
                    switch (computer.Hardware[i].SubHardware[k].HardwareType)
                    {
                        case HardwareType.CPU:
                            retval += "\"CPU\"";
                            break;
                        case HardwareType.GpuAti:
                            retval += "\"GPU Ati\"";
                            break;
                        case HardwareType.GpuNvidia:
                            retval += "\"GPU NVidia\"";
                            break;
                        case HardwareType.HDD:
                            retval += "\"HDD\"";
                            break;
                        case HardwareType.Heatmaster:
                            retval += "\"Heatmaster\"";
                            break;
                        case HardwareType.Mainboard:
                            retval += "\"Mainboard\"";
                            break;
                        case HardwareType.RAM:
                            retval += "\"RAM\"";
                            break;
                        case HardwareType.SuperIO:
                            retval += "\"SuperIO\"";
                            break;
                        case HardwareType.TBalancer:
                            retval += "\"TBalancer\"";
                            break;
                        default:
                            break;

                    }
                    retval += ",\"Name\":\"" + computer.Hardware[i].SubHardware[k].Name + "\",";
                    if (computer.Hardware[i].SubHardware[k].Sensors.Length > 0) retval += "\"Sensors\":[";
                    for (int shs = 0; shs < computer.Hardware[i].SubHardware[k].Sensors.Length; shs++)
                    {
                        retval += "{\"Type\":";
                        switch (computer.Hardware[i].SubHardware[k].Sensors[shs].SensorType)
                        {
                            case SensorType.Clock:
                                retval += "\"Clock\",\"Unit\":";
                                retval += "\"MHz\"";
                                break;
                            case SensorType.Control:
                                retval += "\"Control\",\"Unit\":";
                                retval += "\"%\"";
                                break;
                            case SensorType.Data:
                                retval += "\"Data\",\"Unit\":";
                                retval += "\"GB\"";
                                break;
                            case SensorType.Factor:
                                retval += "\"Factor\",\"Unit\":";
                                retval += "\"???\"";                    // Not documented
                                break;
                            case SensorType.Fan:
                                retval += "\"Fan\",\"Unit\":";
                                retval += "\"RPM\"";
                                break;
                            case SensorType.Flow:
                                retval += "\"Flow\",\"Unit\":";
                                retval += "\"L/h\"";
                                break;
                            case SensorType.Level:
                                retval += "\"Level\",\"Unit\":";
                                retval += "\"%\"";
                                break;
                            case SensorType.Load:
                                retval += "\"Load\",\"Unit\":";
                                retval += "\"%\"";
                                break;
                            case SensorType.Power:
                                retval += "\"Power\",\"Unit\":";
                                retval += "\"W\"";                       // Not documented
                                break;
                            case SensorType.SmallData:
                                retval += "\"SmallData\",\"Unit\":";
                                retval += "\"???\"";                     // Not documented
                                break;
                            case SensorType.Temperature:
                                retval += "\"Temperature\",\"Unit\":";
                                retval += "\"&deg;C\"";
                                break;
                            case SensorType.Throughput:
                                retval += "\"Temperature\",\"Unit\":";
                                retval += "\"???\"";                     // Not documented
                                break;
                            case SensorType.Voltage:
                                retval += "\"Voltage\",\"Unit\":";
                                retval += "\"V\"";
                                break;
                            default:
                                break;
                        }
                        float v = (float)computer.Hardware[i].SubHardware[k].Sensors[shs].Value;
                        float min = (float)computer.Hardware[i].SubHardware[k].Sensors[shs].Min;
                        float max = (float)computer.Hardware[i].SubHardware[k].Sensors[shs].Max;
                        retval += ",\"Name\":\"" + computer.Hardware[i].SubHardware[k].Sensors[shs].Name + "\"," +
                            "\"Value\":" + v.ToString(System.Globalization.CultureInfo.InvariantCulture) + "," +
                            "\"Min\":" + min.ToString(System.Globalization.CultureInfo.InvariantCulture) + "," +
                            "\"Max\":" + max.ToString(System.Globalization.CultureInfo.InvariantCulture);
                        retval += "},";
                    }
                    if (retval.EndsWith(","))
                        retval = retval.Remove(retval.LastIndexOf(","));

                    if (computer.Hardware[i].SubHardware[k].Sensors.Length > 0) retval += "]},";
                }
                if (retval.EndsWith(","))
                    retval = retval.Remove(retval.LastIndexOf(","));

                if (computer.Hardware[i].SubHardware.Length > 0) retval += "]";
                retval += ",";
                {
                    if (computer.Hardware[i].Sensors.Length > 0) retval += "\"Sensors\":[";
                    for (int j = 0; j < computer.Hardware[i].Sensors.Length; j++)
                    {
                        retval += "{\"Type\":";     
                        switch (computer.Hardware[i].Sensors[j].SensorType)
                        {
                            case SensorType.Clock:
                                retval += "\"Clock\",\"Unit\":";
                                retval += "\"MHz\"";
                                break;
                            case SensorType.Control:
                                retval += "\"Control\",\"Unit\":";
                                retval += "\"%\"";
                                break;
                            case SensorType.Data:
                                retval += "\"Data\",\"Unit\":";
                                retval += "\"GB\"";
                                break;
                            case SensorType.Factor:
                                retval += "\"Factor\",\"Unit\":";
                                retval += "\"???\"";                    // Not documented
                                break;
                            case SensorType.Fan:
                                retval += "\"Fan\",\"Unit\":";
                                retval += "\"RPM\"";
                                break;
                            case SensorType.Flow:
                                retval += "\"Flow\",\"Unit\":";
                                retval += "\"L/h\"";
                                break;
                            case SensorType.Level:
                                retval += "\"Level\",\"Unit\":";
                                retval += "\"%\"";
                                break;
                            case SensorType.Load:
                                retval += "\"Load\",\"Unit\":";
                                retval += "\"%\"";
                                break;
                            case SensorType.Power:
                                retval += "\"Power\",\"Unit\":";
                                retval += "\"W\"";                       // Not documented
                                break;
                            case SensorType.SmallData:
                                retval += "\"SmallData\",\"Unit\":";
                                retval += "\"???\"";                     // Not documented
                                break;
                            case SensorType.Temperature:
                                retval += "\"Temperature\",\"Unit\":";
                                retval += "\"&deg;C\"";
                                break;
                            case SensorType.Throughput:
                                retval += "\"Temperature\",\"Unit\":";
                                retval += "\"???\"";                     // Not documented
                                break;
                            case SensorType.Voltage:
                                retval += "\"Voltage\",\"Unit\":";
                                retval += "\"V\"";
                                break;
                            default:
                                break;
                        }
                        float v = (float)computer.Hardware[i].Sensors[j].Value;
                        float min = (float)computer.Hardware[i].Sensors[j].Min;
                        float max = (float)computer.Hardware[i].Sensors[j].Max;
                        retval += ",\"Name\":\"" + computer.Hardware[i].Sensors[j].Name + "\", " +
                            "\"Value\":" + v.ToString(System.Globalization.CultureInfo.InvariantCulture) + "," +
                            "\"Min\":" + min.ToString(System.Globalization.CultureInfo.InvariantCulture) + "," +
                            "\"Max\":" + max.ToString(System.Globalization.CultureInfo.InvariantCulture);
                        retval += "},";       
                    }
                    if (retval.EndsWith(","))
                        retval = retval.Remove(retval.LastIndexOf(","));
                    if (computer.Hardware[i].Sensors.Length > 0) retval += "]";
                }
                retval += "},";
            }
            if (retval.EndsWith(","))
                retval = retval.Remove(retval.LastIndexOf(","));
            if (computer.Hardware.Length > 0) retval += "]";
            retval += "}";
            computer.Close();
            return retval;
        }
    }
}
