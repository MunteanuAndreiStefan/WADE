import tweepy
import pymongo
import logging
import time # to sl
from config import api_key, api_secret_key, access_token, access_secret_token

auth = tweepy.OAuthHandler(consumer_key=api_key, consumer_secret=api_secret_key)
auth.set_access_token(access_token, access_secret_token)
api = tweepy.API(auth)

mongo_client = pymongo.MongoClient()
db = mongo_client.TwitterFakeNewsDetector
posts_collection = db.posts
users_collection = db.users

logging.basicConfig(filename='crawler.log', filemode='a', level='INFO',
                    format='%(asctime)s - %(levelname)s - %(message)s')

INTERESTING_SUBJECTS = ["#president", "#elections", "#VioricaDancila", "#KlausIohannis", "#Romania", "#government",
                        "#religious", "#uspolitcs", "#Putin", "#Brexit", "#Russia", "#Washington", "#Ukraine",
                        "#America politics", "#political news",
                        "#Donald Trump", "#Barack Obama"]  # keywords


def get_interesting_subject():
    s = INTERESTING_SUBJECTS.pop(0)
    INTERESTING_SUBJECTS.append(s)
    return s


def crawl_twitter():
    # TODO retry decorator in case api request fails

    while True:
        keyword = get_interesting_subject()
        search_results = api.search(q=keyword, lang="en", truncated=False)
        logging.info(msg="API request - Searched for {} keyword".format(keyword))
        print("API request - Searched for {} keyword".format(keyword))
        for result in search_results:
            tweet_id = result.id
            if posts_collection.find_one({'id': tweet_id}):
                continue
            try:
                tweet_status = api.get_status(tweet_id)
            except Exception as e:
                continue
            time.sleep(2)
            posts_collection.insert_one(tweet_status._json)
            logging.info(msg="API request - Searched for tweed id {}".format(tweet_id))
            print("API request - Searched for tweed id {}".format(tweet_id))
            user_id = tweet_status._json['user']['id']
            if users_collection.find_one({'id': user_id}):
                continue
            user = api.get_user(user_id)
            time.sleep(2)
            logging.info(msg="API request - Searched for user id {}".format(user_id))
            print("API request - Searched for user id {}".format(user_id))
            users_collection.insert_one(user._json)
        logging.info(msg="Sleepin 1 minute..")
        print("Sleepin 1 minute..")
        time.sleep(1 * 60)


if __name__ == '__main__':
    crawl_twitter()
