using UnityEngine;
using System.Collections;

public class ConnectionState : MonoBehaviour {
    public ManagerBITalino manager;
    public BITalinoReader reader;
    public BITalinoSerialPort serial;
    public GUIText state;
    public GUIText data;

	// Use this for initialization
    void Start()
    {
        state.text = "";
        data.text = "";
        StartCoroutine(start());
	}

    /// <summary>
    /// Initialise the connection
    /// </summary>
    private IEnumerator start()
    {
        state.text = "Connecting port " + serial.portName;
        while (!manager.IsReady)
            yield return new WaitForSeconds(0.5f);
        state.text = "Connected";
        while ((int)manager.Acquisition_State != 0)
            yield return new WaitForSeconds(0.5f);
        state.text = "Acquisition start";


    }
	
	/// <summary>
	/// Write the data read from the bitalino
	/// </summary>
	void Update () 
    {
        if (reader.asStart)
        {
            data.text = reader.getBuffer()[reader.BufferSize - 1].ToString();
        }
	}
}
