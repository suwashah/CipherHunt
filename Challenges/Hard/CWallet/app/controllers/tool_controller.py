from flask import Blueprint, render_template, request, redirect, url_for, session, flash
import base64
import zlib


tool_bp = Blueprint('tool', __name__)
@tool_bp.route('/decoder', methods=['GET', 'POST'])
def decoder():    
    if 'decoder_password' not in session:
        return redirect(url_for('user.login'))
    decoder_password=session['decoder_password']
    decoded_string=""
    if request.method == 'POST':
        try:
            cookievalue=request.form['cookievalue']
            password=request.form['password']

            if decoder_password==password:
                padding = "=" * (-len(cookievalue) % 4)  # Adjust padding
                session_data = base64.urlsafe_b64decode(cookievalue +padding)
                decompressed_data = zlib.decompress(session_data)
                decoded_string=decompressed_data.decode('utf-8')
            else:
                flash(f'Invalid password: {e}', 'danger')
                return redirect(url_for('tool.decoder'))
        
        except (base64.binascii.Error, zlib.error) as e:
            flash(f'Error decoding session cookie: {e}', 'danger')
            return redirect(url_for('tool.decoder'))
    return render_template('tool/decoder.html',decoded_string=decoded_string)