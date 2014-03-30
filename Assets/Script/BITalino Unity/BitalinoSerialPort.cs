using UnityEngine;
using System.Collections;
using System.IO.Ports;

public class BitalinoSerialPort : MonoBehaviour {

    public string portName = "COM4";
    public int baudRate = 9600;
    public Parity parity = Parity.None;
    public int dataBits = 8;
    public StopBits stopBits = StopBits.One;
    public int ReadTimeOut = 5000;
    public int WriteTimeOut = 5000;

    private ManagerBitalino managerB;
    private SerialPort serialPort;
    private IBITalinoCommunication bitalinoCommunication;

    public ManagerBitalino ManagerB { get; set; }

	// Use this for initialization
	void Start () {

        serialPort = new SerialPort ( portName, baudRate, parity, dataBits, stopBits );
        
        serialPort.ReadTimeout = ReadTimeOut;
        serialPort.WriteTimeout = WriteTimeOut;

        bitalinoCommunication = new BITalinoCommunicationSerialPort ( serialPort );
    }
	
	// Update is called once per frame
	void Update () {
	    if ( ManagerB.BitalinoCommunication == null )
        {
            ManagerB.BitalinoCommunication = bitalinoCommunication;
        }
	}
}
