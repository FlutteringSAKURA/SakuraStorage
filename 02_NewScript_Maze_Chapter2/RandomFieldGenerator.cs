using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.10.13

// NOTE: //# 3D 게임 - 자동 미로생성 프로그램
//#          1) MAZE_LINE_X, MAZE_LINE_Y로 지정한 크기의 미로를 자동으로 생성하는 프로그램
//#          2) 
//#          3) 
//#          4) 
//#          5) 

//~ ---------------------------------------------------------
public class RandomFieldGenerator : MonoBehaviour
{
    public GameObject m_blockObject = null;     //^ 미로를 구성하는 블록 오브젝트
    public GameObject m_playerObject = null;    //^ 조작할 플레이어 캐릭터
    public GameObject m_creatureObject = null;  //^ 에너미

    public GameObject m_goalObject = null;      //^ 도착지점 오브젝트
    public GameObject m_targetObject = null;    //^ 파괴할 타겟 오브젝트
    public GameObject m_stageClearObject = null;    //^ 스테이지 종료할 때 생성할 오브젝트

    public bool m_createAtOnce = true;      //^ true이면 미로를 한꺼번에 생성.. false이면 생성과정을 보여준다

    //! 스테이지 종류
    public enum StageClear
    {
        Goal,       //& 도착 지점을 목표로 한다.
        Target      //& 타겟을 모두 부준사
    }
    public StageClear m_StageClear = StageClear.Goal;

    enum CheckDirection     //! 검사 방향
    {
        Left, Up, Right, Down, EnumMax, None = -1
    }
    enum CheckData
    {
        X, Y, EnumMax
    }

    private static readonly int[][] CHECK_DIR_LIST = new int[(int)CheckDirection.EnumMax][] //! 검사 방향
    {
        new int[(int)CheckData.EnumMax]{-1, 0},
        new int[(int)CheckData.EnumMax]{0,-1},
        new int[(int)CheckData.EnumMax]{1,0},
        new int[(int)CheckData.EnumMax]{0,1}
    };

    private static readonly CheckDirection[] REVERSE_DIR_LIST = new CheckDirection[(int)CheckDirection.EnumMax]
    {   //! 검사 반대 방향 (=반시계)
        CheckDirection.Right,
        CheckDirection.Down,
        CheckDirection.Left,
        CheckDirection.Up
    };

    private static readonly CheckDirection[] CHECK_ORDER_LIST = new CheckDirection[(int)CheckDirection.EnumMax]
    {   //! 검사할 순서
        CheckDirection.Up,
        CheckDirection.Down,
        CheckDirection.Left,
        CheckDirection.Right
    };

    private static readonly int MAZE_LINE_X = 8;    //& 미로의 X통로 갯수
    private static readonly int MAZE_LINE_Y = 8;    //& 미로의 Y통로 갯수
    private static readonly int MAZE_GRID_X = ((MAZE_GRID_X * 2) + 1);    //& 미로의 X배열 갯수
    private static readonly int MAZE_GRID_Y = ((MAZE_GRID_Y * 2) + 1);    //& 미로의 Y배열 갯수
    private static readonly int EXEC_MAZE_COUNT_MAX = (MAZE_LINE_X * MAZE_LINE_Y / 2);     //& 블록을 하나씩 생성할 때 수행 횟수
    private static readonly float MAZE_BLOCK_SCALE = 2.0f;      //& 미로 블록 하나의 스캐일
    private static readonly int TARGET_NUM = 10;        //& 파괴할 타겟 갯수

    private bool[][] m_mazeGrid = null;       //^ 미로 배열
    private GameObject m_blockParent = null;    //^ 미로 블록을 넣어둘 부모
    private int m_makeMazeCounter = 0;      //^  블록을 하나씩 생성할 때 사용하는 카운터
    private bool m_stageClearedFlag = false;     //^ 스테이지 종료 오브젝트 생성하면 true


    //~ ----------------------------------------------------------

