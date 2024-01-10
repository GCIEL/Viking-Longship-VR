using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;
using Valve.VR.InteractionSystem;
using System.IO;
using Unity.Mathematics;
using UnityEditor;

public class ModelShip : MonoBehaviour
{
    public PresentationScreen screen;
    public Transform shipPieceSpawnPoint;
    public GameObject shipPiecePrefab;
    public Material built, transparent, selected;
    private int selectedPiece = -1;
    List<Transform> shipPieces;

    int nextPieceToSpawn = -1;
    bool[] piecesBuildStatus;

    private Transform shipCenter;
    private string lastPickedUpPiece = null;
    private float timeWhenPickedUpLastPiece = 0;

    void Start()
    {
        shipCenter = transform.GetChild(0);
        shipPieces = new List<Transform>();
        for(int i = 0; i < transform.childCount; i++)
        {
            Transform currentChild = transform.GetChild(i).transform;
            if(currentChild.GetComponent<MeshRenderer>() == null) continue;

            currentChild.GetComponent<MeshRenderer>().material = selected;
            foreach(Transform child in currentChild.transform) child.gameObject.SetActive(false);
            currentChild.gameObject.SetActive(false);

            shipPieces.Add(currentChild);
        }

        piecesBuildStatus = new bool[shipPieces.Count];
        for(int i = 0; i < shipPieces.Count; i++) piecesBuildStatus[i] = false;

        SpawnPiece();
    }

    public void SpawnPiece()
    {
        nextPieceToSpawn++;
        if(nextPieceToSpawn >= shipPieces.Count) return;

        Transform currentPiece = shipPieces[nextPieceToSpawn];
        currentPiece.gameObject.SetActive(true);

        GameObject shipPiece = Instantiate(shipPiecePrefab, shipPieceSpawnPoint.position, currentPiece.rotation * Quaternion.Euler(0, 90, 0));
        shipPiece.transform.localScale = new Vector3(currentPiece.localScale.x * transform.localScale.x, 
                                                        currentPiece.localScale.y * transform.localScale.y, 
                                                        currentPiece.localScale.z * transform.localScale.z);
        shipPiece.name = currentPiece.name;
        shipPiece.GetComponent<MeshFilter>().mesh = currentPiece.GetComponent<MeshFilter>().mesh;
        shipPiece.AddComponent<BoxCollider>();
        shipPiece.GetComponent<BoxCollider>().size = shipPiece.GetComponent<BoxCollider>().size + Vector3.one * 0.01f;
        shipPiece.GetComponent<Throwable>().onPickUp.AddListener(delegate{PlayerPickedUp(shipPiece.name);});
        shipPiece.GetComponent<Placeable>().onPlace.AddListener(delegate{PlacePieces(shipPiece.name, nextPieceToSpawn, currentPiece);});
    }

    private string RemoveNumber(string pieceName)
    {
        Regex nonDigits = new Regex("[^\\d]*");
        return nonDigits.Match(pieceName).ToString();
    }

    public void PlayerPickedUp(string pieceName)
    {
        if (lastPickedUpPiece != pieceName)
        {
            lastPickedUpPiece = pieceName;
            timeWhenPickedUpLastPiece = Time.realtimeSinceStartup;
        }
        screen.UpdateDescription(RemoveNumber(pieceName));
        screen.ShowDescription();
    }

    private void Highlight(int piece)
    {
        if(selectedPiece > -1)
        {
            if(!piecesBuildStatus[selectedPiece]){
                shipPieces[selectedPiece].GetComponent<MeshRenderer>().material = transparent;
            }
                
        }

        selectedPiece = piece;
        shipPieces[selectedPiece].GetComponent<MeshRenderer>().material = selected;
    }

    public void PlacePieces(string pieceName, int pieceInd, Transform target)
    {
        piecesBuildStatus[pieceInd] = true;
        PrintData(RemoveNumber(pieceName), timeWhenPickedUpLastPiece, shipPieceSpawnPoint.position, target.position);
        StartCoroutine(PlacePiecesCotorutine(pieceName));
    }
    private IEnumerator PlacePiecesCotorutine(string pieceName)
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Transform currentChild = transform.GetChild(i).transform;
            string currPieceName = currentChild.name;
            if (currPieceName != pieceName) continue;

            currentChild.GetComponent<MeshRenderer>().material = built;
            yield return new WaitForSeconds(0.5f);

            foreach(Transform child in currentChild.transform)
            {
                child.gameObject.SetActive(true);
                child.GetComponent<MeshRenderer>().material = built;
                yield return new WaitForSeconds(0.5f);
            }
            break;
        }
        SpawnPiece();
    }

    void PrintData(string pieceName, float startTime, Vector3 pieceStartPos, Vector3 pieceEndPos)
    {
        string path = Application.dataPath + "/Log.txt";
        if (!File.Exists(path)) File.WriteAllText(path, "piece, time, distance\n");
        string observation = pieceName + ", " + (Time.realtimeSinceStartup - startTime) + ", " + (pieceEndPos - pieceStartPos).magnitude + "\n";
        File.AppendAllText(path, observation);
    }

    void OnTriggerStay(Collider other)
    {
        if(other.tag != "Ship Piece") return;

        for(int i = 0; i < shipPieces.Count; i++)
        {
            Transform piece = shipPieces[i];

            if(piece.name != other.name) continue;

            //Highlight(i);

            //Debug.Log((piece.position - other.transform.position).magnitude);
            if (other.GetComponent<Placeable>().onPlayerHand) return;

            if(!piecesBuildStatus[i]){
                other.GetComponent<Placeable>().TranslateToFinalTarget(piece.transform, 1f);
            }
            return;
        }

    }

    float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float v321 = p3.x * p2.y * p1.z;
        float v231 = p2.x * p3.y * p1.z;
        float v312 = p3.x * p1.y * p2.z;
        float v132 = p1.x * p3.y * p2.z;
        float v213 = p2.x * p1.y * p3.z;
        float v123 = p1.x * p2.y * p3.z;
        return (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123);
    }

    float VolumeOfMesh(Mesh mesh)
    {
        float volume = 0;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            Vector3 p1 = vertices[triangles[i + 0]];
            Vector3 p2 = vertices[triangles[i + 1]];
            Vector3 p3 = vertices[triangles[i + 2]];
            volume += SignedVolumeOfTriangle(p1, p2, p3);
        }
        return Mathf.Abs(volume);
    }
}
