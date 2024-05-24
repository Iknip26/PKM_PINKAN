# from cvzone.HandTrackingModule import HandDetector
# import cv2
# import socket
# import pickle

# # Inisialisasi Video Capture
# cap = cv2.VideoCapture(0)
# cap.set(3, 1280)
# cap.set(4, 720)
# success, img = cap.read()
# h, w, _ = img.shape

# # Inisialisasi Hand Detector
# detector = HandDetector(detectionCon=0.8, maxHands=2)

# # Inisialisasi UDP Socket
# sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
# serverAddressPort = ("127.0.0.1", 5056)

# while True:
#     # Dapatkan frame dari kamera
#     success, img = cap.read()
#     if not success:
#         print("Failed to capture image")
#         break

#     # Temukan tangan dan landmark-nya
#     hands, img = detector.findHands(img)  # dengan gambar

#     data = []
#     if hands:
#         # Hand 1
#         hand = hands[0]
#         lmList = hand["lmList"]  # List of 21 Landmark points
#         for lm in lmList:
#             data.extend([lm[0], h - lm[1], lm[2]])
    
#     # Kompresi gambar menjadi format JPEG
#     _, buffer = cv2.imencode('.jpg', img)

#     # Gabungkan data landmarks dan gambar
#     message = {
#         'landmarks': data,
#         'image': buffer.tobytes()
#     }

#     # Serialisasi pesan menggunakan pickle
#     message_pickle = pickle.dumps(message)
    
#     # Kirim pesan
#     sock.sendto(message_pickle, serverAddressPort)

#     # Tampilkan gambar
#     cv2.imshow("Image", img)

#     # Jeda waktu singkat untuk menghindari penggunaan CPU berlebihan
#     if cv2.waitKey(1) & 0xFF == ord('q'):
#         break

# # Melepas kamera dan menutup jendela OpenCV
# cap.release()
# cv2.destroyAllWindows()
# sock.close()

from cvzone.HandTrackingModule import HandDetector
import cv2
import socket

cap = cv2.VideoCapture(0)
cap.set(3, 1280)
cap.set(4, 720)
success, img = cap.read()
h, w, _ = img.shape
detector = HandDetector(detectionCon=0.8, maxHands=2)

sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
serverAddressPort = ("127.0.0.1", 5056)

while True:
    
    # Get image frame
    success, img = cap.read()
    # Find the hand and its landmarks
    hands, img = detector.findHands(img)  # with draw
    # hands = detector.findHands(img, draw=False)  # without draw
    data = []

    if hands:
        # Hand 1
        hand = hands[0]
        lmList = hand["lmList"]  # List of 21 Landmark points
        for lm in lmList:
            data.extend([lm[0], h - lm[1], lm[2]])

        sock.sendto(str.encode(str(data)), serverAddressPort)

    # Display
    cv2.imshow("Image", img)
    cv2.waitKey(1)
