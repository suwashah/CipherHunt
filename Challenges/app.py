from flask import Flask, request, jsonify
import os
import subprocess
import threading
import time
from flask_httpauth import HTTPBasicAuth
from flask_cors import CORS
app = Flask(__name__)
auth = HTTPBasicAuth()
CORS(app, origins=["http://localhost:1800","http://cipherhunt.com"])
# Define a dictionary of users for basic authentication
users = {
    "AppUser": "Nepal@123"  # Replace with your own username and password
}

# Verify the username and password
@auth.verify_password
def verify_password(username, password):
    if username in users and users[username] == password:
        return username

# Function to run Docker Compose in the specified folder
def run_docker_compose(parent_folder, foldername):
    # Construct the absolute target folder path
    target_folder = os.path.abspath(os.path.join(parent_folder, foldername))
    print("Docker folder", target_folder)
    try:
        # Build and start Docker Compose in the target folder
        subprocess.run(['docker-compose', 'up', '--build', '-d'], cwd=target_folder, check=True)

        # Sleep for 30 minutes (1800 seconds)
        time.sleep(1800)

        # Stop and remove the Docker containers after 30 minutes
        subprocess.run(['docker-compose', 'down'], cwd=target_folder, check=True)

    except subprocess.CalledProcessError as e:
        print(f"Error occurred: {e}")

# API route to start the application
@app.route('/start', methods=['POST'])
@auth.login_required
def start_application():
    data = request.json

    # Extract the difficulty and folder name from the POST request
    difficulty = data.get('difficulty')
    foldername = data.get('foldername')

    # Validate inputs
    if not difficulty or not foldername:
        return jsonify({'error': 'Difficulty and folder name are required.'}), 400

    # Construct the absolute path of the target folder
    parent_folder = os.path.abspath(difficulty)
    target_folder = os.path.abspath(os.path.join(parent_folder, foldername))
    print("Target folder:", target_folder)

    # Check if the target folder exists
    if not os.path.isdir(target_folder):
        return jsonify({'code':'100','message': 'Invalid folder path or folder does not exist.'}), 400

    # Start the Docker Compose build script in a separate thread
    thread = threading.Thread(target=run_docker_compose, args=(parent_folder, foldername,))
    thread.start()

    return jsonify({'code':'0','message': f'Application in {difficulty}/{foldername} is running for 2 minutes.'}), 200

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000)
