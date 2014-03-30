using UnityEngine;
using System.Collections;

public class GUIBitalino : MonoBehaviour {

    private ManagerBitalino managerB;

    public ManagerBitalino ManagerB { get; set; }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI ( )
    {
        if ( GUI.Button ( new Rect ( 10, 10, 100, 25 ), "CONNECTION" ) )
        {
            ManagerB.Connection ( );
        }

        if ( GUI.Button ( new Rect ( 10, 50, 100, 25 ), "DECONNECTION" ) )
        {
            ManagerB.Deconnection ( );
        }

        if ( GUI.Button ( new Rect ( 10, 100, 100, 25 ), "START ACQUISITION" ) )
        {
            ManagerB.StartAcquisition ( );
        }

        if ( GUI.Button ( new Rect ( 110, 100, 100, 25 ), "STOP ACQUISITION" ) )
        {
            ManagerB.StopAcquisition ( );
        }

        if ( GUI.Button ( new Rect ( 10, 150, 100, 25 ), "VERSION" ) )
        {
            ManagerB.GetVersion ( );
        }
    }
}
