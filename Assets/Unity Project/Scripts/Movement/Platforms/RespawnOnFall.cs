using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnOnFall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Move them to the location of the last reached checkpoint :D

            // TODO: Probably just making and setting a modified falling state would be better.
            Rigidbody playerRb = other.GetComponent<Rigidbody>();
            CharacterController2D cc2d = other.GetComponent<CharacterController2D>();
            playerRb.position = GameManager.Instance.CurrentLevelManager.LastReachedCheckpoint.transform.position;
            playerRb.velocity = Vector3.zero;
            cc2d.CharacterVelocity = Vector3.zero;
            cc2d.PlayerMovementVector = Vector3.zero;
        }
    }
}
