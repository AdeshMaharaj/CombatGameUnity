using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    public abstract State RunCurrentState();

    // i MADE IT VIRUAL BC IT SAID i HAD TO 
    public virtual void HandleTriggerEnter2D(Collider2D enter){}
    }
