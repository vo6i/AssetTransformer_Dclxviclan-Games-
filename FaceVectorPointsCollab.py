# 1. Устанавливаем библиотеку
!pip install mediapipe opencv-python

import cv2
import mediapipe as mp
import numpy as np

# Инициализация
mp_face_mesh = mp.solutions.face_mesh
face_mesh = mp_face_mesh.FaceMesh(static_image_mode=True, max_num_faces=1)

# Загрузи файл в Colab (слева в панель файлов) и укажи имя тут
VIDEO_PATH = "input_video.mp4" 
cap = cv2.VideoCapture(VIDEO_PATH)

all_frames_data = []

while cap.isOpened():
    success, image = cap.read()
    if not success: break

    results = face_mesh.process(cv2.cvtColor(image, cv2.COLOR_BGR2RGB))

    if results.multi_face_landmarks:
        for face_landmarks in results.multi_face_landmarks:
            frame_points = []
            for lm in face_landmarks.landmark:
                frame_points.append(f"{lm.x:.4f},{1-lm.y:.4f},{lm.z:.4f}")
            all_frames_data.append("|".join(frame_points)) # Сохраняем кадр в одну строку

# Сохраняем все кадры в один файл
with open("sequence_points.txt", "w") as f:
    f.write("\n".join(all_frames_data))

print(f"Обработка завершена. Файл 'sequence_points.txt' готов.")
