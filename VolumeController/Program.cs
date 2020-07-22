// Use this code inside a project created with the Visual C# > Windows Desktop > Console Application template.
// Replace the code in Program.cs with this code.

using AudioSwitcher.AudioApi.CoreAudio;
using System;
using System.IO.Ports;
using System.Threading;

public class PortChat
{
    static bool _continue;
    static SerialPort _serialPort;
    public static void Main()
    {
        string portname = "";
        string[] ports = SerialPort.GetPortNames();
        // string message;
        StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
        Thread readThread = new Thread(Read);

        // Create a new SerialPort object with default settings.
        _serialPort = new SerialPort();

        // Allow the user to set the appropriate properties.
        foreach (string port in ports)
        {
            Console.WriteLine(port);
            _serialPort.PortName = port;
            _serialPort.BaudRate = 9600;
            _serialPort.DataBits = 8;
            // Set the read/write timeouts
            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;

            try
            {
                _serialPort.Open();

                Console.WriteLine("deze poort is open" + port);
                portname = port;

                _serialPort.Close();

            }
            catch (Exception ex)
            {

                Console.WriteLine("error voor poort " + port + " " + ex.Message);
            }




        }
        _serialPort.PortName = "COM10";
        _serialPort.BaudRate = 9600;
        _serialPort.DataBits = 8;
        // Set the read/write timeouts
        _serialPort.ReadTimeout = 500;
        _serialPort.WriteTimeout = 500;

        _serialPort.Open();
        _continue = true;
        readThread.Start();
        readThread.Join();
        _serialPort.Close();
    }
    public static void Read()
    {
        CoreAudioDevice defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;
        while (_continue)
        {
            try
            {
                String message = _serialPort.ReadLine();
                int convert = int.Parse(message);
                defaultPlaybackDevice.Volume = convert;
            }
            catch (TimeoutException) { }
        }
    }
}