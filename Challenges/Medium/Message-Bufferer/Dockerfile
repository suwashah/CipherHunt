# Use an official Debian-based image with build tools
FROM debian:latest
ARG FLAG

# Install necessary packages
RUN apt-get update && apt-get install -y \
    gcc \
    netcat-traditional \
    make \
    libc6-dev \
    && rm -rf /var/lib/apt/lists/*

# Set the working directory
WORKDIR /app

COPY ./app /app/

RUN gcc -g -o vuln vuln.c

RUN echo ${FLAG} > /app/flag.txt

RUN ls -l /app

EXPOSE 1223

WORKDIR /app
CMD ["sh", "-c", "while :; do echo -ne 'Listening on port 1223...\n' | nc -lvp 1223 -e ./vuln; done"]

#docker build command
#docker-compose up --build
#use nc or telnet to run the program. nc localhost 1223