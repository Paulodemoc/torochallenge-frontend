db.createCollection('Users');
db.createCollection('Stocks');
db.createCollection('Accounts');
db.Users.insert({
    'username': 'paulo',
    'password': '123456',
    'email': 'paulodemoc@gmail.com'
});