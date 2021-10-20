using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TVMinigame : MonoBehaviour
{
    public delegate void GameEvent();
    public static event GameEvent GameQuittedEvent;
    public static event GameEvent GameWonEvent;


    private bool isInteracting = false;

    private const int UI_Layer = 5;
    private const float localScaleMultiplier = 0.015625f;
    private const float jumpHeight = 12f;
    private const float airResistance = 0.3f;

    private float startHeight;
    private float jumpSpeed;
    private int obstacleTimer;
    private const int obstacleDefaultDelay = 250;
    private int obstacleDelay = obstacleDefaultDelay;
    private int obstacleCount = 0;

    private int blinkingTextTimer = 50;
    private bool isGameStarted = false;
    private int score = 0;
    private bool isInAir = false;
    private float localJumpSpeed;

    [SerializeField] private Canvas canvas;
    [SerializeField] private Text hero;
    [SerializeField] private Text hint;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text leaderboard;


    List<GameObject> obstacles = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        TVInteractable.TVInteractedEvent += TVInteractedHandler;
        GameWonEvent += Win;
        GameQuittedEvent += QuitGame;
        startHeight = hero.GetComponent<RectTransform>().localPosition.y;
        jumpSpeed = -Convert.ToSingle(Math.Sqrt(Math.Abs(jumpHeight - startHeight)));
        obstacleTimer = obstacleDelay;
    }

    // Bounds is -50 / 50

    void FixedUpdate()
    {
        //  Exit game
        if (Input.GetKeyDown(KeyCode.Escape) && isInteracting)
        {
            GameQuittedEvent?.Invoke();
            isInteracting = false;

            return;
        }

        //  Blinking hint
        if (!isGameStarted)
        {
            if (Input.GetKeyDown(KeyCode.X) && isInteracting)
            {
                StartGame();
            }
            else if (blinkingTextTimer-- == 0)
            {
                hint.enabled = !hint.enabled;
                blinkingTextTimer = 50;
            }

            return;
        }

        //  Jump
        if (Input.GetKey(KeyCode.X) && !isInAir)
        {
            isInAir = true;
            localJumpSpeed = jumpSpeed;
        }
        if (isInAir)
        {
            hero.GetComponent<RectTransform>().localPosition = new Vector3(
                hero.GetComponent<RectTransform>().localPosition.x,
                jumpHeight - localJumpSpeed * localJumpSpeed,
                hero.GetComponent<RectTransform>().localPosition.z
                );

            localJumpSpeed += airResistance;

            if (hero.GetComponent<RectTransform>().localPosition.y <= startHeight && localJumpSpeed >= 0)
            {
                hero.GetComponent<RectTransform>().localPosition = new Vector3(
                hero.GetComponent<RectTransform>().localPosition.x,
                startHeight,
                hero.GetComponent<RectTransform>().localPosition.z
                );

                isInAir = false;
            }
        }


        //  Creating new obstacle
        obstacleTimer -= 1;

        if (obstacleTimer == 0 && obstacleCount < 10)
        {
            obstacleCount++;
            obstacles.Add(new GameObject("Obstacle"));
            GameObject gTemp = obstacles[obstacles.Count - 1];

            gTemp.layer = UI_Layer;
            gTemp.AddComponent<Text>();
            gTemp.GetComponent<Text>().text = "I";
            gTemp.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            gTemp.GetComponent<Text>().font = hero.font;
            gTemp.GetComponent<Text>().fontStyle = hero.fontStyle;
            gTemp.GetComponent<Text>().fontSize = 16;

            // Position: X, Y, Z
            gTemp.GetComponent<RectTransform>().position = new Vector3(
                50f * localScaleMultiplier,
                startHeight * localScaleMultiplier,
                hero.GetComponent<RectTransform>().position.z
                );

            // Width, Height
            gTemp.GetComponent<RectTransform>().sizeDelta = hero.GetComponent<RectTransform>().sizeDelta;

            // Scale: X, Y, Z
            gTemp.GetComponent<RectTransform>().localScale = new Vector3(
                hero.GetComponent<RectTransform>().localScale.x * localScaleMultiplier,
                hero.GetComponent<RectTransform>().localScale.y * localScaleMultiplier,
                hero.GetComponent<RectTransform>().localScale.z * localScaleMultiplier);


            gTemp.transform.SetParent(canvas.transform);

            System.Random rand = new System.Random();
            obstacleTimer = rand.Next(obstacleDelay / 2, obstacleDelay);
            obstacleDelay -= 20;
        }

        //  Moving obstacles and death check 
        int toDestroy = -1, i = 0;
        foreach (GameObject goTemp in obstacles)
        {
            goTemp.transform.localPosition = new Vector3(
                goTemp.transform.localPosition.x - 0.5f,
                goTemp.transform.localPosition.y,
                goTemp.transform.localPosition.z
                );
            //  death check
            if (goTemp.transform.localPosition.x <= hero.transform.localPosition.x + 5f && goTemp.transform.localPosition.x >= hero.transform.localPosition.x - 5f && hero.transform.localPosition.y <= startHeight + 8f)
            {
                Restart();
                return;
            }


            if (goTemp.transform.localPosition.x <= -50f)
            {
                toDestroy = i;
            }

            i++;
        }

        //  Destroying obstacles that are off screen
        if (toDestroy != -1)
        {
            Destroy(obstacles[toDestroy]);
            obstacles.RemoveAt(toDestroy);
            score++;
            scoreText.text = score + "/10";
        }


        //  Win check
        if(score == 10)
        {
            GameWonEvent?.Invoke();
        }
    }

    public void TVInteractedHandler()
    {
        isInteracting = true;
    }

    public void StartGame()
    {
        isGameStarted = true;
        GameProgression.SetIsInMinigame(true);
        hint.enabled = false;
    }

    public void QuitGame()
    {
        isInteracting = false;
        isGameStarted = false;
        GameProgression.SetIsInMinigame(false);
        hint.enabled = true;
        score = 0;
        scoreText.text = score + "/10";

        obstacleDelay = obstacleDefaultDelay;
        obstacleTimer = obstacleDelay;
        obstacleCount = 0;

        for (int i = obstacles.Count - 1; i >= 0 ; i--)
        {
            Destroy(obstacles[i]);
            obstacles.RemoveAt(i);
        }
        Debug.Log("OBS = " + obstacles.Count + " score = " + score + " obstacleCount = " + obstacleCount);

    }

    public void Restart()
    {
        score = 0;
        scoreText.text = score + "/10";

        obstacleDelay = obstacleDefaultDelay;
        obstacleTimer = obstacleDelay;
        obstacleCount = 0;

        for (int i = obstacles.Count - 1; i >= 0; i--)
        {
            Destroy(obstacles[i]);
            obstacles.RemoveAt(i);
        }
    }

    public void Win()
    {
        leaderboard.enabled = true;
        scoreText.enabled = false;
        hero.enabled = false;
        hint.enabled = false;
        this.enabled = false;
        GameProgression.SetIsInMinigame(false);
        //TODO
    }
}