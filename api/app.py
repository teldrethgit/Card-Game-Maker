from flask import Flask, render_template, request, redirect, session, jsonify
from flask_sqlalchemy import SQLAlchemy

app = Flask(__name__)
app.config['SQLALCHEMY_DATABASE_URI'] = 'mysql://capstone2021otcg:CS467cardgame!@osu-otcg-db.cs3ccyqxyr4e.us-west-1.rds.amazonaws.com/osu-otcg-db'

db = SQLAlchemy(app)


class Cards(db.Model):
    id = db.Column(db.Integer, primary_key=True, nullable=False)
    name = db.Column(db.String(80), unique=False, nullable=False)
    attack = db.Column(db.Integer, unique=False, nullable=False)
    defense = db.Column(db.Integer, unique=False, nullable=False)
    cost = db.Column(db.Integer, unique=False, nullable=False)


class Users(db.Model):
   id = db.Column(db.Integer, primary_key=True, nullable=False)
   name = db.Column(db.String(15), unique=True, nullable=False)
   password = db.Column(db.String(15), unique=False, nullable=False)


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