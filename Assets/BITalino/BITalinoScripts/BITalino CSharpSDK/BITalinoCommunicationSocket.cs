
/// <summary>Has not been implemented yet. </summary>
public class BITalinoCommunicationSocket : IBITalinoCommunication {

    public void Write ( int data ) { return; }
    public BITalinoFrame [ ] ReadFrames ( int nbBytes, int nbAnalogChannels, int nbSamples ) { return null; }
    public void Close ( ) { return; }
    public void Open ( ) { return; }
    public string ReadVersion ( ) { return null; }
}
