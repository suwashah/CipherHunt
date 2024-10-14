import sqlite3
import os

basepath = os.path.abspath(os.path.dirname(__file__))
db_path = os.path.join(basepath, 'ThoughtSpace.db')
def init_db():
    conn = sqlite3.connect(db_path)
    cur = conn.cursor()
    # Create users table
    cur.execute('''
    CREATE TABLE IF NOT EXISTS users (
        id INTEGER PRIMARY KEY AUTOINCREMENT,
        username TEXT NOT NULL UNIQUE,
        email TEXT NOT NULL UNIQUE,        
        password TEXT NOT NULL,
        role TEXT DEFAULT 'user',
        created_ts TIMESTAMP DEFAULT CURRENT_TIMESTAMP
    );
    ''')
    
    # Create posts table
    cur.execute('''
    CREATE TABLE IF NOT EXISTS posts (
        id INTEGER PRIMARY KEY AUTOINCREMENT,
        title TEXT NOT NULL,
        image TEXT,
        content TEXT NOT NULL,
        encrypted INTEGER DEFAULT 0,
        author_id INTEGER,
        created_ts TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
        FOREIGN KEY (author_id) REFERENCES users(id)
    );
    ''')

    # Create comments table
    cur.execute('''
    CREATE TABLE IF NOT EXISTS comments (
        id INTEGER PRIMARY KEY AUTOINCREMENT,
        post_id INTEGER,
        comment TEXT NOT NULL,
        author_id INTEGER,
        created_ts TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
        FOREIGN KEY (post_id) REFERENCES posts(id),
        FOREIGN KEY (author_id) REFERENCES users(id)
    );
    ''')

    cur.execute('SELECT COUNT(*) FROM users')
    if cur.fetchone()[0] == 0:
        cur.execute("INSERT INTO users (username, password, email, role) VALUES ('admin', 'admin123', 'admin@thoughtspace.com', 'admin')")
        cur.execute("INSERT INTO users (username, password, email, role) VALUES ('user', 'user123','user@thoughtspace.com', 'user')")
    
    cur.execute('SELECT COUNT(*) FROM posts')
    if cur.fetchone()[0] == 0:
        cur.execute("INSERT INTO posts (title, image, content, encrypted, author_id) VALUES ('Welcome Post','welcome.jpeg','Welcome to the blog!', 0, 1)")
        cur.execute("INSERT INTO posts (title, image, content, encrypted, author_id) VALUES ('Encrypted Post', 'encrypted.jpeg', 'Encrypted content', 1, 1)")

    conn.commit()
    conn.close()

if __name__ == "__main__":
    init_db()
    print("Database initialized successfully.")
