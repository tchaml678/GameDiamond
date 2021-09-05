using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    [SerializeField] int preCol, preRow; 
    public int col, row;
    [SerializeField] int targetX, targetY;
    public bool isMatched = false;
    [SerializeField] float swipeAngle = 0f;
    [SerializeField] float swipeResist = 0.1f;

    private FindMatches findMatches;
    private GameObject otherGem;
    private Board board;
    private Vector2 firstTouchPos;
    private Vector2 finalTouchPos;
    private Vector2 tempPos;
    

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        findMatches = FindObjectOfType<FindMatches>();
        //targetX = (int)transform.position.x;
        //targetY = (int)transform.position.y;
        //row = targetY;
        //col = targetX;
        //preCol = col;
        //preRow = row;
    }

    // Update is called once per frame
    void Update()
    {
        FindMatches();
        ChangeLocation();
    }

    private void OnMouseDown()
    {
        if (board.curState == GameState.move)
        {
            firstTouchPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }
    }
    private void OnMouseUp()
    {
        if (board.curState == GameState.move)
        {
            finalTouchPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            CalulateAngle();
        }
    }
    public IEnumerator CheckMove()
    {
        yield return new WaitForSeconds(0.3f);
        if (otherGem != null)
        {
            if(!isMatched && !otherGem.GetComponent<Gem>().isMatched)
            {
                otherGem.GetComponent<Gem>().row = row;
                otherGem.GetComponent<Gem>().col = col;
                row = preRow;
                col = preCol;
                yield return new WaitForSeconds(0.3f);
                board.curState = GameState.move;
            }
            else
            {
                board.DestroyMatches();
                
            }
            otherGem = null;
        }
       
    }
    void CalulateAngle()
    {
       
        swipeAngle = Mathf.Atan2(finalTouchPos.y - firstTouchPos.y, finalTouchPos.x - firstTouchPos.x) * 180 / Mathf.PI;
        MoveGems();
        board.curState = GameState.wait;
    }
    void MoveGems()
    {
        if (swipeAngle >= -45 && swipeAngle <= 45 && col < board.width - 1)//Right Swipe
        {
            otherGem = board.allGems[col + 1, row];
            preCol = col;
            preRow = row;
            otherGem.GetComponent<Gem>().col -= 1;
            col += 1;
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row <  board.height - 1)//Up Swipe
        {
            otherGem = board.allGems[col, row + 1];
            preCol = col;
            preRow = row;
            otherGem.GetComponent<Gem>().row -= 1;
            row += 1;
        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && col > 0)//Left Swipe
        {
            otherGem = board.allGems[col - 1, row];
            preCol = col;
            preRow = row;
            otherGem.GetComponent<Gem>().col += 1;
            col -= 1;
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0)//Down Swipe 
        {
            otherGem = board.allGems[col, row - 1];
            preCol = col;
            preRow = row;
            otherGem.GetComponent<Gem>().row += 1;
            row -= 1;
        }
        StartCoroutine(CheckMove());
    }
    void ChangeLocation()
    {
        targetX = col;
        targetY = row;
        if (Mathf.Abs(targetX - transform.position.x) > 0.1f)
        {
            // Move towards the target
            tempPos = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPos, 0.5f);
            if (board.allGems[col, row] != this.gameObject)
            {
                board.allGems[col, row] = this.gameObject;
            }
            findMatches.FindAllMatches();
        }
        else
        {
            // Directly set the position
            tempPos = new Vector2(targetX, transform.position.y);
            transform.position = tempPos;
            //board.allGems[col, row] = this.gameObject;
        }
        if (Mathf.Abs(targetY - transform.position.y) > 0.1f)
        {
            // Move towards the target
            tempPos = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPos, 0.5f);
            if (board.allGems[col, row] != this.gameObject)
            {
                board.allGems[col, row] = this.gameObject;
            }
            findMatches.FindAllMatches();

        }
        else
        {
            // Directly set the position
            tempPos = new Vector2(transform.position.x, targetY);
            transform.position = tempPos;
            //board.allGems[col, row] = this.gameObject;
        }
    }
    void FindMatches()
    {
        if(col > 0 && col < board.width -1)
        {
            GameObject leftGem1 = board.allGems[col - 1, row];
            GameObject rightGem1 = board.allGems[col + 1, row];
            if (leftGem1 != null && rightGem1 != null)
            {
                if (leftGem1.tag == this.gameObject.tag && rightGem1.tag == this.gameObject.tag)
                {
                    leftGem1.GetComponent<Gem>().isMatched = true;
                    rightGem1.GetComponent<Gem>().isMatched = true;
                    isMatched = true;

                }
            }
        }
        if (row > 0 && row < board.height - 1)
        {
            GameObject upGem1 = board.allGems[col, row+1];
            GameObject downGem1 = board.allGems[col, row-1];
            if (upGem1 != null && downGem1 != null)
            {
                if (upGem1.tag == this.gameObject.tag && downGem1.tag == this.gameObject.tag)
                {
                    upGem1.GetComponent<Gem>().isMatched = true;
                    downGem1.GetComponent<Gem>().isMatched = true;
                    isMatched = true;

                }
            }
        }
    }
}
