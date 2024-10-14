## Title: Simple Reverse Engineering

## Details:

- difficulty: Easy
- category: Reversing
- domain: -
- author: Subash
- flags: flag{simple_reverser}

## Description:

In this challenge, your task is to retrieve the hidden flag from the provided configuration file. The flag is encoded and embedded in the configuration file under certain conditions. You need to understand how to decode the user password to get the hidden flag.

You have been provided with a file named `simple_reversing_answer.py`. The Python script reads the configuration file. You have to check specific conditions, and attempts to decode the user password using reverse engineering techniques. Your goal is to get correct password to reveal the flag.

## Hint:

Try to use decrypting technique for encrypted data.

## Intended Learning and outcome:

- Particpants will be able to learn how different types and levels of encryption and decryption techniques can be used in real programming.
- They will be able to think logically and modify the code according to desired output.

## Deployment:

For the deployment we just need to deploy the docker container. The players should get the file `simple_reversing_answer.py`

## Solution:

To solve this challenge, participants need to:

- Open `simple_reversing_answer.py` file and look for the levels of encryption used for user password
- Add a new code to decrypt the password in reverse way in index route
- Use `aes_decrypt function` with `md5_hased` security key. Then use `base64_decode` function to get the original password
- Print the decrypted password


On application running on docker container, simply use the decrypted password to see the output as `flag{simple_reverser}`
