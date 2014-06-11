using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {
    public BITalinoReader reader;

	// Use this for initialization
	void Start () {
	
	}
	
	/// <summary>
	/// Show the time since the start of the acquisition
	/// </summary>
	void Update () {
        if (reader.asStart)
            GetComponent("GUIText").guiText.text = reader.getTime();
	}
}
