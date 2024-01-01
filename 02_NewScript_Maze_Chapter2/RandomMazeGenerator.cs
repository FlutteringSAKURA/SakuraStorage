using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.10.15

// NOTE: //# 3D 게임 - 자동 미로생성 프로그램
//#          1) MAZE_LINE_X, MAZE_LINE_Y로 지정한 크기의 미로를 자동으로 생성하는 프로그램
//#          2) 
//#          3) 
//#          4) 
//#          5) 

//~ ---------------------------------------------------------

public class RandomMazeGenerator : MonoBehaviour
{
    public GameObject _blockObject = null;
    public GameObject _playerObject = null;
    public GameObject _creatureObject = null;

    public static readonly int FIELD_GRID_X = 9;
    public static readonly int FIELD_GRID_Y = 9;
    public static readonly float BLOCK_SCALE = 2.0f;
    public static readonly Vector3 BLOCK_OFFSET = new Vector3(1, 1, 1);

    public enum ObjectKind
    {
        Empty, Blcok, Player1, Creature
    }

    public static readonly int[] GRID_OBJECT_DATA = new int[]
    {
        1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 2, 0, 0, 0, 0, 0, 0, 1,
        1, 
        1,
        1,
        1,
        1,
        1,
        1, 1, 1, 1, 1, 1, 1, 1, 1,

    };
}
