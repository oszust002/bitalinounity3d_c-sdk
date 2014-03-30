using System;
using System.Collections;

public class ReadEventArgs : EventArgs
{
    private BITalinoFrame [ ] frames;

    public ReadEventArgs ( BITalinoFrame [ ] frames )
    {
        this.frames = frames;
    }

    public BITalinoFrame [ ] Frames
    {
        get { return this.frames; }
        set { this.frames = value; }
    }
}