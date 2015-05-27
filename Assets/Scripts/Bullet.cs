using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullet : MonoBehaviour {
    public GameObject impact;
    public static List<GameObject> ImpactList = new List<GameObject>();

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
        if (ImpactList.Count > 50)
        {
            GameObject firstImpactInList = ImpactList[0];
            ImpactList.Remove(firstImpactInList);
            Destroy(firstImpactInList);
        }
        ImpactList.Add(Instantiate(impact, new Vector3(contact.point.x + 0.001f, contact.point.y + 0.001f, contact.point.z + 0.001f), Quaternion.FromToRotation(Vector3.up, contact.normal)) as GameObject);
        Debug.Log(ImpactList.Count);

        Destroy(this.gameObject);
    }
    //void ontriggerenter(collider other)
    //{
    //    if (other.tag.equals("answer"))
    //    {
    //        instantiate(impact, new vector3(other.transform.position.x + 0.001f, other.transform.position.y, other.transform.position.z), quaternion.fromtorotation(vector3.left, other.transform.eulerangles));
    //        debug.log("answer");
    //    }
    //    //impactdecal bulletclone = (impactdecal) instantiate(impact, other.transform.position, other.transform.rotation);
    //    destroy(this.gameobject);
    //}
}
