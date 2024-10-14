from flask import Flask, render_template, request, redirect, url_for, jsonify
import jwt
from datetime import datetime, timedelta
import os
import hashlib
app = Flask(__name__)

flag="key_to_access_flag"
users_db = {
    'Sup3rAdm1n_Pro': hashlib.sha256(b'Sup3rP@ssw0rd').hexdigest(),  # Admin user
    'john': hashlib.sha256(b'John@123').hexdigest()
}

basepath = os.path.abspath(os.path.dirname(__file__))
key_path = os.path.join(basepath, 'keys')
private_key_path = os.path.join(key_path, 'private.pem')
public_key_path = os.path.join(key_path, 'public.pem')
print(private_key_path)
secret_key="U3VwM3JTZWNyZXRLZXk="

with open(private_key_path, 'rb') as f:
    private_key = f.read()

def create_jwt_token(username,user_role):
    payload = {
        'user': username,
        'role':user_role,
        'exp': datetime.utcnow() + timedelta(minutes=5)
    }
    token = jwt.encode(payload, secret_key, algorithm='HS256')
    return token

def verify_jwt_token(token):
    try:
        unverified_header = jwt.get_unverified_header(token)        
        if unverified_header.get('alg') == 'none':
            decoded = jwt.decode(token, options={"verify_signature": False})
        else:
            decoded = jwt.decode(token, secret_key, algorithms=['HS256'])
        return decoded
    except jwt.InvalidTokenError:
        return None

def hash_password(password):
    return hashlib.sha256(password.encode()).hexdigest()

@app.route('/')
def home():
    return redirect(url_for('login'))

@app.route('/login', methods=['GET', 'POST'])
def login():
    if request.method == 'POST':
        username = request.form.get('username')
        password = request.form.get('password')

        hashed_password = hash_password(password)
        if username in users_db and users_db[username] == hashed_password:
            if username == 'Sup3rAdm1n_Pro':
                user_role = 'admin'
            else:
                user_role = 'user'
            token = create_jwt_token(username,user_role)            
            response = redirect(url_for('dashboard'))
            response.set_cookie('token', token)
            
            return response
        else:
            return "Invalid username or password", 403

    return render_template('login.html')
@app.route('/dashboard')
def dashboard():
    token = request.cookies.get('token')
    decoded_token = verify_jwt_token(token)
    
    if decoded_token:
        user=decoded_token['user']
        role = decoded_token['role']
        return render_template('dashboard.html',user=user, role=role)
    
    return redirect(url_for('login'))
@app.route('/admin/flag')
def admin_flag():
    token = request.cookies.get('token')
    decoded_token = verify_jwt_token(token)
    if decoded_token and decoded_token['role'] == 'admin':
        return render_template('admin.html', flag=flag)
    
    return "Unauthorized access. Only admins can view the flag.", 403

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000)