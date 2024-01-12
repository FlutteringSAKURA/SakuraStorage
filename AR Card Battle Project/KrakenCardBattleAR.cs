using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Vuforia
using Vuforia;

// NOTE://# this is the script about management game play
//* 1.Making State Enum
public enum KrakenGameState
{
    Ready = 0,
    Battle,
    Result
}
//[ExecuteInEditMode]
public class KrakenCardBattleAR : MonoBehaviour
{
    public TrackingObject obj_kurona;
    public TrackingObject obj_zombiegirl;
    //@ 1.making state enum
    public KrakenGameState gameState = KrakenGameState.Ready;
    //@ 4.System message
    public string krknSystem_message = "";

    //@ 8.Animation
    public KrakenAnimationMgr kuronaAnimation;
    public KrakenAnimationMgr zombieAnimation;

    // Update: who attack first bool chk
    public bool kuronaTurnFirst = false;
    public bool zombiegirlTurnFirst = false;

    //! TEST: Custom GUI SKIN
    public GUISkin krakenGUI;
    public bool debugMode = false;

    //! TEST: restet position
    public Transform zombiegirlResetTr;
    public Transform zombieGirlCardTr;
    //public Transform kuranaTr;

    private void OnGUI()
    {
        if (debugMode || Application.isPlayer)
        {
            // TEST: Tracking & Showing Button
            // GUIStyle guiStyle = new GUIStyle("Button");
            // guiStyle.fontSize = 40;
            // if (obj_kurona.is_detected)
            // {
            // 	GUI.Button(new Rect(300,300,440,120), "Kurona chan - Ready", guiStyle);
            // }
            // if (obj_zombiegirl.is_detected)
            // {
            // 	GUI.Button(new Rect(800,300,440,120), "Zombie Girl - Ready", guiStyle);
            // }
            //! TEST: 
            GUI.skin = krakenGUI;
            // Make a button. This will get the default "button" style from the skin assigned to mySkin.
            // GUI.Label (new Rect (10,10,150,20), "Skinned Button");


            //@ 2.Output State
            GUIStyle gui_style = new GUIStyle();
            gui_style.fontSize = 60;
            gui_style.normal.textColor = Color.yellow;

            //@ 4.delete for output system message
            // GUI.Label(new Rect(400, 150, 200, 60), "State : "
            // + gameState.ToString(), gui_style);

            //@ 2-1.Generate Start Battle button when tracking object
            GUIStyle gui_style_btn = new GUIStyle("Button");
            gui_style_btn.fontSize = 50;

            if (obj_kurona.is_detected && obj_zombiegirl.is_detected &&
            gameState == KrakenGameState.Ready)
            {
                if (GUI.Button(new Rect(500, 500, 350, 150), "Start Battle", gui_style_btn))
                {
                    gameState = KrakenGameState.Battle;
                    //@ 4.System message
                    krknSystem_message = "주사위로 <color=red>선공</color> 정하기";
                    //@ 3.Set first attack using Rolling dice(high number)
                    StartCoroutine(RollTheDices());
                }
            }
            if (gameState == KrakenGameState.Ready)
            {
                krknSystem_message = "<color=red>[게임 준비중]</color> 카드를 인식 시켜주세요.";
            }
            GUI.Label(new Rect(400, 150, 800, 60), krknSystem_message);
            // GUI.Label(new Rect(400, 150, 200, 60), "Skull");

            //@ 7.Re-start(initiate)
            if (gameState == KrakenGameState.Result)
            {
                if (GUI.Button(new Rect(500, 500, 350, 150), "Re-Start", gui_style_btn))
                {
                    gameState = KrakenGameState.Ready;
                    // //@ Initiate Animation
                    kuronaAnimation.IdleKurona();
                    zombieAnimation.IdleZombieGirl();

                    //! Update: Trun reset

                    kuronaTurnFirst = false;
                    zombiegirlTurnFirst = false;
                    //! TEST: reset position
                    zombiegirlResetTr.SetParent(zombieGirlCardTr);
                    zombieGirlCardTr.localPosition = Vector3.zero;
                    zombiegirlResetTr.localPosition = Vector3.zero;
                    zombiegirlResetTr.localPosition = zombieGirlCardTr.localPosition;
                    zombiegirlResetTr.localEulerAngles = new Vector3(0,0,0);
                    //zombiegirlResetTr.LookAt(kuranaTr);
                    //@ 7-1 Initiate Info
                    obj_kurona.obj_text_mesh.text = obj_kurona._name + "<color=Green> \n HP :</color> " + "<color=yellow>"+ obj_kurona._hp +"</color>";
                    obj_zombiegirl.obj_text_mesh.text = obj_zombiegirl._name + "<color=green> \n HP :</color> " + "<color=yellow>" + obj_zombiegirl._hp + "</color>";
                }
            }
        }
    }

