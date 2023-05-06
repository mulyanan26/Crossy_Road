using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class Duck : MonoBehaviour
{
    //pp
    [SerializeField, Range(0,1)] float moveDuration = 0.1f;
    [SerializeField, Range(0,1)] float jumpHeight = 0.5f;

    [SerializeField] int leftMoveLimit;
    [SerializeField] int rightMoveLimit;
    [SerializeField] int backMoveLimit;

    public UnityEvent<Vector3> OnJumpEnd;
    public UnityEvent<int> OnGetCoin;
    public UnityEvent OnDie;

    bool isNotMoveable = false;
    void Update()
    {
        if (isNotMoveable)
        {
            return;
        }
        if (DOTween.IsTweening(transform))
        {
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
        var targetPosition = transform.position + direction;
        
        if (targetPosition.x < leftMoveLimit
            || targetPosition.x > rightMoveLimit
            || targetPosition.z < backMoveLimit
            || Tree.AllPosition.Contains(targetPosition))
        {
            targetPosition = transform.position;
        }

        transform.DOJump(
            targetPosition,
            jumpHeight,
            1,
            moveDuration
        ).onComplete = BroadCastPositionOnJumpEnd;
        
        transform.forward = direction;
    }

    public void SetNotMoveAble(bool value){
        isNotMoveable = value;
    }

    public void UpdateMoveLimit(int horizontalSize, int backLimit){
        leftMoveLimit = -horizontalSize/2;
        rightMoveLimit = horizontalSize/2;
        backMoveLimit = backLimit;
    }
    private void BroadCastPositionOnJumpEnd(){
        OnJumpEnd.Invoke(transform.position);
    }

    //Ketika bertabrakan dengan collider maka mati
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            if (isNotMoveable == true)
            {
                return;
            }
            transform.DOScale(new Vector3(1,0.1f,1), 0.2f);

            isNotMoveable = true;
            Invoke("Die",3);
        }
        else if (other.CompareTag("Coin"))
        {
            var coin = other.GetComponent<Coin>();
            OnGetCoin.Invoke(coin.Value);
            coin.Collected();
        }
        else if (other.CompareTag("Eagle"))
        {
            if(this.transform != other.transform){
                this.transform.SetParent(other.transform);
                Invoke("Die",3);

            }
        }
    }
    private void Die(){
        OnDie.Invoke();
    }

}