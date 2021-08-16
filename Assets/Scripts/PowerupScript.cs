using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupScript : Piece
{
    public GameManager instance;
    public List<GameObject> randomWhitePieces;
    public List<GameObject> randomblackPieces;
    public GameObject deathParticles;
    public bool deathReady;
    
    public override List<Vector2Int> MoveLocations(Vector2Int gridPoint)
    {
        return null;
    }

    

    private void OnCollisionEnter(Collision collision)
    {
        if(deathReady == true)
        {
            if(collision.gameObject.GetComponent<Piece>() != null)
            {
                Destroy(gameObject);
                Instantiate(deathParticles, transform.position, transform.rotation);
            }
        }
    }


}
