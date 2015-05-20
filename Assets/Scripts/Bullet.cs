using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
    public GameObject impact;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision collision)
    {
      /*  foreach (ContactPoint contact in collision.contacts)
        {
            Instantiate(impact, new Vector3(contact.point.x+0.001f, contact.point.y, contact.point.z), Quaternion.FromToRotation(Vector3.up, contact.normal));
        }*/
        ContactPoint contact = collision.contacts[0];
        //if (collision.collider.tag.Equals("Answer"))
       // {
            Instantiate(impact, new Vector3(contact.point.x + 0.001f, contact.point.y + 0.001f, contact.point.z + 0.001f), Quaternion.FromToRotation(Vector3.up, contact.normal));
       // }

        Destroy(this.gameObject);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Answer"))
        {
            Instantiate(impact, new Vector3(other.transform.position.x+0.001f, other.transform.position.y, other.transform.position.z), Quaternion.FromToRotation(Vector3.left, other.transform.eulerAngles));
            Debug.Log("answer");
        }
       //ImpactDecal bulletClone = (ImpactDecal) Instantiate(impact, other.transform.position, other.transform.rotation);
        Destroy(this.gameObject);
    }
}
