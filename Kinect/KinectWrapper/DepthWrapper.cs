using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Level of indirection for the depth image,
/// provides:
/// -a frames of depth image (no player information),
/// -an array representing which players are detected,
/// -a segmentation image for each player,
/// -bounds for the segmentation of each player.
/// </summary>
public class DepthWrapper: MonoBehaviour {

	public DeviceOrEmulator devOrEmu;
	private Kinect.KinectInterface kinect;
	public int lowest = 100;
	public int highest = 3000;
	public int left = 0;
//	private Calibrate cal;
//		//Component cal= GameObject.Find("KinectPrefab").GetComponent<Calibrate>();
	public int right= 320;
	public int top = 0;
	public int bottom = 240;
//	public int topleftx = 0;
//	public int toplefty = 0;
//	public int toprightx = 320;
//	public int toprighty = 0;
//	public int bottomleftx = 0;
//	public int bottomlefty = 240;
//	public int bottomrightx = 320;
//	public int bottomrighty = 240;
	public bool enableHomography = true;
//	[HideInInspector]
	public int l=0;
//	[HideInInspector]
	public int r=0;
	//[HideInInspector]
	public int start=0;
	//[HideInInspector]
	public float hit=0;
	[HideInInspector]
	public int li=0;
	[HideInInspector]
	public int ri=0;
	[HideInInspector]
	public int starti=0;
	[HideInInspector]
	public float hiti=0;

	
	private struct frameData
	{
		public short[] depthImg;
		public bool[] players;
		public bool[,] segmentation;
		public int[,] bounds;
	}
	
	public int storedFrames = 1;
	
	private bool updatedSeqmentation = false;
	[HideInInspector] // Hides var below
	public bool newSeqmentation = false;
	
	private Queue frameQueue;
	
	/// <summary>
	/// Depth image for the latest frame
	/// </summary>
	[HideInInspector]
	public short[] depthImg;
	/// <summary>
	/// players[i] true iff i has been detected in the frame
	/// </summary>
	[HideInInspector]
	public bool[] players;
	/// <summary>
	/// Array of segmentation images [player, pixel]
	/// </summary>
	[HideInInspector]
	public bool[,] segmentations;
	/// <summary>
	/// Array of bounding boxes for each player (left, right, top, bottom)
	/// </summary>
	[HideInInspector]
	//right,left,up,down : but the image is fliped horizontally.
	public int[,] bounds;
	
	// Use this for initialization
	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
	}

	void Start () {
		kinect = devOrEmu.getKinect();
		//allocate space to store the data of storedFrames frames.
		frameQueue = new Queue(storedFrames);
		for(int ii = 0; ii < storedFrames; ii++){	
			frameData frame = new frameData();
			frame.depthImg = new short[320 * 240];
			frame.players = new bool[Kinect.Constants.NuiSkeletonCount];
			frame.segmentation = new bool[Kinect.Constants.NuiSkeletonCount,320*240];
			frame.bounds = new int[Kinect.Constants.NuiSkeletonCount,4];
			frameQueue.Enqueue(frame);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void LateUpdate()
	{
		updatedSeqmentation = false;
		newSeqmentation = false;
	}
	/// <summary>
	/// First call per frame checks if there is a new depth image and updates,
	/// returns true if there is new data
	/// Subsequent calls do nothing have the same return as the first call.
	/// </summary>
	/// <returns>
	/// A <see cref="System.Boolean"/>
	/// </returns>
	public bool pollDepth()
	{
		//Debug.Log("" + updatedSeqmentation + " " + newSeqmentation);
		if (!updatedSeqmentation)
		{
			updatedSeqmentation = true;
			if (kinect.pollDepth())
			{
				newSeqmentation = true;
				frameData frame = (frameData)frameQueue.Dequeue();
				depthImg = frame.depthImg;
				players = frame.players;
				segmentations = frame.segmentation;
				bounds = frame.bounds;
				frameQueue.Enqueue(frame);
				processDepth();
			}
		}
		return newSeqmentation;
	}
	
	private void processDepth()
	{
		for(int player = 0; player < Kinect.Constants.NuiSkeletonCount; player++)
		{
			//clear players
			players[player] = false;
			//clear old segmentation images
			for(int ii = 0; ii < 320 * 240; ii++)
			{
				segmentations[player,ii] = false;
			}
			//clear old bounds
			for(int ii = 0; ii < 4; ii++)
			{
				bounds[player,ii] = 0;
			}
		}

//		right = cal.right;
		//temp image array to fill up with new point data
		short[] homographyImg = new short[320 * 240];
		//-min and max depth values
		short tmin = Convert.ToInt16(lowest);
		short tmax = Convert.ToInt16(highest);
		for(int ii = 0; ii < 320 * 240; ii++)
		{
			//get x and y coords
			int xx = ii % 320;
			int yy = ii / 320;
			//extract the depth and player
			depthImg[ii] = (short)(kinect.getDepth()[ii] >> 3);
			//-limit depth values and set for 0
			if (depthImg[ii] < tmin) depthImg[ii] = 0;
			else if (depthImg[ii] > tmax) depthImg[ii] = 0;
			//set for max short value 32767
			else depthImg[ii] = 32767;

			if (enableHomography)
			{
				if (xx < right & xx > left & yy < bottom & yy > top)
				{
					int py= 240*(yy-top)/(bottom-top);
					int px=320*(xx-left)/(right-left);
					int pxy = 320*py+px;
					homographyImg[pxy]=depthImg[ii];

					//check if point is tracked, "white"
					if (depthImg[ii] == 32767 & px < 50)
					{
						if (py<60){
							starti=starti+1;
						}
						if (60<py & py<120){
							li=li+1;
						}
						if (120<py & py<180){
							ri=ri+1;
						}
						if (180<py){
							hiti=hiti+1;
						}
					}
				}

//				int lx =(topleftx + (bottomleftx - topleftx)*yy/240);
//				int rx =(toprightx + (bottomrightx - toprightx)*yy/240);
//				if (xx < rx & xx > lx)
//				{
//					int px = 320*(xx-lx)/(rx-lx);
//					int ty =(toplefty + (toprighty - toplefty)*px/320);
//					int by =(bottomlefty + (bottomrighty - bottomlefty)*px/320);
//					if (yy > ty & yy < by)
//					{
//						int pxy = 240*(yy-ty)*320/(by-ty) + px;
//						homographyImg[pxy] = depthImg[ii];
//					}
//				}
			}
//			int player = (kinect.getDepth()[ii] & 0x07) - 1;
//			if (player > 0)
//			{
//				if (!players[player])
//				{
//					players[player] = true;
//					segmentations[player,ii] = true;
//					bounds[player,0] = xx;
//					bounds[player,1] = xx;
//					bounds[player,2] = yy;
//					bounds[player,3] = yy;
//				}
//				else
//				{
//					segmentations[player,ii] = true;
//					bounds[player,0] = Mathf.Min(bounds[player,0],xx);
//					bounds[player,1] = Mathf.Max(bounds[player,1],xx);
//					bounds[player,2] = Mathf.Min(bounds[player,2],yy);
//					bounds[player,3] = Mathf.Max(bounds[player,3],yy);
//				}
//			}
		}
//		Debug.Log(max);
//		Debug.Log(min);

		//check if there is any point
		if (starti>0){
			start=1;
		}
		else start=0;
		if (li>0){
			l=1;
		}
		else l=0;
		if (ri>0){
			r=1;
		}
		else r=0;
		if (hiti>0){
			hit=1;
		}
		else hit=0;

		if (enableHomography)
		{
			depthImg = homographyImg;
		}
	}
}
