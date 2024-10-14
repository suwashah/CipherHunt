def encrypt(plain_text, key):
    return ''.join(chr(ord(c) ^ key) for c in plain_text)

flag = ""# Set your flag string here
key = 0 # Choose a key (0-255)
encrypted_flag = encrypt(flag, key)

print(f"Encrypted flag: {''.join(format(ord(c), '02x') for c in encrypted_flag)}")
