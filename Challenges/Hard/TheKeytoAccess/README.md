# Title: The Key to Access

## Details:
* difficulty: Hard
* category: Web / Cryptography
* domain: yes
* author:Subash
* flags: flag{None_Algorithm_Takes_Down_Auth}

## Description:
In this challenge, participants will explore vulnerabilities associated with JSON Web Tokens (JWTs) due to improper handling of token signatures. The application uses JWTs for user authentication, but it contains a critical flaw that allows for bypassing authentication checks. Participants must identify and exploit this vulnerability to access the hidden flag that is normally restricted to authorized users.

## Hint:
Use `john:John@123` credential to log into the system

## Intended Learning and Outcome:
- Participants will be able to understand the structure and function of JSON Web Tokens.
- They will be able to learn about common security flaws related to JWT handling.
- They will recognize the importance of proper signature validation in securing applications.
- They will successfully exploit a vulnerability in JWT authentication to gain unauthorized access.

## Deployment:
For the deployment we just need to deploy the docker container. The players should not get any additional files.


## Solution:
Follow these steps to solve this challenge:

### Login Using Provided Credentials:
- Access the login page of the application.
- Enter the provided user credentials

### Attempt to Access the Admin Panel:
- After logging in, try to navigate to the admin panel (e.g., /admin/flag).
- Observe that access is denied, as the logged-in user has a normal role and cannot view the admin content.

### Intercept the Login Request with Burp Suite:
- Open Burp Suite and configure your browser to use it as a proxy.
- Log in again while Burp Suite is intercepting the traffic.

### Analyze the Login Request:
- In Burp Suite, find and inspect the request sent to the server for the login.
- Look for the Set-Cookie header in the response, which contains the JWT token.

### Decode the JWT Token:
- Copy the JWT token value from the cookie.
- Use an online JWT decoder (or a local tool) to decode the token into its components: header, payload, and signature.
- Examine the header section of the token to identify the algorithm type.

### Check the Algorithm Type:
- In the decoded header, observe the alg field. It should show HS256.

### Send the Login Request to Repeater:
- Right-click the login request in Burp Suite and send it to the Repeater tool.

### Modify the token as follows:
- Change the alg value in the header from HS256 to none.
- Remove the signature portion of the JWT (the part after the last period .).

### Send the Modified Request:
- Send the modified request to the server.
- The server should process the request and set the cookie value to the new token, which now has alg set to none and no valid signature.

### Access the Admin Page:
- With the modified token set in your cookie, navigate back to the admin panel (e.g., /admin/flag).
- You should now have access to the admin page, where the flag will be revealed





