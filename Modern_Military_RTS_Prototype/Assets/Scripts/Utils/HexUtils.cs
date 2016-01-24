using UnityEngine;

//http://www.redblobgames.com/grids/hexagons/
public class HexUtils
{

    public enum HexDirection
    {
        UpRight,
        Right,
        DownRight,
        DownLeft,
        Left,
        UpLeft,
        None
    }

    public enum HexDiagonal
    {
        Up,
        UpRight,
        DownRight,
        Down,
        DownLeft,
        UpLeft,
        None
    }

    public static Vector2 GetValueFromHexDir (HexDirection dir)
    {
        switch (dir) {
            case HexDirection.UpRight:
            return new Vector2(0, 1);

            case HexDirection.Right:
            return new Vector2(1, 0);

            case HexDirection.DownRight:
            return new Vector2(0, -1);

            case HexDirection.DownLeft:
            return new Vector2(-1, -1);

            case HexDirection.Left:
            return new Vector2(-1, 0);

            case HexDirection.UpLeft:
            return new Vector2(-1, 1);

            case HexDirection.None:
            return new Vector2(0, 0);

            default:
            return new Vector2(0, 0);
        }
    }

    public static Vector3 GetValueFromHexDiagonal (HexDiagonal dig)
    {
        switch (dig) {
            case HexDiagonal.Up:
            return new Vector3(-1, 2, -1);

            case HexDiagonal.UpRight:
            return new Vector3(1, 1, -2);

            case HexDiagonal.DownRight:
            return new Vector3(2, -1, -1);

            case HexDiagonal.Down:
            return new Vector3(1, -2, 1);

            case HexDiagonal.DownLeft:
            return new Vector3(-1, -1, 2);

            case HexDiagonal.UpLeft:
            return new Vector3(-2, 1, 1);

            case HexDiagonal.None:
            return new Vector3(0, 0, 0);

            default:
            return new Vector3(0, 0, 0);
        }
    }

    public static Vector2 CubeToHex (Vector3 h)
    {
        int q = (int) h.x;
        int r = (int) h.y;
        return new Vector2(q, r);
    }

    public static Vector3 HexToCube (int q, int r)
    {
        int x = q;
        int y = r;
        int z = -x - y;
        return new Vector3(x, y, z);
    }

    public static Vector3 OffsetToCube (int col, int row)
    {
        int x = (int)(col - (row - (row & 1)) * 0.5f);
        int y = row;
        int z = -x - y;
        return new Vector3(x, y, z);
    }

    public static Vector2 CubeToOffset (Vector3 cube)
    {
        int col = (int) (cube.x + (cube.y - ((int) cube.y & 1)) * 0.5f);
        int row = (int) cube.y;
        return new Vector2(col, row);
    }

    public static Vector3 CubeRound (Vector3 h)
    {
        int rx = Mathf.RoundToInt(h.x);
        int ry = Mathf.RoundToInt(h.y);
        int rz = Mathf.RoundToInt(h.z);

        float x_diff = Mathf.Abs(rx - h.x);
        float y_diff = Mathf.Abs(ry - h.y);
        float z_diff = Mathf.Abs(rz - h.z);

        if (x_diff > y_diff && x_diff > z_diff) {
            rx = -ry - rz;
        } else if (y_diff > z_diff) {
            ry = -rx - rz;
        } else {
            rz = -rx - ry;
        }

        return new Vector3(rx, ry, rz);
    }

    public static Vector2 HexRound (int x, int y)
    {
        return CubeToHex(CubeRound(HexToCube(x, y)));
    }

    public static int CubeDistance (Vector3 a, Vector3 b)
    {
        return (int) ((Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) + Mathf.Abs(a.z - b.z)) * 0.5f);
    }

    public static Vector3 CubeLerp (Vector3 a, Vector3 b, float t)
    {
        return new Vector3(a.x + (b.x - a.x) * t,
                           a.y + (b.y - a.y) * t,
                           a.z + (b.z - a.z) * t);
    }

    public static Vector3[] CubeLinedraw (Vector3 a, Vector3 b)
    {
        int N = CubeDistance(a, b);
        Vector3[] results = new Vector3[N + 1];
        for (int i = 0; i < results.Length; i++) {
            results[i] = CubeRound(CubeLerp(a, b, 1.0f / N * i));
        }
        return results;
    }

}
