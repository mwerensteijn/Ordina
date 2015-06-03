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
        // Cast ray to be sure about the position.
        ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (!(Physics.Raycast(ray, out hit, 100)))
        {
            // ray didn't hit an object, return.
            return;
        }

        if (ImpactList.Count > 50)
        {
            GameObject firstImpactInList = ImpactList[0];
            ImpactList.Remove(firstImpactInList);
            Destroy(firstImpactInList);
        }

        // Offset is needed so the impact won't get stuck in an object
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

        // Spawn impact
        ImpactList.Add(Instantiate(impact, new Vector3(hit.point.x + xOffset, hit.point.y + yOffset, hit.point.z + zOffset), Quaternion.FromToRotation(Vector3.up, contact.normal)) as GameObject);
        Debug.Log(ImpactList.Count);

        // Destory bullet
        Destroy(this.gameObject);
    }
}
