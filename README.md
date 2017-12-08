# Mouse-Cam

Emulates mouse functions using Camera and Hand Gestures. Uses EmguCV and Accord.NET.

# IDE Used
- **Visual Studio 2015**

# Dependencies
You can acquire these dependencies by using **NuGet Package Manager**.
- **Accord.3.8.0**
- **Accord.MachineLearning.3.8.0**
- **Accord.Math.3.8.0**
- **Accord.Statistics.3.8.0**
- **EMGU.CV.3.3.0.2824**
- **ZedGraph.5.1.5**

# Overview 

**This is done by the following steps**

1) Loads necessary data for the following:
      - Look Up Table for Skin Color Detection **(HSV Color Space)**<sup>[1]</sup>. 
      - **Support Vector Machine (SVM)**<sup>[2]</sup> Model for classifying the differences between **Actual Hand, Arm, and Head** contours. Trained using **Hu-Moments** as training dataset.
      - **K-Nearest Neighbor (KNN)**<sup>[3]</sup> Model for classifying specific
        Hand Gestures such as **Open, Up, Down, Left,** and **Right**. 
        Trained using their **Hand Convex Defects** training dataset.

2) Start the image acquisition from the camera.
3) Apply Look Up Table based Skin Detection on each pixel of 
   the acquired image frame. This will generate a **Binary Image**
   that contains Skin Image Blobs.

   ![Fig. 1 - Generated Skin Detection Binary Image](https://github.com/jumzzz/Mouse-Cam/blob/master/Mouse-Cam/mouse_cam_images/skin_detection.png?raw=true)
    *Fig. 1 - Generated Skin Detection Binary Image*
     
4) Find the ROI of each contour of blobs. For simplification, it's assumed that Hand is Located in the largest contour blobs.
5) After finding the largest blob, compute its **Hu-Moment** and classify
   it with **SVM**. 
   - If it's classified as the head, return to step **3**.
   - If it's classified as arm, localize its hand then proceed to step 
     **6**.
   - If it's classified as hand, proceed to step to step **6**.
6) After localizing the hand, recompute its Convex Defects<sup>[4]</sup> and classify
   it using KNN in order to distinguish gestures with the following

   ![Fig. 2 - "Open" gesture](https://github.com/jumzzz/Mouse-Cam/blob/master/Mouse-Cam/mouse_cam_images/open_gesture.png?raw=true)
    *Fig. 2 - **Open** gesture classification*

   ![Fig. 3 - "Left" gesture classification](https://github.com/jumzzz/Mouse-Cam/blob/master/Mouse-Cam/mouse_cam_images/left_gesture.png?raw=true)
    *Fig. 3 - **Left** gesture classification*
    
   ![Fig. 4 - "Right" gesture classification](https://github.com/jumzzz/Mouse-Cam/blob/master/Mouse-Cam/mouse_cam_images/right_gesture.png?raw=true)
    *Fig. 4 - **Right** gesture classification*
    
   ![Fig. 5 - "Up" gesture classification](https://github.com/jumzzz/Mouse-Cam/blob/master/Mouse-Cam/mouse_cam_images/up_gesture.png?raw=true)
    *Fig. 5 - **Up** gesture classification*
    
   ![Fig. 6 - "Down" gesture classification](https://github.com/jumzzz/Mouse-Cam/blob/master/Mouse-Cam/mouse_cam_images/down_gesture.png?raw=true)
    *Fig. 6 - **Down** gesture classification*

7) Then proceed to trigger mouse functions programmatically
   - If classified as **Open** perform moving the mouse cursor.
   - If classified as **Left** perform a left click.
   - If classified as **Right** perform a right click.
   - If classified as **Up** perform scroll-up.
   - If classified as **Down** perform scroll-down.
8) Repeat step **3** until image acquisition is stopped.


# References:
1) [Statistical Color Models with Application to Skin Detection](http://www.hpl.hp.com/techreports/Compaq-DEC/CRL-98-11.pdf)
2) [Support Vector Machines Wiki](https://en.wikipedia.org/wiki/Support_vector_machine)
3) [K-Nearest Neighbor Wiki](https://en.wikipedia.org/wiki/K-nearest_neighbors_algorithm)
4) [Hand Tracking and Recognition With OpenCV](http://simena86.github.io/blog/2013/08/12/hand-tracking-and-recognition-with-opencv/)
