using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System;
public class PhotonMap: Photon.MonoBehaviour {

	public GameObject wall_prefab;
	private GameObject breakable_prefab;
	private GameObject startpos_prefab;
	public GameObject floor_prefab;
	private GameObject goal_prefab;
	private GameObject door_prefab;
	private GameObject powerup_prefab;
	public GameObject Map_parent;

	private PhotonView PhotonView;
	private Blocks[, ] array_representation;

	private int start_poses = 10;

	int x = 15;
	int y = 15;

	private void Awake() {
		PhotonView = GetComponent < PhotonView > ();
	}
	// Start is called before the first frame update

	void Start() {
		array_representation = new Blocks[x, y];
		create_map(x, y);
	}

	// Update is called once per frame
	void Update() {

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
						new_instance(i_x, 0, i_y, wall_prefab);
						//                 new_instance(i_x+1, 0, i_y, goal_prefab);
						// add paper powerup here
					} else {
						array_representation[i_x, i_y] = Blocks.Wall;
						new_instance(i_x, 0, i_y, wall_prefab);
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
								GameObject temp_floor1 = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Breakable"), new Vector3(i_x, 0, i_y), Quaternion.identity, 0);
								temp_floor1.transform.SetParent(Map_parent.transform);
							}

						}
					}
				}
			}
		}
	}

	private GameObject new_instance(int x, int y, int z, GameObject prefab) {
		GameObject temp_floor = Instantiate(prefab, new Vector3(x, y, z), Quaternion.identity); // create new prefab instance
		temp_floor.transform.SetParent(Map_parent.transform); // set parent
		return temp_floor;
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

}