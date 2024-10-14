import sqlite3
from flask import Flask, render_template, request, redirect, url_for, session, g
import os
import re
ctf_flag='eclipsedFlag'
app = Flask(__name__)
app.secret_key = 'ADJKAHKDHAKD'

basepath = os.path.abspath(os.path.dirname(__file__))
db_path = os.path.join(basepath, 'eshopping.db')

def get_db():
    db = getattr(g, '_database', None)
    if db is None:
        db = g._database = sqlite3.connect(db_path)
    return db

@app.teardown_appcontext
def close_connection(exception):
    db = getattr(g, '_database', None)
    if db is not None:
        db.close()

@app.route('/')
def index():
    if 'username' in session:
        return redirect(url_for('product_list'))
    return redirect(url_for('login'))

@app.route('/login', methods=['GET', 'POST'])
def login():
    if request.method == 'POST':
        username = request.form['username']
        password = request.form['password']
        
        conn = get_db()
        cursor = conn.cursor()
        cursor.execute("SELECT * FROM users WHERE username=? AND password=?", (username, password))
        user = cursor.fetchone()

        if user:
            session['username'] = username
            return redirect(url_for('product_list'))
        else:
            return render_template('login.html', error="Invalid credentials")
    return render_template('login.html')

@app.route('/logout')
def logout():
    session.pop('username', None)
    return redirect(url_for('login'))

@app.route('/products')
def product_list():
    if 'username' not in session:
        return redirect(url_for('login'))

    conn = get_db()
    cursor = conn.cursor()
    cursor.execute("SELECT * FROM products")
    products = cursor.fetchall()
    print(products)
    
    return render_template('product_list.html', products=products)

@app.route('/products/<int:product_id>', methods=['GET', 'POST'])
def product_detail(product_id):
    if 'username' not in session:
        return redirect(url_for('login'))

    conn = get_db()
    cursor = conn.cursor()
    cursor.execute("SELECT * FROM products WHERE id=?", (product_id,))
    product = cursor.fetchone()
    print(product)
    if not product:
        return "Product not found", 404
    
    if request.method == 'POST':
        new_comment = request.form['comment']
        xssFlag="Try xss attack"        
        username=session["username"]
        cursor.execute("INSERT INTO comments (product_id, comment, username) VALUES (?, ?, ?)", (product_id, new_comment, username ))
        conn.commit()
        return redirect(url_for('product_detail', product_id=product_id))

    cursor.execute("SELECT * FROM comments WHERE product_id=? order by 1 desc", (product_id,))
    comments = cursor.fetchall()
    print(comments)
    return render_template('product_detail.html', product=product, comments=comments)

@app.route('/get_username')
def get_username():
    if 'username' in session:
        return session["username"]
    else:
        return "Access denied"

@app.route('/get_adusername')
def get_adusername():
    if 'username' in session:
        return session["username"]+":"+ctf_flag
    else:
        return "Access denied"


if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000)
