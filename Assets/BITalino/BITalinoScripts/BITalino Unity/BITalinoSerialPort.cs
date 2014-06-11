using UnityEngine;
using System.Collections;
using System.IO.Ports;

public class BITalinoSerialPort : MonoBehaviour {

    public string portName = "COM4";
    public int baudRate = 9600;
    public Parity parity = Parity.None;
    public int dataBits = 8;
    public StopBits stopBits = StopBits.One;
    public int ReadTimeOut = 5000;
    public int WriteTimeOut = 5000;

    private ManagerBITalino managerB;
    private SerialPort serialPort;
    private IBITalinoCommunication bitalinoCommunication;

    public ManagerBITalino ManagerB { get; set; }

	/// <summary>
	/// Initialize the serial connection
	/// </summary>
	void Start () {

        serialPort = new SerialPort ( portName, baudRate, parity, dataBits, stopBits );
        
        serialPort.ReadTimeout = ReadTimeOut;
        serialPort.WriteTimeout = WriteTimeOut;

        bitalinoCommunication = new BITalinoCommunicationSerialPort ( serialPort );
    }
	
	/// <summary>
	/// Set the serial connection in the manager
	/// </summary>
	void Update () {
	    if ( ManagerB.BitalinoCommunication == null )
        {
            ManagerB.BitalinoCommunication = bitalinoCommunication;
        }
	}
}
