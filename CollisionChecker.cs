using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    Bot _playerController;
    Rigidbody _rb;

    void Start(){
        _playerController = transform.parent.GetComponent<Bot>();
        _rb = transform.GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.layer == LayerMask.NameToLayer("CheckPoint"))
        {
            // print("Reached");
            GameObject[] checkPoints = GameObject.FindGameObjectsWithTag("CheckPoint");
            for (int i=0; i < checkPoints.Length; i++)
            {
                if(collider.gameObject == checkPoints[i] && i == (_playerController.getPosition() + 1 + checkPoints.Length) % checkPoints.Length)
                {
                    
                    _playerController.incrementPosition();
                    print(_playerController.getPosition());
                    break;
                }
            }
        }
        else if(collider.gameObject.layer != LayerMask.NameToLayer("Learner"))
        {
            // completely stop ball if entering red space
            _playerController.hasCollided();
            _rb.velocity = Vector3.zero;
            //print("Reached");
        }
    }
}
