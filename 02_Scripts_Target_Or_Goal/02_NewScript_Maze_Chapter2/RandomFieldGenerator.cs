//using System;
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
        new int[(int)CheckData.EnumMax]{0, -1},
        new int[(int)CheckData.EnumMax]{1, 0},
        new int[(int)CheckData.EnumMax]{0, 1}
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
    private static readonly int MAZE_GRID_X = ((MAZE_LINE_X * 2) + 1);    //& 미로의 X배열 갯수
    private static readonly int MAZE_GRID_Y = ((MAZE_LINE_Y * 2) + 1);    //& 미로의 Y배열 갯수
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

    //~ ----------------------------------------------------------
    private void Update()
    {
        //# 미로를 한꺼번에 만들지 않을 때의 처리
        if (!m_createAtOnce)
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
        if (!m_stageClearedFlag)
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


    //@ 미로를 초기화 한다. -> 배열 변수를 초기화해 외벽과 기둥을 만든다
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
                if ((0 == gridX) || (0 == gridY) || ((MAZE_GRID_X - 1) == gridX) || ((MAZE_GRID_Y - 1) == gridY))
                {
                    blockFlag = true;
                }
                else if ((0 == (gridX % 2)) && (0 == (gridY % 2)))
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

    //@ 미로를 하나씩 생성
    private void ExecMaze()
    {
        //& 미로 생성이 완료됐다 = 방어코드
        if (m_makeMazeCounter >= EXEC_MAZE_COUNT_MAX)
        {
            return;
        }

        //& 이번에 생성할 것은 이 번호 블록부터 검사를 시작
        int counter = m_makeMazeCounter;
        //& 카운트 +1
        m_makeMazeCounter++;

        //^ x와 y라인 수 중에서 큰 쪽 입력
        int _lineMax;
        //^ 검사 시작 위치
        int _start1, _start2;

        int gridX_A = 0;
        int gridY_A = 0;
        int gridX_B, gridY_B;
        int gridX_C, gridY_C;

        //% 검사 방향
        CheckDirection checkDirNow;
        //% 한 개 이전의 검사 방향
        CheckDirection checkDirNG;

        //$ 라인의 최대값을 얻기
        _lineMax = Mathf.Max(MAZE_LINE_X, MAZE_LINE_Y);

        //$ 검사 시작 위치 (블록 한 개씩 건너서 검사하므로 2를 곱하기)
        _start1 = ((counter / _lineMax) * 2);
        _start2 = ((counter % _lineMax) * 2);

        //# 상하 좌우 끝에서 한 개씩 가지를 뻗어 벽을 생성해 가는 코드
        int i;
        for (i = 0; i < (int)CheckDirection.EnumMax; i++)
        {
            //& 지금 검사하는 것의 방향
            checkDirNow = CHECK_ORDER_LIST[i];
            //& 어느 쪽 끝에서 어느 방향으로 가지를 늘릴지 정하기
            switch (checkDirNow)
            {
                //^ 왼쪽으로 가지 늘림 (오른쪽 끝에서 시작)
                case CheckDirection.Left:
                    gridX_A = ((MAZE_GRID_X - 1) - _start1);        //@ 가로축은 1을 X에 넣는다.
                    gridY_A = ((MAZE_GRID_Y - 1) - _start2);       //@ 2는 Y축이다.
                    break;

                //^ 위로 가지를 늘림 (아래쪽 끝에서 시작)
                case CheckDirection.Up:
                    gridX_A = ((MAZE_GRID_X - 1) - _start2);     //@ 세로축은 2를 X에 넣는다.
                    gridY_A = ((MAZE_GRID_Y - 1) - _start1);      //@ 1은 Y축이다.
                    break;

                //^ 오른쪽으로 가지를 늘림 (왼쪽 끝에서 시작)
                case CheckDirection.Right:
                    gridX_A = (_start1);
                    gridY_A = (_start2);
                    break;

                //^ 아래쪽으로 가지를 늘림 (위쪽 끝에서 시작)
                case CheckDirection.Down:
                    gridX_A = (_start2);
                    gridY_A = (_start1);
                    break;

                //^ default에 경고를 넣어두면 조기에 버그를 검출할 수 있어 편리하다
                default:
                    Debug.LogError("존재하지 않는 방향(" + checkDirNow + ")");
                    // 일단 의미 없는 값을 넣어줌
                    gridX_A = -1;
                    gridY_A = -1;
                    break;
            }
            //# 장외 검사
            if ((gridX_A < 0) || (gridX_A >= MAZE_GRID_X) || (gridY_A < 0) || (gridY_A >= MAZE_GRID_Y))
            {
                //& 여기는 조사할 블록이 없음
                continue;
            }

            //# 벽이 있는 기둥에 부딪힐 때까지 무한 루프
            for ( ; ; )
            {
                //& 체크할 기둥 위치(시작위치에서 두 개 옆에 있는 블록)
                gridX_B = gridX_A + (CHECK_DIR_LIST[(int)checkDirNow][(int)CheckData.X] * 2);
                gridY_B = gridY_A + (CHECK_DIR_LIST[(int)checkDirNow][(int)CheckData.Y] * 2);

                //& 임의의 블록 주변을 살펴보고 다른 블록과 연결되어 있는지 확인
                if (IsConnectedBlock(gridX_B, gridY_B))
                {
                    //^ 이미 무언가 연결되어 있으면 작업 중단.
                    break;
                }

                //% 시작 위치와 체크 위치 사이의 위치에 블록 넣기
                gridX_C = gridX_A + CHECK_DIR_LIST[(int)checkDirNow][(int)CheckData.X];
                gridY_C = gridY_A + CHECK_DIR_LIST[(int)checkDirNow][(int)CheckData.Y];

                //% 블록 배치
                SetBlock(gridX_C, gridY_C, true);

                //& 다음은 연결한 기둥부터 검색 시작
                gridX_A = gridX_B;
                gridY_A = gridY_B;

                //& 다음부터는 이쪽으로 오면 안됨
                checkDirNG = REVERSE_DIR_LIST[(int)checkDirNow];

                //& 다음에 조사할 기둥 무작위 선택
                checkDirNow = CHECK_ORDER_LIST[Random.Range(0, (int)CheckDirection.EnumMax)];

                //& 한번 이전 위치로 되돌아가지 않도록 진행 방향 검사
                if (checkDirNow == checkDirNG)
                {
                    //^ 돌아가려고 하면 반대쪽을 향하게 함
                    checkDirNow = REVERSE_DIR_LIST[(int)checkDirNow];
                }
            }
        }
    }

    //@ 지정된 위치에 블록이 존재하는지 확인
    private void SetBlock(int gridX, int gridY, bool blockFlag)
    {
        m_mazeGrid[gridX][gridY] = blockFlag;
    }
    //@ 지정된 위치에 블록이 존재하는지 확인 .. 블록이 존재하면 true 반환
    private bool IsBlock(int gridX, int gridY)
    {
        return m_mazeGrid[gridX][gridY];
    }

    private bool IsConnectedBlock(int gridX, int gridY)
    {
        //^ 어떤 것에 연결되어 있으면 true
        bool _connectedFlag = false;

        //^ 검사할 X위치와 Y위치    
        int _checkX, _checkY;

        //% 루프돌려 확인
        int i;
        for (i = 0; i < (int)CheckDirection.EnumMax; i++)
        {
            //& 조사할 블록 위치
            _checkX = (gridX + CHECK_DIR_LIST[i][(int)CheckData.X]);
            _checkY = (gridY + CHECK_DIR_LIST[i][(int)CheckData.Y]);

            //& 장외 검사
            if ((_checkX < 0) || (_checkX >= MAZE_GRID_X) || (_checkY < 0) || (_checkY >= MAZE_GRID_Y))
            {
                //^ 조사할 블럭 없음
                continue;
            }

            //& 이미 블록이 있는지 검사
            if (IsBlock(_checkX, _checkY))
            {
                //^ 블록이 있음
                _connectedFlag = true;
                //^ 바로 종료
                break;
            }
        }
        //! 밖으로 결과 뱉어줌
        return _connectedFlag;
    }

    //@ 타겟 생성
    private void CreateTarget()
    {
        Vector3 _targetPos;
        for (int i = 0; i < TARGET_NUM; i++)
        {
            //% 타겟 생성을 위한 랜덤 장소 설정
            _targetPos = new Vector3((Random.Range(0, MAZE_LINE_X) * 2) + 1, 0, (Random.Range(0, MAZE_LINE_Y) * 2) + 1) * MAZE_BLOCK_SCALE;
            //% 타겟 생성
            Instantiate(m_targetObject, _targetPos, Quaternion.identity);
        }
    }

    //@ 크리처를 생성
    private void CreateCreature()
    {
        Vector3 _creaturePos = new Vector3((MAZE_GRID_X - 2), 0, (MAZE_GRID_Y - 2)) * MAZE_BLOCK_SCALE;
        Instantiate(m_creatureObject, _creaturePos, Quaternion.identity);
        Instantiate(m_creatureObject, new Vector3(2, 0, 15) * MAZE_BLOCK_SCALE, Quaternion.identity);
    }

    //@ 목표 지점 Hirerarchy에 생성
    private void CreateGoal()
    {
        //% 목표종료지점은 플레이어의 반대쪽 모서리에 배치
        Vector3 _goalPos = new Vector3((MAZE_GRID_X - 2), 0, (MAZE_GRID_Y - 2)) * MAZE_BLOCK_SCALE;
        //% 목표종료지점 생성
        Instantiate(m_goalObject, _goalPos, Quaternion.identity);
    }

    //@ 미로를 Hierarchy에 생성

    private void GenerateMaze()
    {
        //% 이미 블록의 부모가 있을경우 삭제
        if (m_blockParent)
        {
            //& 삭제
            Destroy(m_blockParent);
            //& null 넣어둠
            m_blockParent = null;
        }

        //% 블록의 부모 만듬
        m_blockParent = new GameObject();
        m_blockParent.name = "MazeBlockBox";
        m_blockParent.transform.parent = transform;

        //% 블록을 만듬
        GameObject _blockObject;
        Vector3 _blockPos;

        int gridX, gridY;
        for (gridX = 0; gridX < MAZE_GRID_X; gridX++)
        {
            for (gridY = 0; gridY < MAZE_GRID_Y; gridY++)
            {
                //& 블록여부 검사
                if (IsBlock(gridX, gridY))
                {
                    //^ 블록 생성 위치
                    _blockPos = new Vector3(gridX, 0, gridY) * MAZE_BLOCK_SCALE;
                    //^ 블록 생성
                    _blockObject = Instantiate(m_blockObject, _blockPos, Quaternion.identity) as GameObject;
                    //^ 이름 변경
                    _blockObject.name = "MazeBlock(" + gridX + "," + gridY + ")";
                    //^ 로컬 스케일 변경
                    _blockObject.transform.localScale = (Vector3.one * MAZE_BLOCK_SCALE);
                    //^ 앞서 생성한 무모 아래 넣음
                    _blockObject.transform.parent = m_blockParent.transform;
                }
            }
        }
    }

    //@ 플레이어를 Hierarchy에 생성
    private void CreatePlayer()
    {
        Instantiate(m_playerObject, new Vector3(1, 0, 1) * MAZE_BLOCK_SCALE, Quaternion.identity);
    }

    //@ 스테이지 클리어 표시를 Hierarchy에 생성
    private void CreateStageClear()
    {
        //% 스테이지 종료 오브젝트 생성
        Instantiate(m_stageClearObject, Vector3.zero, Quaternion.identity);
    }
    //~ ----------------------------------------------------------
}
