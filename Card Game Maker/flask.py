from flask import Flask, render_template, request, redirect, session
from flask_mysqldb import MySQL
import MySQLdb.cursors
import re
from coolname import generate_slug
import random

app = Flask(__name__)
app.config['MYSQL_HOST'] = ''
app.config['MYSQL_USER'] = ''
app.config['MYSQL_PASSWORD'] = ''
app.config['MYSQL_DB'] = 'flask'

mysql = MySQL(app)


# Route for handling the login page logic
@app.route('/login', methods=['GET', 'POST'])
def login():
    if request.method == 'POST' and 'username' in request.form and 'password' in request.form:
        username = request.form['username']
        password = request.form['password']
        cursor = mysql.connection.cursor(MySQLdb.cursors.DictCursor)
        cursor.execute('SELECT * FROM accounts WHERE username = % s AND password = % s', (username, password,))
        account = cursor.fetchone()
        if account:
            session['loggedin'] = True
            session['id'] = account['id']
            session['username'] = account['username']
            msg = 'Logged in successfully !'
            return render_template('index.html', msg=msg)
        else:
            msg = 'Incorrect username / password !'
    return render_template('login.html', msg=msg)


@app.route('/portal', methods=['POST', 'GET'])
def portal():
    if request.method == 'GET':
        return render_template('portal.html')
    if request.method == 'POST':
        name = request.form['name']
        attack = request.form['attack']
        defense = request.form['defense']
        cost = request.form['cost']
        cursor = mysql.connection.cursor()
        cursor.execute(''' INSERT INTO monster_cards VALUES(%s,%s,%s,%s)''', (name, attack, defense, cost))
        mysql.connection.commit()
        cursor.close()
        return f"Added Card"


@app.route('/delete')
def delete():
    pass


@app.route('/update')
def update():
    pass


# Creates a randomized deck of cards with unique attributes
@app.route('/randomize', methods=['POST', 'GET'])
def randomize():
    if request.method == 'GET':
        return render_template("portal.html")
    if request.method == 'POST':
        for i in range(40):
            name = generate_slug(2)
            attack = random.randint(1000, 4000)
            defense = random.randint(500, 2000)
            cost = random.randint(1, 6)
            cursor = mysql.connection.cursor()
            cursor.execute(''' INSERT INTO monster_cards VALUES(%s,%s,%s,%s)''', (name, attack, defense, cost))
            mysql.connection.commit()
            cursor.close()
    return render_template("portal.html")


if __name__ == "__main__":
    app.run(debug=True)




