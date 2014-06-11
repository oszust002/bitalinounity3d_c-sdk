using System;
using System.Collections.Generic;

public class BITalinoFrame
{

    private double[] analog = new double [ 6 ];

    private int[] digital = new int [ 4 ];

    #region GETTER/SETTER

    public int CRC { get; set; }

    public int Sequence { get; set; }

    public double GetAnalogValue ( int idx )
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

    public void SetAnalogValue ( int idx, double value )
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

    public void SetDigitalValue ( int idx, int value )
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

    public BITalinoFrame ( )
    { }

    public override string ToString ( )
    {
        return "CRC " + CRC +
            " SEQ " + Sequence +
            " Analog values " + String.Join ( ";", new List<double> ( analog ).ConvertAll ( i => i.ToString ( ) ).ToArray ( ) ) +
            " Digital values " + String.Join ( ";", new List<int> ( digital ).ConvertAll ( i => i.ToString ( ) ).ToArray ( ) );
    }
}
