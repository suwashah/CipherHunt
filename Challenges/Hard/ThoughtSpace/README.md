# Title: Thought Space

## Details:
* difficulty: Hard
* category: Web / Cryptography / Forensic
* domain: yes
* author:Subash
* flags: flag{decoding_encrypted_mysteries}

## Description:
In this challenge, you are tasked with exploiting a vulnerable blog platform called Thought Space. The platform has several hidden security flaws that you must uncover. Discover the admin's sensitive information and the flag using various methods. The flag is split into three distinct parts, each hidden within different aspects of the site. Your goal is to find all three parts, combine them, and submit the final flag.

## Hint:
Carefully examine the platform and its responses. The flag pieces are hidden where you would least expect them. Every angle of attack should be considered.

## Intended Learning and Outcome:
- Participants will be abe to learn how to retrieve sensitive information by going beyond and using burp-suite feature like intruder payloads.
- They will learn how to spot subtle vulnerabilities, such as weak authentication.
- They will be able to discover how fragmented data (like flags) can be hidden across multiple locations and combined.
- They will enhance their skills in web security, focusing on exploiting misconfigured headers and weak login mechanisms.
- They will explore how information can be hidden within images in web applications
- They will understand the significance of hidden data in HTTP headers and responses.

## Deployment:
For the deployment we just need to deploy the docker container. The players should not get any additional files.


## Solution:
Follow these steps to solve this challenge:

### Sign Up and Username Enumeration:
- Navigate to the signup page of the Thought Space application.
- Open your browser’s Developer Tools (usually accessible via F12 or Ctrl+Shift+I).
- Go to the Network tab and observe the network traffic as you attempt to sign up. Pay attention to the request made to check if the username already exists.
- Use Burp Suite to intercept this request and send it to the Intruder tool.
- Configure the Intruder to target the username parameter and use a payload list of numbers from 1 to 99, appending these numbers to a prefix such as admin.
- Start the attack. Look for a response indicating that a username like `admin49` exists. This confirms the admin username.

### Retrieve Admin Password:
- In Burp Suite, inspect the intercepted request for the user details. Look for the debug header parameter.
- Set the debug parameter value to `true` and resend the request.
- The response should include the admin user's password. Use these credentials to log in to the application.

### Finding the First Flag:
- After logging in, go to the blog post page and add a comment. Ensure the comment is posted as a admin user.
- Check the response headers after submitting the comment. Look for a header revealing `flag{decoding_`, which is the first piece of the flag.

### Finding the Second Flag:
- Examine the image on the welcome blog post. The image contains a hidden flag using Steghide.
- Use ExifTool to extract the secret passphrase from the image metadata.
- With the passphrase, use Steghide to extract the hidden file from the image.
- The extracted file is encrypted with AES 128. Decrypt it using the same passphrase.
- The decrypted text will reveal the middle part of the flag: `encrypted`.

### Finding the Final Flag:
- Check the robots.txt file located in the root directory of the site. This file may contain encrypted text in SHA256 format.
- Use a SHA256 decryption tool to decipher the encrypted text. This will provide you with a path where the final flag is stored.
- Locate the file named myrobot.txt at the given path.
- Reverse the content of myrobot.txt to reveal the final part of the flag: `_mysteries}`.

### Combine the Flag Parts:
- Combine all three flag parts you’ve gathered: `flag{decoding_`, `encrypted` , and `_mysteries}`.
- The complete flag should be formatted as: `flag{decoding_encrypted_mysteries}`.
- Submit this combined flag to complete the challenge.

These steps will guide you through finding and combining the three pieces of the flag to solve the `Thought Space` challenge.