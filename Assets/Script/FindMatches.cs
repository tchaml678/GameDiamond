using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMatches : MonoBehaviour
{
    private Board board;
    public List<GameObject> curMatches = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void FindAllMatches()
    {
        StartCoroutine(FindAllMatchesCo());
    }
    private IEnumerator FindAllMatchesCo()
    {
        yield return new WaitForSeconds(0.2f);
        for(int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                GameObject curGem = board.allGems[i, j];
                if (curGem != null)
                {
                    if (i > 0 && i < board.width - 1)
                    {
                        GameObject leftGem = board.allGems[i - 1, j];
                        GameObject rightGem = board.allGems[i + 1, j];
                        if (leftGem != null && rightGem != null)
                        {
                            if(leftGem.tag==curGem.tag && rightGem.tag == curGem.tag)
                            {
                                if (!curMatches.Contains(leftGem))
                                {
                                    curMatches.Add(leftGem);
                                }
                                leftGem.GetComponent<Gem>().isMatched = true;
                                if (!curMatches.Contains(rightGem))
                                {
                                    curMatches.Add(rightGem);
                                }
                                rightGem.GetComponent<Gem>().isMatched = true;
                                if (!curMatches.Contains(curGem))
                                {
                                    curMatches.Add(curGem);
                                }
                                curGem.GetComponent<Gem>().isMatched = true;
                            }
                        }
                    }
                    if (j > 0 && j < board.height - 1)
                    {
                        GameObject upGem = board.allGems[i , j + 1];
                        GameObject downGem = board.allGems[i , j - 1];
                        if (upGem != null && downGem != null)
                        {
                            if (upGem.tag == curGem.tag && downGem.tag == curGem.tag)
                            {
                                if (!curMatches.Contains(upGem))
                                {
                                    curMatches.Add(upGem);
                                }
                                upGem.GetComponent<Gem>().isMatched = true;

                                if (!curMatches.Contains(downGem))
                                {
                                    curMatches.Add(downGem);
                                }
                                downGem.GetComponent<Gem>().isMatched = true;
                                if (!curMatches.Contains(curGem))
                                {
                                    curMatches.Add(curGem);
                                }
                                curGem.GetComponent<Gem>().isMatched = true;
                            }
                        }
                    }
                }
            }
        }
    }
}
