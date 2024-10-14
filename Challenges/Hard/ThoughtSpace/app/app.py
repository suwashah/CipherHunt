from flask import Flask, render_template, request, redirect, session, url_for, jsonify, flash, make_response
import sqlite3
import os
from werkzeug.utils import secure_filename
app = Flask(__name__)
app.secret_key = 'ZHAGSJGDAKDGAD'
basepath = os.path.abspath(os.path.dirname(__file__))
db_path = os.path.join(basepath, 'ThoughtSpace.db')
image_upload=os.path.join(basepath,'static','blogs')

flag1="decode_fg"
def get_db_connection():
    conn = sqlite3.connect(db_path)
    conn.row_factory = sqlite3.Row
    return conn

@app.route('/')
def index():
    conn = get_db_connection()
    posts = conn.execute('SELECT * FROM posts').fetchall()
    conn.close()
    return render_template('index.html', posts=posts)

@app.route('/post/<int:post_id>')
def post(post_id):
    conn = get_db_connection()
    post = conn.execute('SELECT p.*,u.username from posts p join users u on p.author_id=u.id WHERE p.id = ?', (post_id,)).fetchone()
    comments = conn.execute('SELECT c.*,u.username as author from comments c join users u on c.author_id=u.id where c.post_id= ?', (post_id,)).fetchall()
    print('Comments:',comments)
    conn.close()
    if post is None:
        return 'Post not found!', 404
    return render_template('post.html', post=post, comments=comments)

# Route to add a comment to a post
@app.route('/post/<int:post_id>/comment', methods=['POST'])
def add_comment(post_id):
    if 'user_id' not in session:
        return redirect(url_for('login'))
    
    comment = request.form['comment']
    conn = get_db_connection()
    conn.execute('INSERT INTO comments (post_id, comment, author_id) VALUES (?, ?, ?)', (post_id, comment, session['user_id']))
    conn.commit()
    conn.close()
    response=make_response(redirect(url_for('post', post_id=post_id)))    
    if 'user_role' in session and session['user_role']=='admin':
        response.headers['X-Flag1'] = flag1
    return response


# Login route
@app.route('/login', methods=['GET', 'POST'])
def login():
    if request.method == 'POST':
        username = request.form['username']
        password = request.form['password']
        
        conn = get_db_connection()

        # query = f"SELECT * FROM users WHERE username= ? AND password= ?",username,password
        user = conn.execute(f"SELECT * FROM users WHERE username= ? AND password= ?",(username,password)).fetchone()
        print(user)
        conn.close()
        
        if user:
            session['user_id'] = user['id']
            session['username'] = user['username']
            session['user_role']=user['role']
            return redirect(url_for('index'))
        else:
            flash('Invalid credentials. Please try again.', 'danger')    
    return render_template('login.html')

@app.route('/signup', methods=['GET', 'POST'])
def signup():
    if request.method == 'POST':
        username = request.form['username']
        password = request.form['password']
        email = request.form['email']

        
        conn = get_db_connection()
        try:
            conn.execute('INSERT INTO users (username, password, email) VALUES (?, ?, ?)', 
                         (username, password, email))
            conn.commit()
            flash('Signup successful! You can now log in.', 'success')
            return redirect(url_for('login'))
        except sqlite3.IntegrityError:
            flash('Username already exists!', 'danger')
        finally:
            conn.close()
    
    return render_template('signup.html')

# Logout route
@app.route('/logout')
def logout():
    session.clear()
    return redirect(url_for('index'))

@app.route('/create_post', methods=['GET', 'POST'])
def create_post():
    if 'username' not in session:
        return redirect(url_for('login'))
    
    if request.method == 'POST':
        title = request.form['title']
        content = request.form['content']
        encrypted = 1 if request.form.get('encrypted') == 'on' else 0

        image = request.files.get('image')
        image_filename = None
        
        if image:
            image_filename = secure_filename(image.filename)
            print('Upload folder:',image_upload)
            image_path = os.path.join(image_upload, image_filename)
            image.save(image_path)
        
        conn = get_db_connection()
        conn.execute('INSERT INTO posts (title, content, encrypted, image, author_id) VALUES (?, ?, ?, ?, ?)', (title, content, encrypted, image_filename, session['user_id']))
        conn.commit()
        conn.close()
        
        return redirect(url_for('index'))
    
    return render_template('create_post.html')



@app.route('/api/user/<username>', methods=['GET'])
def get_user_details(username):
    conn = get_db_connection()
    debug=request.headers.get('debug')
    user = conn.execute('SELECT * FROM users WHERE username = ?', (username,)).fetchone()
    print(user)
    conn.close()
    if user:
        if (debug=="true"):
            response=jsonify(dict(user))
            return response
        else:
            return jsonify({
                'username': user['username'],
                'email': user['email'],
                'role': user['role']
            })
    else:
        return jsonify({'error': 'User not found'}), 404

if __name__ == "__main__":
     app.run(host='0.0.0.0', port=5000)
