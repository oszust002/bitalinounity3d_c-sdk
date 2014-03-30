using UnityEngine;
using System.Collections;
using System.IO;

/*
 * This is a basic test to
 * 1/ See if everything work.
 * 2/ See how the data are received
 * */

public class TestBitalino : MonoBehaviour {

    public ManagerBitalino managerBitalino;

	// Use this for initialization
    void Start ( )
    {
        StartCoroutine ( Test ( ) );
    }

    IEnumerator Test()
    {
        while ( managerBitalino.IsReady == false )
        {
            yield return new WaitForSeconds( 0.5f );
        }


        managerBitalino.Connection ( );
        yield return new WaitForSeconds ( 0.5f );

        managerBitalino.StartAcquisition ( );
        yield return new WaitForSeconds ( 0.5f );

        BITalinoFrame[] sample1 = managerBitalino.Read ( 100 );
        BITalinoFrame[] sample2 = managerBitalino.Read ( 100 );
        BITalinoFrame[] sample3 = managerBitalino.Read ( 100 );
        BITalinoFrame[] sample4 = managerBitalino.Read ( 100 );

        yield return new WaitForSeconds ( 0.5f );

        BITalinoFrame[] sample5 = managerBitalino.Read ( 100 );
        BITalinoFrame[] sample6 = managerBitalino.Read ( 100 );

        managerBitalino.StopAcquisition ( );
       
        foreach ( BITalinoFrame frame in sample1 )
        {
            Debug.Log ( frame.ToString() );

            using ( StreamWriter sw = File.AppendText ( managerBitalino.path ) )
            {
                sw.WriteLine ( frame.ToString ( ) );
                sw.Flush ( );
            }
        }

        managerBitalino.SaveSample ( sample1 );
        managerBitalino.SaveSample ( sample2 );
        managerBitalino.SaveSample ( sample3 );
        managerBitalino.SaveSample ( sample4 );

        using ( StreamWriter sw = File.AppendText ( managerBitalino.path ) )
        {
            sw.WriteLine ( "\n\n1000 SAMPLES \n\n" );
            sw.Flush ( );
        }

        managerBitalino.SaveSample ( sample5 );
        managerBitalino.SaveSample ( sample6 );

        managerBitalino.StopAcquisition ( );

        managerBitalino.Deconnection ( );
    }
	
	// Update is called once per frame
	void Update () {
	    
	}


}
