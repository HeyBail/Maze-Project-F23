using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeMaker : MonoBehaviour
{
    private List<GameObject> cubeWalls = new List<GameObject>();
    private float size = 10f; // size = 5 * scale
    
    public int rowSquares = 5;
    public int colSquares = 5;
    public GameObject target;
    public GameObject exit;
    


    private List<Vector3Int> NeighboringWalls(int row, int col)
    {
        List<Vector3Int> result = new List<Vector3Int>();
        if (row > 0)
        {
            result.Add(new Vector3Int(row - 1, col, 0));
        }
        if (col > 0)
        {
            result.Add(new Vector3Int(row, col - 1, 1));
        }
        if (row < rowSquares - 1)
        {
            result.Add(new Vector3Int(row, col, 0));
        }
        if (col < colSquares - 1)
        {
            result.Add(new Vector3Int(row, col, 1));
        }
        return result;
    }

    void Start()
    {
        cubeWalls = GenerateMaze(new Vector2Int(UnityEngine.Random.Range(0, rowSquares), UnityEngine.Random.Range(0, colSquares)));
    }

    void Awake()
    {
        //cubeWalls = GenerateMaze();
    }

    public void finishLevel(Vector2Int startPosition) // the starting position would probably be different adjusted by the row and col shift
    {
        foreach (GameObject wall in cubeWalls)
        {
            Destroy(wall);
        }

        rowSquares += 2; //increasing by two would make the olayer not spawn in walls
        colSquares += 2;
        cubeWalls = GenerateMaze(startPosition);
    }

    public List<GameObject> GenerateMaze(Vector2Int startPosition) //give a start r and start c based on where the player / the exit is, remake the maze starting from there.
    {
        size = Math.Max(colSquares,rowSquares) * 2f;
        transform.localScale = new Vector3(size / 5f, transform.localScale.y, size / 5f); //resizes the maze grid.


        List<GameObject> result = new List<GameObject>();
        bool[,] inMaze = new bool[rowSquares, colSquares];
        bool[,] rowWalls = new bool[rowSquares - 1, colSquares];
        bool[,] colWalls = new bool[rowSquares, colSquares - 1];

        float rowSize = (2 * size) / rowSquares;
        float colSize = (2 * size) / colSquares;

        for (int i = 0; i < inMaze.GetLength(0); i++)
        {
            for (int j = 0; j < inMaze.GetLength(1); j++)
            {
                inMaze[i, j] = false;
            }
        }

        inMaze[startPosition.x, startPosition.y] = true;

        for (int i = 0; i < rowWalls.GetLength(0); i++)
        {
            for (int j = 0; j < rowWalls.GetLength(1); j++)
            {
                rowWalls[i, j] = true;
            }
        }
        for (int i = 0; i < colWalls.GetLength(0); i++)
        {
            for (int j = 0; j < colWalls.GetLength(1); j++)
            {
                colWalls[i, j] = true;
            }
        }

        List<Vector3Int> candidateWalls = NeighboringWalls(startPosition.x, startPosition.y);

        for (int area = rowSquares * colSquares, squareCount = 1; squareCount < area; squareCount++) // this could be a for loop
        {
            Vector3Int w = candidateWalls[UnityEngine.Random.Range(0, candidateWalls.Count)];
            
            //set current wall to either below or left
            int currentRow = w.x;
            int currentCol = w.y;

            if (w.z == 0)
            {
                rowWalls[currentRow, currentCol] = false;
            }
            else
            {
                colWalls[currentRow, currentCol] = false;
            }

            //checks if wall is already in maze, if it is set to right or top
            if (w.z == 0 && !inMaze[w.x + 1,w.y])
            {
                currentRow = w.x + 1;
            }
            if (w.z == 1 && !inMaze[w.x, w.y + 1])
            {
                currentCol = w.y + 1;
            }

            inMaze[currentRow, currentCol] = true;

            List<Vector3Int> newWalls = NeighboringWalls(currentRow, currentCol);
            for (int i = 0; i < newWalls.Count; i++)
            {
                if (candidateWalls.Contains(newWalls[i])) 
                {
                    candidateWalls.Remove(newWalls[i]);
                }
                else
                {
                    candidateWalls.Add(newWalls[i]);
                }
            }

            

            if (squareCount == 3 * area / 4 || squareCount == 2 * area / 4 || squareCount == area / 4)
            {
                GameObject.Instantiate(target, new Vector3((currentRow + .5f) * rowSize - size, 1f, (currentCol + .5f) * colSize - size), transform.rotation);
                //Debug.Log("TARGET CREATED AT: " + currentRow + currentCol);
            }
            else if (squareCount == area - 1)
            {
                GameObject.Instantiate(exit, new Vector3((currentRow + .5f) * rowSize - size, 0, (currentCol + .5f) * colSize - size), transform.rotation);
                //Debug.Log("Exit at: " + currentRow + currentCol);
                exit.GetComponent<Exit>().exitLocation = new Vector2Int(currentRow, currentCol);
            }
        }

        for (int i = 0; i < rowWalls.GetLength(0); i++)
        {
            int j = 0;
            while(j < rowWalls.GetLength(1))
            {
                int startj = -1;
                if (rowWalls[i, j])
                    startj = j;
                while (j < rowWalls.GetLength(1) && rowWalls[i, j]) j++;
                if (startj != -1)
                {
                    GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    wall.transform.position = new Vector3(-size + (startj + j) * colSize / 2.0f, 1.5f, (i + 1) * rowSize - size);

                    wall.transform.localScale = new Vector3((j - startj) * colSize + 0.5f, 3, 0.5f);

                    result.Add(wall);
                }
                j++;
            }
        }
        for (int i = 0; i < colWalls.GetLength(1); i++)
        {
            int j = 0;
            while (j < colWalls.GetLength(0))
            {
                int startj = -1;
                if (colWalls[j, i])
                {
                    startj = j;
                }
                while (j < colWalls.GetLength(0) && colWalls[j, i]) j++;
                if (startj != -1)
                {
                    GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    wall.transform.position = new Vector3((i + 1) * colSize -size , 1.5f, -size + (startj + j) * rowSize / 2.0f);

                    wall.transform.localScale = new Vector3(0.5f, 3, (j - startj) * rowSize + 0.5f);

                    result.Add(wall);
                }
                j++;
            }
        }

        return result;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
