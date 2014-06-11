using UnityEngine;
using System.Collections;

public class Helicopter : MonoBehaviour {
    public BITalinoReader reader;
    public GUIText text;

    private bool _asStart = false;
    private double Cmax = 0;
    private float rapport = 0;
    private bool movement = false;

	// Use this for initialization
	void Start () 
    {
        StartCoroutine(start());
	}

    /// <summary>
    /// get if the object is ready
    /// </summary>
    public bool asStart
    {
        get{return _asStart;}
    }

    /// <summary>
    /// Initialisation of the object
    /// </summary>
    private IEnumerator start()
    {
        while(!reader.asStart)
        {
            yield return new WaitForSeconds(0.5f);
        }
        text.text = "Calibration";

        foreach(BITalinoFrame frame in reader.getBuffer())
        {
            Cmax += frame.GetAnalogValue(2);
        }
        Cmax = Cmax / reader.BufferSize;
        rapport = (float)(9.0 / Cmax);
        text.text = "Ready ?";
        movement = true;
        while (!(getPos() < 2.5f && getPos() > -2.5f))
            yield return new WaitForSeconds(0.5f);
        text.text = "3";
        yield return new WaitForSeconds(1f);
        text.text = "2";
        yield return new WaitForSeconds(1f);
        text.text = "1";
        yield return new WaitForSeconds(1f);
        text.text = " ";
        _asStart = true;

    }

    /// <summary>
    /// Calculation of the Y position of the object
    /// </summary>
    /// <returns>Return the Y position calculated</returns>
    private float getPos()
    {
        BITalinoFrame[] frames = reader.getBuffer();
        float pos = 0;
        for (int i = 20; i > 0; i--)
            pos += (float)frames[reader.BufferSize - i].GetAnalogValue(2);
        pos = ((pos / 20f) * rapport) - 5f;
        return pos;
    }
	
	/// <summary>
	/// Calcalation of the movement of the object
	/// </summary>
	void Update () {
        if (movement)
        {
            transform.position = new Vector3(-9.0f,getPos(),0.0f);
        }
	}

    /// <summary>
    /// Collision detector
    /// </summary>
    void OnCollisionEnter()
    {
        if (_asStart)
        {
            movement = false;
            _asStart = false;
            text.text = "GameOver";
        }
    }
}
