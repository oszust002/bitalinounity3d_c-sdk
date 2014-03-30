using System;
using System.Collections;
using System.IO.Ports;
using System.Text;

public sealed class BITalinoCommunicationSerialPort : IBITalinoCommunication {

    private SerialPort serialPort;

    public BITalinoCommunicationSerialPort ( SerialPort serialPort )
    {
        this.serialPort = serialPort;
    }

    #region GETTER/SETTER

    public SerialPort SerialPort
    {
        get { return serialPort; }
    }

    #endregion

    public void Write ( int data )
    {
        try
        {
            byte[] buffer = BitConverter.GetBytes ( data );

            serialPort.Write ( buffer, 0, 1 );
            serialPort.DiscardOutBuffer ( );
        }
        catch ( Exception ex )
        {
            throw new Exception ( "Exception on Write: " + ex.Message );
        }
    }

    public BITalinoFrame [ ] ReadFrames ( int nbBytes, int nbAnalogChannels, int nbSamples )
    {
        try
        {
            BITalinoFrame[] frames = new BITalinoFrame [ nbSamples ];

            byte[] buffer = new byte [ nbBytes ];
            byte[] bTemp = new byte [ 1 ];

            int sampleCounter = 0;

            serialPort.DiscardInBuffer ( );

            while ( sampleCounter < nbSamples )
            {
                serialPort.Read ( buffer, 0, nbBytes );

                BITalinoFrame decodedFrame = BITalinoFrameDecoder.Decode ( buffer, nbBytes, nbAnalogChannels );

                if ( decodedFrame.Sequence == -1 )
                {
                    while ( decodedFrame.Sequence == -1 )
                    {
                        serialPort.Read ( bTemp, 0, 1 );

                        for ( int j = nbBytes - 2; j >= 0; j-- )
                        {
                            buffer [ j + 1 ] = buffer [ j ];
                        }

                        buffer [ 0 ] = bTemp [ 0 ];

                        decodedFrame = BITalinoFrameDecoder.Decode ( buffer, nbBytes, nbAnalogChannels );
                    }

                    frames [ sampleCounter ] = decodedFrame;
                }
                else
                {
                    frames [ sampleCounter ] = decodedFrame;
                }

                sampleCounter++;
            }

            return frames;
        }
        catch ( Exception ex )
        {
            throw new Exception ( "Exception on ReadFrames: " + ex.Message );
        }
    }

    public string ReadVersion ( )
    {
        try
        {
            string message = "";

            while ( message.Length == 0 || message [ message.Length - 1 ] != '\n' )
            {
                byte[] buffer = new byte [ serialPort.ReadBufferSize ];

                int bytesRead = serialPort.Read ( buffer, 0, buffer.Length );

                message += Encoding.ASCII.GetString ( buffer, 0, bytesRead );
            }

            return message;
        }
        catch ( TimeoutException ex )
        {
            throw new Exception ( "Timeout on GetVersion: " + ex.Message );
        }
    }

    public void Close()
    {
        serialPort.Close ( );
    }

    public void Open()
    {
        serialPort.Open ( );
    }
}
