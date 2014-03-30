using UnityEngine;
using System.Collections;
using System;
using System.IO;

public delegate void ReadEventHandler ( object sender, ReadEventArgs e );

public class ManagerBitalino : MonoBehaviour {

    public enum AcquisitionState
    {
        Run = 0,
        NotRun = 1
    }

    // if need a GUI
    public GUIBitalino GUIB;
    public BitalinoSerialPort scriptSerialPort;

    public int[] defaultAnalogChannels = { 0, 1, 2, 3, 4, 5 };
    public int defaultSamplingRate = 1000;
    public string path = "C:\\Users\\supercourgette\\Finally2.txt";

    private bool isReady = false;
    private static BITalinoDevice device;
    private IBITalinoCommunication bitalinoCommunication;
    private string version;

    private AcquisitionState acquisitionState = AcquisitionState.NotRun;
    private bool stop = false;

    public event ReadEventHandler ReadEvent;

    #region GETTER/SETTER
    public bool IsReady
    {
        get { return isReady; }
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
        if ( GUIB != null )
        {
            GUIB.ManagerB = this;
        }
        
        if( scriptSerialPort != null )
        {
            scriptSerialPort.ManagerB = this;
        } 
	}
	
	// Update is called once per frame
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

    public void Connection()
    {
        try
        {
            if ( bitalinoCommunication != null && device == null )
            {
                device = new BITalinoDevice ( bitalinoCommunication, defaultAnalogChannels, defaultSamplingRate );
            }

            device.Connection ( );
            Debug.Log ( "DONE Connection" );
        }
        catch ( Exception ex )
        {
            Debug.Log ( "ERROR Connection" + ex.Message );
        }
    }

    public void Deconnection ( )
    {
        try
        {
            device.Deconnection ( );
            acquisitionState = AcquisitionState.NotRun;
            Debug.Log ( "DONE Deconnection" );
        }
        catch ( Exception ex )
        {
            Debug.Log ( "ERROR Deconnection" + ex.Message );
        }

    }

    public void GetVersion ()
    {
        try
        {
            version = device.GetVersion ( );
            Debug.Log ( "Bitalino's version: " + version );
        }
        catch ( Exception ex )
        {
            Debug.Log ( "ERROR Version: " + ex.Message );
        } 
    }

    public void StartAcquisition()
    {
        try
        {
            device.SamplingRate = defaultSamplingRate;
            device.AnalogChannels = defaultAnalogChannels;

            device.StartAcquisition ();
            acquisitionState = AcquisitionState.Run;
            Debug.Log ( "DONE acquisition started" );
        }
        catch ( Exception ex )
        {
            Debug.Log ( "ERROR Acquisition: " + ex.Message );
        }
        
    }

    public void StopAcquisition()
    {
        try
        {
            device.StopAcquisition ( );
            acquisitionState = AcquisitionState.NotRun;
            Debug.Log ( "DONE acquisition stopped" );
        }
        catch ( Exception ex )
        {
            Debug.Log ( "ERROR Acquisition: " + ex.Message );
        }
    }

    protected void OnReadEvent ( ReadEventArgs e )
    {
        if ( ReadEvent != null )
        {
            ReadEvent ( this, e );
        }
    }  

    public BITalinoFrame [ ] Read ( int nbSamples )
    {
        try
        {
            return device.ReadFrames ( nbSamples );
        }
        catch ( Exception ex )
        {
            Debug.Log ( "ERROR Read frames: " + ex.Message );
        }

        return null;
    }

    private void OnApplicationQuit()
    {
        if ( device != null )
        {
            device.Deconnection ( );
        }
    }

    public void SaveSample( BITalinoFrame[] sample )
    {
        foreach ( BITalinoFrame frame in sample )
        {
            string data = frame.Sequence.ToString ( ) + "\t";

            for ( int d = 0; d < 4; d++ )
            {
                data += frame.GetDigitalValue ( d ).ToString ( ) + "\t";
            }

            for ( int an = 0; an < defaultAnalogChannels.Length; an++ )
            {
                data += frame.GetAnalogValue ( an ).ToString ( ) + "\t";
            }

            using ( StreamWriter sw = File.AppendText ( this.path ) )
            {
                sw.WriteLine ( data );
                sw.Flush ( );
            }
        }
    }
}
