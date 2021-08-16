using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    

    public override List<Vector2Int> MoveLocations(Vector2Int gridPoint)
    {
        var locations = new List<Vector2Int>();
        

        int forwardDirection = GameManager.instance.currentPlayer.forward;
        Vector2Int forward = new Vector2Int(gridPoint.x, gridPoint.y + forwardDirection);
        if (GameManager.instance.PieceAtGrid(forward) == false)
        {
            locations.Add(forward);
            
        }

        Vector2Int forwardTwo = new Vector2Int(gridPoint.x, gridPoint.y + forwardDirection * 2);
        if (GameManager.instance.PieceAtGrid(forwardTwo) == false && GameManager.instance.PieceAtGrid(forward) == false && hasMoved == false)
        {
            locations.Add(forwardTwo);
            
        }

        Vector2Int forwardRight = new Vector2Int(gridPoint.x + 1, gridPoint.y + forwardDirection);
        if (GameManager.instance.PieceAtGrid(forwardRight))
        {
            locations.Add(forwardRight);
            
        }

        Vector2Int forwardLeft = new Vector2Int(gridPoint.x - 1, gridPoint.y + forwardDirection);
        if (GameManager.instance.PieceAtGrid(forwardLeft))
        {
            locations.Add(forwardLeft);
            
        }

        return locations;
    }
}
