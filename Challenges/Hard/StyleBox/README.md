# Title: Style Box

## Details:
* difficulty: Integrated
* category: Web / Cryptography
* domain: yes
* author:Subash
* flags: flag{Int3gr4t3d_Explo1t_Ch4llenge_C0mpleted}

## Description:
In this challenge, participants will explore a web application filled with various security vulnerabilities. Each vulnerability presents a puzzle to solve. Successfully exploiting each flaw will reveal hints that guide participants closer to the final flag.

## Hint:
Pay close attention to the error message you encounter. It provides clues about what format is expected and what is being received.

## Intended Learning and Outcome:
- Participants will be able to understand the principles behind brute force attacks and learn how to implement them to test the strength of authentication mechanisms.
- They will be gain hands-on experience in exploiting SQL injection vulnerabilities to manipulate database queries and extract sensitive information.
- They will learn how to identify and exploit vulnerabilities in the checkout process, including price manipulation and unauthorized access to admin functionalities.
- They will enhance practical skills in ethical hacking and penetration testing, preparing participants for real-world cybersecurity challenges.

## Deployment:
For the deployment we just need to deploy the docker container. The players should not get any additional files.


## Solution:
Follow these steps to solve this challenge:

- Start from registration. Notice that user checking in system API is implement to check if username already exist in system. Open burpsuite tool and send the request to repeater. Do SQL injection to the username parameter. Observe the response, it will reveal all the available user and hint for next step.

- Next step, try to brute-force the password of admin user shown from the SQL injection. Send the request to intruder and brute-force the password with prefix, 'Password@' and loop in the request through two digit number.
- Once matched, you will know that admin23 password is `Password@23`
- When login using the user, it will reveal next hint in alert message to do the checkout manipulation.
- register as a normal user and login and add the product to order.
- Go to My order menu and do the checkout.
- On checkout process, inspect the source, change the total_amount of hidden field and complete the checkout.
- As amount is manipulated, it will show hints that flag can be reveal in My transaction page.
- When you go to transaction page, it will say login as admin and go to product detail page to get the flag.
- Login back as admin user and go to any product detail page.
- Inspect the web source, you will see the final ctf flag in hidden field.

### Registration:
- Start by registering a new user in the application.

### SQL Injection on Username Check:
- Use the Burp Suite tool to intercept the API request that checks if the username already exists.
- Send the intercepted request to the Repeater tool.
- Perform SQL injection on the username parameter to extract all available usernames from the database.
- Observe the response; it will reveal all existing users and provide a hint for the next step.

### Brute-Force Admin Password:
- Take note of the admin username revealed from the SQL injection.
- Send the login request to the Intruder tool in Burp Suite.
- Set up the Intruder to brute-force the admin password using the prefix Password@ followed by a two-digit number (e.g., Password@00, Password@01, etc.).
- Loop through the password requests until you find a match.
- Once matched, confirm that the admin password is Password@23.

### Login as Admin:
- Use the credentials (`admin23` username and `Password@23`) to log in.
- Upon successful login, observe the alert message that provides a hint to perform checkout manipulation.

### Register as a Normal User:
- Register as a normal user in the application.
- Log in using the newly created normal user account.
- Add a product to the order.

### Checkout Process:
- Navigate to the "My Orders" menu and initiate the checkout process.
- Inspect the page source during checkout to locate the hidden fields, particularly total_amount.

### Manipulate Checkout Amount:
- Change the total_amount value in the hidden field to manipulate the checkout total.
- Complete the checkout process.
- After manipulation, note any hints or messages that indicate the flag can be revealed on the "My Transactions" page.

### Access My Transactions Page:
- Navigate to the "My Transactions" page.
- You will see a message indicating that you need to log in as the admin user to proceed.

### Login as Admin Again:
- Log back in using the admin credentials (`admin23` username and `Password@23`).

### View Product Details:
- Go to any product detail page while logged in as an admin user.
- Inspect the web source to reveal the final CTF flag `flag{Int3gr4t3d_Explo1t_Ch4llenge_C0mpleted}` located in a hidden field.


