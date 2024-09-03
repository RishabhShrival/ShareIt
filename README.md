# File Transfer Over Local Wi-Fi

This project enables users to transfer files over a local Wi-Fi network using a simple and user-friendly interface built with Python and .NET. The repository contains:
- A Python file that sets up a file-sharing service using Flask.
- A built executable (.exe) version of the Python program (requires administrative rights).
- A C# Windows Forms application developed in .NET for file transfer.

**Note**: The executable and .NET application require administrative rights to run properly, as they need access to network resources for obtaining local IP addresses and serving files.

## Workflow

1. **Get Local IP Address**  
   The program retrieves the local IP address of the machine by creating a socket connection to Google's DNS (8.8.8.8). This ensures that the correct IP within the Wi-Fi network is obtained.

2. **File Selection**  
   The user selects a file via a file dialog interface. The selected file is then copied to a folder named `SharedFiles` within the application's directory.

3. **URL Generation**  
   After the file is copied, the program generates a URL in the following format:  
   `http://<local_ip>:5000/download/<filename>`.  
   This URL can be accessed by any device on the same Wi-Fi network to download the selected file.

4. **File Transfer**  
   The Flask server listens for incoming requests and serves the file over HTTP. When the generated URL is accessed from another device, the file is transferred directly from the local machine to the requesting device.

## Protocols Used

- **HTTP (Hypertext Transfer Protocol)**:  
  The Flask server listens on the local IP address and serves files using HTTP over port 5000. This protocol facilitates the file transfer within the local network.
  
- **TCP/IP (Transmission Control Protocol/Internet Protocol)**:  
  TCP/IP is used for network communication between devices. The IP address allows the request to be routed to the correct machine, and TCP ensures reliable delivery of the file.

## Components Overview

1. **Python Code**  
   The core functionality of file selection, URL generation, and file serving is implemented in Python using the Flask web framework.

2. **Executable (.exe)**  
   The Python script is compiled into an executable using PyInstaller. The executable allows the program to run on Windows without requiring Python to be installed.

3. **C# Windows Forms Application**  
   A .NET-based GUI application that provides a native Windows interface for file transfer, making it easier for users who prefer a more integrated experience.

## Getting Started

To get started with the project:
1. **Clone the repository**  
   ```bash
   git clone https://github.com/RishabhShrival/ShareIt
   cd <repository-directory>
   ```
2. **Make sure Administration Permission allowed**
   run as Admin
3. **Ensure all devices are connected to same wifi**
4. **Sometimes browser block downloading in client because of security reasons**

## Images
Windows Form App
![Screenshot 2024-09-03 163627](https://github.com/user-attachments/assets/1e32b1ee-0256-43f5-b75d-b6493cc41f4c)

Python exe 
![Screenshot 2024-09-03 164018](https://github.com/user-attachments/assets/b2d4ee90-fe7c-45e3-9f21-b03aa94c862f)


