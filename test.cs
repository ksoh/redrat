 #r "RedRat.dll"
 #r "System.Xml.dll"
 #r "System.IO.dll"
 #r "System.Xml.Serialization.dll"

 using System;
 using System.Collections.Generic;
 using System.Linq;
 using System.Text;
 using RedRat.RedRat3;
 using RedRat.RedRat3.USB;
 using System.Threading.Tasks;
 using RedRat.AVDeviceMngmt;
 using System.Xml.Serialization;
 using System.Xml;
 using System.IO;
 using RedRat.Util;
 using RedRat.IR;

 public class Startup
 {

 public async Task<object> Invoke(object input)
 {
 await Task.Run(async () => {
 SimpleSignalOutput();
 });
 return 0;
 }


 public void SimpleSignalOutput()
 {
 try
 {
 string[] TestArgs = {"AVDeviceDB.xml", "Humax", "Program+"};
 //CheckArgs(TestArgs);
 using (var rr3 = FindRedRat3())
 {
 var signalDB = LoadSignalDB(TestArgs[0]);
 var signal = GetSignal(signalDB, TestArgs[1], TestArgs[2]);
 rr3.OutputModulatedSignal(signal);
 Console.WriteLine("Signal {0}->{1} output.\n", TestArgs[1], TestArgs[2]);
 }
 }
 catch (Exception ex)
 {
 Console.WriteLine("Error: " + ex.Message);
 }
 }

 /// <summary>
 /// Simple check of program arguments. We need all three in the correct order to work.
 /// </summary>
 ///

 //private void CheckArgs(string[] args)
 //{
 // Need all three arguments.
 //    if ((args != null) && (args.Length == 3)) return;
 //    throw new Exception("Invalid argument list.\n\nUsage: SimpleSignalOutput database-file.xml device-name signal-name\n");
 //}


 /// <summary>
 /// Simply finds the first RedRat3 attached to this computer.
 /// </summary>
 public IRedRat3 FindRedRat3()
 {
 var rr3li = RRUtil.FindRedRats(RRUtil.RedRatTypes.REDRAT3).FirstOrDefault();

 if (rr3li == null)
 {
 throw new Exception("Unable to find any RedRat3 devices on this computer.");
 }
 return rr3li.GetRedRat() as IRedRat3; ;
 }

 /// <summary>
 /// Loads the signal database XML file.
 /// </summary>
 public AVDeviceDB LoadSignalDB(string dbFileName)
 {
 var ser = new XmlSerializer(typeof(AVDeviceDB));
 var fs = new FileStream((new FileInfo(dbFileName)).FullName, FileMode.Open);
 var avDeviceDB = (AVDeviceDB)ser.Deserialize(fs);
 return avDeviceDB;
 }

 /// <summary>
 /// Returns an IR signal object from the signal DB file using the deviceName and signalName to
 /// look it up.
 /// </summary>
 public IRPacket GetSignal(AVDeviceDB signalDB, string deviceName, string signalName)
 {
 var device = signalDB.GetAVDevice(deviceName);
 if (device == null)
 {
 throw new Exception(
 string.Format("No device of name '{0}' found in the signal database.", deviceName));
 }
 var signal = device.GetSignal(signalName);
 if (signal == null)
 {
 throw new Exception(
 string.Format("No signal of name '{0}' found for device '{1}' in the signal database.",
 signalName, deviceName));
 }
 return signal;
 }
 }