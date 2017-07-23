using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameMaster : MonoBehaviour {

	private float PAUSE_TIME = 1f;
	private float currTimer;

	public SpawnMaster spawn;
	public Grid grid;
    public GameObject gameOverPanel;


	private GameObject activeBlock;
	private int move;

	private int gravity;
	private Dictionary<KeyCode, int> gravityKeys;
    private bool isGameOver;

	// Use this for initialization
	void Start () {
		// Initialize gravity
		gravityKeys = new Dictionary<KeyCode, int>();
		gravityKeys.Add (KeyCode.DownArrow, 0);
		gravityKeys.Add (KeyCode.LeftArrow, 1);
		gravityKeys.Add (KeyCode.RightArrow, 2);

		// start down first
		gravity = 0;

		currTimer = PAUSE_TIME;

		spawn.SpawnRandomBlock (gravity);
		activeBlock = spawn.getActiveBlock ();
		move = 4;
		grid.UpdateGrid (activeBlock.transform, move);

        isGameOver = false;
	}

	// Update is called once per frame
	void Update () {
        // Disable keys if game over.
        if (isGameOver)
        {
            return;
        }

		// TODO: Automate this change
		if (Input.GetKeyDown (KeyCode.A)) {
			gravity = (gravity + 1) % 3;
		}


		if (Input.GetKeyDown (KeyCode.UpArrow) && grid.CheckMove (activeBlock.transform, 3)) {
			// Rotate
			move = 3;
		} else if (gravity != 2 && checkKey(KeyCode.LeftArrow)) {
			// Move left
			if (gravity == gravityKeys [KeyCode.LeftArrow]) {
				currTimer = 0.95f * PAUSE_TIME;
			}
			move = 1;
		} else if (gravity != 1 && checkKey(KeyCode.RightArrow)) {
			// Move right
			if (gravity == gravityKeys [KeyCode.RightArrow]) {
				currTimer = 0.95f * PAUSE_TIME;
			}
			move = 2;
		} else if (checkKey(KeyCode.DownArrow)) {
			// Move down
			if (gravity == gravityKeys [KeyCode.DownArrow]) {
				currTimer = 0.95f * PAUSE_TIME;
			}
			move = 0;
		} else if (currTimer < 0 && !grid.CheckMove (activeBlock.transform, gravity)) {
			move = 4;
		} else {
			move = -1;
		}
			
		currTimer -= Time.deltaTime;
	}

	void LateUpdate() {
		if (move != -1) {
			if (move == 4) {
				currTimer = PAUSE_TIME;
				grid.checkGrid (gravity);

                // Before spawning random block. Need to check if game is over.
                if (grid.CheckGameOver())
                {
                    // Stop game, show panel
                    isGameOver = true;
                    gameOverPanel.SetActive(true);
                } else
                {
                    spawn.SpawnRandomBlock(gravity);
                    activeBlock = spawn.getActiveBlock();
                }
			}
			grid.UpdateGrid (activeBlock.transform, move);
		}
	}

	bool checkKey(KeyCode key) {
		bool isHeld = (Input.GetKey (key) && currTimer - PAUSE_TIME * 0.90 < 0);
		bool isTime = currTimer < 0;
		bool isPressed = Input.GetKeyDown (key);

		bool isGravity = ((gravity == gravityKeys [key]) && (isHeld || isTime));

		return (isGravity || isPressed) && grid.CheckMove (activeBlock.transform, gravityKeys [key]);
	}

    public void RestartGame()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void ExitToTitle()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
