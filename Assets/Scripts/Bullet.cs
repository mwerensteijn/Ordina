using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullet : MonoBehaviour {
    public GameObject impact;
    public static List<GameObject> ImpactList = new List<GameObject>();

    private float xOffset = 0.0f;
    private float yOffset = 0.0f;
    private float zOffset = 0.0f;

    private float offsetValue = 0.001f;

    private Ray ray;
    private RaycastHit hit;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        //if (collision.collider.tag.Equals("Answer"))
        // {
        // Check position of hit with a raycast
        //ray = Camera.main.ScreenPointToRay(contact.point);
        ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(ray, out hit, 100))
        {
            Debug.Log("rayCasted.");
        }

        if (ImpactList.Count > 50)
        {
            GameObject firstImpactInList = ImpactList[0];
            ImpactList.Remove(firstImpactInList);
            Destroy(firstImpactInList);
        }
        //Debug.Log(contact.normal);
        //Debug.Log(Quaternion.FromToRotation(Vector3.up, contact.normal));
        if (contact.normal.x > 0)
        {
            xOffset = offsetValue;
        }
        else if (contact.normal.x < 0)
        {
            xOffset = -offsetValue;
        }
        else
        {
            xOffset = 0;
        }
        if (contact.normal.y > 0)
        {
            yOffset = offsetValue;
        }
        else if (contact.normal.y < 0)
        {
            yOffset = -offsetValue;
        }
        else
        {
            yOffset = 0;
        }
        if (contact.normal.z > 0)
        {
            zOffset = offsetValue;
        }
        else if (contact.normal.z < 0)
        {
            zOffset = -offsetValue;
        }
        else
        {
            zOffset = 0;
        }
        //Debug.Log(contact.point);
        //Debug.Log(collision.contacts.Length);
        //Debug.Log(hit.point);
        //ImpactList.Add(Instantiate(impact, new Vector3(hit.point.x + xOffset, hit.point.y + yOffset, hit.point.z + zOffset), Quaternion.FromToRotation(Vector3.up, contact.normal)) as GameObject);
        
       
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Debug.Log(rotation.eulerAngles);
        // Quad needs to be rotated 90 degrees
        Quaternion test = Quaternion.Euler(rotation.eulerAngles.x , rotation.eulerAngles.y, rotation.eulerAngles.z);
        test.eulerAngles = new Vector3(rotation.eulerAngles.x + 270, rotation.eulerAngles.y + 180, rotation.eulerAngles.z);
        Debug.Log(test.eulerAngles);
        ImpactList.Add(Instantiate(impact, new Vector3(contact.point.x + xOffset, contact.point.y + yOffset, contact.point.z + zOffset), test) as GameObject);
        Debug.Log(ImpactList.Count);

        Destroy(this.gameObject);
    }
}
