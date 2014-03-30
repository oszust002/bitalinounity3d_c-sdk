using System;
using System.Collections;
using System.Collections.Generic;

public class BITalinoFrame {

    private int cRC;
    private int sequence;
    private int[] analog = new int[6];
    private int[] digital = new int[4];

    public BITalinoFrame()
    {

    }

#region GETTER/SETTER

    public int CRC
    {
      get { return cRC; }
      set { cRC = value; }
    }

    public int Sequence
    {
      get { return sequence; }
      set { sequence = value; }
    }

    public int GetAnalogValue ( int  idx )
    {
        try
        {
            return analog [ idx ];
        }
        catch ( IndexOutOfRangeException ex )
        {
            throw ex;
        }
    }

    public void SetAnalogValue( int idx, int value )
    {
        try
        {
            analog [ idx ] = value;
        }
        catch ( IndexOutOfRangeException ex )
        {
            throw ex;
        }
    }

    public int GetDigitalValue ( int idx )
    {
        try
        {
            return digital [ idx ];
        }
        catch ( IndexOutOfRangeException ex )
        {
            throw ex;
        }
    }

    public void SetDigitalValue( int idx, int value )
    {
        try
        {
            digital [ idx ] = value;
        }
        catch ( IndexOutOfRangeException ex )
        {
            throw ex;
        }
    }

#endregion

    public override string ToString() {
        return "CRC " + cRC +
            " SEQ " + sequence +
            " Analog values " + String.Join ( ";", new List<int> ( analog ).ConvertAll ( i => i.ToString ( ) ).ToArray ( ) ) +
            " Digital values " + String.Join ( ";", new List<int> ( digital ).ConvertAll ( i => i.ToString ( ) ).ToArray ( ) );
    }
}
