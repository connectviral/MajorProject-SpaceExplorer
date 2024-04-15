import cv2
import mediapipe as mp
import socket
import numpy as np

mp_hands = mp.solutions.hands
hands = mp_hands.Hands()
mp_drawing = mp.solutions.drawing_utils

# Create a TCP/IP socket
server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server_socket.bind(('localhost', 12345))
server_socket.listen(1)
print("Server is listening...")

# Accept a single connection and make a file-like object out of it
connection, address = server_socket.accept()
print(f"Connection from {address} established.")

while True:
    # Receive the frame size
    size = int.from_bytes(connection.recv(4), byteorder='big')

    # Receive the frame data
    data = b''
    while len(data) < size:
        packet = connection.recv(size - len(data))
        if not packet:
            break
        data += packet

    # Convert the received data into a numpy array
    frame = np.frombuffer(data, dtype=np.uint8)

    # Decode the frame
    frame = cv2.imdecode(frame, cv2.IMREAD_COLOR)

    # Process the frame using MediaPipe Hands
    rgb_frame = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
    results = hands.process(rgb_frame)

    if results.multi_hand_landmarks:
        for landmarks in results.multi_hand_landmarks:
            handedness = results.multi_handedness[results.multi_hand_landmarks.index(landmarks)].classification[0].label
            mp_drawing.draw_landmarks(frame, landmarks, mp_hands.HAND_CONNECTIONS)

            thumb_tip = landmarks.landmark[mp_hands.HandLandmark.THUMB_TIP]
            index_tip = landmarks.landmark[mp_hands.HandLandmark.INDEX_FINGER_TIP]
            middle_tip = landmarks.landmark[mp_hands.HandLandmark.MIDDLE_FINGER_TIP]
            ring_tip = landmarks.landmark[mp_hands.HandLandmark.RING_FINGER_TIP]
            pinky_tip = landmarks.landmark[mp_hands.HandLandmark.PINKY_TIP]

            if handedness == "Left" and thumb_tip.y > middle_tip.y and index_tip.y > middle_tip.y and ring_tip.y > middle_tip.y and pinky_tip.y > middle_tip.y:
                command = 'space'
                connection.send(command.encode())

            # Check for right hand gestures
            if handedness == "Right":
                # Check for right gesture
                if thumb_tip.y > index_tip.y and thumb_tip.y > middle_tip.y and thumb_tip.y > ring_tip.y and thumb_tip.y > pinky_tip.y:
                    command = 'right'
                    connection.send(command.encode())
                # Check for left gesture
                elif thumb_tip.y < index_tip.y and thumb_tip.y < middle_tip.y and thumb_tip.y < ring_tip.y and thumb_tip.y < pinky_tip.y:
                    command = 'left'
                    connection.send(command.encode())

    # Display the frame
    cv2.imshow('MediaPipe Hands', frame)
    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

# Clean up
connection.close()
server_socket.close()
cv2.destroyAllWindows()
