from flask import Flask,render_template
from flask_sqlalchemy import SQLAlchemy
from flask_migrate import Migrate
from flask_login import LoginManager
from models import db
from controllers.auth_controller import auth_bp
from controllers.product_controller import product_bp
from controllers.order_controller import order_bp
from controllers.category_controller import category_bp


app = Flask(__name__)
app.config.from_object('config.Config')

# Initialize the database
db.init_app(app)

migrate = Migrate(app, db)
with app.app_context():
    db.create_all()

#Init LoginManager
login_manager=LoginManager()
login_manager.init_app(app)
login_manager.login_view='auth.login'

# Register blueprints (controllers)
app.register_blueprint(auth_bp, url_prefix='/auth')
app.register_blueprint(product_bp, url_prefix='/products')
app.register_blueprint(order_bp, url_prefix='/orders')
app.register_blueprint(category_bp, url_prefix='/categories') 

@login_manager.user_loader
def load_user(user_id):
    from models import User
    return User.query.get(int(user_id)) 

@app.route('/', endpoint='home')
def home():
    return render_template('index.html')

@app.route('/')
def index():
    return render_template('index.html')


if __name__ == '__main__':
    # app.run(debug=True)
    app.run(host='0.0.0.0', port=5000)

