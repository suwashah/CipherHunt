# services/transaction_service.py
from models.models import transfer_money, list_transactions

def transfer_money_service(from_user, to_user, amount):
    from_user_data = get_user(from_user)
    to_user_data = get_user(to_user)
    
    if not from_user_data or not to_user_data:
        raise ValueError('User not found')
    if from_user_data[2] < amount:
        raise ValueError('Insufficient funds')
    
    transfer_money(from_user, to_user, amount)

def get_transactions_service():
    return list_transactions()
