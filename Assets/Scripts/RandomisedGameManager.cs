using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomisedGameManager : GameManager
{
    public List<GameObject> randomisedWhitePieces;
    public List<GameObject> randomisedBlackPieces;



    IEnumerator StartUp()
    {
        AddPiece(randomisedWhitePieces[Random.Range(0, randomisedWhitePieces.Count)], white, 0, 0);
        yield return new WaitForSecondsRealtime(.2f);
        AddPiece(randomisedWhitePieces[Random.Range(0, randomisedWhitePieces.Count)], white, 1, 0);
        yield return new WaitForSecondsRealtime(.1f);
        AddPiece(randomisedWhitePieces[Random.Range(0, randomisedWhitePieces.Count)], white, 2, 0);
        yield return new WaitForSecondsRealtime(.1f);
        AddPiece(randomisedWhitePieces[Random.Range(0, randomisedWhitePieces.Count)], white, 3, 0);
        yield return new WaitForSecondsRealtime(.1f);
        AddPiece(whiteKing, white, 4, 0);
        yield return new WaitForSecondsRealtime(.1f);
        AddPiece(randomisedWhitePieces[Random.Range(0, randomisedWhitePieces.Count)], white, 5, 0);
        yield return new WaitForSecondsRealtime(.1f);
        AddPiece(randomisedWhitePieces[Random.Range(0, randomisedWhitePieces.Count)], white, 6, 0);
        yield return new WaitForSecondsRealtime(.1f);
        AddPiece(randomisedWhitePieces[Random.Range(0, randomisedWhitePieces.Count)], white, 7, 0);
        yield return new WaitForSecondsRealtime(.1f);

        for (int i = 0; i < 8; i++)
        {
            AddPiece(whitePawn, white, i, 1);
            yield return new WaitForSecondsRealtime(.1f);
        }

        AddPiece(randomisedBlackPieces[Random.Range(0, randomisedBlackPieces.Count)], black, 0, 7);
        yield return new WaitForSecondsRealtime(.1f);
        AddPiece(randomisedBlackPieces[Random.Range(0, randomisedBlackPieces.Count)], black, 1, 7);
        yield return new WaitForSecondsRealtime(.1f);
        AddPiece(randomisedBlackPieces[Random.Range(0, randomisedBlackPieces.Count)], black, 2, 7);
        yield return new WaitForSecondsRealtime(.1f);
        AddPiece(randomisedBlackPieces[Random.Range(0, randomisedBlackPieces.Count)], black, 3, 7);
        yield return new WaitForSecondsRealtime(.1f);
        AddPiece(blackKing, black, 4, 7);
        yield return new WaitForSecondsRealtime(.1f);
        AddPiece(randomisedBlackPieces[Random.Range(0, randomisedBlackPieces.Count)], black, 5, 7);
        yield return new WaitForSecondsRealtime(.1f);
        AddPiece(randomisedBlackPieces[Random.Range(0, randomisedBlackPieces.Count)], black, 6, 7);
        yield return new WaitForSecondsRealtime(.1f);
        AddPiece(randomisedBlackPieces[Random.Range(0, randomisedBlackPieces.Count)], black, 7, 7);
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

    public override void InitialSetup()
    {
        
        StartCoroutine(StartUp());
    }
}
