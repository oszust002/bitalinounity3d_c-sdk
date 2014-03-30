using System;
using System.Collections;

public sealed class BITalinoFrameDecoder {

    public static BITalinoFrame Decode ( byte[] buffer, int nbBytes, int nbAnalogChannels )
    {
        try
        {
            BITalinoFrame decodeFrame;

            int j = ( nbBytes - 1 ), CRC = 0, x0 = 0, x1 = 0, x2 = 0, x3 = 0, outs = 0, inp = 0;
            CRC = ( buffer [ j - 0 ] & 0x0F ) & 0xFF;

            // check CRC
            for ( int bytes = 0; bytes < nbBytes; bytes++ )
            {
                for ( int bit = 7; bit > -1; bit-- )
                {
                    inp = ( buffer [ bytes ] ) >> bit & 0x01;
                    if ( bytes == ( nbBytes - 1 ) && bit < 4 )
                    {
                        inp = 0;
                    }
                    outs = x3;
                    x3 = x2;
                    x2 = x1;
                    x1 = outs ^ x0;
                    x0 = inp ^ outs;
                }
            }

            if ( CRC == ( ( x3 << 3 ) | ( x2 << 2 ) | ( x1 << 1 ) | x0 ) )
            {
                /*parse frames*/
                decodeFrame = new BITalinoFrame ( );
                decodeFrame.Sequence = ( short ) ( ( buffer [ j - 0 ] & 0xF0 ) >> 4 ) & 0xf;
                decodeFrame.SetDigitalValue ( 0, ( short ) ( ( buffer [ j - 1 ] >> 7 ) & 0x01 ) );
                decodeFrame.SetDigitalValue ( 1, ( short ) ( ( buffer [ j - 1 ] >> 6 ) & 0x01 ) );
                decodeFrame.SetDigitalValue ( 2, ( short ) ( ( buffer [ j - 1 ] >> 5 ) & 0x01 ) );
                decodeFrame.SetDigitalValue ( 3, ( short ) ( ( buffer [ j - 1 ] >> 4 ) & 0x01 ) );

                switch ( nbAnalogChannels )
                {
                    case 6:
                        decodeFrame.SetAnalogValue ( 5, ( short ) ( ( buffer [ j - 7 ] & 0x03F ) ) );
                        goto case 5;
                    case 5:
                        decodeFrame.SetAnalogValue ( 4, ( short ) ( ( ( ( buffer [ j - 6 ] & 0x0F ) << 2 ) | ( ( buffer [ j - 7 ] & 0xC0 ) >> 6 ) ) & 0x03F ) );
                        goto case 4;
                    case 4:
                        decodeFrame.SetAnalogValue ( 3, ( short ) ( ( ( ( buffer [ j - 5 ] & 0x3F ) << 4 ) | ( ( buffer [ j - 6 ] & 0xf0 ) >> 4 ) ) & 0x3FF ) );
                        goto case 3;
                    case 3:
                        decodeFrame.SetAnalogValue ( 2, ( short ) ( ( ( ( buffer [ j - 4 ] & 0xFF ) << 2 ) | ( ( ( buffer [ j - 5 ] & 0xC0 ) >> 6 ) ) ) & 0x3FF ) );
                        goto case 2;
                    case 2:
                        decodeFrame.SetAnalogValue ( 1, ( short ) ( ( ( ( buffer [ j - 2 ] & 0x03 ) << 8 ) | ( buffer [ j - 3 ] ) & 0xFF ) & 0x3FF ) );
                        goto case 1;
                    case 1:
                        decodeFrame.SetAnalogValue ( 0, ( short ) ( ( ( ( buffer [ j - 1 ] & 0x0F ) << 6 ) | ( ( buffer [ j - 2 ] & 0XFC ) >> 2 ) ) & 0x3FF ) );
                        break;
                }
            }
            else
            {
                decodeFrame = new BITalinoFrame ( );
                decodeFrame.Sequence = -1;
            }

            return decodeFrame;
        }
        catch ( Exception ex )
        {
            throw new Exception ( "Incorrect Decoding " + ex.ToString ( ) );
        }
    }

}
