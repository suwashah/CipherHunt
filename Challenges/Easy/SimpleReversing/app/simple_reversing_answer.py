from flask import Flask, request, render_template_string
import configparser
from Crypto.Cipher import AES
from Crypto.Util.Padding import pad, unpad
from Crypto.Random import get_random_bytes
import base64
import hashlib

# Initialize Flask application
app = Flask(__name__)

section_name='hidden'
secret_key='MySecret'
stored_pwd='/d3lG3EdE5K/GyfVItXpgQ=='


def md5_hash(input_string):
    md5 = hashlib.md5()
    md5.update(input_string.encode('utf-8'))
    hashed_value = md5.hexdigest()    
    return hashed_value

def aes_encrypt(plaintext, key):
    if len(key) != 32:
        raise ValueError("Key must be 32 bytes long (256 bits).")    
    if isinstance(key, str):
        key = key.encode('utf-8')
    cipher = AES.new(key, AES.MODE_ECB)
    padded_plaintext = pad(plaintext.encode('utf-8'), AES.block_size)
    encrypted_bytes = cipher.encrypt(padded_plaintext)
    encrypted_text = base64.b64encode(encrypted_bytes).decode('utf-8')
    
    return encrypted_text


def base64_encode(data):
    byte_data = data.encode('utf-8')
    encoded_data = base64.b64encode(byte_data)
    return encoded_data.decode('utf-8')


# Try to reveal the user password using reversing technique to get the flag from the original program
def getForm():
    return '''<form method="POST">
                    <label for="password">Password:</label>
                    <input type="password" id="password" name="password">
                    <input type="submit" value="Submit">
                </form>
    '''
@app.route('/', methods=['GET', 'POST'])
def index():    
    if request.method == 'POST':        
        password = request.form['password']
        b64_pwd = base64_encode(password)
        aes_enc_pwd = aes_encrypt(b64_pwd, md5_hash(secret_key))
        if aes_enc_pwd == stored_pwd:
            return render_template_string('''
                <p>Your entered password is correct. Use this password in app hosted in docker container to get flag.</p>'''+
                getForm())
        else:
            return render_template_string('''
                <p>Wrong password</p>'''+
                getForm()
            )
    else:        
        print('Print user password here')
    return render_template_string(getForm())

if __name__ == "__main__":
    app.run(debug=True)