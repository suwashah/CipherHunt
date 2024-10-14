import sqlite3
import os

basepath = os.path.abspath(os.path.dirname(__file__))
db_path = os.path.join(basepath, 'eshopping.db')

def init_db():
    conn = sqlite3.connect(db_path)
    cur = conn.cursor()

    # Create tables
    cur.execute('''CREATE TABLE IF NOT EXISTS users (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        username TEXT NOT NULL UNIQUE,
                        password TEXT NOT NULL)''')

    cur.execute('''CREATE TABLE IF NOT EXISTS products (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        name TEXT NOT NULL,
                        image TEXT NOT NULL,
                        price DECIMAL(10,2) NOT NULL,
                        description TEXT NOT NULL)''')

    cur.execute('''CREATE TABLE IF NOT EXISTS comments (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        product_id INTEGER,
                        comment TEXT NOT NULL,
                        username TEXT NOT NULL,
                        FOREIGN KEY (product_id) REFERENCES products(id))''')

    # Insert default user and products
    cur.execute("SELECT COUNT(*) FROM users")
    if cur.fetchone()[0] == 0:
        cur.execute("INSERT INTO users (username, password) VALUES ('admin', 'p7k5')")

    cur.execute("SELECT COUNT(*) FROM products")
    if cur.fetchone()[0] == 0:
        cur.execute("INSERT INTO products (name, image, price, description) VALUES ('Nike Air Max Pulse','1.png', '150', 'Mixing one part urban to one part tough, the Air Max Pulse brings an underground touch to the iconic Air Max line')")
        cur.execute("INSERT INTO products (name, image, price, description) VALUES ('Nike Air Max Dn SE','2.png', '200', 'Say hello to the next generation of Air technology. The Air Max Dn features our Dynamic Air unit system of dual-pressure tubes, creating a reactive sensation with every step.')")

    conn.commit()
    conn.close()

if __name__ == '__main__':
    init_db()
