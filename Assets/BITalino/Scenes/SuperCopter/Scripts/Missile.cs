using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour {
    public Helicopter helico;
    public GUIText score;
    public float speed = 0.1f;

	// Use this for initialization
	void Start () {
	
	}
	
	/// <summary>
	/// Move the obstacles
	/// </summary>
    void Update()
    {
        if (helico.asStart)
        {
            transform.Translate(new Vector3(-speed, 0f, 0f));
            if (transform.position.x < -11)
            {
                score.text = (int.Parse(score.text) + 1).ToString();
                Destroy(this.gameObject);
            }
        }
    }
}
