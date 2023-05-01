using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class Duck : MonoBehaviour
{
    [SerializeField, Range(0,1)] float moveDuration = 0.1f;
    [SerializeField, Range(0,1)] float jumpHeight = 0.5f;


    public UnityEvent<Vector3> OnJumpEnd;
    // Update is called once per frame
    void Update()
    {
        if (DOTween.IsTweening(transform)){
            return;
        }

        Vector3 direction = Vector3.zero;

        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)){
            direction += Vector3.forward;
        } else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)){
            direction +=Vector3.back;
        }else if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)){
            direction += Vector3.right;
        } else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)){
            direction+=Vector3.left;
        }

        if(direction == Vector3.zero){
            return;
        }
        Move(direction);

    }

    public void Move(Vector3 direction){
        transform.DOJump(
            transform.position +direction,
            jumpHeight,
            1,
            moveDuration
        ).onComplete = () => BroadCastPositionOnJumpEnd();
        transform.forward = direction;
    }

    private void BroadCastPositionOnJumpEnd(){
        OnJumpEnd.Invoke(transform.position);
    }


}