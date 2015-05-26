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
        ContactPoint contact = collision.contacts[0];
        Debug.Log(contact.point);
       // if (collision.collider.tag.Equals("Answer"))
       // {
            if (Quaternion.FromToRotation(Vector3.up, contact.normal).x > 0)
            {
                Instantiate(impact, new Vector3(contact.point.x + 0.001f, contact.point.y + 0.001f, contact.point.z + 0.001f), Quaternion.FromToRotation(Vector3.up, contact.normal));
            }
            else
            {
                Instantiate(impact, new Vector3(contact.point.x + 0.001f, contact.point.y + 0.001f, contact.point.z - 0.001f), Quaternion.FromToRotation(Vector3.up, contact.normal));
            }
            
       // }

        Destroy(this.gameObject);
    }
    void OnTriggerEnter(Collider other)
    {
        Collision collision = other.GetComponent<Collision>();
 
        Vector3 point = collision.contacts[0].point;
        if (other.tag.Equals("Answer"))
        {
            Instantiate(impact, new Vector3(other.transform.position.x+0.001f, other.transform.position.y, other.transform.position.z), Quaternion.FromToRotation(Vector3.left, other.transform.eulerAngles));
            Debug.Log("answer");
        }
       //ImpactDecal bulletClone = (ImpactDecal) Instantiate(impact, other.transform.position, other.transform.rotation);
        Destroy(this.gameObject);
    }
}
