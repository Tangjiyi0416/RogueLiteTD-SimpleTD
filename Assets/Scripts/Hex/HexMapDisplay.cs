using UnityEngine;
public class HexMapDisplay : MonoBehaviour
{
    public int maxWidth = 10, maxHeight = 10;
    public float hexSize = 1f;
    private HexMap map;

    Plane m_Plane;
    Vector3 m_DistanceFromCamera;
    public float m_DistanceZ;

    void Start()
    {
        if (map == null)
        {
            map = new HexMap(maxWidth, maxHeight, hexSize);
        }
        Camera.main.transform.position = new Vector3(maxWidth/2*hexSize*1.7320508f,maxHeight*hexSize*2/3,-m_DistanceZ);
        m_DistanceFromCamera = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z + m_DistanceZ);
        m_Plane = new Plane(Vector3.back, m_DistanceFromCamera);
    }
    void Update()
    {
        //Detect when there is a mouse click
        if (Input.GetMouseButtonDown(0))
        {
            //Create a ray from the Mouse click position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //Initialise the enter variable
            float enter = 0.0f;

            if (m_Plane.Raycast(ray, out enter))
            {
                //Get the point that is clicked
                Vector3 hitPoint = ray.GetPoint(enter);
                Debug.Log(map.GetMapCoordFromWorldCoord(hitPoint.x, hitPoint.y));
            }
        }
    }

    void OnDrawGizmos()
    {
        if (map == null) return;

        Gizmos.color = Color.red;
        for (int r = 0; r < 10; r++)
        {
            for (int q = -(r >> 1); q < 10 - (r >> 1); q++)
            {
                Gizmos.DrawWireSphere(map.GetWorldCoordFromMapCoord(q, r), 0.7f);
            }
        }
    }
}