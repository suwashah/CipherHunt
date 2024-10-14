## Title: Nature lover

## Details:

* difficulty: Easy
* category: Forensics
* domain: -
* author: Subash
* flags: flag{traveller}

## Description:

A seasoned explorer has left clues from their latest adventure. Dive into the details and discover the hidden flag in their travel memento.

## Hint:
This person loves to explore new places and collect memories from different parts of the world.

## Intended Learning and outcome:
- Participants will be able to identify and use appropriate forensic tools to uncover hidden information.
- Participants will successfully extract and interpret embedded data from a digital file to retrieve the hidden flag.


## Deployment:

Players should get the nature.jpg image file

## Solution:

To solve this challenge, participants need to:
- Open the location of file in terminal
- Run a command 'steghide extract -sf nature.jpg
- This command will extract a hidden file message.txt from the image. Within the hidden text file, they can see a flag{traveller}.
- The flag should be submitted as found 
