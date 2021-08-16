using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSelector : MonoBehaviour
{
    public GameObject moveLocationPrefab;
    public GameObject tileHighlightPrefab;
    public GameObject attackLocationPrefab;

    private GameObject tileHighlight;
    private GameObject movingPiece;

    private List<Vector2Int> moveLocations;
    private List<GameObject> locationHighlights;


    // Start is called before the first frame update
    void Start()
    {
        this.enabled = false;
        tileHighlight = Instantiate(tileHighlightPrefab, Geometry.PointFromGrid(new Vector2Int(0, 0)),
            Quaternion.identity, gameObject.transform);
        tileHighlight.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if(PauseMenu.pause.currentMenu != PauseMenu.pause.pauseMenu)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 point = hit.point;
                Vector2Int gridPoint = Geometry.GridFromPoint(point);

                tileHighlight.SetActive(true);
                tileHighlight.transform.position = Geometry.PointFromGrid(gridPoint);
                if (Input.GetMouseButtonDown(0))
                {
                    if (GameManager.instance.playerInput == true)
                    {
                        // Reference Point 2: check for valid move location
                        if (!moveLocations.Contains(gridPoint))
                        {
                            return;
                        }

                        if (GameManager.instance.PieceAtGrid(gridPoint) == null)
                        {
                            GameManager.instance.Move(movingPiece, gridPoint);

                        }
                        else
                        {
                            GameManager.instance.CapturePieceAt(gridPoint, movingPiece);
                            GameManager.instance.Move(movingPiece, gridPoint);

                        }

                        ExitState();
                    }


                }
            }
            else
            {
                tileHighlight.SetActive(false);
            }

            if (Input.GetButtonDown("Fire2"))
            {
                this.enabled = false;
                tileHighlight.SetActive(false);
                GameManager.instance.DeselectPiece(movingPiece);
                movingPiece = null;
                TileSelector selector = GetComponent<TileSelector>();
                selector.EnterState();
                foreach (GameObject highlight in locationHighlights)
                {
                    Destroy(highlight);
                }
            }
        }
       

    }
    public void EnterState(GameObject piece)
    {
        movingPiece = piece;
        this.enabled = true;

        moveLocations = GameManager.instance.MovesForPiece(movingPiece);
        locationHighlights = new List<GameObject>();

        foreach (Vector2Int loc in moveLocations)
        {
            GameObject highlight;
            if (GameManager.instance.PieceAtGrid(loc))
            {
                highlight = Instantiate(attackLocationPrefab, Geometry.PointFromGrid(loc),
                    Quaternion.identity, gameObject.transform);
            }
            else
            {
                highlight = Instantiate(moveLocationPrefab, Geometry.PointFromGrid(loc),
                    Quaternion.identity, gameObject.transform);
            }
            locationHighlights.Add(highlight);
        }

    }
    private void ExitState()
    {
        this.enabled = false;
        tileHighlight.SetActive(false);
        GameManager.instance.DeselectPiece(movingPiece);
        movingPiece = null;
        TileSelector selector = GetComponent<TileSelector>();
        GameManager.instance.NextPlayer();
        
        if(GameManager.instance.multiplayer == false)
        {
            GameManager.instance.computerWaiting = true;
            GameManager.instance.playerInput = false;
        }
        
        selector.EnterState();
        foreach (GameObject highlight in locationHighlights)
        {
            Destroy(highlight);
        }

    }


}
