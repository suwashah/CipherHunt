FROM python:3.9-slim
ARG FLAG

WORKDIR /app
COPY ./app /app/

RUN sed -i s/eclipsedFlag/${FLAG}/g /app/app.py


RUN rm -rf /var/lib/apt/lists/*
RUN pip install --no-cache-dir -r requirements.txt

EXPOSE 5000

CMD ["python", "app.py"]