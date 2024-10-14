from flask import Flask, request, render_template, redirect, url_for, session, make_response, jsonify
import json
import os
app = Flask(__name__)
app.secret_key = 'XADHJAHDJKDDAK'  # Change this to a strong secret key

# Dummy user credentials for demonstration
users = {
    'guest': {'password': 'guest123#', 'isAdmin': False},
    'admin1': {'password': 'P@ssw0rd!', 'isAdmin': True}
}
flag="treasureflag"
messages=[
    {"date": "2024-08-01", "message": "System updated successfully. Notification has been sent to all admin users."},
    {"date": "2024-08-02", "message": "User 'admin' logged in."},
    {"date": "2024-08-03", "message": "New user 'user1' created."},
    {"date": "2024-08-04", "message": "Password changed for user 'admin'."},
    {"date": "2024-08-05", "message": "System backup completed. "},
    {"date": "2024-08-05", "message": "Hi, Admin. I could not get a flag. Could you please give me some hint to get the flag."},
    {"date": "2024-08-07", "message": "User 'admin' logged in from a new device."},
    {"date": "2024-08-08", "message": "System maintenance scheduled."},
    {"date": "2024-08-09", "message": "User 'user2' attempted to access restricted area."},
    {"date": "2024-08-10", "message": "Security settings updated by 'admin'."}
]

    
@app.route('/')
def home():
    return render_template('home.html')

@app.route('/login', methods=['GET', 'POST'])
def login():
    if request.method == 'POST':
        username = request.form.get('username')
        password = request.form.get('password')
        user=users.get(username)
        if user and user['password'] == password:
            isadmin=user['isAdmin']
            session['username'] = username
            session['admin']=isadmin
            resp = make_response(redirect(url_for('dashboard',a='1' if isadmin==True else '0')))
                
            return resp
        return render_template('login.html', error='Invalid credentials')
    return render_template('login.html')

@app.route('/logout')
def logout():
    session.pop('username', None)
    session.pop('admin', None)
    resp = make_response(redirect(url_for('home')))
    return resp

@app.route('/dashboard')
def dashboard():
    param_value = request.args.get('a', default='0')
    session['ad']=param_value
    if 'username' not in session:
        return redirect(url_for('login'))
    return render_template('dashboard.html', username=session['username'])

@app.route('/admin')
def admin():
    if 'username' not in session:
        return redirect(url_for('login'))
    admin = session['ad']
    if admin == '1':
        return render_template('admin.html',messages=messages, flag=flag, admin=session['admin'])
    else:
        return render_template('access_denied.html',flag="No flag")

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000)
