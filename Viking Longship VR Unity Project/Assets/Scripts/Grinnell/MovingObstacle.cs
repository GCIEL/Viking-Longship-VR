using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    public Vector3 localMove;
    private bool isMoving = false, hasMoved = false;

    public void SwitchPosition()
    {
        if (!isMoving && !hasMoved) StartCoroutine(MoveCoroutine());
    }

    private IEnumerator MoveCoroutine()
    {
        isMoving = true;
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + localMove;

        float timer = 0;
        while(timer < 2f)
        {
            transform.position = Vector3.Lerp(startPos, endPos, timer / 2f);
            yield return null;
            timer += Time.deltaTime;
        }
        isMoving = false;
        hasMoved = true;
    }
}
