using UnityEngine;

public class Log : MonoBehaviour
{
    public Mesh[] stages;
    public float currentLerpValue = 0;
    public int nextStage = 0;
    private MeshFilter meshFilter;
    private Vector3[] startVertexPositions, nextVertexPositions;
    
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        startVertexPositions = meshFilter.mesh.vertices;
        nextVertexPositions = stages[nextStage].vertices;
        Debug.Log("Current: " + startVertexPositions.Length);
        for(int i = 0; i < stages.Length; i++)
            Debug.Log(stages[i].name + ": " + stages[i].vertices.Length);
    }

    void AdvanceToNextState()
    {
        startVertexPositions = stages[nextStage].vertices;
        nextStage++;
        nextVertexPositions = stages[nextStage].vertices;
    }

    void LerpToNextStage()
    {
        currentLerpValue = Mathf.Clamp(currentLerpValue, 0, 1);;

        Vector3[] newVertexPositions = new Vector3[meshFilter.mesh.vertices.Length];
        for(int i = 0; i < startVertexPositions.Length; i++)
            newVertexPositions[i] = Vector3.Lerp(startVertexPositions[i], nextVertexPositions[i], currentLerpValue);
        meshFilter.mesh.vertices = newVertexPositions;
        meshFilter.mesh.RecalculateBounds();
        if(currentLerpValue == 1 && nextStage < stages.Length) AdvanceToNextState();
    }

    void Update()
    {
        LerpToNextStage();
    }
}
