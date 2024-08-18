using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHandler : MonoBehaviour
{
    public CapsuleCollider2D capsuleCollider2;
    public enum pickupType { Shrink, Normal, Grow, Collectable }
    [SerializeField] 
    public pickupType PickupType;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(PickupType == pickupType.Collectable)
        {
            Debug.Log("item picked up!");
            //Add one to the global manager
        }
        if(PickupType == pickupType.Shrink)
        {
            collision.gameObject.GetComponent<PlayerMovement>().SetSize(PlayerAbilityManager.PlayerSize.Mini);
        }
        if (PickupType == pickupType.Normal)
        {
            collision.gameObject.GetComponent<PlayerMovement>().SetSize(PlayerAbilityManager.PlayerSize.Normal);
        }
        if (PickupType == pickupType.Grow)
        {
            collision.gameObject.GetComponent<PlayerMovement>().SetSize(PlayerAbilityManager.PlayerSize.Big);
        }
        Destroy(this.gameObject);
        
        //Debug.Log(collision.gameObject.name);
    }
}
