using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class CFoulManager : MonoBehaviour
{

    public Text _foulText;	
	public GameObject _foulPanel;
	public GameObject _distPanel;
    public static CFoulManager instance = null;
	
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            InitGame();
        }
    }
	
	private void Start() {
		_foulPanel.SetActive(false);
		_distPanel.SetActive(false);
	}
	
    private void InitGame()
    {

    }
	public void BaseBallScript()
	{
		
	}
	public void UpdateFoulChk(string _text)
	{
		_foulPanel.SetActive(true);
		_distPanel.SetActive(false);
		_foulText.text = _text;

        StartCoroutine(InitCanvas());
		
	}

    private IEnumerator InitCanvas()
    {
        yield return new WaitForSeconds(2.0f);
        _foulPanel.SetActive(false);
        // _distPanel.SetActive(false);
    }
}
