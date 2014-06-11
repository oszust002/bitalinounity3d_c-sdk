using System;
using System.Collections;

public interface IBITalinoCommunication
{
    void Write ( int data );
    BITalinoFrame [ ] ReadFrames ( int nbBytes, int nbAnalogChannels, int nbSamples );
    void Close ( );
    void Open ( );
    string ReadVersion ( );
}
