using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using System.IO;
using UnityEditor;
using UnityEngine.XR.Interaction.Toolkit;
using System.Runtime.InteropServices.WindowsRuntime;

/*
This is the code for the model ship
Its spawns ship pieces for the player to place and checks if the player has placed a piece in the ship area
It creates the the ship pieces based on the children of this game object, e. g. :
    If this game object has children
    > Piece1
    > Piece2
    This script will spawn first a ship piece for Piece1, then one for Piece2
    If Piece1 has sub children, these would be filled automatically when the user places Piece1
*/
public class ModelShip : MonoBehaviour
{
    public Transform shipPieceSpawnPoint;
    public GameObject shipPiecePrefab;
    public Material highlihtedMaterial;
    private Material previousMaterial;
    private int selectedPiece = -1;
    List<Transform> shipPieces;

    private int currentPieceIndex = -1;
    bool[] piecesBuildStatus;
    private string lastPickedUpPiece = null;
    private float timeWhenPickedUpLastPiece = 0;

    // Disables all meshes in the ship and adds them to a list which will be used to spawn the pieces to be placed
    // Note that the order of the pieces inside this gameobject will be the order the pieces will spawn in
    void Start()
    {
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

    // Spawns the current piece that needs to be placed
    public void SpawnPiece()
    {
        currentPieceIndex++;
        if(currentPieceIndex >= shipPieces.Count) return;

        Transform currentPiece = shipPieces[currentPieceIndex];
        currentPiece.gameObject.SetActive(true);
        previousMaterial = currentPiece.GetComponent<MeshRenderer>().material;
        currentPiece.GetComponent<MeshRenderer>().material = highlihtedMaterial;

        GameObject shipPiece = Instantiate(shipPiecePrefab, shipPieceSpawnPoint.position, currentPiece.rotation * Quaternion.Euler(0, 90, 0));
        shipPiece.transform.localScale = new Vector3(currentPiece.localScale.x * transform.localScale.x, 
                                                        currentPiece.localScale.y * transform.localScale.y, 
                                                        currentPiece.localScale.z * transform.localScale.z);
        shipPiece.name = currentPiece.name;
        shipPiece.GetComponent<MeshFilter>().mesh = currentPiece.GetComponent<MeshFilter>().mesh;
        shipPiece.GetComponent<MeshCollider>().sharedMesh = shipPiece.GetComponent<MeshFilter>().mesh;

        // Hooks up to events to when the player picks up the piece and when they place the piece in the model ship
        shipPiece.GetComponent<XRGrabInteractable>().selectEntered.AddListener(delegate{PlayerPickedUp(shipPiece.name);});
        shipPiece.GetComponent<Placeable>().onPlace.AddListener(delegate{PlacePieces(currentPieceIndex, currentPiece);});
    }

    private string RemoveNumber(string pieceName)
    {
        Regex nonDigits = new Regex("[A-Za-z]*");
        return nonDigits.Match(pieceName).ToString();
    }

    // Function that is called when the playerpicks up a piece
    // Possibly useful for data collection or to triggger other events
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

    // Places a piece in the ship
    // All children of this piece you are placing will also be placed one after the other
    public void PlacePieces(int pieceInd, Transform target)
    {
        piecesBuildStatus[pieceInd] = true;
        //PrintData(RemoveNumber(pieceName), timeWhenPickedUpLastPiece, shipPieceSpawnPoint.position, target.position);
        StartCoroutine(PlacePiecesCotorutine(pieceInd));
    }

    // Coroutine that deals with placing a piece
    // Note that it places pieces in the following way
    // Consider you are placing piece x, which in the hierarchy has two childs
    //
    //  Piece x
    //      Child 1
    //          Sub child 1
    //      Child 2
    //
    // This coroutine will make piece x visible again (The piece the player placed themselves)
    // After 0.5 seconds, it will made child 1 visible, at the same time with all of its children
    // After 0.5 seconds, it will made child 2 visible
    private IEnumerator PlacePiecesCotorutine(int pieceInd)
    {
        Transform currentChild = transform.GetChild(pieceInd).transform;

        currentChild.GetComponent<MeshRenderer>().material = previousMaterial;

        Color pieceColor = currentChild.GetComponent<MeshRenderer>().material.GetColor("_BaseColor");
        currentChild.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", new Color(0, 1, 0, 1));
        PlayPlacePieceSound();
        yield return new WaitForSeconds(0.5f);
        currentChild.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", pieceColor);

        foreach(Transform child in currentChild.transform)
        {
            child.gameObject.SetActive(true);
            PlayPlacePieceSound();
            foreach(MeshRenderer mr in child.GetComponentsInChildren<MeshRenderer>()) mr.material.SetColor("_BaseColor", new Color(0, 1, 0, 1));
            child.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", new Color(0, 1, 0, 1));

            yield return new WaitForSeconds(0.5f);

            foreach(MeshRenderer mr in child.GetComponentsInChildren<MeshRenderer>()) mr.material.SetColor("_BaseColor", pieceColor);
            currentChild.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", pieceColor);
        }

        SpawnPiece();
    }

    // Plays sound
    // Note that when playing sounds that are expected to repeat a lot you shold randomize the pitch
    private void PlayPlacePieceSound()
    {
        GetComponent<AudioSource>().pitch = 1 + Random.Range(-0.2f, 0.2f);
        GetComponent<AudioSource>().Play();
    }

    void PrintData(string pieceName, float startTime, Vector3 pieceStartPos, Vector3 pieceEndPos)
    {
        string path = Application.dataPath + "/Log.txt";
        if (!File.Exists(path)) File.WriteAllText(path, "piece, time, distance\n");
        string observation = pieceName + ", " + (Time.realtimeSinceStartup - startTime) + ", " + (pieceEndPos - pieceStartPos).magnitude + "\n";
        File.AppendAllText(path, observation);
    }

    // OnTrigger that checks when stuff is inside the model ship area
    // If the thing inside the area is a ship piece AND this ship piece is on on the hand of the player
    // This scripts makes the ship piece translate to its correct position on the ship
    void OnTriggerStay(Collider other)
    {
        if(other.tag != "Ship Piece") return;
        if (other.GetComponent<Placeable>().onPlayerHand) return;

        Transform piece = shipPieces[currentPieceIndex];

        if(piece.name != other.name) return;

        if(!piecesBuildStatus[currentPieceIndex]) other.GetComponent<Placeable>().TranslateToFinalTarget(piece.transform, 1f);

    }
}
