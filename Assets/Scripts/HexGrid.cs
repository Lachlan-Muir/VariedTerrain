using UnityEngine;
using UnityEngine.UI;


public class HexGrid : MonoBehaviour
{
    public int width = 6;
    public int height = 6;

    public HexCell cellPrefab;

    HexCell[] cells;

    public Text cellLabelPrefab;
    Canvas gridCanvas;

    HexMesh hexMesh;

    public Color defaultColor = Color.white;
    public Color touchedColor = Color.magenta;

    void CreateCell (int x, int z, int cellIdx, int height)
    {
        Vector3 pos;
        pos.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
        pos.y = height;
        pos.z = z * (HexMetrics.outerRadius * 1.5f);

        HexCell cell = cells[cellIdx] = Instantiate(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = pos;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        cell.color = defaultColor;

        // Adding cell labels
        Text label = Instantiate<Text>(cellLabelPrefab);
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(pos.x, pos.z);
        label.text = cell.coordinates.ToStringOnSeparateLines();
    }

    private void Awake()
    {
        gridCanvas = GetComponentInChildren<Canvas>();
        hexMesh = GetComponentInChildren<HexMesh>();

        cells = new HexCell[width * height];

        for (int z = 0, i = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                CreateCell(x, z, i++, z*2);
            }
        }
    }

    private void Start()
    {
        // invoked here to ensure it happens after the hex mesh component has also had a chance to call Awake
        hexMesh.Triangulate(cells);
    }


    public void TouchCell (Vector3 position, Color color)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
        HexCell cell = cells[index];
        cell.color = color;
        hexMesh.Triangulate(cells);
    }
}
