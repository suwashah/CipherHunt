from flask import current_app,Blueprint, render_template, redirect, url_for, flash, request, jsonify
from models import db, Product, Order, Transaction
from forms.product_form import ProductForm
from flask_login import login_required, current_user
import os
from werkzeug.utils import secure_filename
from datetime import datetime
product_bp = Blueprint('product', __name__)
ctf_flag='StyleBox_flag'
@product_bp.route('/add', methods=['GET', 'POST'])
@login_required
def add_product():
    if not current_user.is_admin:
        flash('You do not have permission to access this page.', 'danger')
        return redirect(url_for('product.index'))  # Redirect to the product listing or wherever suitable
    form = ProductForm()
    form.set_category_choices()
    image_file = form.image.data
    if image_file:
        filename = secure_filename(image_file.filename)
        upload_folder = os.path.join(current_app.root_path, 'static/images/products')

            # Create the folder if it doesn't exist
        if not os.path.exists(upload_folder):
            os.makedirs(upload_folder)

            # Define the full path to save the image
        image_path = os.path.join(upload_folder, filename)
        # image_path = os.path.join(current_app.config['UPLOAD_FOLDER'], filename)
        image_file.save(image_path)

        new_product = Product(
            name=form.name.data,
            price=form.price.data,
            image=filename,  # Save the filename in the database
            description=form.description.data,
            category_id=form.category.data,
            created_by=current_user.id
        )
        db.session.add(new_product)
        db.session.commit()
        flash('New product added successfully!', 'success')
        return redirect(url_for('product.index'))
    return render_template('add_product.html', form=form)

@product_bp.route('/', endpoint='index')
def view_products():
    products = Product.query.all()
    return render_template('products.html', products=products)

@product_bp.route('/<int:product_id>', methods=['GET'])
def product_detail(product_id):
    # Retrieve the product based on its ID
    product = Product.query.get_or_404(product_id)
    pending_txn=get_pending_transaction_count()
    flag=None
    if pending_txn>0:
        flag=ctf_flag
        print('Flag: ',flag)
    return render_template('product_detail.html', product=product, is_admin=current_user.is_admin,flag=flag)

@product_bp.route('/add_to_order/<int:product_id>', methods=['POST'])
@login_required
def add_to_order(product_id):
    product = Product.query.get_or_404(product_id)
    
    # Get the quantity from the form
    quantity = request.form.get('quantity', type=int)

    if quantity is None or quantity <= 0:
        flash('Invalid quantity. Please enter a valid number.', 'danger')
        return redirect(url_for('product.product_detail', product_id=product.id))

    # Create a new order
    new_order = Order(user_id=current_user.id, product_id=product.id, quantity=quantity, created_ts=datetime.now())
    db.session.add(new_order)

    try:
        db.session.commit()
        flash(f'{quantity} of {product.name} has been added to your order.', 'success')
    except Exception as e:
        db.session.rollback()
        flash(f'Error occurred: {str(e)}', 'danger')

    # Redirect back to the product detail page or any other suitable page
    return redirect(url_for('product.product_detail', product_id=product.id))

@product_bp.route('/my_orders')
@login_required
def my_orders():
    orders = Order.query.filter_by(user_id=current_user.id).all()
    return render_template('my_orders.html', orders=orders)

@login_required
def get_pending_transaction_count():
    pending_count = Transaction.query.filter_by(status='Pending').count()
    return pending_count


