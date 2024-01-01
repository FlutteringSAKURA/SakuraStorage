using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//% XML 사용하기 위한 코드
using System.Xml;
//% C# 전용문법 사용하기 위한 코드 (구조체 사용하기 위함)
using System;

// Update: //@ 2023.10.30 

// NOTE: //# 3D 게임 - XML 데이터 관리 스크립트
//#          1)

//~ ------------------------------------------------------------------------
public class XmlManager : MonoBehaviour
{
    public static XmlManager instance;
    //& 바이너리 파일(ex. XML)들을 표현하기 위한..
    public TextAsset _dinosaurXmlFile;

    //#  NOTE: struct .. 여러개의 변수들을 넣어 이 (구조체)를 하나의 상자처럼 활용.
    //#         메소드라 불리는 함수의 기능이 빠진것.                
    struct Struct_DinosaurParams
    {
        public string _name;
        public int _level;
        public int _maxHp;
        public int _attackMinPower;
        public int _attackMaxPower;
        public int _defense;
        public int _exp;
        public int _rewardGold;
    }
    //% 딕셔너리 이용해서 객체 생성
    Dictionary<string, DinosaurParams> _dicDinosaur = new Dictionary<string, DinosaurParams>();

    //~ ------------------------------------------------------------------------
    private void Awake()
    {
        if (instance == null) instance = this;
    }
    //~ ------------------------------------------------------------------------
    private void Start()
    {
        MakeDinosaurXML();
    }
    //~ ------------------------------------------------------------------------

    //@ XML 데이터 파일로부터 데이터를 읽어들이는 함수.
    void MakeDinosaurXML()
    {
        XmlDocument _dinosaurXMLDoc = new XmlDocument();
        _dinosaurXMLDoc.LoadXml(_dinosaurXmlFile.text);
        //% XML파일안의 row 안에있는 데이터를 가져다 쓰는 코드
        XmlNodeList _dinosaurNodeList = _dinosaurXMLDoc.GetElementsByTagName("row");

        //% 배열(컬렉션) == in _dinosaurNodeList
        foreach (XmlNode dinoNode in _dinosaurNodeList)
        {
            //& 구조체라는 객체를 생성하는 코드
            DinosaurParams _dinoParams = new DinosaurParams();
            //& 부모노드(dinoNode)의 자식노드(ChildNode)를 탐색
            foreach (XmlNode childdNode in dinoNode.ChildNodes)
            {
                //^ 자식 노드의 Text데이터 값을 가져오는 코드
                if (childdNode.Name == "_name")
                    _dinoParams._name = childdNode.InnerText;
                //^ 문자열을 정수로 바꿔주는 코드
                if (childdNode.Name == "_level")
                    _dinoParams._level = Int16.Parse(childdNode.InnerText);     //# <_level>InnerText<\_level>
                if (childdNode.Name == "_maxHp")
                    _dinoParams._maxHp = Int16.Parse(childdNode.InnerText);
                if (childdNode.Name == "_attackMinPower")
                    _dinoParams._attackMinPower = Int16.Parse(childdNode.InnerText);
                if (childdNode.Name == "_attackMaxPower")
                    _dinoParams._attackMaxPower = Int16.Parse(childdNode.InnerText);
                if (childdNode.Name == "_defense")
                    _dinoParams._defense = Int16.Parse(childdNode.InnerText);
                if (childdNode.Name == "_exp")
                    _dinoParams._exp = Int16.Parse(childdNode.InnerText);
                if (childdNode.Name == "_rewardGold")
                    _dinoParams._rewardGold = Int16.Parse(childdNode.InnerText);

                ////Debug.Log("Parsing TEST -> " + childdNode.Name + " : " + childdNode.InnerText);
            }
            //& _dinoParams의 정보가 3개의 구조체에 담겨 딕셔너리(_dicDinosaur에 저장
            _dicDinosaur[_dinoParams._name] = _dinoParams;
        }
    }
    //% 외부로부터 전달받은 인자(공룡의 개별적 데이터와 객체==구조체)를 전달받아 적용
    //#  NOTE: class(DinosaurParams)명과 struct(Struct_DinosaurParams)의 명칭을 동일하게 하면 안된다.
    //#         >> class(DinosaurParams) .. struct(Struct_DinosaurParams) 달리 써줌
    public void LoadDinosaurParamsFromXML(string dinoName, DinosaurParams dinoPrams)
    {
        dinoPrams._level = _dicDinosaur[dinoName]._level;
        dinoPrams._currentHp = dinoPrams._maxHp = _dicDinosaur[dinoName]._maxHp;
        dinoPrams._attackMinPower = _dicDinosaur[dinoName]._attackMinPower;
        dinoPrams._attackMaxPower = _dicDinosaur[dinoName]._attackMaxPower;
        dinoPrams._defense = _dicDinosaur[dinoName]._defense;
        dinoPrams._exp = _dicDinosaur[dinoName]._exp;
        dinoPrams._rewardGold = _dicDinosaur[dinoName]._rewardGold;
    }
}
