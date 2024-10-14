from flask import Flask, request, render_template, redirect, url_for, session, make_response
import sqlite3
import os
from Crypto.Cipher import AES
from Crypto.Util.Padding import pad, unpad
from Crypto.Random import get_random_bytes
import base64
import hashlib

flag='barrierflag'
app = Flask(__name__)
app.secret_key = 'ADJKAHKDHAKD'

basepath = os.path.abspath(os.path.dirname(__file__))
db_path = os.path.join(basepath, 'employees.db')

def md5_hash(input_string):
    md5 = hashlib.md5()
    md5.update(input_string.encode('utf-8'))
    hashed_value = md5.hexdigest()    
    return hashed_value

def aes_encrypt(plaintext, key):
    if len(key) != 32:
        raise ValueError("Key must be 32 bytes long (256 bits).")    
    if isinstance(key, str):
        key = key.encode('utf-8')
    cipher = AES.new(key, AES.MODE_ECB)
    padded_plaintext = pad(plaintext.encode('utf-8'), AES.block_size)
    encrypted_bytes = cipher.encrypt(padded_plaintext)
    encrypted_text = base64.b64encode(encrypted_bytes).decode('utf-8')
    
    return encrypted_text

def init_db():
    conn = sqlite3.connect(db_path)
    cur = conn.cursor()

    cur.execute("CREATE TABLE IF NOT EXISTS users (id INTEGER PRIMARY KEY, username TEXT, password TEXT)")
    cur.execute("CREATE TABLE IF NOT EXISTS employees (id INTEGER PRIMARY KEY, Name TEXT NOT NULL, Position TEXT, Department TEXT, HireDate DATE NOT NULL, Salary INTEGER)")

    cur.execute("SELECT COUNT(*) FROM users")
    if cur.fetchone()[0] == 0:
        cur.execute("INSERT INTO users (username, password) VALUES (?, ?)", ('admin', ')tF@Dw!n'))

    cur.execute('SELECT COUNT(*) FROM employees')
    if cur.fetchone()[0] == 0:
        # Insert employee data
        employee_data = [
            (1, 'Alice Johnson', 'Manager', 'Sales', '2023-06-15', 80000),
            (2, 'Bob Smith', 'Developer', 'IT', '2022-03-12', 70000),
            (3, 'Charlie Brown', 'Designer', 'Marketing', '2021-08-22', 65000),
            (4, 'David Lee', 'Analyst', 'Finance', '2023-01-10', 72000),
            (5, 'Emma Wilson', 'Developer', 'IT', '2022-07-01', 74000),
            (6, 'Frank Harris', 'Manager', 'HR', '2021-05-19', 78000),
            (7, 'Grace Lee', 'Designer', 'Marketing', '2020-12-05', 68000),
            (8, 'Henry Clark', 'Analyst', 'Finance', '2022-02-21', 71000),
            (9, 'Ivy Moore', 'Developer', 'IT', '2023-03-13', 73000),
            (10, 'Jack Thomas', 'Manager', 'Operations', '2021-09-15', 77000),
            (11, 'Karen Martinez', 'Analyst', 'Finance', '2022-08-30', 69000),
            (12, 'Liam Anderson', 'Developer', 'IT', '2021-10-11', 72000),
            (13, 'Maria Rodriguez', 'Designer', 'Marketing', '2022-05-04', 66000),
            (14, 'Noah Adams', 'Manager', 'Sales', '2023-01-22', 82000),
            (15, 'Olivia Taylor', 'Analyst', 'HR', '2021-07-14', 70000),
            (16, 'Paul Jackson', 'Developer', 'IT', '2023-04-18', 75000),
            (17, 'Quinn Harris', 'Designer', 'Marketing', '2021-06-21', 67000),
            (18, 'Rachel Lewis', 'Manager', 'Operations', '2022-11-16', 79000),
            (19, 'Samuel Walker', 'Analyst', 'Finance', '2023-02-25', 72000),
            (20, 'Tina Robinson', 'Developer', 'IT', '2022-12-03', 76000),            
        ]

        # Insert data into the table
        cur.executemany("""
        INSERT INTO employees (id, Name, Position, Department, HireDate, Salary)
        VALUES (?, ?, ?, ?, ?, ?)
        """, employee_data)

    conn.commit()
    conn.close()


@app.route('/')
def index():
    if 'logged_in' in session:
        return redirect(url_for('dashboard'))
    return redirect(url_for('login'))

@app.route('/login', methods=['GET', 'POST'])
def login():
    if request.method == 'POST':
        username = request.form['username']
        password = request.form['password']
        
        conn = sqlite3.connect(db_path)        
        cur = conn.cursor()
        query = f"SELECT * FROM users WHERE username = '{username}' AND password = '{password}'"
        cur.execute(query)
        user = cur.fetchone()
        conn.close()

        if user:
            session['logged_in'] = True
            session['username'] = username
            
            hash_key=md5_hash(username)
            fg=aes_encrypt(flag,hash_key)

            user_agent = request.headers.get('User-Agent')
            
            response = make_response(redirect(url_for('employee_list')))
            if user_agent == 'ctfbrowser':
                # response = make_response(redirect(url_for('dashboard')))
                session['ctf-browser']=True  
                session['hash-key']=hash_key             
                response.headers['X-Flag'] = fg
                return response
            
            return response
        else:
            return render_template('login.html', error='Invalid credentials.')
    
    return render_template('login.html')

@app.route('/logout')
def logout():
    session.clear()
    return redirect(url_for('login'))

@app.route('/dashboard')
def dashboard():
    if 'logged_in' not in session:
        return redirect(url_for('login'))
    ctf_browser=session.get('ctf-browser')
    hash_key=session.get('hash-key')
    return render_template('dashboard.html',ctf_browser=ctf_browser,hash_key=hash_key)

@app.route('/employees')
def employee_list():
    if 'logged_in' not in session:
        return redirect(url_for('login'))
    
    ctf_browser=session.get('ctf-browser')
    conn = sqlite3.connect(db_path)    
    cur = conn.cursor()
    cur.execute('SELECT * FROM employees')
    employees = cur.fetchall()
    conn.close()
    print(employees)
    return render_template('employees.html', employees=employees,ctf_browser=ctf_browser)

if __name__ == '__main__':
    init_db()
    app.run(host='0.0.0.0', port=5000)
