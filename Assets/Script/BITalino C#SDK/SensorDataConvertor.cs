using System;
using System.Collections;

/**
 * Shamefully copied from the BITalino Java SDK ... :)
 * Also... http://forum.bitalino.com/viewtopic.php?f=12&t=128
 * 
 * This class holds methods for converting/scaling raw data from BITalino
 * included sensors to human-readable data.
 */
public static class SensorDataConvertor {

    private const double VCC = 3.3; // volts
    private const int ACC_MIN = 281;
    private const int ACC_MAX = 287;
    private const int ACC_DIFF = ACC_MAX - ACC_MIN;

    public static double ScaleLux( int raw )
    {
        // LUX% = 100 * (LUXB / (2^n - 1))
        return 100 * ( (double) raw / 63.0 );
    }

    public static double ScaleAcc ( int raw )
    {
        // ACCg = 2 * ((ACCB - Cm) / (CM - Cm)) - 1
        return 2.0 * ( double ) ( ( raw - ACC_MIN ) / ACC_DIFF ) - 1.0;
    }

    public static double ScaleEDA ( int raw )
    {
        // RMOhm = 1 - EDAB / ( 2^n - 1)
        // EDAµS = 1 / RMOhm
        return 1 / ( 1 - raw / 1023.0f );
    }
}
