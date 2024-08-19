using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpointer : MonoBehaviour
{
    GameObject currCheckpoint;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.CompareTag("Checkpoint"))
        {
            currCheckpoint = collision.gameObject;
        }
        else if (collision.gameObject.CompareTag("Death"))
        {
            transform.position = currCheckpoint.transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);

    }

}
