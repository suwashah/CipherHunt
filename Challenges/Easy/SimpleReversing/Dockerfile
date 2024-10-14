# Use the official Python image from the Docker Hub
FROM python:3.9-slim

# Define an ARG for the FLAG value
ARG FLAG

WORKDIR /app

# Copy the application code into the container
COPY ./app /app/

RUN sed -i s/reverse_flag/${FLAG}/g /app/simple_reversing.py
RUN rm -rf /var/lib/apt/lists/*
RUN pip install --no-cache-dir -r requirements.txt

EXPOSE 5000
# Command to run on container start
CMD ["python", "simple_reversing.py"]