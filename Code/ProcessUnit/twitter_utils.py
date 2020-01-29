import tweepy
import logging
import time
import json
import re
from logger import LogDecorator, log
from config import Config as cfg

auth = tweepy.OAuthHandler(consumer_key=cfg.api_key, consumer_secret=cfg.api_secret_key)
auth.set_access_token(cfg.access_token, cfg.access_secret_token)
api = tweepy.API(auth)

LogDecorator()
def get_tweet_data(tweet_url):

    id = int(re.findall(r'twitter.com/\w+/status/([0-9]+).*', tweet_url)[0])
    return api.get_status(id)._json

if __name__ == '__main__':

    test_url = r'https://twitter.com/ABSCBNNews/status/1199477938162302976'

    print(json.dumps(get_tweet_data(test_url), indent=4))