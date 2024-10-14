# Title: Digital Mirage

## Details:
* difficulty: Intermediate
* category: Forensic
* domain: no
* author:Subash
* flags: flag{svg_surprise}

## Description:
Welcome to the "Digital Mirage" challenge! In this challenge, You have been provided with a document that seems to be a typical file. Your task is to uncover hidden secrets within it. Follow the clues, carefully analyze the contents, and decipher the hidden message that lies beneath the surface.

## Hint:
Focus on examining the file’s metadata. Sometimes, the most critical information isn't immediately visible.

## Intended Learning and Outcome:
- Participants will be abe to understand file metadata and its use in steganography.
- They will be familiar with tools like `steghide` and `exiftool` for extracting hidden data.
- They will able to know how svg images works.

## Deployment:
The players should be provided with `HiddenInformation.docx` file

## Solution:
Follow these steps to solve this challenge:

### Extract Metadata using exiftool:
- Run `Exiftool` on the `HiddenInformation.docx` file to view metadata and its orginal type and extension
- exiftool HiddenInformation.docx
- You can see the image extension as jpg in metadata
- Rename extension to `HiddenInformation.jpg`

### Use steghide to get the hidden file:
On using steghide extract command, you will be able to see the extracted file which is in .7z extension
Extract the file and check for the files.

### Check for SVG content
Open the SVG in text editor or browser view source
You will be able to see the flag