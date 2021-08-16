using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum PieceType { King, Queen, Bishop, Knight, Rook, Pawn, Powerup, HordePawn };

public abstract class Piece : MonoBehaviour
{
    public PieceType type;
    public Material myMaterial;
    public bool hasMoved;
    public bool taken;
    public NavMeshAgent navMeshAgent;
    public Rigidbody myRigidBody;
    public Collider myCollider;
    public GameObject particles;
    
    

    public void Start()
    {
        hasMoved = false;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.enabled = false;
        myRigidBody = GetComponent<Rigidbody>();
        myCollider = GetComponentInChildren<Collider>();
        particles.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Board")
        {
            if(taken == false)
            {
                navMeshAgent.enabled = true;
                myRigidBody.constraints = RigidbodyConstraints.FreezeAll;
            }
            
        }
        
    }

    
    protected Vector2Int[] RookDirections = {new Vector2Int(0,1), new Vector2Int(1, 0),
        new Vector2Int(0, -1), new Vector2Int(-1, 0)};
    protected Vector2Int[] BishopDirections = {new Vector2Int(1,1), new Vector2Int(1, -1),
        new Vector2Int(-1, -1), new Vector2Int(-1, 1)};

    public abstract List<Vector2Int> MoveLocations(Vector2Int gridPoint);

   
}
