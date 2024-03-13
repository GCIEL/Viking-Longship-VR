using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HnefataflBoard : MonoBehaviour
{
    HnefataflPiece selectedPiece = null;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.tag == "Hnefatafl Piece") selectedPiece = hit.collider.GetComponent<HnefataflPiece>();
                else if (hit.collider.tag == "Hnefatafl Square" && selectedPiece != null && selectedPiece.CanMoveTo(hit.collider.transform))
                {
                    selectedPiece.transform.position = new Vector3(hit.collider.transform.position.x, 0.5f, hit.collider.transform.position.z);
                    selectedPiece.GetComponent<HnefataflPiece>().KillsCheck();
                    selectedPiece = null;
                }
            }
        }
    }
}
