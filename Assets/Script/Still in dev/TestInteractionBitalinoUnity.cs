using UnityEngine;
using System.Collections;
using System.Threading;
using System;
using System.IO;

public class TestInteractionBitalinoUnity : MonoBehaviour {

    public ManagerBitalino myManagerBitalino;
    private static ManagerBitalino _managerBitalino;

    public int nbFrames = 10;
    private static int _nbFrames;

    public int samplingRate = 100;

    public TextMesh textMesh;
    public double theresfoldAcc = 10;

    private static double currentValueAcc = 0.0;
 
    private bool canRead = false;

    private static bool refreshRenderer = false;
    private static bool moving = false;

    private Thread readThread;
    private static bool _continue = true;

    private System.Object _lock = new System.Object ( );

	// Use this for initialization
	void Start () 
    {
        _managerBitalino = myManagerBitalino;
        _nbFrames = nbFrames;

        if ( textMesh.gameObject.activeSelf == false )
        {
            textMesh.gameObject.SetActive ( true );
        }

        _managerBitalino.defaultAnalogChannels = new int[] { 4 }; // Use Acc
        _managerBitalino.defaultSamplingRate = samplingRate;

        readThread = new Thread ( Read );

        StartCoroutine ( Test ( ) );

        _managerBitalino.ReadEvent += new ReadEventHandler ( HandleChangeFrame );
	}

    IEnumerator Test ()
    {
        while ( _managerBitalino.IsReady == false )
        {  
            yield return null;
        }


        _managerBitalino.Connection ( );
        yield return new WaitForSeconds ( 0.5f );

        _managerBitalino.StartAcquisition ( );
        yield return new WaitForSeconds ( 0.5f );

        canRead = true;
    }

    void Update ( )
    {

        if ( canRead == true && readThread.IsAlive == false && _continue == true )
        {
            readThread.Start ( );
        }

        if ( Input.GetKey ( KeyCode.Return ) )
        {
            _continue = !_continue;

            if ( _continue == false )
            {
                readThread.Join ( );
            }
        }

        if ( refreshRenderer == true )
        {
            Debug.Log ( moving );
            if ( moving == false )
            {
                textMesh.text = "You are not moving me.";
            }
            else
            {
                textMesh.text = "You are moving me...\n Stop it please.\n\n( " + currentValueAcc + " )";
            }

            refreshRenderer = false;
        }

    }

    public void HandleChangeFrame ( object sender, ReadEventArgs e )
    {
        double val;
        _managerBitalino.SaveSample ( e.Frames );
        foreach ( BITalinoFrame frame in e.Frames )
        {
            val = SensorDataConvertor.ScaleAcc ( frame.GetAnalogValue ( 0 ) );

            if ( val > theresfoldAcc )
            {
                currentValueAcc = val;
                moving = true;
                refreshRenderer = true;
                return ;
            }
        }

        moving = false;
        refreshRenderer = true;
    }

    private void Read ( )
    {
        while ( _continue )
        {
            _managerBitalino.Read ( _nbFrames );
        }
    }

    void OnApplicationQuit ( )
    {
        if ( readThread != null && readThread.IsAlive == true )
        {
            _continue = false;

            readThread.Abort ( );
        }
    }
}
