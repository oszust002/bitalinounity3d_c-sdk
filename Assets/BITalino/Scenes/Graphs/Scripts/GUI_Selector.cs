using UnityEngine;
using System.Collections;

public class GUI_Selector : MonoBehaviour {
    private bool EMG = true;
    public LineRenderer LineEMG;
    private bool EDA = true;
    public LineRenderer LineEDA;
    private bool LUX = true;
    public LineRenderer LineLUX;
    private bool ECG = true;
    public LineRenderer LineECG;
    private bool ACC = true;
    public LineRenderer LineACC;
    private bool BATT = true;
    public LineRenderer LineBATT;
	// Use this for initialization
	void Start () {
	}
	
	/// <summary>
	/// Update the state of the line renderer
	/// </summary>
	void Update () {
        LineEMG.enabled = EMG;
        LineEDA.enabled = EDA;
        LineLUX.enabled = LUX;
        LineECG.enabled = ECG;
        LineACC.enabled = ACC;
        LineBATT.enabled = BATT;
	}

    /// <summary>
    /// Drawn the GUI
    /// </summary>
    void OnGUI () {
        GUI.Box(new Rect(10, 10, 120, 160), "Graphe Selector");
        EMG = GUI.Toggle(new Rect(15, 30, 50, 20), EMG, new GUIContent("EMG"));
        EDA = GUI.Toggle(new Rect(15, 50, 50, 20), EDA, new GUIContent("EDA"));
        LUX = GUI.Toggle(new Rect(15, 70, 50, 20), LUX, new GUIContent("LUX"));
        ECG = GUI.Toggle(new Rect(15, 90, 50, 20), ECG, new GUIContent("ECG"));
        ACC = GUI.Toggle(new Rect(15, 110, 50, 20), ACC, new GUIContent("ACC"));
        BATT= GUI.Toggle(new Rect(15, 130, 50, 20), BATT, new GUIContent("BATT"));

    }
}
