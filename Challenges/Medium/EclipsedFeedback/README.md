# Title: Eclipsed Feedback

## Details:
* difficulty: Intermediate
* category: Web
* domain: yes
* author:Subash
* flags: flag{XSS_Injection_Detected}

## Description:
Welcome to the "Eclipsed Feedback" challenge! In this task, you will engage with a web application of online shopping. The goal is to gain access to the application and reveal a hidden flag. This challenge tests participants' ability to identify and exploit common security vulnerabilities in web applications.

## Hint:
A list of potential passwords for the admin user is provided. Use this list wisely to gain access to the application.

## Intended Learning and Outcome:
- Participants will be abe to learn the significance of securing authentication mechanisms to prevent unauthorized access.
- How vulnerabilities in user input fields can lead to severe security breaches, such as the exposure of sensitive information.

## Deployment:
For the deployment we just need to deploy the docker container. The players should get a file `dictionary.txt` with list of passwords.

## Solution:
Follow these steps to solve this challenge:

### Brute force Login:
Use the provided password list to attempt login as the admin user. Identify the correct password to gain access.

### Exploit the comment section:
- Once logged in, analyze the comment section for vulnerabilities. 
- Analyse the page using inspect elements.
- You can see the `getUserName()` function returns the username calling the APIURL.
- Observe it has default parameter set as `isAdmin=false`.
- Inject XSS script code in comment section as   `<script>getUserName(true);</script>`.
- See the console window and get the flag.