using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	//SerializeFields
	[SerializeField] private int startWait;
	[SerializeField] private int waveWait;
	[SerializeField] private int spawnWait;

	[SerializeField] GameObject hazard;
	[SerializeField] Vector3 spawnValues;
	[SerializeField] private int hazardCount;

	[SerializeField] Text scoreText;

	//Variables
	private int score;



	// Use this for initialization
	void Start () {
		StartCoroutine(SpawnWaves());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	 IEnumerator SpawnWaves(){
		yield return new WaitForSeconds(startWait);
		while(true){
				for (int i=0; i<hazardCount; i++){
				Vector3 spawnPosition = new Vector3 (Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate(hazard, spawnPosition, spawnRotation);
				yield return new WaitForSeconds(spawnWait);
		}
			yield return new WaitForSeconds(waveWait);
		}

	}

	public void AddScore(int newScoreValue){
		score += newScoreValue;
		scoreText.text = "Score: " + score;
	}
}
