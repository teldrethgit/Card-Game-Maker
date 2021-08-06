from flask import Flask, render_template, request, redirect, session, jsonify
from flask_cors import CORS
import redis
from flask_session import Session
from flask_sqlalchemy import SQLAlchemy
#from flask_login import LoginManager, UserMixin, login_user, current_user, logout_user, login_required
import os

# Database Configs
app = Flask(__name__, static_url_path="/",static_folder="./", template_folder="./")
cors = CORS(app, supports_credentials=True)
r = redis.from_url('redis://:p79ab5ac4e27e5b357bc3cb8769f768bdc14ef9f623f952696475bee269f929e5@ec2-54-162-232-48.compute-1.amazonaws.com:7749')
SESSION_TYPE = 'redis'
SESSION_REDIS = r
app.config.from_object(__name__)
Session(app)
app.config['SECRET_KEY'] = os.urandom(24)
app.config['SQLALCHEMY_DATABASE_URI'] = 'mysql://capstone2021otcg:CS467cardgame!@osu-otcg-db.cs3ccyqxyr4e.us-west-1.rds.amazonaws.com/osu-otcg-db'

db = SQLAlchemy(app)

# Flask Login Requirements
#login_manager = LoginManager()
#login_manager.login_view = '/'
#login_manager.init_app(app)
#@login_manager.user_loader
#def load_user(user_id):
#    return Users.query.get(user_id)


def JSONcard(card):
    return (
        {
            "id": card.id, 
            "name": card.name, 
            "health": card.health, 
            "attack": card.attack, 
            "cost": card.cost, 
            "image": card.image, 
            "game": card.game
        })

def JSONgame(game):
    return (
        {
            "id": game.id,
            "name": game.name,
            "description": game.description,
            "health_pool": game.health_pool,
            "total_hand": game.total_turns,
            "starting_hand": game.time_limit,
            "user_id": game.user_id
        })

def JSONdeck(deck):
      return (
        {
            "id": deck.id,
            "name": deck.name,
            "description": deck.description
        })


class Cards(db.Model):
    id = db.Column(db.Integer, primary_key=True, nullable=False)
    name = db.Column(db.String(15), unique=False, nullable=False)
    health = db.Column(db.Integer, unique=False, nullable=False)
    attack = db.Column(db.Integer, unique=False, nullable=False)
    cost = db.Column(db.Integer, unique=False, nullable=False)
    image = db.Column(db.LargeBinary, unique=False, nullable=True)
    game = db.Column(db.Integer,db.ForeignKey('games.id'),  nullable=True)


class Decks(db.Model):
    id = db.Column(db.Integer, primary_key=True, nullable=False)
    name = db.Column(db.String(80), unique=False, nullable=False)
    description = db.Column(db.Text, unique=False, nullable=True)
    user = db.Column(db.Integer, db.ForeignKey('users.id'), nullable=True)
    game = db.Column(db.Integer, db.ForeignKey('games.id'), nullable=True)


#class Users(UserMixin, db.Model):
class Users(db.Model):
    id = db.Column(db.Integer, primary_key=True, nullable=False)
    name = db.Column(db.String(15), unique=True, nullable=False)
    password = db.Column(db.String(15), unique=False, nullable=False)


class Games(db.Model):
    id = db.Column(db.Integer, primary_key=True, nullable=False)
    name = db.Column(db.String(15), unique=True, nullable=False)
    description = db.Column(db.Text, unique=False, nullable=True)
    total_hand = db.Column(db.Integer, unique=True, nullable=False)
    health_pool = db.Column(db.Integer, unique=False, nullable=False)
    starting_hand = db.Column(db.Integer, unique=True, nullable=False)
    user_id = db.Column(db.Integer, db.ForeignKey('users.id'), nullable=False)

class CardsDecks(db.Model):
    id = db.Column(db.Integer, primary_key=True, nullable=False)
    card = db.Column(db.Integer, db.ForeignKey('cards.id'), nullable=True)
    deck = db.Column(db.Integer, db.ForeignKey('decks.id'), nullable=True)


@app.route('/')
def index():
    return render_template('index.html')


@app.route('/signup', methods=['POST', 'GET'])
def signup():
    if request.method == 'POST':
        name = request.form['name']
        password = request.form['password']
        user = Users.query.filter_by(name=name).first()
        if user:
            return ('', 401)
        else:
            new_user = Users(name=name, password=password)
            db.session.add(new_user)
            db.session.commit()
            session['id'] = new_user.id
            return ('', 201)
    
    
@app.route('/login', methods=['POST'])
def login():
    if request.method == 'POST':
        name = request.form['name']
        password = request.form['password']
        user = Users.query.filter_by(name=name).first()
        remember = True
        if not user or user.password != password:
            return ('', 401)
        else:
            #login_user(user, remember=remember)
            session['id'] = user.id
            return ('', 200)

        
@app.route('/logout')
def logout():
    #logout_user()
    session.pop('id', None)
    return ''
    
    
