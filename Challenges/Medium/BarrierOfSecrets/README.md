# Title: Barrier of Secrets

## Details:
* difficulty: Intermediate
* category: Web
* domain: yes
* author:Subash
* flags: flag{secret_gateway_opened}

## Description:
Welcome to the "Barrier of Secrets" challenge! In this intricate CTF task, you'll engage with a multi-layered web application designed to test your ability to navigate through various security measures. Your goal is to access hidden content and reveal a secret flag by overcoming several barriers within the application. Explore and uncover the secrets hidden behind the barriers to achieve victory.

## Hint:
The login page may have vulnerabilities that could allow you to bypass authentication.

## Intended Learning and Outcome:
- Participants will be abe to learn and practice for bypassing authentication through SQL injection.
- They will be able to understand how to manipulate HTTP headers to access restricted areas of a web application.
- They will gain experience with AES encryption by decrypting a flag using a provided hashed key.

## Deployment:
For the deployment we just need to deploy the docker container. The players should not get any additional files.

## Solution:
Follow these steps to solve this challenge:

### Bypass Login:
Use a payload like ' OR 1=1-- on the login page to log in without valid credentials.

### Modify User-Agent:
After logging in, the pages will say: 'To view this content, you should login from ctfbrowser'.
Use Burp Suite to change the User-Agent header to ctfbrowser and send the request.

### Capture Encrypted Flag:
In Burp Suite, look for the X-Flag header in the response. It contains encrypted data.

### View Employee List:
Open the response in your browser to see the employee list. Look for a message at the bottom of the page telling you to check all pages.

### Find the Secret Key:
On the dashboard page, you’ll find a secret key that changes based on your username.

### Decrypt the Flag:
Use the encrypted flag from the X-Flag header and the secret key from the dashboard page with an AES decryption tool to get the original flag.
