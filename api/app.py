from flask import Flask, render_template, request, redirect, session, jsonify
from flask_sqlalchemy import SQLAlchemy
from flask_cors import CORS

app = Flask(__name__)
cors = CORS(app)
app.config['SQLALCHEMY_DATABASE_URI'] = 'mysql://capstone2021otcg:CS467cardgame!@osu-otcg-db.cs3ccyqxyr4e.us-west-1.rds.amazonaws.com/osu-otcg-db'

db = SQLAlchemy(app)

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
    user = db.Column(db.Integer, db.ForeignKey('user.id'), nullable=True)


class Users(db.Model):
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
    user_id = db.Column(db.Integer, db.ForeignKey('user.id'), nullable=False)


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
    
    
@app.route('/cards', methods=['GET', 'POST'])
def cards_get_post():
    if request.method == 'GET':
        data = Cards.query.all()
        return jsonify([{"name": card.name} for card in data])

    elif request.method == 'POST':
        name = request.form['name']
        attack = request.form['attack']
        defense = request.form['defense']
        cost = request.form['cost']
        card_contents = Cards(name=name, attack=attack, defense=defense, cost=cost)
        db.session.add(card_contents)
        db.session.commit()
        return ('', 204)

@app.route('/cards/<int:id>', methods=['PUT', 'DELETE'])
def cards_put_delete(id):
    if request.method == 'PUT':
        card = Cards.query.get_or_404(id)
        try:
            card.name = request.form['name']
            card.attack = request.form['attack']
            card.defense = request.form['defense']
            card.cost = request.form['cost']
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
