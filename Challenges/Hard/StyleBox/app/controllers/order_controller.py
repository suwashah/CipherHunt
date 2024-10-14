from flask import Blueprint, render_template, redirect, url_for, flash, request
from flask_login import login_required, current_user
from models import Order, db, Transaction

order_bp = Blueprint('order', __name__)
ctf_flag='You are almost there. Login as admin user and check product detail page to reveal the flag.'
@order_bp.route('/checkout', methods=['GET', 'POST'])
@login_required
def checkout():
    # Retrieve user's orders
    orders = Order.query.filter_by(user_id=current_user.id).all()
    total_amount = sum(order.product.price * order.quantity for order in orders)

    if request.method == 'POST':
        # Here we intentionally create a vulnerability
        req_total = request.form.get('total_amount', type=float)
        tran_status='Completed'
        if req_total != total_amount:
                tran_status='Pending'
                flash('Hint: You have manipulated the amount! Next step: find the hidden flag in the transaction history.', 'info')
        if req_total:
            transaction = Transaction(
                user_id=current_user.id,
                total_amount=req_total,
                status=tran_status
            )
            db.session.add(transaction)
            db.session.commit()

            # Here you would typically process payment and finalize orders
            db.session.query(Order).filter_by(user_id=current_user.id).delete()  # Clear user's orders after checkout
            db.session.commit()
            flash(f'Checkout completed with total amount: ${req_total:.2f}', 'success')
            return redirect(url_for('product.index'))
        else:
            flash('Invalid amount.', 'danger')

    return render_template('checkout.html', orders=orders, total_amount=total_amount)

@order_bp.route('/transactions', methods=['GET'])
@login_required
def view_transactions():
    transactions = Transaction.query.filter_by(user_id=current_user.id).all()
    flag = None
    for transaction in transactions:
        if transaction.status == 'Pending':
            flag = ctf_flag
            break

    return render_template('transactions.html', transactions=transactions, flag=flag)


