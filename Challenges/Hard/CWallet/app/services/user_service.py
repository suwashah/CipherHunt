# services/user_service.py
from models.models import create_user, get_user, list_users, get_user_by_id
import sqlite3
import hashlib
def create_user_service(username, password):
    hashed_password = hashlib.sha256(password.encode()).hexdigest()
    try:
        create_user(username, hashed_password)
    except sqlite3.IntegrityError:
        raise ValueError('Username already exists')

def login_user_service(username, password):
    user = get_user(username)
    if user:
        hashed_password = hashlib.sha256(password.encode()).hexdigest()
        return hashed_password == user[1]
    return False