    private IEnumerator RollTheDices()
    {
        //@ 3.counting random number 1~6 per 0.1sec (loof 30 times)
        int last_kurona_dice = 0;
        int last_zombiegirl_dice = 0;

        for (int i = 0; i < 30; i++)
        {
            last_kurona_dice = 1 + Random.Range(0, 6);
            last_zombiegirl_dice = 1 + Random.Range(0, 6);

            obj_kurona.obj_text_mesh.text = "<color=blue>주사위 :</color> " + last_kurona_dice;
            obj_zombiegirl.obj_text_mesh.text = "<color=red>주사위 :</color> " + last_zombiegirl_dice;
            yield return new WaitForSeconds(0.1f);
        }
        //@ 5.Judgement who has high dices number and win or lose
        if (last_kurona_dice > last_zombiegirl_dice)
        {
            kuronaTurnFirst = true;
            krknSystem_message = "<color=blue>Kurona chan</color> Attack First!!!";
            //@ 6.Start battle
            StartCoroutine(StartBattle(obj_kurona, obj_zombiegirl));
        }
        else if (last_kurona_dice < last_zombiegirl_dice)
        {
            zombiegirlTurnFirst = true;
            krknSystem_message = "<color=red>Zombie Girl</color> Attack First!!!";
            //@ 6.start battle
            StartCoroutine(StartBattle(obj_zombiegirl, obj_kurona));
        }
        else// if(last_kurona_dice == last_zombiegirl_dice)
        {
            krknSystem_message = "<color=yellow>무승부</color> - 주사위 다시 굴리기";
            StartCoroutine(RollTheDices());
        }

    }

    //! NOTE: Battle START!
    private IEnumerator StartBattle(TrackingObject first_turn, TrackingObject second_turn)
    {
        //@ 6-1.start battle (detail)
        yield return new WaitForSeconds(1.0f);

        float first_hp = first_turn._hp;
        float second_hp = second_turn._hp;

        //@ 6-2 renew hp info
        first_turn.obj_text_mesh.text = first_turn._name + "<color=green> \n HP :</color> " + "<color=yellow>" + first_hp + "</Color>";
        second_turn.obj_text_mesh.text = second_turn._name + "<color=green> \n  HP :</color>" + "<color=yellow>"+ second_hp + "</color>";
        //@ 6-3 play (tern based)
        while (true)
        {
            //# NOTE: first atk(turn)
            //@ 8.Animation Update: who attack first turn
            if (kuronaTurnFirst == true)
            {
                kuronaAnimation.PlayKuronaComboAtk();
                yield return new WaitForSeconds(kuronaAnimation.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length);
                zombieAnimation.ZombieGirlHit();
                kuronaAnimation.StopKuronaComboAtk();

            }
            if (zombiegirlTurnFirst == true)
            {
                zombieAnimation.PlayZombieGrilAtk();
                yield return new WaitForSeconds(zombieAnimation.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length);
                kuronaAnimation.HitKurona();
                zombieAnimation.StopZombieGirlAtk();
            }

            second_hp -= first_turn._atk;
            //# renew hp info
            first_turn.obj_text_mesh.text = first_turn._name + "<color=green> \n HP :</color> " + "<color=yellow>" + first_hp + "</color>"; 
            second_turn.obj_text_mesh.text = second_turn._name + "<color=green> \n HP : </color>" + "<color=yellow>" + second_hp + "</color>";
            //# seond turn lose
            if (second_hp <= 0.0f)
            {
                krknSystem_message = first_turn._name + " 가 <color=yellow>[승리]</color> 하였습니다.";
                //@ 8.Animation
                zombieAnimation.DieZombieGirl();
                //@ 8.Animation
                kuronaAnimation.KuronaWin();
                break;
            }
            yield return new WaitForSeconds(1.0f);

            //# NOTE: second atk (turn)
            //@ 8.Animation Update: who attack first trun
            if (zombiegirlTurnFirst == false)
            {
                zombieAnimation.PlayZombieGrilAtk();
                yield return new WaitForSeconds(zombieAnimation.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length);
                kuronaAnimation.HitKurona();
                zombieAnimation.StopZombieGirlAtk();
            }

            if (kuronaTurnFirst == false)
            {
                kuronaAnimation.PlayKuronaComboAtk();
                yield return new WaitForSeconds(kuronaAnimation.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length);
                zombieAnimation.ZombieGirlHit();
                kuronaAnimation.StopKuronaComboAtk();

            }

            first_hp -= second_turn._atk;
            //# renew hp info
            first_turn.obj_text_mesh.text = first_turn._name + "<color=green> \n HP :</color> " + "<color=yellow>" + first_hp + "</color>";
            second_turn.obj_text_mesh.text = second_turn._name + "<color=green> \n HP : </color>" + "<color=yellow>" + second_hp + "</color>";
            //# first turn lose
            if (first_hp <= 0)
            {
                krknSystem_message = second_turn._name + " 가 <color=yellow>[승리]</color> 하였습니다.";
                zombieAnimation.DieZombieGirl();
                kuronaAnimation.KuronaWin();
                break;
            }
            yield return new WaitForSeconds(1.0f);
        }
        //@ 7-2 result and initiate
        gameState = KrakenGameState.Result;
    }
}
