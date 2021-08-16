using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public Material whiteMaterial;
    public Material blackMaterial;
    public Material selectedMaterial;

    private void Start()
    {
        
        gameObject.GetComponent<TileSelector>().enabled = false;
        gameObject.GetComponent<MoveSelector>().enabled = false;
    }

    public GameObject AddPiece(GameObject piece, int col, int row)
    {
        Vector2Int gridPoint = Geometry.GridPoint(col, row);
        GameObject newPiece = Instantiate(piece, Geometry.PointFromGrid(gridPoint) + Vector3.up, Quaternion.identity, gameObject.transform);
        return newPiece;
    }

    public GameObject AddPieceFlat(GameObject piece, int col, int row)
    {
        Vector2Int gridPoint = Geometry.GridPoint(col, row);
        GameObject newPiece = Instantiate(piece, Geometry.PointFromGrid(gridPoint), Quaternion.identity, gameObject.transform);
        return newPiece;
    }

    public void RemovePiece(GameObject piece)
    {
        Destroy(piece);
    }

    public void MovePiece(GameObject piece, Vector2Int gridPoint)
    {
        //piece.transform.position = Geometry.PointFromGrid(gridPoint);
        piece.GetComponent<Piece>().navMeshAgent.SetDestination(Geometry.PointFromGrid(gridPoint));
        piece.GetComponent<Piece>().hasMoved = true;
    }

    public void SelectPiece(GameObject piece)
    {
        MeshRenderer renderers = piece.GetComponentInChildren<MeshRenderer>();
        renderers.material = selectedMaterial;
    }

    public void DeselectPiece(GameObject piece)
    {
        if(GameManager.instance.currentPlayer.name == "white")
        {
            MeshRenderer renderers = piece.GetComponentInChildren<MeshRenderer>();
            renderers.material = whiteMaterial;
        }
        else
        {
            MeshRenderer renderers = piece.GetComponentInChildren<MeshRenderer>();
            renderers.material = blackMaterial;
        }
        
        
        
    }
}
