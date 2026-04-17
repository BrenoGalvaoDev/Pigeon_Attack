using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private static readonly int StateHash = Animator.StringToHash("State");

    Animator anim;


    private void Awake() => anim = GetComponent<Animator>();

    public void SetAnimation(PlayerState state)
    {
        //Idle = 0, Moving = 1, Hurt = 2, Dead = 3
        
        anim.SetInteger(StateHash, (int)state);

        if(state == PlayerState.Dead)
        {
            anim.SetBool("Flag", true);
            StartCoroutine(ResetFlag(0.5f));
        }
    }

    IEnumerator ResetFlag(float timer)
    {
        yield return new WaitForSeconds(timer);
        anim.SetBool("Flag", false);
    }
}