@app.route('/sessointest')
def test():
    return jsonify(session.get('id'))

@app.route('/games', methods=['GET','POST'])
def games_get_post():
    if request.method == 'GET':
        data = Games.query.all()
        return jsonify([JSONgame(game) for game in data])
    
    elif request.method == 'POST':
        name = request.form['name']
        description = request.form['description']
        starting_hand = request.form['starting_hand']
        health_pool = request.form['health_pool']
        total_hand = request.form['total_hand']
        user_id = session.get('id')
        game_names = Games.query.filter_by(name=name).first()
        if game_names:
            return ('', 401)
        game_contents = Games(name=name, description=description, starting_hand=starting_hand, health_pool=health_pool,
                            total_hand=total_hand, user_id=user_id)
        db.session.add(game_contents)
        db.session.commit()
        return ('', 204)
        
    
@app.route('/games/<int:id>', methods=['POST', 'DELETE'])
def games_put_delete(id):
    if request.method == 'POST':
        game = Games.query.get_or_404(id)
        try:
            name = request.form['name']
            description = request.form['description']
            total_hand = request.form['total_hand']
            health_pool = request.form['health_pool']
            starting_hand = request.form['starting_hand']
            user_id = session.get('id')
            db.session.commit()
            return ('', 204)
        except:
            return 'There was an issue updating the game'

    elif request.method == 'DELETE':
        game_to_delete = Games.query.get_or_404(id)
        db.session.delete(game_to_delete)
        db.session.commit()
        return ('', 204)


@app.route('/decks', methods=['GET','POST'])
def decks_get_post():
    if request.method == 'GET':
        data = Decks.query.filter_by(user = session.get('id'),game=request.args.get('game'))
        return jsonify([JSONdeck(deck) for deck in data])

    elif request.method == 'POST':
        name = request.form['name']
        description = request.form['description']
        game = request.form['game']
        user = session.get('id')
        deck_contents = Decks(name=name, description=description, game=game, user=user)
        db.session.add(deck_contents)
        db.session.commit()
        return ('', 204)

    else:
        return ('', 400)  

@app.route('/decks/<int:id>/cards', methods=['GET'])
def deck_get_cards(id):
    result = []
    card_ids = []
    card_decks = CardsDecks.query.filter_by(deck=id)
    for cd in card_decks:
        card_ids.append(cd.card)
    data = Cards.query.filter(Cards.id.in_(card_ids)).all()
    return jsonify([JSONcard(card) for card in data])

@app.route('/decks/<int:deck_id>/cards/<int:card_id>', methods=['POST', 'DELETE'])
def add_to_deck(deck_id, card_id):
    if request.method == 'POST':
        db.session.add(CardsDecks(card=card_id,deck=deck_id))
        db.session.commit()
        return ('', 204)
    if request.method == 'DELETE':
        card_deck = CardsDecks.query.filter_by(card=card_id,deck=deck_id).first()
        db.session.delete(card_deck)
        db.session.commit()
        return ('', 204)
      
@app.route('/decks/<int:id>', methods=['GET', 'PUT', 'DELETE'])
def deck_get_put_delete(id):

    if request.method == 'GET':
        return jsonify(JSONdeck(Decks.query.get_or_404(id)))

    elif request.method == 'PUT':
        deck = Decks.query.get_or_404(id)
        try:
            deck.name = request.args.get('name')
            deck.description = request.args.get('description')
            deck.user = session.get('id')
            db.session.commit()
            return ('', 204)
        except:
            return 'There was an issue updating the game'

    elif request.method == 'DELETE':
        deck_to_delte = Decks.query.get_or_404(id)
        db.session.delete(deck_to_delte)
        db.session.commit()
        return ('', 204)
        
@app.route('/cards', methods=['GET', 'POST'])
def cards_get_post():
    if request.method == 'GET':
        data = Cards.query.all()
        return jsonify([JSONcard(card) for card in data])

    elif request.method == 'POST':
        name = request.form['name']
        health = request.form['health']
        attack = request.form['attack']
        cost = request.form['cost']
        image = request.form['image']
        card_contents = Cards(name=name, health=health, attack=attack, cost=cost, image=image)
        db.session.add(card_contents)
        db.session.commit()
        return ('', 204)
    else:
        return ('', 400)

    
@app.route('/cards/<int:id>', methods=['PUT', 'DELETE'])
def cards_put_delete(id):
    if request.method == 'PUT':
        card = Cards.query.get_or_404(id)
        try:
            name = request.form['name']
            health = request.form['health']
            attack = request.form['attack']
            cost = request.form['cost']
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
        return ('', 204)

@app.route('/cards/games/<int:id>', methods=['GET', 'POST'])
def get_game_cards(id):
    if request.method == 'GET':
        data = Cards.query.filter_by(game=id).all()
        return jsonify([JSONcard(card) for card in data])

if __name__ == "__main__":
    db.create_all()
    app.run(port=8000, debug=True)
