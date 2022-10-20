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

    public MeshRenderer playerMesh;
    public Material[] playerColors;
    
    
    private void Start()
    {
        playerModel.SetActive(false);
    }

    private void Update()
    {
        
        if (SceneManager.GetActiveScene().name == "Game")
        {
            if (playerModel.activeSelf == false)
            {
                SetPosition();
                playerModel.SetActive(true);
                PlayerCosmeticsSetup();
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

    public void PlayerCosmeticsSetup()
    {
        playerMesh.material = playerColors[GetComponent<PlayerObjectController>().playerColor];
    }
    
    
}
