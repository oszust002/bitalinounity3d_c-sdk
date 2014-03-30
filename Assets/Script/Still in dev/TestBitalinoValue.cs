using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;
using System.Threading;
using System.ComponentModel;


/*
 * This is a basic test to
 * 1/ See if everything work.
 * 2/ See how the data are received
 * */

public class TestBitalinoValue : MonoBehaviour
{
    public ManagerBitalino myManagerBiltalino;
    public int nbSamples = 100;

    private Thread readThread;
    private static bool _continue = false;
    private static ManagerBitalino _managerBitalino;
    
    public string path = "C:\\Users\\supercourgette\\";

    public TextMesh textUnity;
    private static bool refresh = false;

    // Use this for initialization
    void Start ( )
    {
        _managerBitalino = myManagerBiltalino;

        path += DateTime.Now.ToString ( "MMddHHmmssfff" ) + ".txt";
        _managerBitalino.path = path;

        readThread = new Thread ( Read );

        StartCoroutine ( Test ( ) );
    }

    IEnumerator Test ( )
    {
        while ( _managerBitalino.IsReady == false )
        {
            yield return new WaitForSeconds ( 0.25f );
        }

        _managerBitalino.Connection ( );
        yield return new WaitForSeconds ( 0.25f );

        yield return null;
    }

    // Update is called once per frame
    void Update ( )
    {
        if ( Input.GetKeyUp ( KeyCode.Keypad0 ) )
        {
            if ( readThread.IsAlive == true )
            {
                Debug.Log ( "Thread is about to end... " );

                _continue = false;
            }
        }

        if ( Input.GetKeyUp ( KeyCode.Keypad1 ) )
        {
            if ( readThread.IsAlive == false )
            {
                _managerBitalino.StartAcquisition ( );

                Debug.Log ( "Thread is about to start... " );

                _continue = true;

                readThread = new Thread ( Read );
                readThread.Name = "Read and Save";

                readThread.Start ( );
            }
        }

        if ( refresh == true )
        {
            textUnity.text = count + " " + val;
            
            refresh = false;
        }

        time = Time.time;
    }

    private static string val;
    private static int count = 0;
    private static float time;

    void Read ( )
    {
        Debug.Log ( " START " + Thread.CurrentThread.Name );

        while ( _continue == true )
        {
            Debug.Log ( time );
            BITalinoFrame[] f = _managerBitalino.Read ( nbSamples );
            Debug.Log ( time );
            val = f [ 0 ].ToString ( );

            refresh = true;

            count++;
        }

        Debug.Log ( " STOP " + Thread.CurrentThread.Name );
    }

    void OnApplicationQuit()
    {
        _continue = false;
    }
}
