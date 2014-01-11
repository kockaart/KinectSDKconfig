using UnityEngine;
using System.Collections;

public class Calibrate : MenuBase {
		
//	protected Rect upRect;
//	protected Rect downRect;
	protected Rect readyRect;
	public DepthWrapper dw;

	
	// Use this for initialization
	new void Start () {
		base.Start();
		
//		Vector2 middle = new Vector2(this.screenWidth / 2f, this.screenHeight / 2f);
//		readyRect = new Rect((middle.x -  this.screenWidth /12f), (this.screenHeight/1.2f), this.screenWidth / 6f, this.screenHeight / 8f);
//		Vector2 buttonSize = new Vector2(this.screenWidth / 3f, this.screenHeight / 5f);
//		upRect = new Rect((middle.x -  buttonSize.x/2), (middle.y - buttonSize.y - 20), buttonSize.x, buttonSize.y);
//		downRect = new Rect((middle.x - buttonSize.x/2), (middle.y + 20), buttonSize.x, buttonSize.y);
	
	}


	void OnGUI() {		
		GUI.skin = menuSkin;
//		aa=Convert.ToSingle(dw.lowest);
		dw.lowest = Mathf.RoundToInt(GUI.HorizontalSlider(new Rect(10, 50, 150, 30), (float)dw.lowest, 1, 4000));
		dw.highest = Mathf.RoundToInt(GUI.HorizontalSlider(new Rect(10, 140, 150, 30), (float)dw.highest, 1000, 5000));
		dw.right = Mathf.RoundToInt(GUI.HorizontalSlider(new Rect(Screen.width/2-155, Screen.height/2, 150, 30), (float)dw.right, 250, 320));
		dw.left = Mathf.RoundToInt(GUI.HorizontalSlider(new Rect(Screen.width/2+15, Screen.height/2, 150, 30), (float)dw.left, 0 , 60));
		dw.top = Mathf.RoundToInt(GUI.VerticalSlider(new Rect(Screen.width/2, Screen.height/2-155, 30, 150), (float)dw.top, 50, 0));
		dw.bottom = Mathf.RoundToInt(GUI.VerticalSlider(new Rect(Screen.width/2, Screen.height/2+5, 30, 150), (float)dw.bottom,240, 180  ));

		GUI.Box (new Rect (10, 10,150,30), ("DepthMIN: "+dw.lowest));
		GUI.Box (new Rect (10, 90,150,30), ("DepthMAX: "+dw.highest));
		GUI.Box (new Rect (Screen.width/2-155, Screen.height/2-35,100,30), ("Left: " + dw.right));
		GUI.Box (new Rect (Screen.width/2+100, Screen.height/2-35,100,30), ("Right: "+dw.left));
		GUI.Box (new Rect (Screen.width/2+15, Screen.height/2-155,80,30), ("Top: "+dw.top));
		GUI.Box (new Rect (Screen.width/2+25, Screen.height/2+130,110,30), ("Bottom: "+dw.bottom));

		readyRect = new Rect (Screen.width-120, 80,100,40);
		//Buttons
		
		//Back to settings
		if(GUI.Button(readyRect,"Ready")){
			Application.LoadLevel("Flipper06");
		}
//		else if(GUI.Button(upRect,"UP")){
//			dw.right=dw.right-10;
////			this.kinectController.moveKinect(0.2f);
//		}
//		else if(GUI.Button(downRect,"DOWN")){
////			this.kinectController.moveKinect(-0.2f);
//		}
		
		//GUI.Label(new Rect(this.screenWidth / 7, (float)(this.screenHeight / 1.2),(float)(this.screenWidth / 1.5), this.screenHeight / 6), "Play and play it again !");
		GUI.Box (new Rect(0, 0, (float)(Screen.width), (float)(Screen.height)), "andras.koczka@gmail.com");
			
	}
}
