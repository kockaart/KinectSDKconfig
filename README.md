KinectSDKconfig
===============

depth image modification, angle &amp; size to cover the appropriate projection

Kinect
KinectModelControllerV2 - This is the script that you will attach to your model that you want to manipulate - you will drag each of the bones to be controlled into the appropriate slot, and determine which player controls which model.
KinectPointController - This script will place GameObjects that you define onto points that are tracked by the Kinect, generating a skeleton. The starting scene comes with an example of how this looks / should be done.
DisplayDepth - This script will get the depth image. Attach it to a game object with renderer. NOTE: In unity, you need to restart unity everytime after running your world, otherwise this script will not work (because of SDK issue).
DisplayColor - This script will get the RGB image. Attach it to a game object with renderer. NOTE: In unity, you need to restart unity everytime after running your world, otherwise this script will not work (because of SDK issue).
KinectRecorder - This script will record your movement and output playback files for the emulator.
KinectEmulator - This script will act as a virtual Kinect. It works with playback files. For now it only simulate the skeleton data.
KinectSensor - This script gets data from the physic Kinect.
DeviceOrEmulator - This script sets whether to use physic Kinect or the emulator.
SkeletonWrapper - This script grabs skeleton data.
DepthWrapper - This script grabs depth image data.
KinectInterop - This script grabs data from Microsoft Kinect SDK.
Recordings/playbackDefault - This is the default playback file for emulator. Do NOT remove this file.

Device Or Emulator
Use Emulator - Check this to use emulator.
Kinect Sensor
NOTE: do NOT enable this script manually, it is controlled by DeviceOrEmulator.
Sensor Height - How high (in meters) off the ground is the sensor.
Kinect Center - This tells the Kinect where it should be looking for it's 0,0,0 point (relative to the ground directly under the sensor). The default works pretty well.
Look At - Tells the Kinect how to orient the camera using the motor control.
NOTE: The following values allow you to smooth the skeleton data. Usually the default values work fine. Do NOT change them unless you find magic numbers for you project.
Smoothing - Default value 0.5.
Correction - Default value 0.5.
Prediction - Default value 0.5.
Jitter Radius - Default value 0.05.
Max Deviation Radius - Default value 0.04.
Kinect Emulator
NOTE:
do NOT enable this script manually, it is controlled by DeviceOrEmulator.
If you are in the emulator mode, it will automatically play the playbackDefault file.
Press F12 to play file of the path Input File. Press F12 again to switch back.
Input File - The file you recorded for the emulator. The default value is "Assets/Kinect/Recordings/playback0".
Sensor Height - How high (in meters) off the ground is the sensor.
Kinect Center - This tells the Kinect where it should be looking for it's 0,0,0 point (relative to the ground directly under the sensor). The default works pretty well.
Look At - Tells the Kinect how to orient the camera using the motor control.
Kinect Recorder
NOTE:
Press F10 to start recording, and press F10 again to stop.
You can record multiple files in one run.
You recording files will be created sequentially in the folder determined by Output File. The default value is "Assets/Kinect/Recordings/playback", so the your recording files in one run will be named as "playback0", "playback1", etc.
The recorder will overwrite existing files. Remember to save your file to another folder before you run your world again and do the recording; or you can change the value of Output File to save the new files to another place.
Output File - Where your recording files will go. The default value is "Assets/Kinect/Recordings/playback".
Controlling your Character
Simply put, to get your character moving, you need to first attach either the KinectModelControllerV2 to control the bones of the model, or KinectPointController to control a series of GameObjects.
NewModelController
To set this up, do the following:
Drag the KinectModelControllerV2 script on to your model.
Select the model, and find the Sw variable (it stands for Skeleton Wrapper). Drag your Kinect_Prefab from your CURRENT SCENE into this variable. This gives us a pointer to the main update script.
Now, expand your model fully so that each bone is visible in the hierarchy.
One by one, drag each bone you want to control onto the appropriate variable. Make sure to double-check the bones, as it's very easy to place it incorrectly.
After all the bones are placed, set the player that should control this model. 0 is player one, and 1 is player two.
Next, determine whether you want the entire skeleton to be affected by the Kinect (or just some parts) and set the appropriate mask. If you don't see one that you want, write your own!
If you intend to animate your model while the player is controlling it, set "animated" flag and determine how much blending between the animation and the Kinect should occur - this is a range from 0 to 1.
KinectPointController
To set this up, do the following:
Create an empty object to store each of the objects you want to control.
Parent each of the objects into this empty node, and place them all at the same location.
Attach the KinectPointController to the parent node.
Select the parent node, and find the Sw variable (it stands for Skeleton Wrapper). Drag your Kinect_Prefab from your CURRENT SCENE into this variable. This gives us a pointer to the main update script.
Drag each of the objects you want to be controlled into the appropriate variable.
Set the player number (0 = player one, 1 = player two) and the Mask.
