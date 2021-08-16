using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HordeGameManager : GameManager
{
    public float timePassedForComputer;
    public float timeUntilComputerAttacks;
    public float timePassedForWave;
    public float timeUntilWaveAttacks;
    bool beginCounting;


    public override void InitialSetup()
    {
        beginCounting = false;
        StartCoroutine(StartUp());
    }

    IEnumerator StartUp()
    {
        AddPiece(whiteRook, white, 0, 0);
        yield return new WaitForSecondsRealtime(.2f);
        AddPiece(whiteKnight, white, 1, 0);
        yield return new WaitForSecondsRealtime(.1f);
        AddPiece(whiteBishop, white, 2, 0);
        yield return new WaitForSecondsRealtime(.1f);
        AddPiece(whiteQueen, white, 3, 0);
        yield return new WaitForSecondsRealtime(.1f);
        AddPiece(whiteKing, white, 4, 0);
        yield return new WaitForSecondsRealtime(.1f);
        AddPiece(whiteBishop, white, 5, 0);
        yield return new WaitForSecondsRealtime(.1f);
        AddPiece(whiteKnight, white, 6, 0);
        yield return new WaitForSecondsRealtime(.1f);
        AddPiece(whiteRook, white, 7, 0);
        yield return new WaitForSecondsRealtime(.1f);

        for (int i = 0; i < 8; i++)
        {
            AddPiece(whitePawn, white, i, 1);
            yield return new WaitForSecondsRealtime(.1f);
        }

        
        for (int i = 0; i < 8; i++)
        {
            int randomNumber = Random.Range(0, 10);
            if (randomNumber <= 7)
            {
                AddPiece(hordePiece, black, i, 7);
                yield return new WaitForSecondsRealtime(.1f);
            }
            
        }

        for (int i = 0; i < 8; i++)
        {
            int randomNumber = Random.Range(0, 10);
            if (randomNumber <= 3)
            {
                AddPiece(hordePiece, black, i, 6);
                yield return new WaitForSecondsRealtime(.1f);
            }
        }

        for (int i = 0; i < 8; i++)
        {
            int randomNumber = Random.Range(0, 10);
            if (randomNumber <= 1)
            {
                AddPiece(hordePiece, black, i, 5);
                yield return new WaitForSecondsRealtime(.1f);
            }
        }

        board.GetComponent<TileSelector>().enabled = true;
        board.GetComponent<MoveSelector>().enabled = true;
        yield return new WaitForSecondsRealtime(.5f);
        startText.SetActive(true);
        beginCounting = true;
        yield return null;
    }

    public override void NextPlayer()
    {
        currentPlayer = white;
        turnCount += 1;
        
    }

    public override void ComputerTurn()
    {
        
        if (gameOver == false)
        {
            piecesThatCanAttack.Clear();
            piecesThatCanMove.Clear();

            foreach (GameObject piece in black.pieces)
            {
                attackMoveLocations = MovesForComputerPiecesThatAttack(piece);
                if (attackMoveLocations.Count > 0)
                {
                    piecesThatCanAttack.Add(piece);
                }

                moveLocations = MovesForComputerPiece(piece);
                if (moveLocations.Count > 0)
                {
                    piecesThatCanMove.Add(piece);
                }
            }

            if (piecesThatCanAttack.Count > 0)
            {
                randompiece = piecesThatCanAttack[Random.Range(0, piecesThatCanAttack.Count)];
                possibleMoves = MovesForComputerPiecesThatAttack(randompiece);
            }
            else
            {
                if (piecesThatCanMove.Count > 0)
                {
                    randompiece = piecesThatCanMove[Random.Range(0, piecesThatCanMove.Count)];
                    possibleMoves = MovesForComputerPiece(randompiece);
                }
                    
            }

            Vector2Int randomMove = possibleMoves[Random.Range(0, possibleMoves.Count)];
            Vector3 randomPosition = Geometry.PointFromGrid(randomMove);
            Vector2Int gridPoint = Geometry.GridFromPoint(randomPosition);

            if(possibleMoves.Count > 0)
            {
                if (PieceAtGrid(gridPoint) == null)
                {
                    Move(randompiece, randomMove);
                }
                else
                {
                    CapturePieceAt(gridPoint, randompiece);
                    Move(randompiece, randomMove);
                }

                Debug.Log(piecesThatCanAttack.Count.ToString());
            }
            
            
        }
    }

    public bool FriendlyComputerPieceAt(Vector2Int gridPoint)
    {
        GameObject piece = PieceAtGrid(gridPoint);

        if (piece == null)
        {
            return false;
        }

        if (white.pieces.Contains(piece))
        {
            return false;
        }

        if (powerUpPlayer.pieces.Contains(piece))
        {
            return false;
        }

        return true;
    }

    public virtual List<Vector2Int> MovesForComputerPiece(GameObject pieceObject)
    {
        Piece piece = pieceObject.GetComponent<Piece>();
        Vector2Int gridPoint = GridForPiece(pieceObject);
        var locations = piece.MoveLocations(gridPoint);

        // filter out offboard locations
        locations.RemoveAll(tile => tile.x < 0 || tile.x > 7
            || tile.y < 0 || tile.y > 7);

        // filter out locations with friendly piece
        locations.RemoveAll(tile => FriendlyComputerPieceAt(tile));

        return locations;
    }

    public virtual List<Vector2Int> MovesForComputerPiecesThatAttack(GameObject pieceObject)
    {
        Piece piece = pieceObject.GetComponent<Piece>();
        Vector2Int gridPoint = GridForPiece(pieceObject);
        var locations = piece.MoveLocations(gridPoint);

        // filter out offboard locations
        locations.RemoveAll(tile => tile.x < 0 || tile.x > 7
            || tile.y < 0 || tile.y > 7);

        // filter out locations with friendly piece
        locations.RemoveAll(tile => FriendlyComputerPieceAt(tile));

        // filter out moves that don't attack
        locations.RemoveAll(tile => PieceAtGrid(tile) == false);


        foreach (Vector2Int move in locations)
        {
            if (PieceAtGrid(move).GetComponent<Piece>().type == PieceType.King)
            {
                kingDetected = true;

                Debug.Log("king detected");
            }

        }

        if (kingDetected == true)
        {
            locations.RemoveAll(tile => PieceAtGrid(tile).GetComponent<Piece>().type != PieceType.King);
        }

        return locations;
    }

    public override void Update()
    {
        if (computerWaiting == true)
        {
            time += Time.deltaTime;
            if (time >= timeUntilComputerMoves)
            {
                ComputerTurn();
                computerWaiting = false;
                time = 0;
                playerInput = true;
            }
        }
        if(beginCounting == true)
        {
            timePassedForWave += Time.deltaTime;
            timePassedForComputer += Time.deltaTime;
        }
        
        if (timePassedForWave >= timeUntilWaveAttacks)
        {
            AddWave();
            timePassedForWave = 0;
            
        }

        
        if(timePassedForComputer >= timeUntilComputerAttacks)
        {
            if(black.pieces.Count > 0)
            {
                ComputerTurn();
                timePassedForComputer = 0;
            }
            else
            {
                timePassedForComputer = 0;
            }
            
        }

        
    }

    public void AddWave()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 4; j < 8; j++)
            {
                Vector2Int vector = new Vector2Int(i, j);
                GameObject piece = PieceAtGrid(vector);
                if (piece == null)
                {
                    locations.Add(vector);
                }
            }


        }

        locations.RemoveAll(tile => PieceAtGrid(tile));

        if (locations.Count > 0)
        {
            Vector2Int randomlocation = locations[Random.Range(0, locations.Count)];
            AddPiece(randomBlackPieces[Random.Range(0, randomBlackPieces.Count)], black, randomlocation.x, randomlocation.y);
        }
    }
}
