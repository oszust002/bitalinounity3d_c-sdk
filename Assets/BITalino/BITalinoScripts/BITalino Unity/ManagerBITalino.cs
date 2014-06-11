using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Globalization;
using System.Threading;

public class ManagerBITalino : MonoBehaviour {

    public enum AcquisitionState
    {
        Run = 0,
        NotRun = 1
    };

    public enum Channels
    {
        EMG,
        EDA,
        LUX,
        ECG,
        ACC,
        BATT
    };

    // if need a GUI
    public GUIBitalino GUIB;
    public BITalinoSerialPort scriptSerialPort;

    public Channels[] AnalogChannels = { Channels.EMG,Channels.EDA,Channels.LUX,Channels.ECG,Channels.ACC,Channels.BATT };
    public int SamplingRate = 1000;
    public bool logFile = false;
    public string logPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

    private bool isReady = false;
    private static BITalinoDevice device;
    private IBITalinoCommunication bitalinoCommunication;
    private string version;
    private StreamWriter sw = null;
    private AcquisitionState acquisitionState = AcquisitionState.NotRun;


    #region GETTER/SETTER
    public bool IsReady
    {
        get { return isReady; }
    }
    public AcquisitionState Acquisition_State
    {
        get { return acquisitionState; }
    }
    public IBITalinoCommunication BitalinoCommunication
    {
        get { return bitalinoCommunication; }
        set { bitalinoCommunication = value; }
    }
    #endregion



    // Use this for initialization
	void Start ()
    {
        logPath += "\\" + DateTime.Now.ToString("MMddHHmmssfff") + "_Log.txt";
        if ( GUIB != null )
        {
            GUIB.ManagerB = this;
        }
        
        if( scriptSerialPort != null )
        {
            scriptSerialPort.ManagerB = this;
        } 
	}
	
	/// <summary>
	/// Update the state of the manager
	/// </summary>
	void Update () 
    {
        if ( bitalinoCommunication != null )
        {
            isReady = true;
        }
        else if ( isReady == true )
        {
            isReady = false;
        }
	}

    /// <summary>
    /// Initialize the connection with the BITalino
    /// </summary>
    public void Connection()
    {
        try
        {
            if ( bitalinoCommunication != null && device == null )
            {
                device = new BITalinoDevice(bitalinoCommunication, convertChannels(), SamplingRate);
            }

            device.Connection ( );
            WriteLog("Done, Connection");
        }
        catch ( Exception ex )
        {
            WriteLog("Error on connection" + ex.Message);
        }
    }

    /// <summary>
    /// Stop the connection with the BITalino
    /// </summary>
    public void Deconnection ( )
    {
        try
        {
            device.Deconnection ( );
            acquisitionState = AcquisitionState.NotRun;
            WriteLog("Done, Deconnection");
        }
        catch ( Exception ex )
        {
            WriteLog("Error on deconnection" + ex.Message);
        }

    }

    /// <summary>
    /// Get the version of the BITalino
    /// </summary>
    public void GetVersion ()
    {
        try
        {
            version = device.GetVersion ( );
            WriteLog("Bitalino's version: " + version);
        }
        catch ( Exception ex )
        {
            WriteLog("Error getting version: " + ex.Message);
        } 
    }

    /// <summary>
    /// Start the acquisition
    /// </summary>
    public void StartAcquisition()
    {
        try
        {
            device.SamplingRate = SamplingRate;

            Array.Sort(AnalogChannels);
            device.AnalogChannels = convertChannels();
            device.StartAcquisition ();
            acquisitionState = AcquisitionState.Run;
            WriteLog("Done, acquisition started");
        }
        catch ( Exception ex )
        {
            WriteLog("Error acquisition: " + ex.Message);
        }
        
    }

    /// <summary>
    /// Stop the acquisition
    /// </summary>
    public void StopAcquisition()
    {
        try
        {
            device.StopAcquisition ( );
            acquisitionState = AcquisitionState.NotRun;
            WriteLog("DONE acquisition stopped");
        }
        catch ( Exception ex )
        {
            WriteLog( "Error stopping the acquisition: " + ex.Message );
        }
    }

    /// <summary>
    /// Read data from the BITalino
    /// </summary>
    /// <param name="nbSamples">number of sample reade</param>
    /// <returns>Samples read</returns>
    public BITalinoFrame [ ] Read ( int nbSamples )
    {
        try
        {
            return device.ReadFrames ( nbSamples );
        }
        catch ( Exception ex )
        {
            WriteLog( "Error reading the frames: " + ex.Message );
        }

        return null;
    }

    /// <summary>
    /// Write the log data in a file if log_file is true, else write them in the console
    /// </summary>
    /// <param name="log">Data write</param>
    public void WriteLog(String log)
    {
        if (logFile)
        {
            if (sw == null)
            {
                sw = File.AppendText(logPath);
            }
            sw.WriteLine(log);
            sw.Flush();
        }
        else
        {
            Debug.Log(log);
        }
    }

    /// <summary>
    /// Convert the tab of AnalogChannels into an int tab
    /// </summary>
    /// <returns>Int tab of the AnalogChannels</returns>
    public int[] convertChannels()
    {
        int[] convertChannels = new int[AnalogChannels.Length];
        int i = 0;
        foreach (Channels channel in AnalogChannels)
        {
            convertChannels[i] = (int)channel;
            i++;
        }
        return convertChannels;
    }
}
