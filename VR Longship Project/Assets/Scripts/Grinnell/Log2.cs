using System;
using System.Collections;
using UnityEngine;

public class Log2 : MonoBehaviour
{
    public GameObject splittedLog;
    private Wedge[] wedges;
    private Mesh mesh;
    private Vector3[] vertices;
    private bool canHit = true;
    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;

        wedges = GetComponentsInChildren<Wedge>();
    }

    public void SplitCheck()
    {
        foreach(Wedge wedge in wedges) if(!wedge.ready) return;

        splittedLog.SetActive(true);

        foreach(Wedge wedge in wedges) wedge.RemoveFromLog();

        Destroy(gameObject);
    }

    public void PushClosestVertex(Vector3 pushPoint, Vector3 pushDirection, float pushDist)
    {
        float closestVertexDistance = float.PositiveInfinity;
        Vector3 closestVertex = Vector3.zero;
        for (var i = 0; i < vertices.Length; i++)
        {
            float vertexDist = (transform.TransformPoint(vertices[i]) - pushPoint).magnitude;
            if (vertexDist < closestVertexDistance)
            {
                closestVertexDistance = vertexDist;
                closestVertex = vertices[i];
            }
        }
        for (var i = 0; i < vertices.Length; i++)
        {
            if (vertices[i] == closestVertex) vertices[i] += pushDirection.normalized * pushDist;
        }
        
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
    }

    private IEnumerator HitCooldownCoroutine(){
        canHit = false;
        yield return new WaitForSeconds(0.5f);
        canHit = true;
    }
}
