import base64
import zlib
import json

def session_encoder(session_data):
    try:
        session_str = json.dumps(session_data)       
        compressed_data = zlib.compress(session_str.encode('utf-8'))
        encoded_data = base64.urlsafe_b64encode(compressed_data).decode('utf-8')        
        return encoded_data    
    except Exception as e:
        print(f"Error encoding session data: {e}")
        return None


def session_decoder(cookie_value):
    try:
        padding = "=" * (-len(cookie_value) % 4)  # Adjust padding
        print(padding)
        session_data = base64.urlsafe_b64decode(cookie_value + "==")
        decompressed_data = zlib.decompress(session_data)
        return decompressed_data.decode('utf-8')
    
    except (base64.binascii.Error, zlib.error) as e:
        print(f"Error decoding session cookie: {e}")
        return None
    

cookie_value = "eJyrVkosLcmIL8nPTs1TslIKL3c1S8v1MXfJK3V39zOqcsnwy8539TYrCjEvz8gPynNM9XJxNk8xT3YPtFXSUUouSYtPy0lMB-pMLk_MyUktgXB1lEqLU4viM1OUrEwg7LzE3FSgqozU5OzUIqVaAGNRJ_M"
# cookie_value="eJyrVopPy0kszkgtVrJSiK5WUigB0UrFpcnJqcXFSjoKSj756Zl5ClCBtNIcRaXY2ligeGJpSUZ8SX52ah5Qh5JnrqeFk3d6YXa2r0Fajmm4oXdpvJOBcWiRV7lTsW-6j2uoX1aJRZ6JUaWvLcjU0uLUovjMFKBWQygvLzE3FWRSYkpuZp5SLQDp9jEO"
decoded_session = session_decoder(cookie_value)
if decoded_session:
    print("Decoded session:", decoded_session)


# session_data = {
#     "_flashes":[{" t":["success","Login successful!"]}],
#     "auth_token":"hz-8G_7SdYEAhSvu40csvHOQX2bvLh5SCN_Xg7DyLYo=",
#     "user_id":1,
#     "username":"admin"}

# encoded_session = session_encoder(session_data)
# if encoded_session:
#     print("Encoded session cookie:", encoded_session)