from flask import Flask, render_template, request, redirect, session, jsonify
from flask_sqlalchemy import SQLAlchemy
from flask_login import LoginManager
from coolname import generate_slug
import random

app = Flask(__name__)
app.config['SQLALCHEMY_DATABASE_URI'] = 'mysql://capstone2021otcg:CS467cardgame!@osu-otcg-db.cs3ccyqxyr4e.us-west-1.rds.amazonaws.com/osu-otcg-db'

db = SQLAlchemy(app)


class Cards(db.Model):
    id = db.Column(db.Integer, primary_key=True, nullable=False)
    name = db.Column(db.String(15), unique=False, nullable=False)
    attack = db.Column(db.Integer, unique=False, nullable=False)
    cost = db.Column(db.Integer, unique=False, nullable=False)
    description = db.Column(db.Text, unique=False, nullable=True)
    image = db.Column(db.LargeBinary, unique=False, nullable=True)
    deck = db.Column(db.Integer, db.ForeignKey('decks.id'),  nullable=True)


class Decks(db.Model):
    id = db.Column(db.Integer, primary_key=True, nullable=False)
    name = db.Column(db.String(80), unique=False, nullable=False)
    image = db.Column(db.LargeBinary, unique=False, nullable=True)
    description = db.Column(db.Text, unique=False, nullable=True)
    user = db.Column(db.Integer, db.ForeignKey('user.id'), nullable=True)


class Users(db.Model):
    id = db.Column(db.Integer, primary_key=True, nullable=False)
    name = db.Column(db.String(15), unique=True, nullable=False)
    password = db.Column(db.String(15), unique=False, nullable=False)

    def is_active(self):
        """True, as all users are active."""
        return True

    def get_id(self):
        """Return the email address to satisfy Flask-Login's requirements."""
        return self.email

    def is_authenticated(self):
        """Return True if the user is authenticated."""
        return self.authenticated

    def is_anonymous(self):
        """False, as anonymous users aren't supported."""
        return False


class Games(db.Model):
    id = db.Column(db.Integer, primary_key=True, nullable=False)
    name = db.Column(db.String(15), unique=True, nullable=False)
    description = db.Column(db.Text, unique=False, nullable=True)
    total_turns = db.Column(db.Integer, unique=True, nullable=False)
    health_pool = db.Column(db.Integer, unique=False, nullable=False)
    time_limit = db.Column(db.Integer, unique=True, nullable=False)
    user_id = db.Column(db.Integer, db.ForeignKey('user.id'), nullable=False)


login_manager = LoginManager()


@login_manager.user_loader
def load_user(user_id):
    return Users.get(user_id)


@app.route('/signup', methods=['POST','GET'])
def login():
    if request.method == 'GET':
        return render_template('signup.html')
    if request.method == 'POST':
        name = request.form['name']
        password = request.form['password']
        user = Users.query.filter_by(name=name).first()
        if user:
            return "User already exists"
        else:
            new_user = Users(name=name, password=password)
            db.session.add(new_user)
            db.session.commit()
            return redirect('/')


@app.route('/games', methods=['POST', 'GET'])
def games():
    if request.method == 'POST':
        name = request.form['name']
        description = request.form['description']
        total_turns = request.form['total_turns']
        health_pool = request.form['health_pool']
        time_limit = request.form['time_limit']
        game_contents = Cards(name=name, description=description, total_turns=total_turns, health_pool=health_pool)
        db.session.add(game_contents)
        db.session.commit()
        return redirect('/')


@app.route('/', methods=['POST', 'GET'])
def index():
    if request.method == 'POST':
        name = request.form['name']
        attack = request.form['attack']
        defense = request.form['defense']
        cost = request.form['cost']
        card_contents = Cards(name=name, attack=attack, defense=defense, cost=cost)
        db.session.add(card_contents)
        db.session.commit()
        return redirect('/')
    else:
        return render_template('index.html', card_list=Cards.query.all())


@app.route('/delete/<int:id>')
def delete(id):
    card_to_delete = Cards.query.get_or_404(id)
    db.session.delete(card_to_delete)
    db.session.commit()
    return redirect('/')


@app.route('/update/<int:id>', methods=['GET', 'POST'])
def update(id):
    card = Cards.query.get_or_404(id)
    if request.method == 'POST':
        try:
            card.name = request.form['name']
            card.attack = request.form['attack']
            card.defense = request.form['defense']
            card.cost = request.form['cost']
            db.session.commit()
            return redirect('/')
        except:
            return 'There was an issue updating the card'
    else:
        return render_template('update.html', card=card)


# Creates a randomized deck of cards with unique attributes
@app.route('/randomize', methods=['POST', 'GET'])
def randomize():
    if request.method == 'POST':
        for i in range(29):
            name = generate_slug(2)
            attack = random.randint(1000, 4000)
            defense = random.randint(500, 2000)
            cost = random.randint(1, 6)
            card_contents = Cards(name=name, attack=attack, defense=defense, cost=cost)
            db.session.add(card_contents)
            db.session.commit()
    return render_template('index.html')


if __name__ == "__main__":
    db.create_all()
    app.run(debug=True)




