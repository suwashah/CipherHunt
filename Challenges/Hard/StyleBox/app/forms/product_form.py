from flask_wtf import FlaskForm
from wtforms import StringField, FloatField, TextAreaField, SelectField, SubmitField
from flask_wtf.file import FileField, FileAllowed, FileRequired
from wtforms.validators import DataRequired, NumberRange, Length
from models import Category

class ProductForm(FlaskForm):
    name = StringField('Product Name', validators=[DataRequired(), Length(min=1, max=100)])
    price = FloatField('Price', validators=[DataRequired(), NumberRange(min=0.01)])
    image = FileField('Image', validators=[
        FileRequired(), 
        FileAllowed(['jpg', 'png', 'jpeg'], 'Images only!')
    ])
    description = TextAreaField('Description', validators=[Length(max=200)])
    category = SelectField('Category', coerce=int, validators=[DataRequired()])
    submit = SubmitField('Add Product')

    def set_category_choices(self):
        self.category.choices = [(cat.id, cat.name) for cat in Category.query.all()]
