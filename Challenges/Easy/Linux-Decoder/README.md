## Title: Linux Decoder

## Details:

- difficulty: Easy
- category: Linux
- domain: -
- author: Subash
- flags: flag{hex_dump_decoder}

## Description:

In this challenge, you are given a file where there is a message in hexadecimal format. Your task is to decode the message from the file and get the flag.

## Hint:

The data may look like gibberish, but it's just a different representation of something familiar. Convert it back to its original form and search for the answer within.

## Intended Learning and outcome:

- Particpants will learn how to convert hex strings to plain text using command `xxd`.
- They will be able search the text by using command like `grep` to revel hidden messages.

## Deployment:

The players should get file named `message`

## Solution:

To solve this challenge, participants need to:

- Open a terminal and log into the Linux system and open message file
- convert hex to string using command `xxd -r -p message > output.txt`
- Search for the flag using command `grep "flag" output.txt`
- You will be able to get the flag `flag{hex_dump_decoder}`
