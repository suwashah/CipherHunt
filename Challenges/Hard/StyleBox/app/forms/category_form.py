from flask_wtf import FlaskForm
from wtforms import StringField, FloatField, TextAreaField, SelectField, SubmitField
from wtforms.validators import DataRequired, NumberRange, Length

# Category form (for admin to add a category)
class CategoryForm(FlaskForm):
    class Meta:
        csrf = False
    name = StringField('Category Name', validators=[DataRequired(), Length(min=1, max=100)])
    submit = SubmitField('Add Category')