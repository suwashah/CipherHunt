# Title: CWallet

## Details:
* difficulty: Hard
* category: Web / Reversing / Cryptography
* domain: yes
* author:Subash
* flags: flag{Cross_S1te_F0rgery_Fl4g}

## Description:
In this challenge, participants will explore an e-wallet banking application that implements various functionalities such as user registration, balance checking, and money transfers. However, hidden within these functionalities are security vulnerabilities. The goal is to find and exploit these vulnerabilities to reveal a hidden flag.

## Hint:
 Pay attention to any parameters that might be manipulated to influence the behavior of the application unexpectedly. Look for opportunities to leverage the established session to achieve your goal.

## Intended Learning and Outcome:
- Participants will be able to understand the concepts of CSRF and SSRF vulnerabilities.
- They will be able to identify and exploit authentication token manipulation techniques.
- They will gain practical experience in web application security and ethical hacking.
- They will develop skills in assessing the security posture of web applications and identifying potential weaknesses.

## Deployment:
For the deployment we just need to deploy the docker container. The players should not get any additional files.


## Solution:
Follow these steps to solve this challenge:

### Register as a New User:
- Sign up for a new account in the CWallet application.

### Set Up Burp Suite:
- Launch the application in a browser configured to use Burp Suite as a proxy.

### Log In to the System:
- Navigate to the login page and authenticate using your newly created account.

### Enable Intercept Mode:
- In Burp Suite, enable intercept mode to capture requests.

### Navigate to the Transfer Page:
- Go to the transfer money page, enter your username as the recipient, specify an amount, and click the transfer button.

### Inspect the Transfer Form:
- Since the transfer form is vulnerable to CSRF, inspect the browser to copy the form's HTML.

### Create a Malicious Form:
- Create a new HTML file containing a form with the action set to the transfer URL.
- Manually add the `senderid` parameter to the form, specifying any user ID to simulate sending money from that account.

### Submit the Malicious Form:
- Open the HTML file in a browser and submit the form. This action will create a transaction using the CSRF vulnerability.

### Check the Transaction Response:
- In Burp Suite, review the response of the request made by the external form to find the session token.

### Access the Decoder Page:
- Navigate to decoder.html in the tool controller of the application.

### Decode the Session Token:
- Copy the first part of the session token (the part before the first period `.`) and paste it into the input box on the decoder page.
- In the password field, enter the password revealed during the CSRF attack.

### Reveal the Flag:
- Submit the form to decode the session string. The decoded output will reveal the hidden flag stored in the session.



