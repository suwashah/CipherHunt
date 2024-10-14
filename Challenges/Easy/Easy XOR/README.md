## Title: Easy XOR

## Details:

* difficulty: Easy
* category: Reversing
* domain: -
* author: Subash
* flags: flag{easy_xor_challenge}

## Description:
Welcome to the simple reversing challenge! Your task is to decrypt a flag that has been encrypted using a basic operation with a single-byte key. You can see the encryption script in enc.py. The flag follows the format flag{...}. Can you uncover the correct key and retrieve the flag?


## Hint:
Sometimes, the simplest operations can be the hardest to break. Think about how basic encryption works and how you might reverse it. Try different single-byte keys until you get something readable. Remember, the flag format is flag{...}.

## Intended Learning and outcome:
- Particpants will be able to understand how XOR encryption works. especially with single-byte keys.
- Participants will practice basic cryptanalysis by reversing a simple XOR operation
- They will gain experience in writing scripts to automate the decryption process.


## Deployment:

Players should only get challenge.txt and enc.py

## Solution:

To solve this challenge, participants need to:
- Get the encrypted flag from challenge.txt file.
- Write a decrypting XOR script and brute-force the XOR key to decrypt the flag.
