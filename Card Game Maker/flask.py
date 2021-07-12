from flask import Flask, render_template, request
from flask_mysqldb import MySQL
from coolname import generate_slug
import random

app = Flask(__name__)
app.config['MYSQL_HOST'] = ''
app.config['MYSQL_USER'] = ''
app.config['MYSQL_PASSWORD'] = ''
app.config['MYSQL_DB'] = 'flask'

mysql = MySQL(app)


@app.route('/', methods=['POST', 'GET'])
def index():
    if request.method == 'GET':
        return render_template("index.html")
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


#Creates a randomized deck of cards with unique attributes
@app.route('/randomize', methods=['POST', 'GET'])
def randomize():
    if request.method == 'GET':
        return render_template("index.html")
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
    return render_template("index.html")


if __name__ == "__main__":
    app.run(debug=True)

