from coolname import generate_slug
from flask import Flask, render_template, request, redirect, session, jsonify
from flask_cors import CORS
from flask_sqlalchemy import SQLAlchemy
from flask_login import LoginManager, UserMixin, login_user, current_user, logout_user, login_required
import random
import os

# Database Configs
app = Flask(__name__)
cors = CORS(app)
app.config['SECRET_KEY'] = os.urandom(24)
app.config['SQLALCHEMY_DATABASE_URI'] = 'mysql://capstone2021otcg:CS467cardgame!@osu-otcg-db.cs3ccyqxyr4e.us-west-1.rds.amazonaws.com/osu-otcg-db'

db = SQLAlchemy(app)

# Flask Login Requirements
login_manager = LoginManager()
login_manager.login_view = '/'
login_manager.init_app(app)
@login_manager.user_loader
def load_user(user_id):
    return Users.query.get(user_id)


class Cards(db.Model):
    id = db.Column(db.Integer, primary_key=True, nullable=False)
    name = db.Column(db.String(15), unique=False, nullable=False)
    attack = db.Column(db.Integer, unique=False, nullable=False)
    cost = db.Column(db.Integer, unique=False, nullable=False)
    description = db.Column(db.Text, unique=False, nullable=True)
    image = db.Column(db.LargeBinary, unique=False, nullable=True)
    deck = db.Column(db.Integer,db.ForeignKey('decks.id'),  nullable=True)


class Decks(db.Model):
    id = db.Column(db.Integer, primary_key=True, nullable=False)
    name = db.Column(db.String(80), unique=False, nullable=False)
    image = db.Column(db.LargeBinary, unique=False, nullable=True)
    description = db.Column(db.Text, unique=False, nullable=True)
    user = db.Column(db.Integer, db.ForeignKey('users.id'), nullable=True)


class Users(UserMixin, db.Model):
    id = db.Column(db.Integer, primary_key=True, nullable=False)
    name = db.Column(db.String(15), unique=True, nullable=False)
    password = db.Column(db.String(15), unique=False, nullable=False)


class Games(db.Model):
    id = db.Column(db.Integer, primary_key=True, nullable=False)
    name = db.Column(db.String(15), unique=True, nullable=False)
    description = db.Column(db.Text, unique=False, nullable=True)
    total_turns = db.Column(db.Integer, unique=True, nullable=False)
    health_pool = db.Column(db.Integer, unique=False, nullable=False)
    time_limit = db.Column(db.Integer, unique=True, nullable=False)
    user_id = db.Column(db.Integer, db.ForeignKey('users.id'), nullable=False)


@app.route('/signup', methods=['POST', 'GET'])
def signup():
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
            return ('', 201)
    
    
@app.route('/login', methods=['POST'])
def login():
    if request.method == 'POST':
        name = request.form['name']
        password = request.form['password']
        user = Users.query.filter_by(name=name).first()
        remember = True
        if not user or user.password != password:
            return 'Incorrect credentials or user does not exist'
        else:
            login_user(user, remember=remember)
            return ('', 200)

        
@app.route('/logout')
def logout():
    logout_user()
    return ''
    
    
@app.route('/games', methods=['GET','POST'])
def games_get_post():
    if request.method == 'GET':
        data = Games.query.all()
        return jsonify([{"name": game.name} for game in data])
    
    elif request.method == 'POST':
        name = request.form['name']
        description = request.form['description']
        total_turns = request.form['total_turns']
        health_pool = request.form['health_pool']
        time_limit = request.form['time_limit']
        user_id = current_user.id
        game_contents = Games(name=name, description=description, total_turns=total_turns, health_pool=health_pool, time_limit=time_limit ,user_id=user_id)
        db.session.add(game_contents)
        db.session.commit()
        return ('', 204)
    
    
@app.route('/cards', methods=['GET', 'POST'])
def cards_get_post():
    if request.method == 'GET':
        data = Cards.query.all()
        return jsonify([{"name": card.name} for card in data])

    elif request.method == 'POST':
        name = request.form['name']
        attack = request.form['attack']
        cost = request.form['cost']
        description = request.form['description']
        image = request.form['image']
        deck = request.form['deck']
        card_contents = Cards(name=name, attack=attack, cost=cost, description=description, image=image, deck=deck)
        db.session.add(card_contents)
        db.session.commit()
        return ('', 204)

    
@app.route('/cards/<int:id>', methods=['PUT', 'DELETE'])
def cards_put_delete(id):
    if request.method == 'PUT':
        card = Cards.query.get_or_404(id)
        try:
            name = request.form['name']
            attack = request.form['attack']
            cost = request.form['cost']
            description = request.form['description']
            image = request.form['image']
            deck = request.form['deck']
            db.session.commit()
            return ('', 204)
        except:
            return 'There was an issue updating the card'

    elif request.method == 'DELETE':
        card_to_delete = Cards.query.get_or_404(id)
        db.session.delete(card_to_delete)
        db.session.commit()
        return redirect('', 204)

if __name__ == "__main__":
    db.create_all()
    app.run(port=8000, debug=True)
