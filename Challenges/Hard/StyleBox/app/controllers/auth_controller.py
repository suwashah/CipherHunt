from flask import Blueprint, render_template, redirect, url_for, flash, request, jsonify
from models import User, db
from forms.user_form import UserForm, LoginForm
from flask_login import login_user, logout_user, login_required
from sqlalchemy import text
auth_bp = Blueprint('auth', __name__)

@auth_bp.route('/register', methods=['GET', 'POST'])
def register():
    form = UserForm()
    
    if form.validate_on_submit():
        print('Form is valid')
        user = User(username=form.username.data)
        user.set_password(form.password.data)  # Assuming your User model has this method
        db.session.add(user)
        try:
            print('Entered try block')
            db.session.commit()
            flash('Registration successful! Please log in.', 'success')
            return redirect(url_for('auth.login'))
        except Exception as e:
            db.session.rollback()
            flash(f'Error occurred: {str(e)}','danger')
    else:
        print('Form is invalid')
    return render_template('register.html', form=form)

@auth_bp.route('/login', methods=['GET', 'POST'])
def login():
    form = LoginForm()
    if form.validate_on_submit():
        user = User.query.filter_by(username=form.username.data).first()
        if not user:
            flash('Invalid username.', 'danger')
        elif not user.check_password(form.password.data): 
            flash('Invalid password.', 'danger')
        else:
            login_user(user)
            if user.username == 'admin23':
                flash('Hint: Check the order vulnerabilities for next hint.', 'info')
            flash('Login successful!', 'success')
            return redirect(url_for('index'))
    return render_template('login.html', form=form)

@auth_bp.route('/logout')
@login_required
def logout():
    logout_user()
    flash('You have been logged out.', 'info')
    return redirect(url_for('auth.login'))

@auth_bp.route('/check_username', methods=['GET'])
def check_username():
    username = request.args.get('username')    
    query = text(f"SELECT id,username FROM user WHERE username = '{username}'")
    
    try:
        result = db.session.execute(query).fetchall()
        if result:
            user_data = [{'id': row[0], 'username': row[1]} for row in result]
            return jsonify({'exists': True, 'message': 'Username already taken.', 'users': user_data, 'rem':'Hint: Do brute force attack to admin user with password prefix `Password@<numbers>`'}), 200
        else:
            return jsonify({'exists': False, 'message': 'Username is available.'}), 200
    except Exception as e:
        return jsonify({'error': str(e)}), 500
