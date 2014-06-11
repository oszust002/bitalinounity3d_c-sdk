using UnityEngine;
using System.Collections;

public class MissileManager : MonoBehaviour {
    public Helicopter helico;
    public GameObject obstacle;
    public GUIText score;

    private float time = 3f;

	// Use this for initialization
	void Start () {
        StartCoroutine(manager());
	}

    /// <summary>
    /// When the game start, the manager generate obstacles
    /// </summary>
    private IEnumerator manager()
    {
        while (!helico.asStart)
            yield return new WaitForSeconds(0.5f);
        while(helico.asStart)
        {
            float posY = Random.Range(-4f, 4f);
            GameObject go = (GameObject)Instantiate(obstacle, new Vector3(11f, posY, 0f), new Quaternion());
            Missile ob = (Missile)go.GetComponent("Missile");
            ob.helico = this.helico;
            ob.score = this.score;
            yield return new WaitForSeconds(time);
            if (time > 1f)
                time -= 0.1f;
        }
    }
    	
	// Update is called once per frame
	void Update () 
    {
	}
}
