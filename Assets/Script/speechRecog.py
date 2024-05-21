import speech_recognition as sr
import pyttsx3
import pyaudio
import socket

sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
serverAddressPort = ("127.0.0.1", 5052)

r = sr.Recognizer()

# # List audio devices that are available in the device
# p = pyaudio.PyAudio()
# info = p.get_host_api_info_by_index(0)
# numdevices = info.get('deviceCount')

# for i in range(0, numdevices):
#     if (p.get_device_info_by_host_api_device_index(0, i).get('maxInputChannels')) > 0:
#         print("Input Device id ", i, " - ", p.get_device_info_by_host_api_device_index(0, i).get('name'))

while True:
    # Mic dengan index ke-x
    with sr.Microphone(device_index=0) as source2:
        # White noise addition
        r.adjust_for_ambient_noise(source2, duration=0.2)
        
        # Listen from audio source
        audio = r.listen(source2)
        
        # Print & send text dari google API
        try:
            Text = str(r.recognize_google_cloud(audio, credentials_json="pathkefile.json", language="id-ID")).lower()
            
            # Yg bisa: A-C, G, K-N, P-T, V-Z
            # dengan catatan: K = Ka, Q = Ki, S = Es, V = Fe, W = Why
            if (Text == "ka"):
                Text = "k"
            elif (Text == "ki"):
                Text = "q"
            elif (Text == "es"):
                Text = "s"
            elif (Text == "fe"):
                Text = "v"
            elif (Text == "why"):
                Text = "w"
            
            print(Text)
            sock.sendto(str.encode(Text), serverAddressPort)
            
        # Catch errors if there are any    
        except sr.UnknownValueError:
            print("Google speech recognition could not understand audio")
            
        except sr.RequestError as e:
            print(f"Could not request results from google speech recognition service; {e}")