import os
import shutil
import socket
from flask import Flask, send_file, abort
from tkinter import Tk, filedialog
from werkzeug.utils import secure_filename
import sys

# Directory to store shared files
def resource_path(relative_path):
    """ Get the absolute path to a resource, works for dev and for PyInstaller """
    try:
        base_path = sys._MEIPASS
    except AttributeError:
        base_path = os.path.dirname(__file__)
    return os.path.join(base_path, relative_path)

SHARED_FOLDER = resource_path("SharedFiles")

# Flask app
app = Flask(__name__)

# Ensure the shared files directory exists
if not os.path.exists(SHARED_FOLDER):
    os.makedirs(SHARED_FOLDER)

def get_local_ip():
    try:
        s = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
        s.connect(('8.8.8.8', 80))
        local_ip = s.getsockname()[0]
        s.close()
        return local_ip
    except Exception as e:
        print(f"Error getting local IP: {e}")
        return None

def generate_url(filename):
    local_ip = get_local_ip()
    if local_ip:
        download_url = f"http://{local_ip}:5000/download/{filename}"
        print(f"Download URL: {download_url}")
        return download_url
    else:
        return "Unable to determine local IP address", 500

def select_file():
    root = Tk()
    root.withdraw()  # Hide the root window
    file_path = filedialog.askopenfilename()

    if file_path:
        filename = secure_filename(os.path.basename(file_path))
        shutil.copy(file_path, os.path.join(SHARED_FOLDER, filename))
        print(f"File '{filename}' copied to SharedFiles.")
        root.destroy()  # Ensure the root window is destroyed
        return filename
    else:
        print("No file selected.")
        return None
    
    

@app.route(f"/download/<filename>")
def download_file(filename):
    file_path = os.path.join(SHARED_FOLDER, filename)
    try:
        return send_file(file_path, as_attachment=True)
    except FileNotFoundError:
        abort(404)
    except Exception as e:
        return str(e), 500
    

if __name__ == '__main__':
    selected_filename = select_file()

    if selected_filename:
        generate_url(selected_filename)
        app.run(host='0.0.0.0', port=5000, debug=True)
    else:
        print("No file to share.")
