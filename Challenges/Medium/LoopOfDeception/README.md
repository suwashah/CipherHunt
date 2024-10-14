# Title: Loop of Deception

## Details:
* difficulty: Intermediate
* category: Forensic
* domain: -
* author:Subash
* flags: flag{loop_of_confusion}

## Description:
Welcome to the "Loop of Deception" challenge! In this challenge, you are given an image that seems innocent on the surface, but it hides a secret within. Additionally, you're provided with a text file containing a list of passphrases. Your task is to retrieve the hidden file from the image.

Once you extract the file, you will need to figure out how to run it and retrieve the hidden flag within. Good luck!

## Hint:
Think about ways to hide data in plain sight. Tools can help you extract information hidden deep within files, and brute-force methods can come in handy when facing many potential keys.

## Intended Learning and Outcome:
- Participants will be abe to learn the concept of `steganography`.
- They will be able to write Python scripts to `automate brute-force` attacks for password execution.
- They will learn how to execute `reverse-engineering` and interact with a compiled C program to locate hidden data.
- They will understand how to work with `passphrase-protected` files and use wordlists effectively. 

## Deployment:
The players should get `Deception.jpeg` and `passphrases.txt` files.

## Solution:
Follow these steps to solve this challenge:

- Create a Python script to automate brute-forcing with the passphrase list provided to extract the hidden file from the image using Steghide.

- Once the file is extracted, you will get a compiled C program. When running this program, you will get a text looping around 10000 times. Flag is hidden to random location.

- Use the command like `grep` while running the program to reveal a flag. For example: ./program | grep "flagP{"


