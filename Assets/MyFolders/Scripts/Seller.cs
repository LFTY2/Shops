using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seller : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void PlayWaveAnimation()
    {
        anim.SetTrigger("Wave");
    }

    public void PlayWaveInteract()
    {
        anim.SetTrigger("Interact");
    }
}
