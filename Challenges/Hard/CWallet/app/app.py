from flask import Flask, redirect, url_for
from controllers.user_controller import user_bp
from controllers.transaction_controller import transaction_bp
from controllers.tool_controller import tool_bp

from models.models import close_db

app = Flask(__name__)
app.secret_key = 'vG9f34LjK2pVs5YxQ7wNbHsT8cXaZ1Re'

# Register blueprints
app.register_blueprint(user_bp, url_prefix='/user')
app.register_blueprint(transaction_bp, url_prefix='/transaction')
app.register_blueprint(tool_bp, url_prefix='/tool')

@app.route('/')
def index():
    return redirect(url_for('user.login'))

@app.teardown_appcontext
def teardown_db(exception):
    close_db(exception)

if __name__ == '__main__':
      app.run(host='0.0.0.0', port=5000)
