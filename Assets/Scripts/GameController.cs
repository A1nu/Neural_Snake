using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
	public int maxSize;
	public int CurrentSize;
	public int xBound;
	public int yBound;
	public int score;
	public GameObject foodPrefab;
	public GameObject currentFood;
	public GameObject snakePrefab;
	public Snake head;
	public Snake tail;
	public int NESW;
	public Vector2 nextPos;
	public Text scoreText;
	public Transform BorderTop;
	public Transform BorderBottom;
	public Transform BorderLeft;
	public Transform BorderRight;

	void OnEnable()
	{
		Snake.hit += hit;
	}
	
	// Use this for initialization
	void Start () {
		InvokeRepeating("TimerInvoke", 0, .5f );
		FoodFunction();
	}

	private void OnDisable()
	{
		Snake.hit -= hit;
	}

	// Update is called once per frame
	void Update () {
		ComCHangeD();
	}

	void TimerInvoke()
	{
		Movement();
		if (CurrentSize >= maxSize)
		{
			TailFunction();
		}
		else
		{
			CurrentSize++;
		}
	}

	void Movement()
	{
		GameObject temp;
		nextPos = head.transform.position;

		switch (NESW)
		{
				 case 0: 
					 nextPos = new Vector2(nextPos.x, nextPos.y + 1);
					 break;
				 case 1:
					 nextPos = new Vector2(nextPos.x + 1, nextPos.y);
					 break;
				 case 2:
					 nextPos = new Vector2(nextPos.x, nextPos.y - 1);
					 break;
				 case 3:
					 nextPos = new Vector2(nextPos.x - 1, nextPos.y);
					 break;
		}

		temp = (GameObject) Instantiate(snakePrefab, nextPos, transform.rotation);
		head.SetNext(temp.GetComponent<Snake>());
		head = temp.GetComponent<Snake>();
	}

	void ComCHangeD()
	{
		if (NESW != 2 && Input.GetKeyDown(KeyCode.UpArrow ))
		{
			NESW = 0;
		}
		if (NESW != 3 && Input.GetKeyDown(KeyCode.RightArrow))
		{
			NESW = 1; 
		}
		if (NESW != 0 && Input.GetKeyDown(KeyCode.DownArrow ))
		{
			NESW = 2;
		}
		if (NESW != 1 && Input.GetKeyDown(KeyCode.LeftArrow ))
		{
			NESW = 3 ;
		}
	}

	void TailFunction()
	{
		Snake tempSnake = tail;
		tail = tail.GetNext();
		tempSnake.RemoveTail();
	}

	void FoodFunction()
	{
		int xPos = (int)Random.Range(BorderLeft.position.x, BorderRight.position.x);
		int yPos = (int)Random.Range(BorderBottom.position.y, BorderTop.position.y);

		currentFood = (GameObject) Instantiate(foodPrefab, new Vector2(xPos, yPos), transform.rotation);

		StartCoroutine(CheckRender(currentFood));
	}

	IEnumerator CheckRender(GameObject IN)
	{
		yield return new WaitForEndOfFrame();
		if (IN.GetComponent<Renderer>().isVisible == false)
		{
			if (IN.tag == "Food")
			{
				Destroy(IN);
				FoodFunction();
			}
		}
	}

	void hit(String WhatWasSet)
	{
		if (WhatWasSet == "Food")
		{
			FoodFunction();
			maxSize++;
			score++;
			scoreText.text = score.ToString();
			int temp = PlayerPrefs.GetInt("HighScore");
			if (score > temp)
			{
				PlayerPrefs.SetInt("HighScore", score);
			}
		}
		else
		{
			CancelInvoke("TimerInvoke");
			Exit();
		}
	}

	public void Exit()
	{
		SceneManager.LoadScene(0);
	}
}
