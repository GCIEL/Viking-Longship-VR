using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using System.IO;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine.XR.Interaction.Toolkit;

/*
This is the code for the model ship
Its spawns ship pieces for the player to place and checks if the player has placed a piece in the ship area
*/
public class ModelShip : MonoBehaviour
{
    //public PresentationScreen screen;
    public Transform shipPieceSpawnPoint;
    public GameObject shipPiecePrefab;
    public Material highlihtedMaterial;
    private Material previousMaterial;
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
        previousMaterial = currentPiece.GetComponent<MeshRenderer>().material;
        currentPiece.GetComponent<MeshRenderer>().material = highlihtedMaterial;

        GameObject shipPiece = Instantiate(shipPiecePrefab, shipPieceSpawnPoint.position, currentPiece.rotation * Quaternion.Euler(0, 90, 0));
        shipPiece.transform.localScale = new Vector3(currentPiece.localScale.x * transform.localScale.x, 
                                                        currentPiece.localScale.y * transform.localScale.y, 
                                                        currentPiece.localScale.z * transform.localScale.z);
        shipPiece.name = currentPiece.name;
        shipPiece.GetComponent<MeshFilter>().mesh = currentPiece.GetComponent<MeshFilter>().mesh;
        //shipPiece.AddComponent<BoxCollider>();
        // shipPiece.GetComponent<BoxCollider>().size = shipPiece.GetComponent<BoxCollider>().size + Vector3.one * 0.01f;
        shipPiece.GetComponent<MeshCollider>().sharedMesh = shipPiece.GetComponent<MeshFilter>().mesh;
        shipPiece.GetComponent<XRGrabInteractable>().selectEntered.AddListener(delegate{PlayerPickedUp(shipPiece.name);});
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
        //screen.UpdateDescription(RemoveNumber(pieceName));
        //screen.ShowDescription();
    }

    public void PlacePieces(string pieceName, int pieceInd, Transform target)
    {
        piecesBuildStatus[pieceInd] = true;
        //PrintData(RemoveNumber(pieceName), timeWhenPickedUpLastPiece, shipPieceSpawnPoint.position, target.position);
        StartCoroutine(PlacePiecesCotorutine(pieceName));
    }
    private IEnumerator PlacePiecesCotorutine(string pieceName)
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Transform currentChild = transform.GetChild(i).transform;
            string currPieceName = currentChild.name;
            if (currPieceName != pieceName) continue;

            currentChild.GetComponent<MeshRenderer>().material = previousMaterial;
            yield return new WaitForSeconds(0.5f);

            foreach(Transform child in currentChild.transform)
            {
                child.gameObject.SetActive(true);
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

            if (other.GetComponent<Placeable>().onPlayerHand) return;

            if(!piecesBuildStatus[i]){
                other.GetComponent<Placeable>().TranslateToFinalTarget(piece.transform, 1f);
            }
            return;
        }

    }
}
