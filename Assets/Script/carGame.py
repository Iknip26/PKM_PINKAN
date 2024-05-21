from cvzone.PoseModule import PoseDetector
import cv2
import socket

# Set up camera
cap = cv2.VideoCapture(0)
cap.set(3, 1280)
cap.set(4, 720)
success, img = cap.read()
h, w, _ = img.shape

detectorPose = PoseDetector()

# Initialize socket for UDP data trasnfer
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
serverAddressPort = ("127.0.0.1", 5052)
            
while True:
    # Get img frame
    success, img = cap.read()
    
    img = detectorPose.findPose(img)

    lmList, bboxInfo = detectorPose.findPosition(img)

    poseData = []
    pose = ""
    
    # Chekc if the user is leaning to the left or right
    if bboxInfo:
        for lm in lmList:
            poseData.extend([lm[0], h - lm[1], lm[2]])
            right1 = h - lmList[19][1] > h - lmList[9][1] 
            right2 = h - lmList[24][1] > h - lmList[20][1] 
            left1 = h - lmList[20][1] > h - lmList[9][1] 
            left2 = h - lmList[24][1] > h - lmList[19][1]
                
            if (left1 and left2):
                pose = "left"
            elif (right1 and right2):
                pose = "right"
                                    
        poseData.append(pose)
    
    if pose != "":
        print(pose)
        sock.sendto(str.encode(pose),serverAddressPort)
            
    # Display
    cv2.imshow("img", img)
    cv2.waitKey(1)