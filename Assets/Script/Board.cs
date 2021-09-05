using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GameState
{
    wait,
    move
}


public class Board : MonoBehaviour
{
    
    public GameState curState = GameState.move;
    public int width, height;
    public GameObject[,] allGems;
    public int basePieceValue;
    public int streakValue = 1;

    [SerializeField] int offSet;
    [SerializeField] GameObject gemBGPrefab;
    [SerializeField] GameObject[] listGems;
    [SerializeField] GameObject gemDestroyEffect;
    
   
    private FindMatches findMatches;
    private ScoreManager scoreManager;

    
    // Start is called before the first frame update
    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        findMatches = FindObjectOfType<FindMatches>();
        
        allGems = new GameObject[width, height];
        SetUp();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetUp()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                Vector2 tempPos = new Vector2(i, j + offSet);
                GameObject backgroundTile = Instantiate(gemBGPrefab, new Vector2(i,j), Quaternion.identity) as GameObject;
                backgroundTile.transform.parent = this.transform;
                backgroundTile.name = "( " + i + ", " + j + " )";
                          
                int gemInList = Random.Range(0, listGems.Length);
                int maxIterations = 0;
                while (MatchesAt(i, j, listGems[gemInList]) && maxIterations < 200)
                {
                    gemInList = Random.Range(0, listGems.Length);
                    maxIterations++;
                    
                }
                maxIterations = 0;
                GameObject gem = Instantiate(listGems[gemInList], tempPos, Quaternion.identity);
                gem.GetComponent<Gem>().row = j;
                gem.GetComponent<Gem>().col = i;
                gem.transform.parent = this.transform;
                gem.name = "( " + i + ", " + j + " )";
                allGems[i, j] = gem;
            }
        }
    }
    private bool MatchesAt(int col, int row, GameObject piece)
    {
        if (col > 1 && row > 1)
        {
            if(allGems[col - 1, row].tag == piece.tag && allGems[col - 2, row].tag == piece.tag)
            {
                return true;
            }
            if (allGems[col, row - 1].tag == piece.tag && allGems[col, row - 2].tag == piece.tag)
            {
                return true;
            }
        }
        else if (col <= 1 || row <= 1)
        {
            if(row > 1)
            {
                if(allGems[col,row-1].tag == piece.tag && allGems[col,row - 2].tag == piece.tag)
                {
                    return true;
                }
            }
            if (col > 1)
            {
                if (allGems[col - 1, row].tag == piece.tag && allGems[col - 2, row].tag == piece.tag)
                {
                    return true;
                }
            }
        }
        return false;
    }
    private void DestroyMatchesAt(int col, int row)
    {
        if (allGems[col, row].GetComponent<Gem>().isMatched)
        {
            findMatches.curMatches.Remove(allGems[col, row]);
            GameObject particle = Instantiate(gemDestroyEffect, allGems[col, row].transform.position, Quaternion.identity);
            Destroy(particle, 0.5f);
            Destroy(allGems[col, row]);
            scoreManager.AddScore(basePieceValue * streakValue);
            allGems[col, row] = null;
        }
    }
    public void DestroyMatches()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                if (allGems[i, j] != null)
                {
                    DestroyMatchesAt(i, j);
                }
            }
        }
        StartCoroutine(DecreaseRowCol());
    }
    private IEnumerator DecreaseRowCol()
    {
        int nullCount = 0;
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                if(allGems[i,j]== null)
                {
                    nullCount++;
                }
                else if (nullCount > 0)
                {
                    allGems[i, j].GetComponent<Gem>().row -= nullCount;
                    allGems[i, j] = null;
                }
            }
            nullCount = 0;
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(SpawnGemsCo());
    }
    private void SpawnGems()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                if (allGems[i, j] == null)
                {
                    Vector2 tempPos = new Vector2(i, j + offSet);
                    int gemInList = Random.Range(0, listGems.Length);
                    GameObject piece = Instantiate(listGems[gemInList], tempPos, Quaternion.identity);
                    allGems[i, j] = piece;
                    piece.GetComponent<Gem>().row = j;
                    piece.GetComponent<Gem>().col = i;
                }
            }
        }
    }
    private bool MatchesOnBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allGems[i, j] != null)
                {
                    if (allGems[i, j].GetComponent<Gem>().isMatched)
                    {
                        return true;
                    }
                }
            }
        }
                return false;
    }
    private IEnumerator SpawnGemsCo()
    {
        SpawnGems();
        yield return new WaitForSeconds(0.5f);
        while (MatchesOnBoard())
        {
            streakValue++;
            yield return new WaitForSeconds(0.5f);
            DestroyMatches();
        }
        yield return new WaitForSeconds(0.5f);
        curState = GameState.move;
        streakValue = 1;
    }
}
