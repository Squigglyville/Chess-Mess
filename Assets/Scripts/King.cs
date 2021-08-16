using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    public override List<Vector2Int> MoveLocations(Vector2Int gridPoint)
    {
        var locations = new List<Vector2Int>();

        int forwardDirection = GameManager.instance.currentPlayer.forward;
        Vector2Int forward = new Vector2Int(gridPoint.x, gridPoint.y + forwardDirection);
        if (GameManager.instance.PieceAtGrid(forward) == false | true)
        {
            locations.Add(forward);
        }

        Vector2Int forwardRight = new Vector2Int(gridPoint.x + 1, gridPoint.y + forwardDirection);
        if (GameManager.instance.PieceAtGrid(forwardRight) == false | true)
        {
            locations.Add(forwardRight);
        }

        Vector2Int forwardLeft = new Vector2Int(gridPoint.x - 1, gridPoint.y + forwardDirection);
        if (GameManager.instance.PieceAtGrid(forwardLeft) == false | true)
        {
            locations.Add(forwardLeft);
        }

        Vector2Int right = new Vector2Int(gridPoint.x + 1, gridPoint.y);
        if (GameManager.instance.PieceAtGrid(right) == false | true)
        {
            locations.Add(right);
        }

        Vector2Int left = new Vector2Int(gridPoint.x - 1, gridPoint.y);
        if (GameManager.instance.PieceAtGrid(left) == false | true)
        {
            locations.Add(left);
        }

        Vector2Int backward = new Vector2Int(gridPoint.x, gridPoint.y - forwardDirection);
        if (GameManager.instance.PieceAtGrid(backward) == false | true)
        {
            locations.Add(backward);
        }

        Vector2Int backwardLeft = new Vector2Int(gridPoint.x - 1, gridPoint.y - forwardDirection);
        if (GameManager.instance.PieceAtGrid(backwardLeft) == false | true)
        {
            locations.Add(backwardLeft);
        }

        Vector2Int backwardRight = new Vector2Int(gridPoint.x + 1, gridPoint.y - forwardDirection);
        if (GameManager.instance.PieceAtGrid(backwardRight) == false | true)
        {
            locations.Add(backwardRight);
        }

        return locations;
    }
}
