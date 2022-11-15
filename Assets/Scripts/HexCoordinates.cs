using UnityEngine;

[System.Serializable]
public struct HexCoordinates
{
    [SerializeField]
    private int x, z; // serialized fields so that we can see them in unity inspector

    public int X { get { return x; } }

    public int Z { get { return z; } }

    // To efficiently produce tile translations (such as moving between tiles), we can use Cube Coordinates with a third coordinate
    // since X + Z + Y = 0, we don't have to store Y. We can easily compute it at any time with X and Z
    public int Y // MAY WANT TO CHANGE TO A DIFFERENT COORD NAME IN FUTURE - HEIGHT MAPS!!!
    {
        get { return -X - Z; }
    }

    public HexCoordinates (int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public static HexCoordinates FromOffsetCoordinates (int x, int z)
    {
        return new HexCoordinates(x - z / 2, z); // undo the horizontal (x) shift
    }

    public static HexCoordinates FromPosition(Vector3 position)
    {
        float x = position.x / (HexMetrics.innerRadius * 2f);
        float y = -x;
        float offset = position.z / (HexMetrics.outerRadius * 3f);
        x -= offset;
        y -= offset;

        int iX = Mathf.RoundToInt(x);
        int iY = Mathf.RoundToInt(y);
        int iZ = Mathf.RoundToInt(-x - y); // x + y + z = 0

        // dealing with rounding errors
        if (iX + iY + iZ != 0)
        {
            float dX = Mathf.Abs(x - iX);
            float dY = Mathf.Abs(y - iY);
            float dZ = Mathf.Abs(-x - y - iZ);

            if (dX > dY && dX > dZ)
            {
                iX = -iY - iZ;
            }
            else if (dZ > dY)
            {
                iZ = -iX - iY;
            }
        }

        return new HexCoordinates(iX, iZ);
    }

    public override string ToString()
    {
        return "(" + X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + ")";
    }

    public string ToStringOnSeparateLines()
    {
        return X.ToString() + "\n" + Y.ToString() + "\n" + Z.ToString();

    }
}
