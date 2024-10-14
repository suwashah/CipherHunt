import sqlite3
import os

basepath = os.path.abspath(os.path.dirname(__file__))
db_path = os.path.join(basepath, 'cwallet.db')
def init_db():
    conn = sqlite3.connect(db_path)
    cur = conn.cursor()
    # Create users table
    cur.execute('''
    CREATE TABLE IF NOT EXISTS users (
        id INTEGER PRIMARY KEY AUTOINCREMENT,
        username TEXT NOT NULL UNIQUE,   
        password TEXT NOT NULL,
        role TEXT DEFAULT 'user',
        balance REAL DEFAULT 0,
        created_ts TIMESTAMP DEFAULT CURRENT_TIMESTAMP
    );
    ''')
    # Create transactions table
    cur.execute('''
        CREATE TABLE IF NOT EXISTS transactions (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            sender_id INTEGER,
            receiver_id INTEGER,
            amount REAL,
            transaction_type TEXT CHECK(transaction_type IN ('deposit', 'withdraw')),
            created_ts TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
            FOREIGN KEY (sender_id) REFERENCES users (id),
            FOREIGN KEY (receiver_id) REFERENCES users (id)
        )
    ''')

    cur.execute('SELECT COUNT(*) FROM users')
    if cur.fetchone()[0] == 0:
        cur.execute("INSERT INTO users (username, password, role, balance) VALUES ('chris', 'J3rryCo', 'admin','1000')")

    conn.commit()
    conn.close()

if __name__ == "__main__":
    init_db()
    print("Database initialized successfully.")

