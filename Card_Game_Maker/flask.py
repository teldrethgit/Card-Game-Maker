from flask import Flask, render_template, request, redirect, session
from flask_sqlalchemy import SQLAlchemy

app = Flask(__name__)
app.config['SQLALCHEMY_DATABASE_URI'] = 'mysql://capstone_u2021_otcg:CS467cardgame!@classmysql.engr.oregonstate.edu/capstone_u2021_otcg'

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


@app.route('/cards', methods=['GET'])
def get_data():
    data = Cards.query.all()
    return jsonify([str(card) for card in data])


@app.route('/cards', methods=['POST', 'GET'])
def index():
    if request.method == 'POST':
        name = request.form['name']
        attack = request.form['attack']
        defense = request.form['defense']
        cost = request.form['cost']
        card_contents = Cards(name=name, attack=attack, defense=defense, cost=cost)
        db.session.add(card_contents)
        db.session.commit()
        return redirect('/cards')
    else:
        return redirect('/cards')

    
@app.route('/cards/delete/<int:id>')
def delete(id):
    card_to_delete = Cards.query.get_or_404(id)
    db.session.delete(card_to_delete)
    db.session.commit()
    return redirect('/cards')


@app.route('cards/update/<int:id>', methods=['GET', 'POST'])
def update(id):
    card = Cards.query.get_or_404(id)
    if request.method == 'POST':
        try:
            card.name = request.form['name']
            card.attack = request.form['attack']
            card.defense = request.form['defense']
            card.cost = request.form['cost']
            db.session.commit()
            return redirect('/cards')
        except:
            return 'There was an issue updating the card'
    else:
        return redirect('/cards')

if __name__ == "__main__":
    db.create_all()
    app.run(debug=True)


