from flask import Blueprint, render_template, request, redirect, url_for, session, flash
from models.models import get_db, generate_token, verify_token
import sqlite3

user_bp = Blueprint('user', __name__)

@user_bp.route('/register', methods=['GET', 'POST'])
def register():
    if request.method == 'POST':
        username = request.form['username']
        password = request.form['password']

        db = get_db()
        try:
            db.execute("INSERT INTO users (username, password, balance) VALUES (?, ?, 100)", (username, password))
            db.commit()
            flash('User registered successfully! Please log in.', 'success')
            return redirect(url_for('user.login'))
        except sqlite3.IntegrityError:
            flash('Username already exists.', 'danger')
            return redirect(url_for('user.register'))

    return render_template('user/register.html')


@user_bp.route('/login', methods=['GET', 'POST'])
def login():
    if request.method == 'POST':
        username = request.form['username']
        password = request.form['password']
        if "'" in username:
            return "<h1>STOP TRYING TO HACK US</h1>"
        
        db = get_db()
        user = db.execute("SELECT * FROM users WHERE username = ?", (username,)).fetchone()

        if user and user['password']==password:
            token=generate_token(username)
            print(token)
            session['user_id'] = user['id']
            session['username'] = user['username']
            session['auth_token']=token
            flash('Login successful!', 'success')
            return redirect(url_for('user.dashboard'))
        else:
            flash('Invalid username or password', 'danger')

    return render_template('user/login.html')


@user_bp.route('/dashboard')
def dashboard():
    if 'user_id' not in session:
        flash('Please login first!', 'warning')
        return redirect(url_for('user.login'))
    username = session.get('username')
    token = session.get('auth_token')

    if not username or not token or not verify_token(username, token):
        flash("Authentication failed. Invalid token.")
        return redirect(url_for('user.login'))
    
    # Check for token manipulation by checking token length
    if len(token) != len(generate_token(username)):
        flash(f"Congratulations! You've manipulated the token. Try CSRF attacks which will reveal further step.")

    db = get_db()
    user = db.execute("SELECT * FROM users WHERE id = ?", (session['user_id'],)).fetchone()
    return render_template('user/dashboard.html', user=user)


@user_bp.route('/logout')
def logout():
    session.clear()
    flash('You have been logged out.', 'success')
    return redirect(url_for('user.login'))
