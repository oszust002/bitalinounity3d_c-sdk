using System;
using System.Collections;
using System.IO.Ports;
using System.Text;

public sealed class BITalinoCommunicationSerialPort : IBITalinoCommunication
{
    #region GETTER/SETTER

    SerialPort SerialPort { get; set; }

    #endregion

    public BITalinoCommunicationSerialPort ( SerialPort serialPort )
    {
        SerialPort = serialPort;
    }

    public void Write ( int data )
    {
        try
        {
            byte[] buffer = BitConverter.GetBytes ( data );

            SerialPort.Write ( buffer, 0, 1 );

            SerialPort.DiscardOutBuffer ( );
        }
        catch ( Exception ex )
        {
            if ( ex is InvalidOperationException )
            {
                throw new BITalinoException ( BITalinoErrorTypes.COM_NOT_OPEN );
            }
            else if ( ex is ArgumentOutOfRangeException || ex is ArgumentNullException || ex is ArgumentException )
            {
                throw new BITalinoException ( BITalinoErrorTypes.INVALID_ARGUMENT );
            }
            else if ( ex is TimeoutException )
            {
                throw new BITalinoException ( BITalinoErrorTypes.TIME_OUT );
            }
            else
            {
                throw ex;
            }
        }
    }

    /// <summary>Read frames from the device. </summary>
    /// <param name="nbBytes">Size of a frame in byte. Calculate from <see>CalcNbBytes()</see>.</param>
    /// <param name="nbAnalogChannels">Number of channel read.</param>
    /// <param name="nbSamples">Number of frames read.</param>
    /// <returns>Return a table of frame that contain the frames read.</returns>
    /// <exception cref="BITalinoException"/>
    public BITalinoFrame [ ] ReadFrames ( int nbBytes, int nbAnalogChannels, int nbFrames )
    {
        try
        {
            BITalinoFrame[] frames = new BITalinoFrame [ nbFrames ];

            byte[] buffer = new byte [ nbBytes ];

            byte[] bTemp = new byte [ 1 ];

            int sampleCounter = 0;

            BITalinoFrame decodedFrame;
            while ( sampleCounter < nbFrames )
            {
                buffer = new byte [ nbBytes ];

                for ( int i = 0; i < nbBytes; i++ )
                {
                    SerialPort.Read ( bTemp, 0, 1 );

                    buffer [ i ] = bTemp [ 0 ];

                    bTemp = new byte [ 1 ];
                }

                decodedFrame = BITalinoFrameDecoder.Decode ( buffer, nbBytes, nbAnalogChannels );

                if ( decodedFrame.Sequence == -1 )
                {
                    while ( decodedFrame.Sequence == -1 )
                    {
                        SerialPort.Read ( bTemp, 0, 1 );

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
            if ( ex is InvalidOperationException )
            {
                throw new BITalinoException ( BITalinoErrorTypes.COM_NOT_OPEN );
            }
            else if ( ex is ArgumentOutOfRangeException || ex is ArgumentNullException || ex is ArgumentException )
            {
                throw new BITalinoException ( BITalinoErrorTypes.INVALID_ARGUMENT );
            }
            else if ( ex is TimeoutException )
            {
                throw new BITalinoException ( BITalinoErrorTypes.TIME_OUT );
            }
            else
            {
                throw ex;
            }
        }
    }

    public string ReadVersion ( )
    {
        try
        {
            string message = "";

            while ( message.Length == 0 || message [ message.Length - 1 ] != '\n' )
            {
                byte[] buffer = new byte [ SerialPort.ReadBufferSize ];

                int bytesRead = SerialPort.Read ( buffer, 0, buffer.Length );

                message += Encoding.ASCII.GetString ( buffer, 0, bytesRead );
            }

            return message;
        }
        catch ( Exception ex )
        {
            if ( ex is InvalidOperationException )
            {
                throw new BITalinoException ( BITalinoErrorTypes.COM_NOT_OPEN );
            }
            else if ( ex is ArgumentOutOfRangeException || ex is ArgumentNullException || ex is ArgumentException )
            {
                throw new BITalinoException ( BITalinoErrorTypes.INVALID_ARGUMENT );
            }
            else if ( ex is TimeoutException )
            {
                throw new BITalinoException ( BITalinoErrorTypes.TIME_OUT );
            }
            else
            {
                throw ex;
            }
        }
    }

    public void Close ( )
    {
        try
        {
            SerialPort.Close ( );
        }
        catch
        {
            throw new BITalinoException ( BITalinoErrorTypes.COM_CONNECTION_ERROR );
        }
    }

    public void Open ( )
    {
        try
        {
            SerialPort.Open ( );
        }
        catch ( Exception ex )
        {
            if ( ex is UnauthorizedAccessException )
            {
                throw new BITalinoException ( BITalinoErrorTypes.COM_ACCESS_NOT_ALLOWED );
            }
            else if ( ex is InvalidOperationException )
            {
                throw new BITalinoException ( BITalinoErrorTypes.COM_ALREADY_OPEN );
            }
            else if ( ex is System.IO.IOException )
            {
                throw new BITalinoException ( BITalinoErrorTypes.COM_CONNECTION_ERROR );
            }
            else if ( ex is ArgumentOutOfRangeException || ex is ArgumentException )
            {
                throw new BITalinoException ( BITalinoErrorTypes.INVALID_ARGUMENT );
            }
            else
            {
                throw ex;
            }
        }
    }
}
