using System.Collections;
using UnityEngine;
using System.IO;

public class CameraDataGathering : MonoBehaviour
{
    private LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        StartCoroutine(GatherSightData());
    }

    void PrintData(Vector3 headPosition, Vector3 lookPosition, Collider lookItem)
    {
        string path = Application.dataPath + "/PlayerVision.txt";
        if (!File.Exists(path)) File.WriteAllText(path, "head position, look position, look item name\n");
        string observation = (headPosition - new Vector3(5.3f, 0, -7.4f)) + ", " + (lookPosition - new Vector3(5.3f, 0, -7.4f)) + ", " + lookItem.name + "\n";
        File.AppendAllText(path, observation);
    }

    private IEnumerator GatherSightData()
    {
        while(true)
        {
            RaycastHit hitInfo;
            bool hit = Physics.Raycast(transform.position, transform.forward, out hitInfo, 100);
            if (hit)
            {
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, hitInfo.point);
                //Debug.Log(hitInfo.collider.name);
                PrintData(transform.position, hitInfo.point, hitInfo.collider);
            }
            yield return new WaitForSeconds(0.5f);
        }
        
    }
}
