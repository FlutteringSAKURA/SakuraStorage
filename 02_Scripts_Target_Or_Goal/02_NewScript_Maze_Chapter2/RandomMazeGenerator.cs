using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.10.15

//! TEST:
//  SUCCESS:
// NOTE: //# 3D 게임 - 자동 미로생성 프로그램 ..
//#          1) MAZE_LINE_X, MAZE_LINE_Y로 지정한 크기의 미로를 자동으로 생성하는 프로그램
//#          2) 
//#          3) 
//#          4) 
//#          5) 

//~ ---------------------------------------------------------

public class RandomMazeGenerator : MonoBehaviour
{
    public GameObject m_blockObject = null;
    public GameObject m_playerObject = null;
    public GameObject m_creatureObject = null;

    public static readonly int FIELD_GRID_X = 9;
    public static readonly int FIELD_GRID_Y = 9;
    public static readonly float BLOCK_SCALE = 2.0f;
    public static readonly Vector3 BLOCK_OFFSET = new Vector3(1, 1, 1);

    public enum ObjectKind
    {
        Empty, Block, Player1, Creature
    }

    public static readonly int[] GRID_OBJECT_DATA = new int[]
    {
        1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 2, 0, 0, 0, 0, 0, 0, 1,
        1, 0, 1, 1, 1, 0, 1, 0, 1,
        1, 0, 0, 0, 0, 0, 0, 0, 1,
        1, 0, 1, 0, 1, 1, 1, 0, 1,
        1, 0, 1, 0, 1, 0, 0, 0, 1,
        1, 0, 1, 0, 0, 0, 1, 0, 1,
        1, 0, 0, 0, 1, 0, 0, 3, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1,
        //! 배치할 때 위아래 뒤집히니 주의
    };

    private GameObject m_blockParent = null;

    //~ ---------------------------------------------------------
    private void Awake()
    {
        //# Maze초기화
        InitializeMaze();
    }

    private void InitializeMaze()
    {
        //& 블록 부모 생성
        m_blockParent = new GameObject();       //^ 부모 객세 생성 선언
        m_blockParent.name = "MazeBlockBox";    //^ 부모 이름 설정
        m_blockParent.transform.parent = transform;     //^ 위치좌표를 이 위치좌표로 치환

        //& 블록 생성
        GameObject m_originalObject;        //^ 생성할 블록의 기준 오브젝트
        GameObject m_instanceObject;        //^ 블록을 넣어두는 임시 변수
        Vector3 m_position;     //^ 블록 생성 위치

        //% 외곽과 안쪽에 기둥을 만들어 가는 코드
        int gridX, gridY;
        for (gridX = 0; gridX < FIELD_GRID_X; gridX++)
        {
            for (gridY = 0; gridY < FIELD_GRID_Y; gridY++)
            {
                //# 이 위치에 무엇을 넣을지 결정
                switch ((ObjectKind)GRID_OBJECT_DATA[gridX + (gridY * FIELD_GRID_X)])
                {
                    // & 벽
                    case ObjectKind.Block:
                        m_originalObject = m_blockObject;
                        break;

                    //& 플레이어
                    case ObjectKind.Player1:
                        m_originalObject = m_playerObject;
                        break;

                    //& 크리처
                    case ObjectKind.Creature:
                        m_originalObject = m_creatureObject;
                        break;

                    //& 그 외 모두 공백
                    default:
                        m_originalObject = null;
                        break;
                }
                //# 공백이라면 다음으로 넘어감
                if (null == m_originalObject)
                {
                    continue;
                }

                // NOTE://# 유니티에서는 XZ평면이 지평선임
                //# 블록 생성 위치
                m_position = new Vector3(gridX * BLOCK_SCALE, 0, gridY * BLOCK_SCALE) + BLOCK_OFFSET;

                //# 블록 생성 .. Instantiage(복사대상, 생성위치, 회전)
                m_instanceObject = Instantiate(m_originalObject, m_position, m_originalObject.transform.rotation) as GameObject;
                //# 생성되는 프리팹 이름 변경 .. (그리드 위치)를 표시
                m_instanceObject.name = "" + m_originalObject.name + "(" + gridX + "," + gridY + ")";
                //# 로컬 스케일을 변경
                m_instanceObject.transform.localScale = (Vector3.one * BLOCK_SCALE);
                //# m_instanceObject를 자식으로 넣는다
                m_instanceObject.transform.parent = m_blockParent.transform;
            }
        }

    }
}
