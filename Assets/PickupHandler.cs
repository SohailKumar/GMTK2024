using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHandler : MonoBehaviour
{
    public CapsuleCollider2D capsuleCollider2;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);

        collision.gameObject.GetComponent<PlayerController>().SetSize(0.5f);
        Debug.Log(collision.gameObject.name);
    }
}
