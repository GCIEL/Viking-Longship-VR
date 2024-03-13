using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Wedge : MonoBehaviour
{
    public bool ready = false;
    private float minHeight;
    private bool canHit = true;
    private Log2 log;

    void Start(){
        minHeight = transform.position.y - 0.3f;
        log = transform.parent.GetComponent<Log2>();
    }

    private IEnumerator HitCooldownCoroutine(){
        canHit = false;
        yield return new WaitForSeconds(0.5f);
        canHit = true;
    }

    public void RemoveFromLog()
    {
        transform.parent = null;
        GetComponent<Rigidbody>().isKinematic = false;
        log = null;
    }

    void OnCollisionEnter(Collision collision){
        //Debug.Log("Hit rivet!");
        //Debug.Log(collision.relativeVelocity.magnitude);
        //Debug.Log(collision.gameObject.tag);

        if (log != null && canHit && collision.gameObject.tag == "Hammer" && collision.relativeVelocity.y < 0 && collision.relativeVelocity.magnitude > 0.3f)
        {
            transform.position += (Vector3.down * 0.07f);
            if (transform.position.y <= minHeight)
            {
                ready = true;
                log.SplitCheck();
                transform.position = new Vector3(transform.position.x, minHeight, transform.position.z);
                return;
            }
            log.PushClosestVertex(transform.position, Vector3.forward, 0.07f);
            StartCoroutine(HitCooldownCoroutine());
        }
    }
}
