using UnityEngine;
using System.Collections;

public class BulletSpawner : MonoBehaviour {
    public GameObject bullet;
    public float speed = 10f;
    public float shotDelay = 0.5f;

    private float lastShotTime = 0;
    private Rigidbody bulletClone;

   

	// Use this for initialization
	void Start () {
        GameObject test = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
        test.gameObject.GetComponent<MeshRenderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void Shoot()
    {
        if ((Time.time - lastShotTime) < shotDelay)
        {
            return;
        }
        lastShotTime = Time.time;

        if (bulletClone != null)
        {
            bulletClone.GetComponent<MeshRenderer>().enabled = true;
            bulletClone.position = transform.position;
            bulletClone.velocity = transform.forward * speed;
        }
        else
        {
            GameObject bulletCloneGO = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
            bulletClone = bulletCloneGO.GetComponent<Rigidbody>();
            bulletClone.velocity = transform.forward * speed;
        }
        
        
        /*Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        if (!(Physics.Raycast(ray, out hit, 100)))
        {
            //return;
        }
        else
        {
            Debug.DrawRay(hit.point, hit.normal, Color.white, 10);

        }
         
       
        ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2 + Screen.width / 100, Screen.height / 2, 0));
        if (!(Physics.Raycast(ray, out hit, 100)))
        {
            //return;
        }
        Debug.DrawRay(hit.point, hit.normal, Color.blue, 10);
        ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2 - Screen.width / 100, Screen.height / 2, 0));
        if (!(Physics.Raycast(ray, out hit, 100)))
        {
            //return;
        }
        Debug.DrawRay(hit.point, hit.normal, Color.blue, 10);

        ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2 + Screen.height / 100, 0));
        if (!(Physics.Raycast(ray, out hit, 100)))
        {
            //return;
        }
        Debug.DrawRay(hit.point, hit.normal, Color.red, 10);
        ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2 - Screen.height / 100, 0));
        if (!(Physics.Raycast(ray, out hit, 100)))
        {
            //return;
        }
        Debug.DrawRay(hit.point, hit.normal, Color.red, 10);

        float width = 1f;
        float height = 1f;
        Mesh m = new Mesh();
        m.name = "ScriptedMesh";
        m.vertices = new Vector3[] {
             new Vector3(-width, -height, 0.01f),
             new Vector3(0, -height, 0.01f),
             new Vector3(width, -height, 0.01f),
             new Vector3(width, 0, 0.01f),
             new Vector3(0, 0, 0.01f),
             new Vector3(-width, 0, 0.01f),
             new Vector3(width, height, 0.01f),
             new Vector3(0, height, 0.01f),
             new Vector3(-width, height, 0.01f)
        };
        m.uv = new Vector2[] {
             new Vector2 (0, 0),
             new Vector2 (0.5f, 0),
             new Vector2(1, 0),
             new Vector2 (1, 0.5f),
             new Vector2 (0.5f, 0.5f),
             new Vector2 (0, 0.5f),
             new Vector2(1, 1),
             new Vector2 (0.5f, 1),
             new Vector2 (0, 1)
        };
        m.triangles = new int[] { 0, 4, 1, 0, 5, 4, 1, 4, 2, 2, 4, 3, 8, 7, 4, 8, 4, 5, 6, 4, 7, 6, 3, 4};
        m.RecalculateNormals();

        GameObject obj = new GameObject("New_Plane_Fom_Script",  typeof(MeshRenderer), typeof(MeshFilter));
        obj.GetComponent<MeshFilter>().mesh = m;*/

    }
}
