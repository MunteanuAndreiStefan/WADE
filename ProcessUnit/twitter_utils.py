import tweepy
import logging
import time
import json
import re
import pymongo
from logger import LogDecorator, log
from config import Config as cfg


auth = tweepy.OAuthHandler(consumer_key=cfg.api_key, consumer_secret=cfg.api_secret_key)
auth.set_access_token(cfg.access_token, cfg.access_secret_token)
api = tweepy.API(auth)

mongo_client = pymongo.MongoClient()
db = mongo_client.TwitterFakeNewsDetector
posts_collection = db.posts
users_collection = db.users

LogDecorator()


def get_tweet_data(tweet_url):
    tweet_id = int(re.findall(r'twitter.com/\w+/status/([0-9]+).*', tweet_url)[0])
    tweet_data = posts_collection.find_one(
        {'id': tweet_id})  # first check if tweet is not saved in Mongo posts collection

    if not tweet_data:
        tweet_data = api.get_status(tweet_id)._json
    return tweet_data


def get_user_data(user_id):
    user_data = users_collection.find_one({'id': user_id})
    if not user_data:
        user_data = api.get_user(user_id)._json
    return user_data


if __name__ == '__main__':
    test_url = r'https://twitter.com/DavidHenigUK/status/1211942899514052610'
    print(get_tweet_data(test_url))
    print(get_user_data(2494186513))
