
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwaterMove : MonoBehaviour {

    private float sweaterMov = 2f;
    Vector3 StartPosition;
    Vector3 EndPos;
    bool StartGame = true;
    // Use this for initialization
    void Start () {
            StartCoroutine(SweaterMove());       
    }
    IEnumerator SweaterMove()
    {
        StartPosition = new Vector3(transform.position.x, transform.position.y, 124f);
        EndPos = new Vector3(transform.position.x, transform.position.y, 128f);
        while(StartGame == true)
        {
            float t = 0f;
            float t1 = 0f;
            while (t <= 1f)
            {
                t += Time.deltaTime / sweaterMov;
                Vector3 CurrentPos = Vector3.Lerp(StartPosition, EndPos, t);
                gameObject.transform.position = CurrentPos;
                yield return null;
            }
            yield return new WaitForSeconds(.1f);
            while (t1 <= 1f)
            {
                t1 += Time.deltaTime / sweaterMov;
                Vector3 CurrentPosBack = Vector3.Lerp(EndPos, StartPosition, t1);
                gameObject.transform.position = CurrentPosBack;
                yield return null;
            }
        }

    }
}
