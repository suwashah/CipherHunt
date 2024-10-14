import sqlite3
from flask import g
import os
import hashlib
import hmac
import base64
import random

SECRET_KEY = 'Cwallet_secret'

# Get the absolute path to the database file
basepath = os.path.abspath(os.path.dirname(__file__))
db_path = os.path.join(basepath, '../cwallet.db')

def get_db():
    # Check if 'db' is already in the app context (g)
    if 'db' not in g:
        g.db = sqlite3.connect(db_path)
        g.db.row_factory = sqlite3.Row  # Allows dictionary-like access to row columns
    return g.db

def close_db(e=None):
    db = g.pop('db', None)
    if db is not None:
        db.close()

def create_user(username, hashed_password):
    db = get_db()
    db.execute('INSERT INTO users (username, password) VALUES (?, ?)', (username, hashed_password))
    db.commit()

def get_user(username):
    db = get_db()
    return db.execute('SELECT id, username, balance FROM users WHERE username = ?', (username,)).fetchone()

def list_users():
    db = get_db()
    return db.execute('SELECT id, username, balance FROM users').fetchall()

def get_user_by_id(user_id):
    db = get_db()
    return db.execute('SELECT id, username, balance FROM users WHERE id = ?', (user_id,)).fetchone()

def transfer_money(from_user, to_user, amount):
    db = get_db()
    db.execute('UPDATE users SET balance = balance - ? WHERE username = ?', (amount, from_user))
    db.execute('UPDATE users SET balance = balance + ? WHERE username = ?', (amount, to_user))
    db.execute('INSERT INTO transactions (sender_id, receiver_id, amount, transaction_type) VALUES (?, ?, ?, ?)',
               (get_user(from_user)[0], get_user(to_user)[0], amount, 'transfer'))
    db.commit()

def list_transactions():
    db = get_db()
    return db.execute('''
        SELECT t.id, u1.username AS sender, u2.username AS receiver, t.amount, t.transaction_type
        FROM transactions t
        LEFT JOIN users u1 ON t.sender_id = u1.id
        LEFT JOIN users u2 ON t.receiver_id = u2.id
    ''').fetchall()

# Generate a token based on the username and a secret key
def generate_token(username):
    return base64.urlsafe_b64encode(hmac.new(SECRET_KEY.encode(), username.encode(), hashlib.sha256).digest()).decode()

# Verify the token for authenticity
def verify_token(username, token):
    expected_token = generate_token(username)
    return hmac.compare_digest(expected_token, token)


def generate_secret_key(username):
    random_number = str(random.randint(1000, 9999))
    secret_key = f"{username}_{random_number}"
    return secret_key