import cv2
import mediapipe as mp
import socket

mp_hands = mp.solutions.hands
hands = mp_hands.Hands()
mp_drawing = mp.solutions.drawing_utils

server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server_socket.bind(('localhost', 12345))
server_socket.listen(1)
print("Server is listening...")

conn, addr = server_socket.accept()
print(f"Connection from {addr} established.")

cap = cv2.VideoCapture(0)

def send_data(data):
    try:
        conn.send(data.encode())
    except ConnectionResetError:
        print("Connection was reset by the client")
        return False
    return True

try:
    prev_gesture = 'stop'
    while True:
        ret, frame = cap.read()
        frame = cv2.flip(frame, 1)
        rgb_frame = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
        results = hands.process(rgb_frame)

        if results.multi_hand_landmarks:
            for landmarks in results.multi_hand_landmarks:
                handedness = results.multi_handedness[results.multi_hand_landmarks.index(landmarks)].classification[0].label
                mp_drawing.draw_landmarks(frame, landmarks, mp_hands.HAND_CONNECTIONS)

                thumb = landmarks.landmark[mp_hands.HandLandmark.THUMB_TIP]
                index = landmarks.landmark[mp_hands.HandLandmark.INDEX_FINGER_TIP]
                middle = landmarks.landmark[mp_hands.HandLandmark.MIDDLE_FINGER_TIP]
                ring = landmarks.landmark[mp_hands.HandLandmark.RING_FINGER_TIP]
                pinky = landmarks.landmark[mp_hands.HandLandmark.PINKY_TIP]

                # Right hand gestures
                if handedness == "Right":
                    # Right movement
                    if thumb.x < index.x and thumb.x < middle.x and thumb.x < ring.x and thumb.x < pinky.x:
                        send_data('left')
                        prev_gesture = 'left'
                    # Left movement
                    elif thumb.x > index.x and thumb.x > middle.x and thumb.x > ring.x and thumb.x > pinky.x:
                        send_data('right')
                        prev_gesture = 'right'

                # Left hand gesture
                if handedness == "Left":
                    # Jump
                    if thumb.x < index.x and thumb.x > pinky.x:
                        send_data('space')
                        prev_gesture = 'jump'
                    # Idle
                    else:
                        send_data('stop')
                        prev_gesture = 'stop'
        else:
            # If no hand gestures detected, stop the player
            if prev_gesture != 'stop':
                send_data('stop')
                prev_gesture = 'stop'

except Exception as e:
    print(f"An error occurred: {e}")

finally:
    cap.release()
    server_socket.close()
