## Title: Web Hidden Treasures

## Details:

- difficulty: Easy
- category: Web
- domain: yes
- author: Subash
- flags: flag{hidden_in_web}

## Description:
In this challenge, you are provided with a simple Flask web application that has login functionality. The application has two types of users: regular users and admin users. Admin users have access to a secret page containing sensitive information. Your task is to gain access to this secret page by exploiting the application’s parameter based access control mechanism.

## Hint:

Use regular users credential as > username:`guest` and password:`guest123#`

## Intended Learning and outcome:

- Particpants will learn how web applications use querystring to manage control in web pages.
- They will understand the security implications of relying on client-side data for access control and how it can be exploited.

## Deployment:

For the deployment we just need to deploy the docker container. The players should not get any additional files.

## Solution:

To solve this challenge, participants need to:

- log in using credentials Username:`guest` and password:`guest123#`
- On successful login you will see the querystring parameter `/dashboard?a=1` or `/dashboard?a=0`. Here a=1 means admin whereas a=0 means non admin user.
- By default your non admin user will get `/dashboard?a=0`
- Change the value of parameter `a` to `1`
- Now, visit the `http://<app-domain>/admin` page manually
- refresh the page to see the admin messages and by viewing page source, they will be able to reveal the flag: `flag{hidden_in_web}`