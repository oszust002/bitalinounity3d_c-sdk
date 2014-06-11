using UnityEngine;
using System.Collections;

public class Line : MonoBehaviour {
    public BITalinoReader reader;
    public int channelRead = 0;
    public double divisor = 1;

    private LineRenderer line;

	// Use this for initialization
	void Start () {
        line = (LineRenderer) this.GetComponent("LineRenderer");
        line.SetVertexCount(reader.BufferSize);
	}
	
	/// <summary>
	/// Draw the new point of the line
	/// </summary>
	void Update () {
        if (reader.asStart)
        {
            int i = 0;
            foreach(BITalinoFrame f in reader.getBuffer())
            {
                float posX = (float) (-7.5f+15f*((1.0/reader.BufferSize)*i));
                float posY = (float) ((f.GetAnalogValue(channelRead)) / divisor);
                line.SetPosition(i, new Vector3(posX, posY, 0));
                i++;
            }
        }
	}
}
