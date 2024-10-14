from flask import Blueprint, render_template, redirect, url_for, flash
from models import db, Category
from forms.category_form import CategoryForm
from flask_login import login_required, current_user

category_bp = Blueprint('category', __name__)

# Route to view all categories
@category_bp.route('/', methods=['GET'], endpoint='index')
def view_categories():    
    categories = Category.query.all()  # Retrieve all categories from the database
    return render_template('category_list.html', categories=categories)

# Route to add a new category
@category_bp.route('/add', methods=['GET', 'POST'])
def add_category():
    if not current_user.is_admin:
        flash('You are not authorized to access this page.', 'danger')
        return redirect(url_for('category.index'))
    form = CategoryForm()
    if form.validate_on_submit():
        category = Category(name=form.name.data)
        db.session.add(category)
        db.session.commit()
        flash('Category added successfully!', 'success')
        return redirect(url_for('category.index'))
    return render_template('add_category.html', form=form)
