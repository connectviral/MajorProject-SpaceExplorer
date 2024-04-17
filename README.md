# Unity Game with Gesture Recognition

This project is a Unity game that uses gesture recognition to control the player character. The game captures input from a camera and sends it to a Python script for gesture recognition. The Python script processes the input and sends commands back to the game to control the player character's movement.

## Features

- Gesture recognition using the MediaPipe library in Python
- Real-time camera feed integration in Unity
- Player character movement based on recognized gestures

## Requirements

- Unity 2022.3.16f1 or later
- Python 3. with the following libraries:
  - OpenCV (cv2)
  - Mediapipe (mediapipe)
- Windows operating system (for the provided setup instructions)

## Setup

1. **Clone the Repository:**

   ```bash
   git clone https://github.com/connectviral/MajorProject-SpaceExplorer.git
   ```

2. **Python Setup:**

   - Install the required Python libraries:
     ```bash
     pip install opencv-python mediapipe
     ```

3. **Unity Setup:**

   - Open the Unity project in the Unity editor.
   - Ensure that the `RawImage` component is attached to a GameObject in your scene and is set to display the camera feed.
   - Attach the `CameraAccess.cs` script to the GameObject with the `RawImage` component.

4. **Python Script:**

   - Run the `test.py` Python script to start the server for receiving gesture commands from Unity. // It will start automatically when level is loaded.

5. **Run the Game:**

   - Build and run the Unity game. (Currently it cannot be Build (you can build but the camera cannot be accessed.), but you can play it in the Unity Editor.)
   - Use gestures in front of the camera, Also you can use the Keyboard input (WSAD + ARROWS + SPACE) to control the player character's movement.

## Usage

- Use gestures to control the player character's movement.

## Contributing

Contributions are welcome! Please fork the repository and create a pull request with your changes.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
