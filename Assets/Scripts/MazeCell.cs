using UnityEngine;

public class MazeCell : MonoBehaviour
{
    [SerializeField] private GameObject _leftWall;
    [SerializeField] private GameObject _rightWall;
    [SerializeField] private GameObject _frontWall;
    [SerializeField] private GameObject _backWall;
    [SerializeField] private GameObject _unvisitedBlock;

    public bool IsVisited { get; private set; }

    public void Setup(float cellSize, float wallHeight, float wallThickness)
    {
        float half = cellSize / 2f;

        SetupWall(_leftWall, new Vector3(-half, wallHeight / 2f, 0), new Vector3(wallThickness, wallHeight, cellSize));
        SetupWall(_rightWall, new Vector3(half, wallHeight / 2f, 0), new Vector3(wallThickness, wallHeight, cellSize));
        SetupWall(_frontWall, new Vector3(0, wallHeight / 2f, half), new Vector3(cellSize, wallHeight, wallThickness));
        SetupWall(_backWall, new Vector3(0, wallHeight / 2f, -half), new Vector3(cellSize, wallHeight, wallThickness));

        if (_unvisitedBlock != null)
            _unvisitedBlock.SetActive(false);
    }

    private void SetupWall(GameObject wall, Vector3 position, Vector3 scale)
    {
        wall.transform.localPosition = position;
        wall.transform.localRotation = Quaternion.identity;
        wall.transform.localScale = scale;

        BoxCollider collider = wall.GetComponent<BoxCollider>();
        if (collider != null)
        {
            collider.size = Vector3.one;
            collider.center = Vector3.zero;
            collider.isTrigger = false;
        }

        Transform graphics = wall.transform.Find("Graphics");
        if (graphics != null)
        {
            graphics.localPosition = Vector3.zero;
            graphics.localRotation = Quaternion.identity;
            graphics.localScale = Vector3.one;
        }
    }

    public void Visit()
    {
        IsVisited = true;
    }

    public void ClearLeftWall() => _leftWall.SetActive(false);
    public void ClearRightWall() => _rightWall.SetActive(false);
    public void ClearFrontWall() => _frontWall.SetActive(false);
    public void ClearBackWall() => _backWall.SetActive(false);
}