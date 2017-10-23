using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TP1Behaviour : MonoBehaviour
{
    void Start()
    {

    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            LoadNextScene();
        }
    }

    public void LoadNextScene()
    {
        Debug.Log("amahandahaaaang");
        Application.LoadLevel(2);
        //SceneManager.LoadScene(2);
    }

}
