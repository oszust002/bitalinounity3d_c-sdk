using System;
using System.Collections;

class BITalinoErrorTypes
{
    public static readonly BITalinoErrorTypes COM_ACCESS_NOT_ALLOWED    = new BITalinoErrorTypes ( 0, "The access to the port COM is not allowed." );
    public static readonly BITalinoErrorTypes COM_ALREADY_OPEN          = new BITalinoErrorTypes ( 1, "The COM port is already open." );
    public static readonly BITalinoErrorTypes COM_NOT_OPEN              = new BITalinoErrorTypes ( 2, "The COM port is not open." );
    public static readonly BITalinoErrorTypes COM_CONNECTION_ERROR      = new BITalinoErrorTypes ( 3, "Bluetooth device not connected OR BITalino unreachable." );
    public static readonly BITalinoErrorTypes ANALOG_CHANNELS_NOT_VALID = new BITalinoErrorTypes ( 4, "Analog Channel value must be set as 0, 1, 2, 3, 4 or 5." );
    public static readonly BITalinoErrorTypes SAMPLING_RATE_NOT_DEFINED = new BITalinoErrorTypes ( 5, "Sampling Rate value must be set as 1000, 100, 10 or 1." );
    public static readonly BITalinoErrorTypes INVALID_ARGUMENT          = new BITalinoErrorTypes ( 6, "Invalid parameter(s)." );
    public static readonly BITalinoErrorTypes TIME_OUT                  = new BITalinoErrorTypes ( 7, "The operation timed out." );
    public static readonly BITalinoErrorTypes INCORRECT_DECODE          = new BITalinoErrorTypes ( 8, "Incorrect data to be decoded." );

    #region GETTER/SETTER

    public int Value { get; private set; }

    public string Message { get; private set; }

    #endregion

    private BITalinoErrorTypes ( int value, String message )
    {
        Value = value;

        Message = message;
    }
}