using System;
using System.Collections;
using System.IO.Ports;
using System.Threading;

public class BITalinoDevice {

    private int[] analogChannels;
    private int samplingRate;
    private int nbBytes;
    private int timeSleep = 200;

    private IBITalinoCommunication bitalinoCommunication;

    public BITalinoDevice( IBITalinoCommunication bitalinoCommunication, int[] analogChannels, int samplingRate )
    {
        try
        {
            if ( CheckBitalinoCommunication ( bitalinoCommunication ) )
            {
                this.bitalinoCommunication = bitalinoCommunication;
            }

            if ( CheckAnalogChannels ( analogChannels ) )
            {
                this.analogChannels = analogChannels;
            }

            if ( CheckSamplingRate ( samplingRate ) )
            {
                this.samplingRate = samplingRate;
            }

            nbBytes = CalcNbBytes ( );
        }
        catch ( Exception ex )
        {
            throw new Exception ( "Exception BITalinoDevice's creation: " + ex.Message );
        }
    }

    #region GETTER/SETTER
    public int [ ] AnalogChannels
    {
        get { return analogChannels; }
        set
        {
            if ( CheckAnalogChannels ( value ) )
            {
                analogChannels = value; 
            }
        }
    }
    

    public int SamplingRate
    {
        get { return samplingRate; }
        set
        {
            if ( CheckSamplingRate ( value ) )
            {
                samplingRate = value;
            }
        }
    }
    #endregion

    #region Check and Format and Calc
    private bool CheckBitalinoCommunication ( IBITalinoCommunication bitalinoCommunication )
    {
        if ( bitalinoCommunication == null )
        {
            throw new Exception ( " BitalinoCommunication is null " );
        }

        return true;
    }

    private bool CheckAnalogChannels ( int [ ] analogChannels )
    {
        if ( analogChannels.Length > 6 | analogChannels.Length == 0 )
        {
            throw new Exception ( "Length analogChannels" );
        }

        foreach ( int i in analogChannels )
        {
            if ( i < 0 | i > 5 )
            {
                throw new Exception ( "Value out of bounds analogChannels" );
            }
        }

        return true;
    }

    private bool CheckSamplingRate( int samplingRate )
    {
        return ( 
            samplingRate == 1000 || 
            samplingRate == 100 ||
            samplingRate == 10 ||
            samplingRate == 1
            );
    }

    private int FormatSamplingRate ( )
    {
        int bit = 0x0;

        switch ( samplingRate )
        {
            case 1000:
                bit = 0x3;
                break;
            case 100:
                bit = 0x2;
                break;
            case 10:
                bit = 0x1;
                break;
            case 1:
                bit = 0x0;
                break;
            default:
                throw new Exception ( "default SamplingRate" );
        }

        bit = ( bit << 6 ) | 0x03;

        return bit;
    }

    private int FormatAnalogChannels()
    {
        int bit = 0x01;

        foreach ( int i in analogChannels )
        {
            if ( i < 0 | i > 5 )
            {
                throw new Exception ( "Value out of bounds analogChannels" );
            }
            else
            {
                bit = bit | 1 << ( 2 + i );
            }
        }

        return bit;
    }

    private int CalcNbBytes ( )
    {
        int numberChannels = analogChannels.Length;

        if ( numberChannels <= 4 )
        {
            return ( int ) Math.Ceiling ( ( ( float ) 12 + ( float ) 10 * numberChannels ) / 8 );
        }
        else
        {
            return ( int ) Math.Ceiling ( ( ( float ) 52 + ( float ) 6 * ( numberChannels - 4 ) ) / 8 );
        }
    }

    #endregion

    public void Connection( )
    {
        try
        {
            bitalinoCommunication.Open ( );

            Thread.Sleep ( timeSleep );

            int bitSamplingRate = FormatSamplingRate ( );

            bitalinoCommunication.Write ( bitSamplingRate );
        }
        catch ( Exception ex )
        {
            throw new Exception ( " Exception on Connection: " + ex.Message );
        }
    }

    public void Deconnection ( ) 
    {
        try 
        {
            bitalinoCommunication.Close();
        } 
        catch ( Exception ex ) 
        {
          throw new Exception( "Exception on Deconnection: " + ex.Message );
        } 
        finally 
        {
            // ... Because
            // bitalinoCommunication = null;
        }
    }

    public void StartAcquisition() 
    {
        try 
        {
            int bitAnalogChannels = FormatAnalogChannels ( );

            bitalinoCommunication.Write ( bitAnalogChannels );
        } 
        catch ( Exception ex ) 
        {
            throw new Exception ( "Exception on StartAcquisition: " + ex.Message );
        }
    }

    public void StopAcquisition()
    {
        try 
        {
            bitalinoCommunication.Write ( 0 );
        } 
        catch ( Exception ex ) 
        {
            throw new Exception ( "Exception on StopAcquisition: " + ex.Message );
        }
    }

    public string GetVersion ( )
    {
        try
        {
            bitalinoCommunication.Write ( 7 );

            return bitalinoCommunication.ReadVersion ( );
        }
        catch ( Exception ex )
        {
            throw new Exception ( ex.Message );
        }
    }

    public BITalinoFrame[] ReadFrames( int nbSamples )
    {
        return bitalinoCommunication.ReadFrames ( nbBytes, analogChannels.Length, nbSamples);
    }
}
