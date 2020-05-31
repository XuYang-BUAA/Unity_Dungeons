using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDetection : MonoBehaviour
{

    public GameObject mapControlor;

    // Start is called before the first frame update
    void Start()
    {
        mapControlor.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            mapControlor.SetActive(!mapControlor.activeSelf);
        }
    }
}
