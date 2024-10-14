from flask_wtf import FlaskForm
from wtforms import SelectField, SubmitField
from wtforms.validators import DataRequired
from models import Product

class OrderForm(FlaskForm):
    products = SelectField('Product', coerce=int, validators=[DataRequired()])
    submit = SubmitField('Place Order')

    def set_product_choices(self):
        self.products.choices = [(prod.id, prod.name) for prod in Product.query.all()]
