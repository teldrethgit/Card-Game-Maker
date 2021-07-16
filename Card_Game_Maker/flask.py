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


@app.route('/display-cards', methods=['GET'])
def get_data():
    data = Cards.query.all()
    return jsonify([str(card) for card in data])


if __name__ == "__main__":
    db.create_all()
    app.run(debug=True)


