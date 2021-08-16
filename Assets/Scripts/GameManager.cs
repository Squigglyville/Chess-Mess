using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FMODUnity;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public BoardManager board;

    public GameObject whiteKing;
    public GameObject whiteQueen;
    public GameObject whiteBishop;
    public GameObject whiteKnight;
    public GameObject whiteRook;
    public GameObject whitePawn;

    public GameObject blackKing;
    public GameObject blackQueen;
    public GameObject blackBishop;
    public GameObject blackKnight;
    public GameObject blackRook;
    public GameObject blackPawn;

    public GameObject hordePiece;

    public GameObject[,] pieces;
    public List<GameObject> randomWhitePieces;
    public List<GameObject> randomBlackPieces;
    public Player white;
    public Player black;
    public Player currentPlayer;
    public Player otherPlayer;
    public Player powerUpPlayer;

    public GameObject playerWinsObject;
    public TextMeshProUGUI playerWinsText;

    public int turnCount;
    public List<Vector2Int> moveLocations;
    public List<Vector2Int> attackMoveLocations;
    public List<GameObject> piecesThatCanMove;
    public List<GameObject> piecesThatCanAttack;
    public bool kingDetected;
    public List<Vector2Int> possibleMoves;
    public GameObject randompiece;
    Vector2Int gridPoint;
    public float timeUntilComputerMoves;
    public float time;
    public GameObject startText;

    public bool multiplayer;
    public bool computerWaiting;
    public bool playerInput;
    public bool gameOver;
    public int powerUpsOn;
    
    public FMOD.Studio.EventInstance musicInstance;
    [FMODUnity.EventRef]
    public string musicEvent;
    [SerializeField] [Range(0f, 12f)]
    public float piecesTaken;

    public GameObject powerUp;
    public List<Vector2Int> locations;

    void Awake()
    {
        instance = this;
    }

    public virtual void Start()
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
        musicInstance = FMODUnity.RuntimeManager.CreateInstance("event:/Music/Music");
        musicInstance.start();
        musicInstance.setParameterByName("PiecesTaken", piecesTaken);
        powerUpsOn = PlayerPrefs.GetInt("PowerUps", 1);
    }

    private void OnDestroy()
    {
        musicInstance.setPaused(true);
    }

    public virtual void InitialSetup()
    {

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

        AddPiece(blackRook, black, 0, 7);
        yield return new WaitForSecondsRealtime(.1f);
        AddPiece(blackKnight, black, 1, 7);
        yield return new WaitForSecondsRealtime(.1f);
        AddPiece(blackBishop, black, 2, 7);
        yield return new WaitForSecondsRealtime(.1f);
        AddPiece(blackQueen, black, 3, 7);
        yield return new WaitForSecondsRealtime(.1f);
        AddPiece(blackKing, black, 4, 7);
        yield return new WaitForSecondsRealtime(.1f);
        AddPiece(blackBishop, black, 5, 7);
        yield return new WaitForSecondsRealtime(.1f);
        AddPiece(blackKnight, black, 6, 7);
        yield return new WaitForSecondsRealtime(.1f);
        AddPiece(blackRook, black, 7, 7);
        yield return new WaitForSecondsRealtime(.1f);

        for (int i = 0; i < 8; i++)
        {
            AddPiece(blackPawn, black, i, 6);
            yield return new WaitForSecondsRealtime(.1f);
        }

        board.GetComponent<TileSelector>().enabled = true;
        board.GetComponent<MoveSelector>().enabled = true;
        yield return new WaitForSecondsRealtime(.5f);
        
        startText.SetActive(true);
        yield return null;
    }

    public void AddPiece(GameObject prefab, Player player, int col, int row)
    {
        GameObject pieceObject = board.AddPiece(prefab, col, row);
        player.pieces.Add(pieceObject);
        pieces[col, row] = pieceObject;
        
    }

    public void SelectPieceAtGrid(Vector2Int gridPoint)
    {
        GameObject selectedPiece = pieces[gridPoint.x, gridPoint.y];
        if (selectedPiece)
        {
            board.SelectPiece(selectedPiece);
        }
    }

    public void SelectPiece(GameObject piece)
    {
        board.SelectPiece(piece);
    }

    public void DeselectPiece(GameObject piece)
    {
        board.DeselectPiece(piece);
    }

    public GameObject PieceAtGrid(Vector2Int gridPoint)
    {
        if (gridPoint.x > 7 || gridPoint.y > 7 || gridPoint.x < 0 || gridPoint.y < 0)
        {
            return null;
        }
        return pieces[gridPoint.x, gridPoint.y];
    }

    public Vector2Int GridForPiece(GameObject piece)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (pieces[i, j] == piece)
                {
                    return new Vector2Int(i, j);
                }
            }
        }

        return new Vector2Int(-1, -1);
    }

    public bool FriendlyPieceAt(Vector2Int gridPoint)
    {
        GameObject piece = PieceAtGrid(gridPoint);

        if (piece == null)
        {
            return false;
        }

        if (otherPlayer.pieces.Contains(piece))
        {
            return false;
        }

        if (powerUpPlayer.pieces.Contains(piece))
        {
            return false;
        }

        return true;
    }

    public bool DoesPieceBelongToCurrentPlayer(GameObject piece)
    {
        return currentPlayer.pieces.Contains(piece);
    }
    public bool DoesPieceBelongToWhite(GameObject piece)
    {
        return white.pieces.Contains(piece);
    }

    public void Move(GameObject piece, Vector2Int gridPoint)
    {
        Vector2Int startGridPoint = GridForPiece(piece);
        pieces[startGridPoint.x, startGridPoint.y] = null;
        pieces[gridPoint.x, gridPoint.y] = piece;
        board.MovePiece(piece, gridPoint);
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/PieceSlide");
      
        if (piece.GetComponent<Pawn>() != null)
        {
            if (gridPoint.y == 7)
            {
                Destroy(piece);
                GameObject pieceObject = board.AddPiece(whiteQueen, gridPoint.x, gridPoint.y);
                white.pieces.Add(pieceObject);
                pieces[gridPoint.x, gridPoint.y] = pieceObject;
                               
                                
            }
            if(gridPoint.y == 0)
            {
                AddPiece(blackQueen, black, gridPoint.x, gridPoint.y);
                
                Destroy(piece);
            }
                        
        }
                
    }
    
    public virtual List<Vector2Int> MovesForPiece(GameObject pieceObject)
    {
        Piece piece = pieceObject.GetComponent<Piece>();
        Vector2Int gridPoint = GridForPiece(pieceObject);
        var locations = piece.MoveLocations(gridPoint);

        // filter out offboard locations
        locations.RemoveAll(tile => tile.x < 0 || tile.x > 7
            || tile.y < 0 || tile.y > 7);

        // filter out locations with friendly piece
        locations.RemoveAll(tile => FriendlyPieceAt(tile));

        return locations;
    }

    public virtual List<Vector2Int> MovesForPiecesThatAttack(GameObject pieceObject)
    {
        Piece piece = pieceObject.GetComponent<Piece>();
        Vector2Int gridPoint = GridForPiece(pieceObject);
        var locations = piece.MoveLocations(gridPoint);
        
        // filter out offboard locations
        locations.RemoveAll(tile => tile.x < 0 || tile.x > 7
            || tile.y < 0 || tile.y > 7);

        // filter out locations with friendly piece
        locations.RemoveAll(tile => FriendlyPieceAt(tile));

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
    public virtual void NextPlayer()
    {
        Player tempPlayer = currentPlayer;
        currentPlayer = otherPlayer;
        otherPlayer = tempPlayer;
        turnCount += 1;
        int randomNumber = Random.Range(0, 15);
        if(powerUpsOn == 1)
        {
            if (turnCount >= 3)
            {
                if (randomNumber <= 1)
                {
                    AddPowerup();
                }
            }
        }
              
        
    }

    public virtual void ComputerTurn()
    {
        if(gameOver == false)
        {
            piecesThatCanAttack.Clear();
            piecesThatCanMove.Clear();

            foreach (GameObject piece in black.pieces)
            {
                attackMoveLocations = MovesForPiecesThatAttack(piece);
                if (attackMoveLocations.Count > 0)
                {
                    piecesThatCanAttack.Add(piece);
                }

                moveLocations = MovesForPiece(piece);
                if (moveLocations.Count > 0)
                {
                    piecesThatCanMove.Add(piece);
                }
            }

            if (piecesThatCanAttack.Count > 0)
            {
                randompiece = piecesThatCanAttack[Random.Range(0, piecesThatCanAttack.Count)];
                possibleMoves = MovesForPiecesThatAttack(randompiece);
            }
            else
            {
                randompiece = piecesThatCanMove[Random.Range(0, piecesThatCanMove.Count)];
                possibleMoves = MovesForPiece(randompiece);
            }

            Vector2Int randomMove = possibleMoves[Random.Range(0, possibleMoves.Count)];
            Vector3 randomPosition = Geometry.PointFromGrid(randomMove);
            Vector2Int gridPoint = Geometry.GridFromPoint(randomPosition);


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
            NextPlayer();
        }
        
    }

    public virtual void Update()
    {
        if(computerWaiting == true)
        {
            time += Time.deltaTime;
            if(time >= timeUntilComputerMoves)
            {
                ComputerTurn();
                computerWaiting = false;
                time = 0;
                playerInput = true;
            }
        }
          
        
    }

    public virtual void CapturePieceAt(Vector2Int gridPoint, GameObject pieceToTake)
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
            playerWinsText.text = (currentPlayer.name + " wins!");
            Destroy(board.GetComponent<TileSelector>());
            Destroy(board.GetComponent<MoveSelector>());
            musicInstance.setPaused(true);
            gameOver = true;
        }
        if(pieceToCapture.gameObject.tag == "RandomisePowerup")
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

    public IEnumerator RandomisePiece(Vector2Int gridpoint, GameObject pieceTaking)
    {
               
        yield return new WaitForSecondsRealtime(.5f);
        black.pieces.Remove(pieceTaking);
        Destroy(pieceTaking);
        yield return new WaitForSecondsRealtime(.1f);
        if (DoesPieceBelongToWhite(pieceTaking) == true)
        {
            AddPiece(randomWhitePieces[Random.Range(0, randomWhitePieces.Count)], white, gridpoint.x, gridpoint.y);
        }
        else
        {
            AddPiece(randomBlackPieces[Random.Range(0, randomBlackPieces.Count)], black, gridpoint.x, gridpoint.y);
        }
        yield return null;
    }

     

    public void AddPowerup()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
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
            AddPiece(powerUp, powerUpPlayer, randomlocation.x, randomlocation.y);
        }
        
    }



}
