using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.AI.Navigation;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private MazeCell _mazeCellPrefab;
    [SerializeField] private int _mazeWidth = 12;
    [SerializeField] private int _mazeDepth = 12;
    [SerializeField] private float _cellSize = 4f;
    [SerializeField] private float _wallHeight = 2.5f;
    [SerializeField] private float _wallThickness = 0.35f;
    [SerializeField] private NavMeshSurface _navMeshSurface;

    private MazeCell[,] _mazeGrid;

    private void Start()
    {
        _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];

        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {
                MazeCell cell = Instantiate(
                    _mazeCellPrefab,
                    new Vector3(x * _cellSize, 0, z * _cellSize),
                    Quaternion.identity
                );

                cell.Setup(_cellSize, _wallHeight, _wallThickness);
                _mazeGrid[x, z] = cell;
            }
        }

        GenerateMaze(null, _mazeGrid[0, 0]);

        // Entrance
        _mazeGrid[0, 0].ClearBackWall();

        // Exit
        _mazeGrid[_mazeWidth - 1, _mazeDepth - 1].ClearFrontWall();

        if (_navMeshSurface != null)
        {
            _navMeshSurface.BuildNavMesh();
        }
    }

    private void GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        currentCell.Visit();
        ClearWalls(previousCell, currentCell);

        MazeCell nextCell;

        do
        {
            nextCell = GetNextUnvisitedCell(currentCell);

            if (nextCell != null)
            {
                GenerateMaze(currentCell, nextCell);
            }

        } while (nextCell != null);
    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        return GetUnvisitedCells(currentCell)
            .OrderBy(_ => Random.Range(1, 10))
            .FirstOrDefault();
    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        int x = Mathf.RoundToInt(currentCell.transform.position.x / _cellSize);
        int z = Mathf.RoundToInt(currentCell.transform.position.z / _cellSize);

        if (x + 1 < _mazeWidth && !_mazeGrid[x + 1, z].IsVisited)
            yield return _mazeGrid[x + 1, z];

        if (x - 1 >= 0 && !_mazeGrid[x - 1, z].IsVisited)
            yield return _mazeGrid[x - 1, z];

        if (z + 1 < _mazeDepth && !_mazeGrid[x, z + 1].IsVisited)
            yield return _mazeGrid[x, z + 1];

        if (z - 1 >= 0 && !_mazeGrid[x, z - 1].IsVisited)
            yield return _mazeGrid[x, z - 1];
    }

    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        if (previousCell == null) return;

        if (previousCell.transform.position.x < currentCell.transform.position.x)
        {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
            return;
        }

        if (previousCell.transform.position.x > currentCell.transform.position.x)
        {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
            return;
        }

        if (previousCell.transform.position.z < currentCell.transform.position.z)
        {
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
            return;
        }

        if (previousCell.transform.position.z > currentCell.transform.position.z)
        {
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
        }
    }
}