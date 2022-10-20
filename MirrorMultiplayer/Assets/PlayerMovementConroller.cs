using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;


public class PlayerMovementConroller : NetworkBehaviour
{
    public float speed = 0.1f;
    public GameObject playerModel;

    private void Start()
    {
        SetPosition();
        playerModel.SetActive(false);
    }

    private void Update()
    {
        
        if (SceneManager.GetActiveScene().name == "Game")
        {
            if (playerModel.activeSelf == false)
            {
                playerModel.SetActive(true);
            }

            if (hasAuthority)
            {
                Movement();
            }
        }
    }

    public void SetPosition()
    {
        transform.position = new Vector3(Random.Range(-5, 5), 0.8f, Random.Range(-15,7));
    }
    
    public void Movement()
    {
        float xDirection = Input.GetAxis("Horizontal");
        float zDirection = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(xDirection, 0.0f, zDirection);

        transform.position += moveDirection * speed;
    }
}
