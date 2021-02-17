using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class main : MonoBehaviour
{
	// set up the board
	public char[,] board = new char [ 3 , 3 ];
	public char p1 = 'X';
	public char p2 = 'O';
	
	public bool victory;
	public char winner;
	public bool boardFull;
	public char curPlayer;
	public int rowInput;
	public int colInput;
	
	public Camera gameCamera;
	public GameObject cellToPlay;


	private bool BoardEmpty(char[,] board) 
	{
		for (int i=0; i<3; i++) {
			for (int j=0; j < 3; j++) {
				if (board[i,j] == p1 || board[i,j] == p2) 
					return false;
				}
			}
		return true;
	}
	
	private bool BoardFull(char[,] board) 
	{
		for (int i=0; i<3; i++) {
			for (int j=0; j < 3; j++) {
				if (board[i,j] != p1 && board[i,j] != p2) 
					return false;
				}
			}
		return true;
	}
	
	private char NextPlayer(char player) {
		if (player == p1) return p2;
		else return p1;
	}
	
	private bool CheckWin(char[,] board){
		
		if (BoardEmpty(board)) {
			return false;
		}
		
		var diagFirst = board[0,0];
		if (!(diagFirst != p1 && diagFirst != p2)){
			if (board[1,1] == diagFirst && board[2,2] == diagFirst){
				return true;
			}
		}
		var diagSecond = board[2,0];
		if (!(diagSecond != p1 && diagSecond != p2)){
			if (board[1,1] == diagSecond && board[0,2] == diagSecond){
				return true;
			}
		}
		
		for (int i=0; i<3; i++){
			var rowPlayed = board[i,0];
			if (rowPlayed == p1 || rowPlayed == p2){
				if (board[i,1] == rowPlayed && board[i, 2] == rowPlayed){
					return true;
				}
			}
		}
		
		for (int j=0; j<3; j++){
			var colPlayed = board[0,j];
			if (colPlayed == p1 || colPlayed == p2){
				if (board[1,j] == colPlayed && board[2, j] == colPlayed){
					return true;
				}
			}
		}
		
		return false;
	}
	
	
	void DoTurn(char p, int row, int col)
	{
		// put symbol on board 
		board[row, col] = p;
				
		cellToPlay = GameObject.Find("Cell " + row + " " + col );
		
		GameObject mesh;
		
		if (curPlayer == p1) {
			mesh = GameObject.CreatePrimitive(PrimitiveType.Cube);
			var meshRenderer = mesh.GetComponent<Renderer>();
			meshRenderer.material.SetColor("_Color", Color.red);
			}
		else {
			mesh = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			var meshRenderer = mesh.GetComponent<Renderer>();
			meshRenderer.material.SetColor("_Color", Color.blue);
			}
		
		GameObject matchingCube = cellToPlay.transform.GetChild(0).gameObject;
		mesh.transform.position = matchingCube.transform.position;
		mesh.transform.SetParent(cellToPlay.transform);	
	}
	

	bool InputValid(int rowInput, int colInput){
		if (!(rowInput >= 0 && rowInput <= 2 && colInput >= 0 && colInput <= 2))
		{
			return false;
		}
		if (!(board[rowInput, colInput] != p1 && board[rowInput, colInput] != p2))
		{
			return false;
		}
		return true;
	}
	
    void Start()
    {
		curPlayer = p1;
		victory = false;	
    }


	void Update(){
		if (winner != 't' && winner != p1 && winner != p2 && !boardFull){
			
		   if (Input.GetMouseButtonDown(0)){
			 Ray ray = gameCamera.ScreenPointToRay(Input.mousePosition);
			 RaycastHit hit;
			 if (Physics.Raycast(ray, out hit)){
			   string[] coordinates = hit.transform.parent.gameObject.name.Split(' ');
			   rowInput = Int32.Parse(coordinates[1]);
			   colInput = Int32.Parse(coordinates[2]);
				
				if (InputValid(rowInput, colInput)){
						
					DoTurn(curPlayer, rowInput, colInput);
					
					victory = CheckWin(board);
					boardFull = BoardFull(board);
					
					if (victory) {
						winner = curPlayer;
					}
					else if (boardFull) {
						winner = 't';
					}
					
					curPlayer = NextPlayer(curPlayer);
				}
			  }
		   }
		}
		else {
			TextMesh textObject = GameObject.Find("Text Object").GetComponent<TextMesh>();
			if (winner == p1){
				textObject.text = "Player One Wins";
				
			}
			else if (winner == p2){
				textObject.text = "Player Two Wins";

			}
			else {
				textObject.text = "Tie";
			}
		}
	}
	 
}
