using UnityEngine;

//http://www.redblobgames.com/grids/hexagons/
public class HexUtils {

    public enum HexDirection {
        UpRight,
        Right,
        DownRight,
        DownLeft,
        Left,
        UpLeft,
        None
    }

    public enum HexDiagonal {
        Up,
        UpRight,
        DownRight,
        Down,
        DownLeft,
        UpLeft,
        None
    }

    public static Vector2 GetValueFromHexDir (HexDirection dir) {
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

    public static Vector3 GetValueFromHexDiagonal (HexDiagonal dig) {
        switch (dig) {
            case HexDiagonal.Up:
            return new Vector3(1, 1, -2);

            case HexDiagonal.UpRight:
            return new Vector3(-1, 2, -1); // Up

            case HexDiagonal.DownRight:
            return new Vector3(-2, 1, 1);  // Upleft

            case HexDiagonal.Down:
            return new Vector3(-1, -1, 2); // DownLeft

            case HexDiagonal.DownLeft:
            return new Vector3(1, -2, 1);

            case HexDiagonal.UpLeft:
            return new Vector3(2, -1, -1);

            case HexDiagonal.None:
            return new Vector3(0, 0, 0);

            default:
            return new Vector3(0, 0, 0);
        }
    }

    public static Vector2 CubeToHex (int x, int y) {
        int q = x;
        int r = y;
        return new Vector2(q, r);
    }

    public static Vector3 HexToCube (int q, int r) {
        int x = q;
        int y = r;
        int z = -x - y;
        return new Vector3(x, y, z);
    }

    public static Vector3 OffsetToCube (int row, int col) {
        int x = col - (row - (row & 1)) / 2;
        int y = row;
        int z = -x - y;
        return new Vector3(x, y, z);
    }

    public static Vector2 CubeToOffset (Vector3 cube) {
        int col = (int)(cube.x + (cube.y - ((int)cube.y&1)) * 0.5f);
        int row = (int)cube.y;
        return new Vector2(col, row);
    }

}
