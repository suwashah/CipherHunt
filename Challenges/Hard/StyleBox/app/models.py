from flask_sqlalchemy import SQLAlchemy
from werkzeug.security import generate_password_hash, check_password_hash
from datetime import datetime

db = SQLAlchemy()

class User(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    username = db.Column(db.String(80), unique=True, nullable=False)
    password_hash = db.Column(db.String(128), nullable=False)
    is_admin = db.Column(db.Boolean, default=False)
    active = db.Column(db.Boolean, default=True)
    created_ts = db.Column(db.DateTime, nullable=False, default=datetime.now())

    def set_password(self, password):
        self.password_hash = generate_password_hash(password, method='pbkdf2:sha256')

    def check_password(self, password):
        print('Pwd hash',self.password_hash)
        print('Pwd',password)
        return check_password_hash(self.password_hash, password)
    
    def is_authenticated(self):
        return True

    def is_active(self):
        return True

    def is_anonymous(self):
        return False

    def get_id(self):
        return self.id

class Category(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    name = db.Column(db.String(100), unique=True, nullable=False)
    products = db.relationship('Product', backref='category', lazy=True)

class Product(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    name = db.Column(db.String(100), nullable=False)
    price = db.Column(db.Float, nullable=False)
    image = db.Column(db.String(200), nullable=True)
    description = db.Column(db.Text, nullable=True)
    category_id = db.Column(db.Integer, db.ForeignKey('category.id'), nullable=False)
    created_ts = db.Column(db.DateTime, nullable=False, default=datetime.now())    
    created_by = db.Column(db.Integer, db.ForeignKey("user.id"), nullable=False)


class Order(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    product_id = db.Column(db.Integer, db.ForeignKey('product.id'), nullable=False)
    user_id = db.Column(db.Integer, db.ForeignKey('user.id'), nullable=False)
    quantity = db.Column(db.Integer, default=1)
    created_ts = db.Column(db.DateTime, nullable=False, default=datetime.now())
    user = db.relationship('User', backref='orders')
    product = db.relationship('Product', backref='orders')

class Transaction(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    user_id = db.Column(db.Integer, db.ForeignKey('user.id'), nullable=False)
    total_amount = db.Column(db.Float, nullable=False)
    status = db.Column(db.String(50), default='Completed')  # Could be 'Completed', 'Pending', etc.
    created_ts = db.Column(db.DateTime, nullable=False, default=datetime.now())
    user = db.relationship('User', backref='transactions')

