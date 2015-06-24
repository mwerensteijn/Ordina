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

    private Camera camera;
    private static GameObject answer = null;

	// Use this for initialization
	void Start () {
        GameObject c = GameObject.FindGameObjectWithTag("CenterCamera");
        if (c == null) {
            c = GameObject.FindGameObjectWithTag("MainCamera");
        }

        camera = c.GetComponent<Camera>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        // Cast ray to be sure about the position.
        ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (!(Physics.Raycast(ray, out hit, 100)))
        {
            // ray didn't hit an object, return.
            return;
        }

        // Check if last bullet did hit an answer
        if (answer != null)
        {
            // If a question is hit the answer should be on the position of the question
            if (hit.collider.tag == "Question")
            {
                hit.collider.GetComponent<PictureQuestion>().setAnswer(answer.GetComponent<Answer>());
                hit.collider.GetComponent<Renderer>().material = answer.GetComponent<Renderer>().material;

                Vector2[] answerUV = answer.GetComponent<MeshFilter>().mesh.uv;
                hit.collider.GetComponent<MeshFilter>().mesh.uv = answerUV;
            }
            else
            {
                answer.SetActive(true);
            }
            answer = null;
        }

        // Check if the bullet did hit an answer
        if (hit.collider.tag == "Answer")
        {
            answer = hit.collider.gameObject;
            answer.SetActive(false);
            // Destroy bullet
            Destroy(this.gameObject);
            return;
        }
        else if(hit.collider.tag == "Submit")
        {
            hit.collider.GetComponent<SubmitAnswers>().Submit();
        }



        if (ImpactList.Count > 50)
        {
            GameObject firstImpactInList = ImpactList[0];
            ImpactList.Remove(firstImpactInList);
            Destroy(firstImpactInList);
        }

        // Offset is needed so the impact won't get stuck in an object
        calculateOffset(hit.normal);

        // Spawn impact
        GameObject impactA = Instantiate(impact, new Vector3(hit.point.x + xOffset, hit.point.y + yOffset, hit.point.z + zOffset), Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject;

        impactA.transform.parent = hit.collider.gameObject.transform;
        ImpactList.Add(impactA);
        //Debug.Log(ImpactList.Count);

        // Destroy bullet
        Destroy(this.gameObject);
    }

    // Offset is needed for impact because otherwise impact will get stuck in the object.
    private void calculateOffset(Vector3 normal)
    {
        if (normal.x > 0)
        {
            xOffset = offsetValue;
        }
        else if (normal.x < 0)
        {
            xOffset = -offsetValue;
        }
        else
        {
            xOffset = 0;
        }
        if (normal.y > 0)
        {
            yOffset = offsetValue;
        }
        else if (normal.y < 0)
        {
            yOffset = -offsetValue;
        }
        else
        {
            yOffset = 0;
        }
        if (normal.z > 0)
        {
            zOffset = offsetValue;
        }
        else if (normal.z < 0)
        {
            zOffset = -offsetValue;
        }
        else
        {
            zOffset = 0;
        }
    }


    public GameObject getAnswer()
    {
        return answer;
    }
}

