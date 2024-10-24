using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Creature : MonoBehaviour
{
    [SerializeField] private NavMeshAgent meshAgent;
    [SerializeField] protected Transform visualTran;

    protected void MoveTo(Vector2 direction)
    {
        Vector3 finalMoveDirection;

        if (direction.x != 0 || direction.y != 0)
        {
            var tempMoveDirection = new Vector3(direction.x, 0, direction.y);
            finalMoveDirection = transform.position + tempMoveDirection;
        
            // rotate

            var lookRotation = Quaternion.LookRotation(tempMoveDirection);
            var euler = lookRotation.eulerAngles;
            visualTran.rotation = Quaternion.Euler(0, euler.y, 0);
        }
        else
        {
            finalMoveDirection = transform.position;            
        }

        meshAgent.SetDestination(finalMoveDirection);
    }
}