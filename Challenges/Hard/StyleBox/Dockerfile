FROM python:3.9-slim
ARG FLAG
WORKDIR /app
COPY ./app /app/

RUN sed -i s/StyleBox_flag/${FLAG}/g /app/controllers/product_controller.py
RUN rm -rf /var/lib/apt/lists/*
RUN pip install --no-cache-dir -r requirements.txt

EXPOSE 5000

CMD ["python", "app.py"]