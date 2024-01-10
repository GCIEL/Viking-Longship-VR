using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HnefataflPiece : MonoBehaviour
{
    public enum Team{
        Black,
        White
    }

    public Team team;

    private Vector3 startPos;

    void Awake()
    {
        startPos = transform.position;
    }

    public bool CanMoveTo(Transform goal)
    {
        if(Mathf.RoundToInt(goal.localPosition.x) != Mathf.RoundToInt(transform.localPosition.x) && Mathf.RoundToInt(goal.localPosition.z) != Mathf.RoundToInt(transform.localPosition.z)) return false;
        
        Vector3 direction = new Vector3(goal.localPosition.x, transform.localPosition.y, goal.localPosition.z) - transform.localPosition;
        RaycastHit hit;
        Physics.Raycast(transform.position, direction, out hit, direction.magnitude);
        if(hit.collider != null) return false;

        return true;
    }

    public int KillsCheck()
    {
        Vector3[] directions = {Vector3.forward, Vector3.right, Vector3.back, Vector3.left};

        int numKills = 0;
        for(int i = 0; i < 4; i++)
        {
            RaycastHit hit;
            Physics.Raycast(transform.localPosition, directions[i], out hit, 0.5f);
            if(hit.collider != null && hit.collider.GetComponent<HnefataflPiece>() != null && hit.collider.GetComponent<HnefataflPiece>().team != team)
            {
                if(hit.collider.GetComponent<HnefataflPiece>().DiesCheck(directions[i])) numKills++;
            }
        }

        return numKills;
    }

    public bool DiesCheck(Vector3 direction)
    {
        RaycastHit hit;
        Physics.Raycast(transform.localPosition, direction, out hit, 0.5f);
        if(hit.collider != null && hit.collider.GetComponent<HnefataflPiece>() != null && hit.collider.GetComponent<HnefataflPiece>().team != team)
        {
            gameObject.SetActive(false);
            return true;
        }
        return false;
    } 
}
