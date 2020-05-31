using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{

    public GameObject doorTop, doorBottom, doorLeft, doorRight;

    public bool roomTop, roomBottom, roomLeft, roomRight;

    // Start is called before the first frame update
    void Start()
    {
        doorTop.SetActive(roomTop);
        doorBottom.SetActive(roomBottom);
        doorLeft.SetActive(roomLeft);
        doorRight.SetActive(roomRight);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if(other.CompareTag("Player"))
        {
            CameraControlor.instance.ChangeTarget(transform);
        }
    }
}
