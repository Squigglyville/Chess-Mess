using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HordeTurnGameManager : GameManager
{
    public float waveFactor;   
    int turnsUntilNextWave;
    public Text turnsUntilNextWaveText;
    public GameObject waveParticles;
    public float score;
    public Text scoreText;

    public override void Start()
    {
        pieces = new GameObject[8, 8];

        white = new Player("white", true);
        black = new Player("black", false);

        currentPlayer = white;
        otherPlayer = black;
        powerUpPlayer = new Player("powerup", false);
        playerWinsObject.SetActive(false);
        InitialSetup();
        playerInput = true;
        startText.SetActive(false);
        piecesTaken = 0;
        musicInstance = FMODUnity.RuntimeManager.CreateInstance("event:/Music/Music 2");
        musicInstance.start();
        musicInstance.setParameterByName("PiecesTaken", piecesTaken);
        powerUpsOn = PlayerPrefs.GetInt("PowerUps", 1);
    }
    public override void InitialSetup()
    {
        turnsUntilNextWave = 5;
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
        
        yield return null;
    }

    

    public override void ComputerTurn()
    {
        turnCount += 1;
        turnsUntilNextWave -= 1;

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

                if (turnsUntilNextWave == 0)
                {
                    StartCoroutine(AddWaveDelay());
                }
                else
                {
                    NextPlayer();
                }

                
                waveFactor = Mathf.Max(Mathf.Ceil(turnCount / 5), 2);
                
                Debug.Log(piecesThatCanAttack.Count.ToString());
            }
            
            
            
        }
    }

    IEnumerator AddWaveDelay()
    {
        yield return new WaitForSecondsRealtime(1);
        if (turnsUntilNextWave == 0)
        {
            turnsUntilNextWave = 5;
        }
        AddWave();
        NextPlayer();
        yield return null;
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

        turnsUntilNextWaveText.text = turnsUntilNextWave.ToString();
        scoreText.text = score.ToString();     
        
    }
    public override void NextPlayer()
    {
        
        Player tempPlayer = currentPlayer;
        currentPlayer = otherPlayer;
        otherPlayer = tempPlayer;
        
        int randomNumber = Random.Range(0, 15);
        if (powerUpsOn == 1)
        {
            if (turnCount >= 3)
            {
                if (randomNumber <= 1)
                {
                    
                }
            }
        }

       
    }

    public void AddPieceFlat(GameObject prefab, Player player, int col, int row)
    {
        GameObject pieceObject = board.AddPieceFlat(prefab, col, row);
        player.pieces.Add(pieceObject);
        pieces[col, row] = pieceObject;

    }

    public void AddWave()
    {
        locations.Clear();
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
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/AddWave");
        if (locations.Count > 0)
        {
            for (int i = 0; i < waveFactor; i++)
            {
                if(locations.Count > 0)
                {
                    Vector2Int randomlocation = locations[Random.Range(0, locations.Count)];
                    randompiece = randomBlackPieces[Random.Range(0, randomBlackPieces.Count)];
                    AddPieceFlat(randompiece, black, randomlocation.x, randomlocation.y);

                    Instantiate(waveParticles, Geometry.PointFromGrid(randomlocation), Quaternion.identity, gameObject.transform);
                    locations.Remove(randomlocation);
                }
                
            }
                
        }
    }

    public override void CapturePieceAt(Vector2Int gridPoint, GameObject pieceToTake)
    {
        GameObject pieceToCapture = PieceAtGrid(gridPoint);
        currentPlayer.capturedPieces.Add(pieceToCapture);
        otherPlayer.pieces.Remove(pieceToCapture);
        pieces[gridPoint.x, gridPoint.y] = null;
        Piece piece = pieceToCapture.GetComponent<Piece>();
        piece.taken = true;
        piece.navMeshAgent.enabled = false;
        piece.myRigidBody.constraints = RigidbodyConstraints.None;
        piecesTaken += 1;
        musicInstance.setParameterByName("PiecesTaken", piecesTaken);
        if (pieceToCapture.GetComponent<Piece>().type == PieceType.King)
        {
            Debug.Log(currentPlayer.name + " wins!");
            playerWinsObject.SetActive(true);
            playerWinsText.text = ("Game Over");
            Destroy(board.GetComponent<TileSelector>());
            Destroy(board.GetComponent<MoveSelector>());
            musicInstance.setPaused(true);
            gameOver = true;
        }

        if(DoesPieceBelongToWhite(pieceToCapture) == false)
        {
            if (pieceToCapture.GetComponent<Piece>().type == PieceType.Pawn)
            {
                score += 100;
            }

            if (pieceToCapture.GetComponent<Piece>().type == PieceType.Bishop || pieceToCapture.GetComponent<Piece>().type == PieceType.Knight)
            {
                score += 200;
            }

            if (pieceToCapture.GetComponent<Piece>().type == PieceType.Rook)
            {
                score += 300;
            }

            if (pieceToCapture.GetComponent<Piece>().type == PieceType.Queen)
            {
                score += 500;
            }
        }
        


        if (pieceToCapture.gameObject.tag == "RandomisePowerup")
        {
            if (pieceToTake.GetComponent<Piece>().type != PieceType.King)
            {
                pieceToCapture.GetComponent<PowerupScript>().deathReady = true;
                StartCoroutine(RandomisePiece(gridPoint, pieceToTake));
            }
            else
            {
                pieceToCapture.GetComponent<PowerupScript>().deathReady = true;
            }

        }
        else
        {
            Animator animator = pieceToCapture.GetComponent<Animator>();
            if (gridPoint.y == 7 || gridPoint.y == 0)
            {
                piece.myRigidBody.AddExplosionForce(500, pieceToCapture.transform.position - transform.forward * Random.Range(-15, 15) - transform.up * 10, 100, 100);
                piece.myCollider.enabled = false;
                animator.SetBool("Shrink", true);
            }
            else
            {
                piece.myRigidBody.AddExplosionForce(500, pieceToCapture.transform.position - transform.forward * Random.Range(-15, 15) - transform.up * 10, 100, 100);
                animator.SetBool("Shrink", true);
            }

            piece.particles.transform.SetParent(null);
            piece.particles.SetActive(true);
        }


    }
}
