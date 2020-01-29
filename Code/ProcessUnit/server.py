from flask import Flask, escape, request, Response
from manager import Manager
import json
from logger import LogDecorator

app = Flask(__name__)
manager = Manager()


@app.route('/')
def index():
    return f'Hello!'


@app.route('/tweet/')
def fake_tweet():
    tweet_url = request.args.get("tweet_url")
    response = manager.analyze_tweet(tweet_url)
    
    resp = Response(response=json.dumps(response, indent=4),
                    status=200,
                    mimetype="application/json")

    return resp



@app.route('/user/')
def fake_user():
    user_id = request.args.get("id")
    response = manager.analyze_user(user_id)
    
    resp = Response(response=json.dumps(response, indent=4),
                    status=200,
                    mimetype="application/json")

    return resp


app.run(host='0.0.0.0', port=12345, debug=True)
