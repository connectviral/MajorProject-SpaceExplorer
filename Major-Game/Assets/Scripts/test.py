import cv2
import mediapipe as mp
import pyautogui
import socket

mp_hands = mp.solutions.hands
hands = mp_hands.Hands()
mp_drawing = mp.solutions.drawing_utils
screen_width, screen_height = pyautogui.size()

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
    while True:
        ret, frame = cap.read()
        frame = cv2.flip(frame, 1)
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
                    send_data('space')

                if handedness == "Right":
                    if thumb_tip.x > index_tip.x and middle_tip.y > index_tip.y and ring_tip.y > index_tip.y and pinky_tip.y > index_tip.y:
                        pyautogui.press('right')
                    elif thumb_tip.x < index_tip.x and middle_tip.y > index_tip.y and ring_tip.y > index_tip.y and pinky_tip.y > index_tip.y:
                        pyautogui.press('left')
except Exception as e:
    print(f"An error occurred: {e}")

finally:
    cap.release()
    server_socket.close()
    cap.release()

