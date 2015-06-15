using UnityEngine;
using System.Collections;

public class Watergun : MonoBehaviour {

    public GameObject waterGun;

	// Use this for initialization
	void Start () {
        waterGun.GetComponent<EllipsoidParticleEmitter>().emit = false;
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void Shoot()
    {
        waterGun.GetComponent<EllipsoidParticleEmitter>().emit = true;
        Invoke("Start", 0.3f);
    }

    void OnParticleCollision(GameObject obj)
    {
        //Debug.Log("haha, hit!");
        if (obj.tag == "Paintspot")
        {
            Destroy(obj);
        }
        if (obj.tag == "AnswerGiven")
        {
            obj.GetComponent<Answer>().reset();
            obj.tag = "Answer";
        }
        if (obj.tag == "Question")
        {
            obj.GetComponent<PictureQuestion>().reset();
        }
    }
}
