## Title: Message Bufferer

## Details:

- difficulty: Medium
- category: pwn
- domain: -
- author: Subash
- flags: flag{binary_exploitation}

## Description:

Welcome to the world of secure communications! But beware, not all messages are as safe as they seem.

In this challenge, you'll be interacting with a simple messaging program. Your task is to send a special message that reveals a hidden secret. But be careful! The program is prone to mishandling long messages, and a carefully crafted input might just cause it to overflow and expose the contents of a hidden file.

## Hint:

Think about how input length can affect memory. You may be able to manipulate the program by sending an oversized message that exceeds what the program can safely handle.

## Intended Learning and outcome:

- Particpants will learn how improper handling of user input can lead to a buffer overflow vulnerability.
- They will be able understand how stack memory is managed in C programs and how overwriting stack data can affect program control flow.
- They will be able to practice crafting input payloads to exploit buffer overflows and gain unauthorized access to sensitive information

## Deployment:

For the deployment we just need to deploy the docker container. The players should get the files `vuln` and `vuln.c`

## Solution:

To solve this challenge, 
- Participants need to run the compiled program and select option 1 to run the vulnerable function.
- Enter a string that exceeds 32 characters, such as `ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFG` to overflow the buffer.
- Create flag.txt and put their own flag text for debugging purpose.
- The overflow causes the program to crash, triggering the sigsegv_handler function.
- The segmentation fault handler prints the contents of the `flag.txt` file, revealing the flag.

Once they can see their debugging flag, they have to run a program hosted in server using command `nc <ip_address> 1223`
Use same technique to exploit the program and reveal the real flag.