using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
    public override List<Vector2Int> MoveLocations(Vector2Int gridPoint)
    {
        var locations = new List<Vector2Int>();

        int forwardDirection = GameManager.instance.currentPlayer.forward;
        Vector2Int forwardRight = new Vector2Int(gridPoint.x + 1, gridPoint.y + forwardDirection * 2);
        if (GameManager.instance.PieceAtGrid(forwardRight) == false | true)
        {
            locations.Add(forwardRight);
        }

        Vector2Int forwardLeft = new Vector2Int(gridPoint.x - 1, gridPoint.y + forwardDirection * 2);
        if (GameManager.instance.PieceAtGrid(forwardLeft) == false | true)
        {
            locations.Add(forwardLeft);
        }

        Vector2Int backwardRight = new Vector2Int(gridPoint.x + 1, gridPoint.y - forwardDirection * 2);
        if (GameManager.instance.PieceAtGrid(backwardRight) == false | true)
        {
            locations.Add(backwardRight);
        }

        Vector2Int backwardLeft = new Vector2Int(gridPoint.x - 1, gridPoint.y - forwardDirection * 2);
        if (GameManager.instance.PieceAtGrid(backwardLeft) == false | true)
        {
            locations.Add(backwardLeft);
        }

        Vector2Int rightUp = new Vector2Int(gridPoint.x + 2, gridPoint.y + forwardDirection);
        if (GameManager.instance.PieceAtGrid(rightUp) == false | true)
        {
            locations.Add(rightUp);
        }

        Vector2Int rightDown = new Vector2Int(gridPoint.x + 2, gridPoint.y - forwardDirection);
        if (GameManager.instance.PieceAtGrid(rightDown) == false | true)
        {
            locations.Add(rightDown);
        }

        Vector2Int leftUp = new Vector2Int(gridPoint.x - 2, gridPoint.y + forwardDirection);
        if (GameManager.instance.PieceAtGrid(leftUp) == false | true)
        {
            locations.Add(leftUp);
        }

        Vector2Int leftDown = new Vector2Int(gridPoint.x - 2, gridPoint.y - forwardDirection);
        if (GameManager.instance.PieceAtGrid(leftDown) == false | true)
        {
            locations.Add(leftDown);
        }

        return locations;
    }
}
