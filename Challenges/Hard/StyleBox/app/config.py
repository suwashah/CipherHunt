import os

class Config:
    BASE_DIR = os.path.abspath(os.path.dirname(__file__))
    SECRET_KEY = os.environ.get('SECRET_KEY') or 'supersecretkey'
    SQLALCHEMY_DATABASE_URI = 'sqlite:///' + os.path.join(BASE_DIR, 'stylebox.db')
    SQLALCHEMY_TRACK_MODIFICATIONS = False
    UPLOAD_FOLDER = 'static/images/products'