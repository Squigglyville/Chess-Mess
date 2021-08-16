using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelector : MonoBehaviour
{
    public GameObject tileHighlightPrefab;

    private GameObject tileHighlight;

    LayerMask layerMask;

    // Start is called before the first frame update
    void Start()
    {
        Vector2Int gridPoint = Geometry.GridPoint(0, 0);
        Vector3 point = Geometry.PointFromGrid(gridPoint);
        tileHighlight = Instantiate(tileHighlightPrefab, point, Quaternion.identity, gameObject.transform);
        tileHighlight.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.pause.currentMenu != PauseMenu.pause.pauseMenu)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            LayerMask layerMask = LayerMask.GetMask("Board");
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, layerMask))
            {
                Vector3 point = hit.point;
                Vector2Int gridPoint = Geometry.GridFromPoint(point);

                tileHighlight.SetActive(true);
                tileHighlight.transform.position = Geometry.PointFromGrid(gridPoint);
                if (Input.GetMouseButtonDown(0))
                {
                    if (GameManager.instance.playerInput == true)
                    {
                        GameObject selectedPiece =
                        GameManager.instance.PieceAtGrid(gridPoint);
                        if (GameManager.instance.DoesPieceBelongToCurrentPlayer(selectedPiece))
                        {
                            GameManager.instance.SelectPiece(selectedPiece);
                            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/PieceSelect");
                            ExitState(selectedPiece);

                        }
                    }


                }
            }
            else
            {
                tileHighlight.SetActive(false);
            }
        }
        

    }
    private void ExitState(GameObject movingPiece)
    {
        this.enabled = false;
        tileHighlight.SetActive(false);
        MoveSelector move = GetComponent<MoveSelector>();
        move.EnterState(movingPiece);
    }

    public void EnterState()
    {
        enabled = true;
    }

}
