using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PhotonMap: Photon.MonoBehaviour {


	public static PhotonMap Instance;
	public GameObject wall_prefab;
	public GameObject stone_prefab;
	public GameObject breakable_prefab;
	private GameObject startpos_prefab;
	public GameObject floor_prefab;
	private GameObject goal_prefab;
	private GameObject door_prefab;
	private GameObject powerup_prefab;
	public GameObject Map_parent;
	public GameObject Stone_parent;

	private PhotonView PhotonView;
	private Blocks[, ] array_representation;

	private int start_poses = 10;

	int x, y, x1, x2, x3, y1, y2, y3;
	bool xplus, yplus, xminus, yminus;
	float timer = 0.0f;
	int second = 0;
	bool startStonestats = false;

	private void Awake() {
		PhotonView = GetComponent < PhotonView > ();
	}
	// Start is called before the first frame update

	void Start() {
		
		if (PhotonNetwork.room.PlayerCount < 5)
		{
			x = 15;
			y = 13;
			x2 = 13;
			y2 = 11;
		}
		else 
		{
			x = 19;
			y = 13;
			x2 = 17;
			y2 = 11;
		}
		array_representation = new Blocks[x, y];
		create_map(x, y);
		startStonestats = false;
		xplus = yplus = xminus = yminus = false;
		x1 = x3 = 0;
		y1 = y3 = 0;
	}

	// Update is called once per frame
	void Update() {
		if (startStonestats){
			timer += Time.deltaTime;
			int seconds = (int) (timer);
			if (seconds > second){
				second = seconds;
				createStone();
			}
		}
	}

	public void createStone() {
		if (xplus) {
			if (x1 < x2) {
				if (stone_stats(x1, y1)){
					x1++;
				}
				if (x1 < x2){
					stone_instance(x1, 0, y1, stone_prefab);
					x1++;
				}
			} else {
				y3++;
				y1++;
				x1--;
				xplus=false;
				yplus=true;
			}
		}
		if (yplus) {
			if (y1 < y2) {
				if (stone_stats(x1, y1)){
					y1++;
				}
				
				if (y1 < y2){
					stone_instance(x1, 0, y1, stone_prefab);
					y1++;
				}
					
			} else {
				x2--;
				x1--;
				y1--;
				yplus = false;
				xminus = true;
			}
		}
		if (xminus) {
			if(x1 >= x3){
				if (stone_stats(x1, y1)){
					x1--;
				}
				
				if(x1 >= x3){
					stone_instance(x1, 0, y1, stone_prefab);
					x1--;
				}
			} else {
				xminus = false;
				yminus = true;
				y2--;
				y1--;
				x1++;
			}
		}
		if(yminus) {
			if (y1 >= y3){
				if (stone_stats(x1, y1)){
					y1--;
				}
				
				if (y1 >= y3) {
					stone_instance(x1, 0, y1, stone_prefab);
					y1--;
				}
			} else {
				yminus = false;
				xplus = true;
				x3++;
				x1++;
				y1++;
			}
		}
	}

	public void create_map(int x, int y) {

		/* Yet again ugly code but fast working for now */
		// player start pos
		//    GameObject t = new_instance(1, 0, y/2 +  (y % 2), startpos_prefab);
		//    t.GetComponent<startpos_script>().player_controller = true;

		array_representation[1, y / 2] = Blocks.Startpos;

		if (start_poses > 1) {
			//    new_instance(1, 0, y-2, startpos_prefab);
			array_representation[1, y - 2] = Blocks.Startpos;

			if (start_poses > 2) {
				//add four more
				//     new_instance(1, 0, 1, startpos_prefab);
				array_representation[1, 1] = Blocks.Startpos;

				if (start_poses > 3) {
					//    new_instance(x-2, 0, y-2, startpos_prefab);
					array_representation[x - 2, y - 2] = Blocks.Startpos;
					if (start_poses > 4) {
						//    new_instance(x-2, 0, 1, startpos_prefab);
						array_representation[x - 2, 1] = Blocks.Startpos;

						if (start_poses > 5) {
							//        new_instance(x/2, 0, 1, startpos_prefab);
							array_representation[(x) / 2, 1] = Blocks.Startpos;
						}
						if (start_poses > 6) {
							//        new_instance(x/2, 0, y-2, startpos_prefab);
							array_representation[(x) / 2, y - 2] = Blocks.Startpos;
						}
						if (start_poses > 7) { // max 8 pers
							//        new_instance(x-2, 0, y/2, startpos_prefab);
							array_representation[x - 2, y / 2] = Blocks.Startpos;
						}
						if (start_poses > 8) { // max 8 pers
							//		new_instance(x-2, 0, y/2, startpos_prefab);
							array_representation[(x) / 2, y / 2] = Blocks.Startpos;
						}
					}
				}
			}
		}

		for (int i_x = 0; i_x < x; i_x++) {
			for (int i_y = 0; i_y < y; i_y++) {

				// Create floor
				// new_instance(i_x, -1, i_y, floor_prefab);

				// Create all wall limits 
				if (i_x == 0 || i_x == x - 1 || i_y == 0 || i_y == y - 1) {

					// add door and goal (only once per map)
					if (i_y == y / 2 && i_x == x - 1) {
						array_representation[i_x, i_y] = Blocks.Door;
						// new_instance(i_x, 0, i_y, wall_prefab);
						//                 new_instance(i_x+1, 0, i_y, goal_prefab);
						// add paper powerup here
					} else {
						array_representation[i_x, i_y] = Blocks.Wall;
						// new_instance(i_x, 0, i_y, wall_prefab);
					}

				} else {

					// add wall in center of map (random later on!)
					if (i_x % 2 == 0 && i_y % 2 == 0 && i_x != x - 2 && i_y != y - 2) {

						array_representation[i_x, i_y] = Blocks.Wall;
						new_instance(i_x, 0, i_y, wall_prefab);
					} else {

						// add breakables
						if (!start_next_to(i_x, i_y)) {
							array_representation[i_x, i_y] = Blocks.Breakable;
							if (PhotonNetwork.isMasterClient) {
								int xx =i_x;
								if (PhotonNetwork.room.PlayerCount < 5)
									xx += 2;

								if (Random.Range(0.0f, 1.0f) > 0.1f) {
									// GameObject temp_floor1 = Instantiate(breakable_prefab, new Vector3(xx, 0, i_y), Quaternion.identity);
									GameObject temp_floor1 = PhotonNetwork.InstantiateSceneObject(Path.Combine("Prefabs", "Breakable"), new Vector3(xx, 0, i_y), Quaternion.Euler(0f, 0f, 0f), 0, null);
									// temp_floor1.transform.SetParent(Map_parent.transform);
								}
								// GameObject temp_floor1 = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Breakable"), new Vector3(i_x, 0, i_y), Quaternion.Euler(-90f, 0f, -90f), 0);
								// temp_floor1.transform.SetParent(Map_parent.transform);
							}

						}
					}
				}
			}
		}
	}

	private GameObject new_instance(int x, int y, int z, GameObject prefab) {
		int xx =x;
		if (PhotonNetwork.room.PlayerCount < 5)
			xx += 2;
		GameObject temp_floor = Instantiate(prefab, new Vector3(xx, y, z), Quaternion.Euler(-90f, 0f, 0f)); // create new prefab instance
		temp_floor.transform.SetParent(Map_parent.transform); // set parent
		return temp_floor;
	}

	private GameObject stone_instance(int x, int y, int z, GameObject prefab) {
		int xx =x-1;
		int zz = z;
		if (PhotonNetwork.room.PlayerCount > 4)
			xx -= 2;
		GameObject temp_floor = Instantiate(prefab, new Vector3(xx, y, zz), Quaternion.Euler(0f, 0f, 0f)); // create new prefab instance
		temp_floor.transform.SetParent(Stone_parent.transform); // set parent
		return temp_floor;
	}

	private bool stone_stats(int x, int y){
		int xx = x+1;
		int yy = y+1;
		// if (PhotonNetwork.room.PlayerCount < 5)
		// 	xx -= 4;
		if (xx > -1 && yy > -1)
			if(array_representation[xx, yy] == Blocks.Wall)
				return true;
		return false;	
	}

	private bool start_next_to(int x, int y) {
		if (array_representation[x - 1, y] == Blocks.Startpos) {
			return true;
		}
		if (array_representation[x + 1, y] == Blocks.Startpos) {
			return true;
		}
		if (array_representation[x, y + 1] == Blocks.Startpos) {
			return true;
		}
		if (array_representation[x, y - 1] == Blocks.Startpos) {
			return true;
		}
		if (array_representation[x, y] == Blocks.Startpos) {
			return true;
		}
		if (array_representation[x - 1, y - 1] == Blocks.Startpos) {
			return true;
		}
		if (array_representation[x + 1, y + 1] == Blocks.Startpos) {
			return true;
		}
		return false;
	}

	public void startStone() {
		Debug.Log("map startstone");
		xplus = true;
		startStonestats = true;
	}


}