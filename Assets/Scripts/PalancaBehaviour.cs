using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalancaBehaviour : MonoBehaviour
{
    public Animator anim;
    public Animator anim2;

    public bool isOpen = false;

	void Start ()
    {

	}

    void Update ()
    {
		
	}

    public void OnMouseEnter()
    {
        Debug.Log("zdgfd");
    }

    public void OnMouseUp()
    {
        Debug.Log("UELAAA");

       isOpen = true;
        
        if(isOpen)
        {
            anim.SetTrigger("Open");

            anim2.SetTrigger("Open");

            isOpen = false;
        }
    }
}
