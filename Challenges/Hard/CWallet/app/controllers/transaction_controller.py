from flask import Blueprint, render_template, request, redirect, url_for, session, flash
from models.models import get_db,generate_secret_key

transaction_bp = Blueprint('transaction', __name__)
flag="cwallet_flag"
decoder_password=""
@transaction_bp.route('/transfer', methods=['GET', 'POST'])
def transfer():
    if 'user_id' not in session:
        flash('Please login first!', 'warning')
        return redirect(url_for('user.login'))

    if request.method == 'POST':
        recipient_username = request.form['recipient']
        csrf_msg=""
        if 'senderid' in request.form:
            senderid = request.form['senderid']
            request_origin = request.headers.get('Origin')
            current_origin = f"{request.scheme}://{request.host}"
            print("Requested origin: ",request_origin)
            print("Current origin: ",current_origin)            
            if request_origin != current_origin:
                dec_password=generate_secret_key(session['username'])
                session['decoder_password']=dec_password
                csrf_msg=f"You almost found the flag. This is a password to access a decoder tool. {dec_password}"
                session['ctf_flag']=flag
                
        else:
            senderid=session['user_id']
        

        amount = float(request.form['amount'])
        if amount % 5 != 0:
            flash('Amount must be divisible by 5.', 'danger')
            return redirect(url_for('transaction.transfer'))

        db = get_db()

        sender = db.execute("SELECT * FROM users WHERE id = ?", (senderid,)).fetchone()
        recipient = db.execute("SELECT * FROM users WHERE username = ?", (recipient_username,)).fetchone()

        if not recipient:
            flash(f'User {recipient_username} does not exist in system. Please make sure the recipient username is correct.', 'danger')
            return redirect(url_for('transaction.transfer'))
        if sender['username'] == recipient_username:
            flash('Cannot send money to own account.', 'danger')
            return redirect(url_for('transaction.transfer'))
        
        if sender['balance'] < amount:
            flash('Insufficient balance.', 'danger')
            return redirect(url_for('transaction.transfer'))
       

        db.execute("UPDATE users SET balance = balance - ? WHERE id = ?", (amount, sender['id']))
        db.execute("UPDATE users SET balance = balance + ? WHERE id = ?", (amount, recipient['id']))
        db.execute("INSERT INTO transactions (sender_id, receiver_id, amount, transaction_type) VALUES (?, ?, ?, 'deposit')",
                   (sender['id'], recipient['id'], amount))
        db.commit()

        flash(f'Transferred {amount} to {recipient_username}. {csrf_msg}', 'success')
        return redirect(url_for('user.dashboard'))

    return render_template('transaction/transfer.html')


@transaction_bp.route('/transactions')
def transactions():
    if 'user_id' not in session:
        flash('Please login first!', 'warning')
        return redirect(url_for('user.login'))

    db = get_db()
    user_transactions = db.execute("SELECT t.*,s.username as sender, r.username as receiver FROM transactions t join users s on t.sender_id=s.id join users r on t.receiver_id=r.id WHERE t.sender_id = ? OR t.receiver_id = ?", 
                                   (session['user_id'], session['user_id'])).fetchall()

    return render_template('transaction/transactions.html', transactions=user_transactions)
