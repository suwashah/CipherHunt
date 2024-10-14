import subprocess

image_file = "Deception.jpeg"
wordlist = "passphrases.txt"

# Read the wordlist
with open(wordlist, 'r') as file:
    passphrases = file.readlines()

for passphrase in passphrases:
    passphrase = passphrase.strip()  # Remove newline characters

    # Run Steghide extraction command
    command = ['steghide', 'extract', '-sf', image_file, '-p', passphrase, '-f']
    try:
        result = subprocess.run(command, stdout=subprocess.PIPE, stderr=subprocess.PIPE, text=True)
        
        # Check if Steghide was successful
        if "wrote extracted data" in result.stderr:
            print(f"Success! Passphrase found: {passphrase}")
            break
        else:
            print(result.stdout)
    except Exception as e:
        print(f"Error with passphrase {passphrase}: {e}")
