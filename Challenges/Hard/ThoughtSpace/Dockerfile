FROM python:3.9-slim
ARG FLAG1
ARG FLAG3

WORKDIR /app
COPY ./app /app/

RUN sed -i s/decode_fg/${FLAG1}/g /app/app.py
RUN sed -i s/third_fg/${FLAG3}/g /app/static/assets/js/myrobot.txt

RUN rm -rf /var/lib/apt/lists/*
RUN pip install --no-cache-dir -r requirements.txt

EXPOSE 5000

CMD ["python", "app.py"]