    private void Awake()
    {
        //# 미로 초기화
        InititalizeMaze();
        //# 미로를 한꺼번에 만들지 여부를 검사 
        if (m_createAtOnce)
        {
            //# 상하 좌우 가장자리에서 중심을 향해 가지를 뻗어 미로 생성
            //int i;
            for (int i = 0; i < EXEC_MAZE_COUNT_MAX; i++)
            {
                ExecMaze();
            }

            //# 미로 생성
            GenerateMaze();
        }
        //# 플레이어 생성
        CreatePlayer();


        //# 게임 종료의 종류에 따른 처리
        switch (m_StageClear)
        {
            case StageClear.Goal:
                CreateGoal();
                CreateCreature();
                break;

            case StageClear.Target:
                CreateTarget();
                break;
        }

    }


    // Explain: //~ 미로를 초기화 한다. -> 배열 변수를 초기화해 외벽과 기둥을 만든다
    private void InititalizeMaze()
    {
        //& 처음에 bool 배열을 만든다. (이것이 true일 때, 블록 배치)
        //^ 왼쪽 배열 선언
        m_mazeGrid = new bool[MAZE_GRID_X][];
        //^ 루프 사용해 오른쪽 배열 선언
        int gridX;
        int gridY;
        for (gridX = 0; gridX < MAZE_GRID_X; gridX++)
        {
            m_mazeGrid[gridX] = new bool[MAZE_GRID_Y];
        }

        //# 처음부터 블록을 놓도록 정해진 장소를 블록으로 채워 넣기
        bool blockFlag;
        for (gridX = 0; gridX < MAZE_GRID_X; gridX++)
        {
            for (gridY = 0; gridY < MAZE_GRID_Y; gridY++)
            {
                //& blockFlag가 ture일 때, 이 위치는 블록을 놓아도 됨.
                blockFlag = false; 
                //# 왼쪽끝, 위쪽끝, 오른쪽끝, 아래쪽끝 .. 오른쪽끝과 아래쪽끝은 인덱스 값이므로 -1해준다.
                if ((0 == gridX) || (0 == gridY) || ((MAZE_GRID_X - 1) == gridX)|| ((MAZE_GRID_Y - 1) == gridY))
                {
                    blockFlag = true;
                }
                else if((0==(gridX %2))&&(0==(gridY %2)))
                {
                    // NOTE: //! %는 잉여 인산자. 나눈 나머지 값 구하는 것
                             //! X, Y가 모두 짝수일 때는 기둥
                    blockFlag = true;
                }
                
                //# 값 대입
                m_mazeGrid[gridX][gridY] = blockFlag;
            }
        }
    }

    private void CreateTarget()
    {
        throw new NotImplementedException();
    }

    private void CreateCreature()
    {
        throw new NotImplementedException();
    }

    private void CreateGoal()
    {
        throw new NotImplementedException();
    }

    private void GenerateMaze()
    {
        throw new NotImplementedException();
    }

    //# 미로를 하나씩 생성
    private void ExecMaze()
    {
        throw new NotImplementedException();
    }

    private void CreatePlayer()
    {
        throw new NotImplementedException();
    }
    //~ ----------------------------------------------------------


    private void Update()
    {
        //# 미로를 한꺼번에 만들지 않을 때의 처리
        if (false == m_createAtOnce)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //^ 생성
                ExecMaze();
                //^ 미로 업데이트
                GenerateMaze();

            }
        }

        //# 스테이지 종료를 확인
        if (false == m_stageClearedFlag)
        {
            //% 스테이지를 클리어한 상태라면 플래그를 True로 지정
            if (GameManagerSrc.IsStageCleared())
            {
                //% 스테이지를 클리어 표시를 생성
                CreateStageClear();

                //% 플래그를 True로 지정
                m_stageClearedFlag = true;
            }
        }
    }

    //~  ----------------------------------------------------------

    private void CreateStageClear()
    {
        throw new NotImplementedException();
    }
}
