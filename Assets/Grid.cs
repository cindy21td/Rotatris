using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Grid : MonoBehaviour {

	// exceeding the height by 2
	public static int HEIGHT = 18;
	public static int WIDTH = 12;

    private static int TOP_IDX = 15;

	public Text text;

	// TOP 		--> 7.5 	==> 15
	// BOTTOM 	--> -7.5 	==> 0
	// LEFT		--> -5.5	==> 0
	// RIGHT	--> 5.5		==> 11

	private Transform[,] grid = new Transform[WIDTH, HEIGHT];
	
	public bool CheckGameOver () {
        for (int i = 0; i < WIDTH; i++)
        {
            if (grid[i, TOP_IDX] != null)
            {
                return true;
            }
        }
        return false;
    }

	public bool CheckMove(Transform obj, int action) {
		switch (action) {
		// down
		case 0:
			foreach (Transform t in obj) {
				Vector3 pos = t.position;
				int idxX = Mathf.RoundToInt(pos.x + 5.5f);
				int idxY = Mathf.RoundToInt(pos.y + 7.5f);

				if (idxY - 1 < 0 || grid [idxX, idxY - 1] != null) {
					if (idxY - 1 >= 0 && grid [idxX, idxY - 1].parent == t.parent) {
						continue;
					}
					return false;
				}
			}
			break;
		// left
		case 1:
			foreach (Transform t in obj) {
				Vector3 pos = t.position;
				int idxX = Mathf.RoundToInt(pos.x + 5.5f);
				int idxY = Mathf.RoundToInt(pos.y + 7.5f);

				if (idxX - 1 < 0 || grid [idxX - 1, idxY] != null) {
					if (idxX - 1 >= 0 && grid [idxX - 1, idxY].parent == t.parent) {
						continue;
					}
					return false;
				}

				if (idxY - 1 < 0 || grid [idxX, idxY - 1] != null) {
					if (idxY - 1 >= 0 && grid [idxX, idxY - 1].parent == t.parent) {
						continue;
					}
					return false;
				}
			}
			break;
		// right
		case 2:
			foreach (Transform t in obj) {
				Vector3 pos = t.position;
				int idxX = Mathf.RoundToInt(pos.x + 5.5f);
				int idxY = Mathf.RoundToInt(pos.y + 7.5f);

				if (idxX + 1 > WIDTH - 1 || grid [idxX + 1, idxY] != null) {
					if (idxX + 1 <= WIDTH - 1 && grid [idxX + 1, idxY].parent == t.parent) {
						continue;
					}
					return false;
				}

				if (idxY - 1 < 0 || grid [idxX, idxY - 1] != null) {
					if (idxY - 1 >= 0 && grid [idxX, idxY - 1].parent == t.parent) {
						continue;
					}
					return false;
				}
			}
			break;
		// rotate
		case 3:
			obj.Rotate(0, 0, -90);
			foreach (Transform t in obj) {
				Vector3 pos = t.position;
				int idxX = Mathf.RoundToInt(pos.x + 5.5f);
				int idxY = Mathf.RoundToInt(pos.y + 7.5f);

				if (idxX < 0 || idxX > WIDTH - 1 || idxY < 0 || grid [idxX, idxY] != null) {
					if ((idxX >= 0 && idxX <= WIDTH - 1 && idxY >= 0) && grid [idxX, idxY].parent == t.parent) {
						continue;
					}
					obj.Rotate(0, 0, 90);
					return false;
				}
			}
			obj.Rotate(0, 0, 90);
			break;
		}
		return true;
	}

	public void UpdateGrid(Transform obj, int action) {
		switch (action) {
		// down
		case 0:
			cleanGrid (obj);
			obj.position += new Vector3 (0, -1, 0);
			fillInGrid (obj);
			break;

		// left
		case 1:
			cleanGrid (obj);
			obj.position += new Vector3 (-1, 0, 0);
			fillInGrid (obj);
			break;

		// right
		case 2:
			cleanGrid (obj);
			obj.position += new Vector3 (1, 0, 0);
			fillInGrid (obj);
			break;

		// up (rotate)
		case 3:
			cleanGrid (obj);
			obj.Rotate(0, 0, -90);
			fillInGrid (obj);
			break;

		// spawn
		case 4:
			fillInGrid (obj);
			break;

		}
		printGrid ();
	}

	void cleanGrid(Transform obj) {
		foreach (Transform t in obj) {
			Vector3 pos = t.position;
			int idxX = Mathf.RoundToInt(pos.x + 5.5f);
			int idxY = Mathf.RoundToInt(pos.y + 7.5f);

			grid [idxX, idxY] = null;
		}
	}

	void fillInGrid(Transform obj) {
		foreach (Transform t in obj) {
			Vector3 pos = t.position;
			int idxX = Mathf.RoundToInt(pos.x + 5.5f);
			int idxY = Mathf.RoundToInt(pos.y + 7.5f);

			grid [idxX, idxY] = t;
		}
	}

	public void checkGrid(int gravity) {
		for (int i = 0; i < HEIGHT; i++) {
			if (checkRow (i)) {
				// delete row & truncate downwards
				truncateGrid(i);
				// check the row again
				i--;
			}
		}
	}

	bool checkRow(int row) {
		for (int i = 0; i < WIDTH; i++) {
			if (grid [i, row] == null) {
				return false;
			}
		}
		return true;
	}

	void truncateGrid(int row) {
		for (int i = 0; i < WIDTH; i++) {
			Transform t = grid [i, row];
			grid [i, row] = null;
			Destroy (t.gameObject);
		}

		for (int i = row + 1; i < HEIGHT; i++) {
			for (int j = 0; j < WIDTH; j++) {
				Transform t = grid [j, i];
				if (t != null) {
					t.gameObject.transform.position += new Vector3 (0, -1, 0);
				}
				grid [j, i - 1] = grid [j, i];
				grid [j, i] = null;
			}
		}
	}


	void printGrid() {
		string x = "";
		for (int i = HEIGHT - 1; i >= 0 ; i--) {
			string s = "";
			for (int j = 0; j < WIDTH; j++) {
				Transform t = grid [j, i];
				if (t == null) {
					s += "0 ";
				} else {
					s += "1 ";
					//s += ((int) (t.position.x + 5.5f)) + "/" + ((int) (t.position.y + 7.5f)) + " ";
					//s += Mathf.FloorToInt(t.position.x + 5.5f)  + " ";
					//s += t.position.x + "/" + (t.position.x + 5.5f) + "/" + Mathf.FloorToInt(t.position.x + 5.5f) + "/" + Mathf.RoundToInt(t.position.x + 5.5f) + " ";
				}
			}
			x += s + "\n";
		}
		//print (x);
		text.text = x;
	}
}
