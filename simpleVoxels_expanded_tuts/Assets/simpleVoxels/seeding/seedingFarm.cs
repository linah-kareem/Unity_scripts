﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class seedingFarm : MonoBehaviour {

	private GameObject currentBlockType;
	public GameObject[] blockTypes;

	[Tooltip("True for minecraft style voxels")]
	public bool SnapToGrid = true;

	public float amp = 10f;
	public float freq = 10f;

	private Vector3 myPos;

	public int seed = 0;

	private GameObject seedManager;


	void Start () {

		myPos = this.transform.position;
	
		generateTerrain ();
	}


	int getMySeed(){

		GameObject seed_manager = GameObject.Find ("seedManager");

		return seed_manager.GetComponent<seedBrain> ().seed;
	}


	void terrainFeatures(){
		freq = Mathf.Sin(seed * 12f);
		amp = freq * 100f;


	}

	void generateTerrain(){


		seed = getMySeed ();

		//terrainFeatures ();



		int cols = 100;
		int rows = 100;

		for (int x = 0; x < cols; x++) {


			for (int z = 0; z < rows; z++) {


				bool makeTree = false;

				float y = 0;

				// First octave.
				y = Mathf.PerlinNoise ((seed + myPos.x + x) / (freq*4f), 
					(myPos.z + z) / (freq*4f)) * amp*4f;

				// Orig octave.
				y += Mathf.PerlinNoise ((seed + myPos.x + x) / freq, 
					(myPos.z + z) / freq) * amp;

				// Final octave.
//				y += Mathf.PerlinNoise ((seed + myPos.x + x) / freq, 
//					(myPos.z + z) / (freq*0.5f)) * (amp/0.5f);

				if (SnapToGrid) {
					y = Mathf.Floor (y);
				}

				if (y > amp * 3f)
					currentBlockType = 
						blockTypes [1];
				else
					currentBlockType = 
						blockTypes [0];

				GameObject newBlock = 
					GameObject.Instantiate (currentBlockType);


				// *&*&*&*&*&*&*&**&*&*&*&*&*&*&*&*&

				// Biomes!

				// Tree biome.
//				if (myPos.z + z < 52f) {
//					makeTree = true;
//					y += Random.value * 1f;
//				}

				// Tundra biome.
				float Bfreq = freq*6f;
				if (myPos.z + z > 100f &&
					(Mathf.PerlinNoise ((myPos.x + x) / Bfreq, 
						(myPos.z + z) / Bfreq)
						> 0.5f))
					newBlock.GetComponent<Renderer> ().
					material.color = Color.white;


				// Taiga biome.
				Bfreq = freq*2f;
				if (Mathf.PerlinNoise ((myPos.x + x) / Bfreq, 
					    (myPos.z + z) / Bfreq)
					> 0.5f && myPos.z + z < 124f) {
					newBlock.GetComponent<Renderer> ().material.color =
						Color.blue;
					makeTree = true;
//						new Color (	0.9f + Random.value/10f, 
//									0.9f+ Random.value/10f, 
//									1f+ Random.value/10f);
					//y += 2f;
				}
				// *&*&*&*&*&*&*&**&*&*&*&*&*&*&*&*&

				// Finally, place block at correct position.
				newBlock.transform.position =
					new Vector3 (myPos.x + x * currentBlockType.transform.localScale.x, 
						y, myPos.z + z * currentBlockType.transform.localScale.z);





				//*********************************************************

				// Now decide whether to create a tree!
				if (Random.value * 100 < 1 && makeTree == true) {
					float adjust = newBlock.transform.lossyScale.y / 2f;

					GameObject treeBabe = 
						GameObject.CreatePrimitive (PrimitiveType.Cube);



					Vector3 tT = treeBabe.transform.localScale;
					tT.y = Random.value * 24;
					treeBabe.transform.localScale =
						tT;

					adjust += treeBabe.transform.localScale.y / 2f;

					treeBabe.transform.position =
						new Vector3 (myPos.x + x * currentBlockType.transform.localScale.x, y + adjust, 
							myPos.z + z * currentBlockType.transform.localScale.z);

					treeBabe.GetComponent<Renderer> ().material.color = new Color(0.6f,0.1f,0.12f);

					// Now for the head of the tree.
					GameObject treeHead = 
						GameObject.CreatePrimitive (PrimitiveType.Cube);

					treeHead.GetComponent<Renderer> ().material.color = new Color (0f, 0.6f, 0f,0.7f);

					treeHead.transform.localScale *= tT.y/4f + 4f;

					treeHead.transform.position =
						new Vector3 (myPos.x + x * currentBlockType.transform.localScale.x
							, y + adjust + tT.y/2f, myPos.z + z * currentBlockType.transform.localScale.z);

				}
					

			}

		}


	}

}
